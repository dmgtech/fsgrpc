namespace FsGrpc

open Helpers
open System
open Xunit
open FsGrpc.Protobuf

open Test.Name.Space
open Grpc.Core
open FSharp.Control
open System.Threading
open System.Threading.Tasks
open Grpc.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Http
open System.Linq
open Microsoft.AspNetCore.Hosting
open FSharp.Core

module Fixtures = 
    let mutable app = Unchecked.defaultof<WebApplication>
    type ServiceFixture() =
        //DO THE TEST SETUP HERE
        //(THIS IS THE CONSTRUCTOR)
        do
            let handler (request: HelloRequest) (context: ServerCallContext) : Task<HelloReply> =
                task {
                    return { Message = "Hello " + request.Name }
                }
            Greeter.Service.sayHelloImpl <- handler
            Greeter.Service.sayHelloServerStreamingImpl <- 
                fun (request: HelloRequest) (writer: IServerStreamWriter<HelloReply>) (context: ServerCallContext) -> 
                    task {
                        for i in [1..5] do
                            let! x = handler request context
                            do! writer.WriteAsync x
                    }
            Greeter.Service.sayHelloClientStreamingImpl <- 
                fun (requestStream: IAsyncStreamReader<HelloRequest>) (context: ServerCallContext) ->
                    AsyncSeq.ofAsyncEnum (requestStream.ReadAllAsync())
                    |> AsyncSeq.toListAsync
                    |> fun xs -> async {
                        let! xs' = xs
                        let names = String.concat ", " (Seq.map (fun x -> x.Name) xs')
                        return { Message = $"""Hello {names}""" } }
                    |> Async.StartAsTask

            Greeter.Service.sayHelloDuplexStreamingImpl <- 
                fun (requestStream: IAsyncStreamReader<HelloRequest>) (writer: IServerStreamWriter<HelloReply>) (context: ServerCallContext) ->
                    AsyncSeq.ofAsyncEnum (requestStream.ReadAllAsync())
                    |> AsyncSeq.mapAsync (fun x -> handler x context |> Async.AwaitTask)
                    |> AsyncSeq.iterAsync (fun x -> writer.WriteAsync(x) |> Async.AwaitTask)
                    |> Async.StartAsTask
                    |> fun x -> task { do! x } 


            let builder : WebApplicationBuilder = WebApplication.CreateBuilder()
            builder.Services.AddGrpc() |> ignore
            builder.WebHost.ConfigureKestrel(fun serverOptions -> 
                serverOptions.ConfigureEndpointDefaults(fun listenOptions -> 
                    listenOptions.Protocols <- Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2)) |> ignore
            app <- builder.Build()
            app.MapGrpcService<Greeter.Service>() |> ignore
            app.StartAsync().Wait()
            
        interface IDisposable with
            member __.Dispose () = 
                app.StopAsync().Wait()
                app.DisposeAsync().AsTask().Wait()
                

module ServiceTests =
    type ServiceTests() =
        [<Fact>]
        member this.``Unary Call Works`` () =
            let channel = Grpc.Net.Client.GrpcChannel.ForAddress(Fixtures.app.Urls.First())
            let client = Greeter.Client(channel)
            let callOptions = Grpc.Core.CallOptions(Unchecked.defaultof<Metadata>, Unchecked.defaultof<Nullable<DateTime>>, Unchecked.defaultof<CancellationToken>)
            let response = client.SayHello callOptions { Name = "World" }
            Assert.Equal("Hello World", response.Message)
            
        [<Fact>]
        member this.``Async Unary Call Works`` () =
            let channel = Grpc.Net.Client.GrpcChannel.ForAddress(Fixtures.app.Urls.First())
            let client = Greeter.Client(channel)
            let callOptions = Grpc.Core.CallOptions(Unchecked.defaultof<Metadata>, Unchecked.defaultof<Nullable<DateTime>>, Unchecked.defaultof<CancellationToken>)
            let t = client.SayHelloAsync callOptions { Name = "World" }
            Assert.Equal("Hello World", t.ResponseAsync.Result.Message)
            
        [<Fact>]
        member this.``ClientStreaming Call Works`` () =
            let channel = Grpc.Net.Client.GrpcChannel.ForAddress(Fixtures.app.Urls.First())
            let client = Greeter.Client(channel)
            let callOptions = Grpc.Core.CallOptions(Unchecked.defaultof<Metadata>, Unchecked.defaultof<Nullable<DateTime>>, Unchecked.defaultof<CancellationToken>)
            let call = (client.SayHelloClientStreamingAsync callOptions)
            let t = task {
                do! call.RequestStream.WriteAsync { Name = "World" }
                do! call.RequestStream.WriteAsync { Name = "asdf" }
                do! call.RequestStream.WriteAsync { Name = "qwerty" }
                do! call.RequestStream.CompleteAsync ()
                let! response = call.ResponseAsync
                return response
            }
            Assert.Equal("Hello World, asdf, qwerty", t.Result.Message)
            
        [<Fact>]
        member this.``ServerStreaming Call Works`` () =
            let channel = Grpc.Net.Client.GrpcChannel.ForAddress(Fixtures.app.Urls.First())
            let client = Greeter.Client(channel)
            let callOptions = Grpc.Core.CallOptions(Unchecked.defaultof<Metadata>, Unchecked.defaultof<Nullable<DateTime>>, Unchecked.defaultof<CancellationToken>)
            let call = (client.SayHelloServerStreamingAsync callOptions { Name = "asdf" })
            let response = 
                call.ResponseStream.ReadAllAsync()
                |> AsyncSeq.ofAsyncEnum
                |> AsyncSeq.map(fun x -> x.Message)
                |> AsyncSeq.toArraySynchronously
            let expected = Array.replicate 5 "Hello asdf"
            Assert.True(expected.SequenceEqual(response))
            
        [<Fact>]
        member this.``DuplexStreaming Call Works`` () =
            let channel = Grpc.Net.Client.GrpcChannel.ForAddress(Fixtures.app.Urls.First())
            let client = Greeter.Client(channel)
            let callOptions = Grpc.Core.CallOptions(Unchecked.defaultof<Metadata>, Unchecked.defaultof<Nullable<DateTime>>, Unchecked.defaultof<CancellationToken>)
            let call = (client.SayHelloDuplexStreamingAsync callOptions)
            let responses = task {
                do! call.RequestStream.WriteAsync { Name = "asdf" }
                let! _ = call.ResponseStream.MoveNext() 
                let response1 = call.ResponseStream.Current
                do! call.RequestStream.WriteAsync { Name = "fdsa" }
                let! _ = call.ResponseStream.MoveNext() 
                let response2 = call.ResponseStream.Current
                return [response1; response2]
            }
            let expected = [ {Message = "Hello asdf"}; {Message = "Hello fdsa"} ] 
            Assert.True(expected.SequenceEqual(responses.Result))

        interface IClassFixture<Fixtures.ServiceFixture>



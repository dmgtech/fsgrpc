namespace rec Test.Name.Space
open FsGrpc.Protobuf
open Google.Protobuf
#nowarn "40"
#nowarn "1182"


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module StructTestMessage =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Metadata: OptionBuilder<Google.Protobuf.Struct> // (1)
            val mutable Data: OptionBuilder<Google.Protobuf.Value> // (2)
            val mutable Items: OptionBuilder<Google.Protobuf.ListValue> // (3)
            val mutable NullField: Google.Protobuf.NullValue // (4)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Metadata.Set (ValueCodec.Message<Google.Protobuf.Struct>.ReadValue reader)
            | 2 -> x.Data.Set (ValueCodec.Message<Google.Protobuf.Value>.ReadValue reader)
            | 3 -> x.Items.Set (ValueCodec.Message<Google.Protobuf.ListValue>.ReadValue reader)
            | 4 -> x.NullField <- ValueCodec.Enum<Google.Protobuf.NullValue>.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Test.Name.Space.StructTestMessage = {
            Metadata = x.Metadata.Build
            Data = x.Data.Build
            Items = x.Items.Build
            NullField = x.NullField
            }

/// <summary>Test message that uses all Struct-related types</summary>
type private _StructTestMessage = StructTestMessage
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type StructTestMessage = {
    // Field Declarations
    [<System.Text.Json.Serialization.JsonPropertyName("metadata")>] Metadata: Google.Protobuf.Struct option // (1)
    [<System.Text.Json.Serialization.JsonPropertyName("data")>] Data: Google.Protobuf.Value option // (2)
    [<System.Text.Json.Serialization.JsonPropertyName("items")>] Items: Google.Protobuf.ListValue option // (3)
    [<System.Text.Json.Serialization.JsonPropertyName("nullField")>] NullField: Google.Protobuf.NullValue // (4)
    }
    with
    static member Proto : Lazy<ProtoDef<StructTestMessage>> =
        lazy
        // Field Definitions
        let Metadata = FieldCodec.Optional ValueCodec.Message<Google.Protobuf.Struct> (1, "metadata")
        let Data = FieldCodec.Optional ValueCodec.Message<Google.Protobuf.Value> (2, "data")
        let Items = FieldCodec.Optional ValueCodec.Message<Google.Protobuf.ListValue> (3, "items")
        let NullField = FieldCodec.Primitive ValueCodec.Enum<Google.Protobuf.NullValue> (4, "nullField")
        // Proto Definition Implementation
        { // ProtoDef<StructTestMessage>
            Name = "StructTestMessage"
            Empty = {
                Metadata = Metadata.GetDefault()
                Data = Data.GetDefault()
                Items = Items.GetDefault()
                NullField = NullField.GetDefault()
                }
            Size = fun (m: StructTestMessage) ->
                0
                + Metadata.CalcFieldSize m.Metadata
                + Data.CalcFieldSize m.Data
                + Items.CalcFieldSize m.Items
                + NullField.CalcFieldSize m.NullField
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: StructTestMessage) ->
                Metadata.WriteField w m.Metadata
                Data.WriteField w m.Data
                Items.WriteField w m.Items
                NullField.WriteField w m.NullField
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Test.Name.Space.StructTestMessage.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeMetadata = Metadata.WriteJsonField o
                let writeData = Data.WriteJsonField o
                let writeItems = Items.WriteJsonField o
                let writeNullField = NullField.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: StructTestMessage) =
                    writeMetadata w m.Metadata
                    writeData w m.Data
                    writeItems w m.Items
                    writeNullField w m.NullField
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : StructTestMessage =
                    match kvPair.Key with
                    | "metadata" -> { value with Metadata = Metadata.ReadJsonField kvPair.Value }
                    | "data" -> { value with Data = Data.ReadJsonField kvPair.Value }
                    | "items" -> { value with Items = Items.ReadJsonField kvPair.Value }
                    | "nullField" -> { value with NullField = NullField.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _StructTestMessage.empty (node.AsObject ())
        }
    static member empty
        with get() = Test.Name.Space._StructTestMessage.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module NestedStructTest =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Name: string // (1)
            val mutable Properties: OptionBuilder<Google.Protobuf.Struct> // (2)
            val mutable Values: RepeatedBuilder<Google.Protobuf.Value> // (3)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Name <- ValueCodec.String.ReadValue reader
            | 2 -> x.Properties.Set (ValueCodec.Message<Google.Protobuf.Struct>.ReadValue reader)
            | 3 -> x.Values.Add (ValueCodec.Message<Google.Protobuf.Value>.ReadValue reader)
            | _ -> reader.SkipLastField()
        member x.Build : Test.Name.Space.NestedStructTest = {
            Name = x.Name |> orEmptyString
            Properties = x.Properties.Build
            Values = x.Values.Build
            }

/// <summary>Test message with nested Struct usage</summary>
type private _NestedStructTest = NestedStructTest
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type NestedStructTest = {
    // Field Declarations
    [<System.Text.Json.Serialization.JsonPropertyName("name")>] Name: string // (1)
    [<System.Text.Json.Serialization.JsonPropertyName("properties")>] Properties: Google.Protobuf.Struct option // (2)
    [<System.Text.Json.Serialization.JsonPropertyName("values")>] Values: Google.Protobuf.Value list // (3)
    }
    with
    static member Proto : Lazy<ProtoDef<NestedStructTest>> =
        lazy
        // Field Definitions
        let Name = FieldCodec.Primitive ValueCodec.String (1, "name")
        let Properties = FieldCodec.Optional ValueCodec.Message<Google.Protobuf.Struct> (2, "properties")
        let Values = FieldCodec.Repeated ValueCodec.Message<Google.Protobuf.Value> (3, "values")
        // Proto Definition Implementation
        { // ProtoDef<NestedStructTest>
            Name = "NestedStructTest"
            Empty = {
                Name = Name.GetDefault()
                Properties = Properties.GetDefault()
                Values = Values.GetDefault()
                }
            Size = fun (m: NestedStructTest) ->
                0
                + Name.CalcFieldSize m.Name
                + Properties.CalcFieldSize m.Properties
                + Values.CalcFieldSize m.Values
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: NestedStructTest) ->
                Name.WriteField w m.Name
                Properties.WriteField w m.Properties
                Values.WriteField w m.Values
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Test.Name.Space.NestedStructTest.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeName = Name.WriteJsonField o
                let writeProperties = Properties.WriteJsonField o
                let writeValues = Values.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: NestedStructTest) =
                    writeName w m.Name
                    writeProperties w m.Properties
                    writeValues w m.Values
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : NestedStructTest =
                    match kvPair.Key with
                    | "name" -> { value with Name = Name.ReadJsonField kvPair.Value }
                    | "properties" -> { value with Properties = Properties.ReadJsonField kvPair.Value }
                    | "values" -> { value with Values = Values.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _NestedStructTest.empty (node.AsObject ())
        }
    static member empty
        with get() = Test.Name.Space._NestedStructTest.Proto.Value.Empty

namespace Test.Name.Space.Optics
open Focal.Core
module StructTestMessage =
    let ``metadata`` : ILens'<Test.Name.Space.StructTestMessage,Google.Protobuf.Struct option> =
        {
            _getter = { _get = fun (s: Test.Name.Space.StructTestMessage) -> s.Metadata }
            _setter = { _over = fun a2b s -> { s with Metadata = a2b s.Metadata } }
        }
    let ``data`` : ILens'<Test.Name.Space.StructTestMessage,Google.Protobuf.Value option> =
        {
            _getter = { _get = fun (s: Test.Name.Space.StructTestMessage) -> s.Data }
            _setter = { _over = fun a2b s -> { s with Data = a2b s.Data } }
        }
    let ``items`` : ILens'<Test.Name.Space.StructTestMessage,Google.Protobuf.ListValue option> =
        {
            _getter = { _get = fun (s: Test.Name.Space.StructTestMessage) -> s.Items }
            _setter = { _over = fun a2b s -> { s with Items = a2b s.Items } }
        }
    let ``nullField`` : ILens'<Test.Name.Space.StructTestMessage,Google.Protobuf.NullValue> =
        {
            _getter = { _get = fun (s: Test.Name.Space.StructTestMessage) -> s.NullField }
            _setter = { _over = fun a2b s -> { s with NullField = a2b s.NullField } }
        }
module NestedStructTest =
    let ``name`` : ILens'<Test.Name.Space.NestedStructTest,string> =
        {
            _getter = { _get = fun (s: Test.Name.Space.NestedStructTest) -> s.Name }
            _setter = { _over = fun a2b s -> { s with Name = a2b s.Name } }
        }
    let ``properties`` : ILens'<Test.Name.Space.NestedStructTest,Google.Protobuf.Struct option> =
        {
            _getter = { _get = fun (s: Test.Name.Space.NestedStructTest) -> s.Properties }
            _setter = { _over = fun a2b s -> { s with Properties = a2b s.Properties } }
        }
    let ``values`` : ILens'<Test.Name.Space.NestedStructTest,Google.Protobuf.Value list> =
        {
            _getter = { _get = fun (s: Test.Name.Space.NestedStructTest) -> s.Values }
            _setter = { _over = fun a2b s -> { s with Values = a2b s.Values } }
        }

namespace Test.Name.Space
open Focal.Core
open System.Runtime.CompilerServices
[<Extension>]
type OpticsExtensionMethods_struct_test_proto =
    [<Extension>]
    static member inline Metadata(lens : ILens<'a,'b,Test.Name.Space.StructTestMessage,Test.Name.Space.StructTestMessage>) : ILens<'a,'b,Google.Protobuf.Struct option,Google.Protobuf.Struct option> =
        lens.ComposeWith(Test.Name.Space.Optics.StructTestMessage.``metadata``)
    [<Extension>]
    static member inline Metadata(traversal : ITraversal<'a,'b,Test.Name.Space.StructTestMessage,Test.Name.Space.StructTestMessage>) : ITraversal<'a,'b,Google.Protobuf.Struct option,Google.Protobuf.Struct option> =
        traversal.ComposeWith(Test.Name.Space.Optics.StructTestMessage.``metadata``)
    [<Extension>]
    static member inline Data(lens : ILens<'a,'b,Test.Name.Space.StructTestMessage,Test.Name.Space.StructTestMessage>) : ILens<'a,'b,Google.Protobuf.Value option,Google.Protobuf.Value option> =
        lens.ComposeWith(Test.Name.Space.Optics.StructTestMessage.``data``)
    [<Extension>]
    static member inline Data(traversal : ITraversal<'a,'b,Test.Name.Space.StructTestMessage,Test.Name.Space.StructTestMessage>) : ITraversal<'a,'b,Google.Protobuf.Value option,Google.Protobuf.Value option> =
        traversal.ComposeWith(Test.Name.Space.Optics.StructTestMessage.``data``)
    [<Extension>]
    static member inline Items(lens : ILens<'a,'b,Test.Name.Space.StructTestMessage,Test.Name.Space.StructTestMessage>) : ILens<'a,'b,Google.Protobuf.ListValue option,Google.Protobuf.ListValue option> =
        lens.ComposeWith(Test.Name.Space.Optics.StructTestMessage.``items``)
    [<Extension>]
    static member inline Items(traversal : ITraversal<'a,'b,Test.Name.Space.StructTestMessage,Test.Name.Space.StructTestMessage>) : ITraversal<'a,'b,Google.Protobuf.ListValue option,Google.Protobuf.ListValue option> =
        traversal.ComposeWith(Test.Name.Space.Optics.StructTestMessage.``items``)
    [<Extension>]
    static member inline NullField(lens : ILens<'a,'b,Test.Name.Space.StructTestMessage,Test.Name.Space.StructTestMessage>) : ILens<'a,'b,Google.Protobuf.NullValue,Google.Protobuf.NullValue> =
        lens.ComposeWith(Test.Name.Space.Optics.StructTestMessage.``nullField``)
    [<Extension>]
    static member inline NullField(traversal : ITraversal<'a,'b,Test.Name.Space.StructTestMessage,Test.Name.Space.StructTestMessage>) : ITraversal<'a,'b,Google.Protobuf.NullValue,Google.Protobuf.NullValue> =
        traversal.ComposeWith(Test.Name.Space.Optics.StructTestMessage.``nullField``)
    [<Extension>]
    static member inline Name(lens : ILens<'a,'b,Test.Name.Space.NestedStructTest,Test.Name.Space.NestedStructTest>) : ILens<'a,'b,string,string> =
        lens.ComposeWith(Test.Name.Space.Optics.NestedStructTest.``name``)
    [<Extension>]
    static member inline Name(traversal : ITraversal<'a,'b,Test.Name.Space.NestedStructTest,Test.Name.Space.NestedStructTest>) : ITraversal<'a,'b,string,string> =
        traversal.ComposeWith(Test.Name.Space.Optics.NestedStructTest.``name``)
    [<Extension>]
    static member inline Properties(lens : ILens<'a,'b,Test.Name.Space.NestedStructTest,Test.Name.Space.NestedStructTest>) : ILens<'a,'b,Google.Protobuf.Struct option,Google.Protobuf.Struct option> =
        lens.ComposeWith(Test.Name.Space.Optics.NestedStructTest.``properties``)
    [<Extension>]
    static member inline Properties(traversal : ITraversal<'a,'b,Test.Name.Space.NestedStructTest,Test.Name.Space.NestedStructTest>) : ITraversal<'a,'b,Google.Protobuf.Struct option,Google.Protobuf.Struct option> =
        traversal.ComposeWith(Test.Name.Space.Optics.NestedStructTest.``properties``)
    [<Extension>]
    static member inline Values(lens : ILens<'a,'b,Test.Name.Space.NestedStructTest,Test.Name.Space.NestedStructTest>) : ILens<'a,'b,Google.Protobuf.Value list,Google.Protobuf.Value list> =
        lens.ComposeWith(Test.Name.Space.Optics.NestedStructTest.``values``)
    [<Extension>]
    static member inline Values(traversal : ITraversal<'a,'b,Test.Name.Space.NestedStructTest,Test.Name.Space.NestedStructTest>) : ITraversal<'a,'b,Google.Protobuf.Value list,Google.Protobuf.Value list> =
        traversal.ComposeWith(Test.Name.Space.Optics.NestedStructTest.``values``)


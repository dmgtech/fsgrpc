dotnet build protoc-gen-fsgrpc
cd FsGrpc\FsGrpc.Tests
buf generate .\testproto.proto
cd ..\..
dotnet build FsGrpc
dotnet build FsGrpc\FsGrpc.Tests
dotnet test FsGrpc\FsGrpc.Tests
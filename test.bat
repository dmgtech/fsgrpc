dotnet build protoc-gen-fsgrpc
cd FsGrpc\FsGrpc.Tests
buf generate 
cd ..\..
cd protoc-gen-fsgrpc\Tests
buf generate 
cd ..\..
dotnet build FsGrpc
dotnet build FsGrpc\FsGrpc.Tests
dotnet test FsGrpc\FsGrpc.Tests
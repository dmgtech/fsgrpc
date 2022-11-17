dotnet build protoc-gen-fsgrpc
cd FsGrpc\FsGrpc.Tests
buf generate --include-imports --include-wkt
cd ..\..
cd protoc-gen-fsgrpc\Tests
buf generate
cd ..\..
dotnet build FsGrpc
dotnet build FsGrpc\FsGrpc.Tests
dotnet test FsGrpc\FsGrpc.Tests
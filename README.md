
# FsGrpc
Idiomatic F# code generation for Protocol Buffers and gRPC

> *⚠️ this is currently a work in progress.  See the "Status" section
> for more info*

Generate idiomatic F# records from proto3 message definitions, complete with oneofs as discriminated unions, and serialize/deserialize to and from protocol buffer wire format.

# System Diagram
```plantuml
' Documentation for C4 diagrams is here: https://github.com/plantuml-stdlib/C4-PlantUML#including-the-c4-plantuml-library
@startuml Basic Sample
!include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Context.puml
!include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Component.puml
HIDE_STEREOTYPE()

Boundary(c1, "Developer Workstation", $link="https://github.com/plantuml-stdlib/C4-PlantUML") {
    Container(your_app, "Web Application"){
        Component(generatedcode, "Generated F# Protobuf Code", "F# representations of your protobuf schema")
    }
    Component(protoc_fsgrpc_plugin_local, "Protoc FsGrpc Plugin", "local protoc-gen-fsgrpc")
}
Boundary(buf, "buf.build"){
    Component(protoc_fsgrpc_plugin, "Protoc FsGrpc Plugin", "Hosted protoc-gen-fsgrpc as a service")
}

Boundary(fsgrpc_repository, "FsGrpc Repository"){
   Boundary(protoc_component, "protoc FsGrpc Plugin"){
       Component(protoc_gen_fsgrpc,"protoc-gen-fsgrpc", "Generates F# representations of your protobuf schema")
       Component(protoc_gen_fsgrpc_tests, "Tests")
   }
   Boundary(fsgrpc_component, "FsGrpc"){
       Component(fsgrpc, "FsGrpc", "Support Library for generated code")
       Component(fsgrpc_tests, "FsGrpc.Tests")
   }
}

Boundary(nuget, "Nuget"){
    Component(fsgrpc_nuget, "FsGrpc Package", "FsGrpc as a nuget package")
}

Rel(generatedcode, fsgrpc, "depends on")
Rel(fsgrpc, fsgrpc_nuget, "Publishes")
Rel(protoc_gen_fsgrpc, protoc_fsgrpc_plugin, "Publishes")
Rel(protoc_gen_fsgrpc, protoc_fsgrpc_plugin_local, "Publishes (for local-only builds)")
Rel(protoc_fsgrpc_plugin, generatedcode, "generates")
Rel(protoc_fsgrpc_plugin_local, generatedcode, "generates")
Rel(protoc_gen_fsgrpc, fsgrpc, "references")
Rel(fsgrpc_tests, fsgrpc, "references")
Rel(protoc_gen_fsgrpc_tests, protoc_gen_fsgrpc, "references")
Rel(your_app, fsgrpc_nuget, "references")

@enduml
```

## Getting Started

There are a couple of approaches you can use to generate code from your protos.

### Option 1: Use the buf cli

1. If using buf (or if you just want to use the buf cli), start by installing the buf cli from https://docs.buf.build/installation.

2. Add the following "remote" line to your buf.gen.yaml (or create a new buf.gen.yaml if one doesn't already exist):
```yaml
version: v1
plugins:
  - remote: buf.build/divisions-maintenance-group/plugins/fsharp
    out: gen
```

3. Generate the code

	- Option 1:
		1. Place your protos in a folder named "protos" in the same folder as your buf.gen.yaml file.

		2. If your protos are in buf, you can export them into that folder by running:
	       `buf export buf.build/path/to/your/protos -o protos`

	       e.g. `buf export buf.build/googleapis/googleapis -o protos` will place the proto defintions for the google apis into the protos folder

		3. Run `buf generate protos --include-imports --include-wkt` in the folder where your buf.gen.yaml file is located.

	- Option 2:
		1. Run buf generate command directly referencing protos in buf
        `buf generate buf.build/googleapis/googleapis`

4. Add the generated code to your F# project
Using `buf generate` with the above example will generate .fs files in the "gen" directory, and also a Protobuf.targets file in that directory which includes those files in correct dependency order.

You then add the following line to your .fsproj in the top-level "project" element:
```xml
<Import Project="gen/Protobuf.targets" />
```

And run
```bash
dotnet add package fsgrpc --prerelease
```
or
```powershell
Install-Package FsGrpc -Version -IncludePrerelease
```

## Usage in F#

You can create a record by specifying all of the fields or using `with` syntax as follows:

```fsharp
let message =
	{ MyMessage.empty with
	    Name = "a name value"
	    Description = "some string here" }
```

Serializing a message to bytes looks like this:
```fsharp
let bytes = message |> FsGrpc.Protobuf.encode
```

And deserializing looks like this:
```fsharp
let message: MyMessage = bytes |> FsGrpc.Protobuf.decode
```

You can also serialize/deserialize from a CodedOutputStream/CodedInputStream using:
```fsharp
// decode from a CodedInputStream named cis
let message = MyMessage.Proto.Decode cis

// encode to a CodedOutputStream named cos
MyMessage.Proto.Encode cos message
```



## Status
Note: This is currently a work in progress.  Code generation for protocol buffers is currently working but considered an alpha version.  gRPC and other features (such as code comments and reflection) are not complete.

The major features intended are:
- [x] Protobuf Messages as immutable F# record types
- [x] Oneofs as Discriminated Unions
- [x] proto3 optional keyword support
- [x] Support for optional wrapper types (e.g. google.protobuf.UInt32Val)
- [x] Support for well-known types Duration and Timestamp (represented using NodaTime types)
- [x] Automatic dependency-sorted inclusion of generated .fs files
- [x] Buf.build integration
- [x] Comment pass-through
- [ ] Protocol Buffer reflection
- [x] Idiomatic functional implementation for gRPC endpoints



namespace rec Ex.Ample.Importable
open FsGrpc.Protobuf
#nowarn "40"


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Imported =

    [<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.EnumConverter<EnumForImport>>)>]
    type EnumForImport =
    | [<FsGrpc.Protobuf.ProtobufName("ENUM_FOR_IMPORT_NO")>] No = 0
    | [<FsGrpc.Protobuf.ProtobufName("ENUM_FOR_IMPORT_YES")>] Yes = 1

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: string // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.String.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Ex.Ample.Importable.Imported = {
            Value = x.Value |> orEmptyString
            }

/// <summary>
/// This comment had a tag associated
/// which should be removed
/// </summary>
type private _Imported = Imported
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
type Imported = {
    // Field Declarations
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: string // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<Imported>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.String (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<Imported>
            Name = "Imported"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: Imported) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: Imported) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Ex.Ample.Importable.Imported.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: Imported) =
                    writeValue w m.Value
                encode
        }
    static member empty
        with get() = Ex.Ample.Importable._Imported.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Args =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: string // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.String.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Ex.Ample.Importable.Args = {
            Value = x.Value |> orEmptyString
            }

type private _Args = Args
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
type Args = {
    // Field Declarations
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: string // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<Args>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.String (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<Args>
            Name = "Args"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: Args) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: Args) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Ex.Ample.Importable.Args.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: Args) =
                    writeValue w m.Value
                encode
        }
    static member empty
        with get() = Ex.Ample.Importable._Args.Proto.Value.Empty

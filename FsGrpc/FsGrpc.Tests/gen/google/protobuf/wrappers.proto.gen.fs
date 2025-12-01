namespace rec Google.Protobuf
open FsGrpc.Protobuf
open Google.Protobuf
#nowarn "40"
#nowarn "1182"


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module DoubleValue =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: double // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.Double.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.DoubleValue = {
            Value = x.Value
            }

/// <summary>
/// Wrapper message for `double`.
/// 
/// The JSON representation for `DoubleValue` is JSON number.
/// 
/// Not recommended for use in new APIs, but still useful for legacy APIs and
/// has no plan to be removed.
/// </summary>
type private _DoubleValue = DoubleValue
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type DoubleValue = {
    // Field Declarations
    /// <summary>The double value.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: double // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<DoubleValue>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.Double (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<DoubleValue>
            Name = "DoubleValue"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: DoubleValue) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: DoubleValue) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.DoubleValue.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: DoubleValue) =
                    writeValue w m.Value
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : DoubleValue =
                    match kvPair.Key with
                    | "value" -> { value with Value = Value.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _DoubleValue.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._DoubleValue.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FloatValue =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: float32 // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.Float.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.FloatValue = {
            Value = x.Value
            }

/// <summary>
/// Wrapper message for `float`.
/// 
/// The JSON representation for `FloatValue` is JSON number.
/// 
/// Not recommended for use in new APIs, but still useful for legacy APIs and
/// has no plan to be removed.
/// </summary>
type private _FloatValue = FloatValue
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type FloatValue = {
    // Field Declarations
    /// <summary>The float value.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: float32 // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<FloatValue>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.Float (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<FloatValue>
            Name = "FloatValue"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: FloatValue) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: FloatValue) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.FloatValue.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: FloatValue) =
                    writeValue w m.Value
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : FloatValue =
                    match kvPair.Key with
                    | "value" -> { value with Value = Value.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _FloatValue.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._FloatValue.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Int64Value =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: int64 // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.Int64.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.Int64Value = {
            Value = x.Value
            }

/// <summary>
/// Wrapper message for `int64`.
/// 
/// The JSON representation for `Int64Value` is JSON string.
/// 
/// Not recommended for use in new APIs, but still useful for legacy APIs and
/// has no plan to be removed.
/// </summary>
type private _Int64Value = Int64Value
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type Int64Value = {
    // Field Declarations
    /// <summary>The int64 value.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: int64 // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<Int64Value>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.Int64 (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<Int64Value>
            Name = "Int64Value"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: Int64Value) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: Int64Value) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.Int64Value.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: Int64Value) =
                    writeValue w m.Value
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : Int64Value =
                    match kvPair.Key with
                    | "value" -> { value with Value = Value.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _Int64Value.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._Int64Value.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module UInt64Value =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: uint64 // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.UInt64.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.UInt64Value = {
            Value = x.Value
            }

/// <summary>
/// Wrapper message for `uint64`.
/// 
/// The JSON representation for `UInt64Value` is JSON string.
/// 
/// Not recommended for use in new APIs, but still useful for legacy APIs and
/// has no plan to be removed.
/// </summary>
type private _UInt64Value = UInt64Value
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type UInt64Value = {
    // Field Declarations
    /// <summary>The uint64 value.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: uint64 // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<UInt64Value>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.UInt64 (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<UInt64Value>
            Name = "UInt64Value"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: UInt64Value) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: UInt64Value) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.UInt64Value.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: UInt64Value) =
                    writeValue w m.Value
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : UInt64Value =
                    match kvPair.Key with
                    | "value" -> { value with Value = Value.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _UInt64Value.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._UInt64Value.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Int32Value =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: int // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.Int32.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.Int32Value = {
            Value = x.Value
            }

/// <summary>
/// Wrapper message for `int32`.
/// 
/// The JSON representation for `Int32Value` is JSON number.
/// 
/// Not recommended for use in new APIs, but still useful for legacy APIs and
/// has no plan to be removed.
/// </summary>
type private _Int32Value = Int32Value
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type Int32Value = {
    // Field Declarations
    /// <summary>The int32 value.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: int // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<Int32Value>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.Int32 (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<Int32Value>
            Name = "Int32Value"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: Int32Value) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: Int32Value) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.Int32Value.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: Int32Value) =
                    writeValue w m.Value
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : Int32Value =
                    match kvPair.Key with
                    | "value" -> { value with Value = Value.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _Int32Value.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._Int32Value.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module UInt32Value =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: uint32 // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.UInt32.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.UInt32Value = {
            Value = x.Value
            }

/// <summary>
/// Wrapper message for `uint32`.
/// 
/// The JSON representation for `UInt32Value` is JSON number.
/// 
/// Not recommended for use in new APIs, but still useful for legacy APIs and
/// has no plan to be removed.
/// </summary>
type private _UInt32Value = UInt32Value
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type UInt32Value = {
    // Field Declarations
    /// <summary>The uint32 value.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: uint32 // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<UInt32Value>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.UInt32 (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<UInt32Value>
            Name = "UInt32Value"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: UInt32Value) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: UInt32Value) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.UInt32Value.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: UInt32Value) =
                    writeValue w m.Value
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : UInt32Value =
                    match kvPair.Key with
                    | "value" -> { value with Value = Value.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _UInt32Value.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._UInt32Value.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module BoolValue =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: bool // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.Bool.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.BoolValue = {
            Value = x.Value
            }

/// <summary>
/// Wrapper message for `bool`.
/// 
/// The JSON representation for `BoolValue` is JSON `true` and `false`.
/// 
/// Not recommended for use in new APIs, but still useful for legacy APIs and
/// has no plan to be removed.
/// </summary>
type private _BoolValue = BoolValue
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type BoolValue = {
    // Field Declarations
    /// <summary>The bool value.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: bool // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<BoolValue>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.Bool (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<BoolValue>
            Name = "BoolValue"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: BoolValue) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: BoolValue) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.BoolValue.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: BoolValue) =
                    writeValue w m.Value
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : BoolValue =
                    match kvPair.Key with
                    | "value" -> { value with Value = Value.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _BoolValue.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._BoolValue.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module StringValue =

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
        member x.Build : Google.Protobuf.StringValue = {
            Value = x.Value |> orEmptyString
            }

/// <summary>
/// Wrapper message for `string`.
/// 
/// The JSON representation for `StringValue` is JSON string.
/// 
/// Not recommended for use in new APIs, but still useful for legacy APIs and
/// has no plan to be removed.
/// </summary>
type private _StringValue = StringValue
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type StringValue = {
    // Field Declarations
    /// <summary>The string value.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: string // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<StringValue>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.String (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<StringValue>
            Name = "StringValue"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: StringValue) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: StringValue) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.StringValue.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: StringValue) =
                    writeValue w m.Value
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : StringValue =
                    match kvPair.Key with
                    | "value" -> { value with Value = Value.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _StringValue.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._StringValue.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module BytesValue =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Value: FsGrpc.Bytes // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Value <- ValueCodec.Bytes.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.BytesValue = {
            Value = x.Value
            }

/// <summary>
/// Wrapper message for `bytes`.
/// 
/// The JSON representation for `BytesValue` is JSON string.
/// 
/// Not recommended for use in new APIs, but still useful for legacy APIs and
/// has no plan to be removed.
/// </summary>
type private _BytesValue = BytesValue
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type BytesValue = {
    // Field Declarations
    /// <summary>The bytes value.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("value")>] Value: FsGrpc.Bytes // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<BytesValue>> =
        lazy
        // Field Definitions
        let Value = FieldCodec.Primitive ValueCodec.Bytes (1, "value")
        // Proto Definition Implementation
        { // ProtoDef<BytesValue>
            Name = "BytesValue"
            Empty = {
                Value = Value.GetDefault()
                }
            Size = fun (m: BytesValue) ->
                0
                + Value.CalcFieldSize m.Value
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: BytesValue) ->
                Value.WriteField w m.Value
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.BytesValue.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValue = Value.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: BytesValue) =
                    writeValue w m.Value
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : BytesValue =
                    match kvPair.Key with
                    | "value" -> { value with Value = Value.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _BytesValue.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._BytesValue.Proto.Value.Empty

namespace Google.Protobuf.Optics
open Focal.Core
module DoubleValue =
    let ``value`` : ILens'<Google.Protobuf.DoubleValue,double> =
        {
            _getter = { _get = fun (s: Google.Protobuf.DoubleValue) -> s.Value }
            _setter = { _over = fun a2b s -> { s with Value = a2b s.Value } }
        }
module FloatValue =
    let ``value`` : ILens'<Google.Protobuf.FloatValue,float32> =
        {
            _getter = { _get = fun (s: Google.Protobuf.FloatValue) -> s.Value }
            _setter = { _over = fun a2b s -> { s with Value = a2b s.Value } }
        }
module Int64Value =
    let ``value`` : ILens'<Google.Protobuf.Int64Value,int64> =
        {
            _getter = { _get = fun (s: Google.Protobuf.Int64Value) -> s.Value }
            _setter = { _over = fun a2b s -> { s with Value = a2b s.Value } }
        }
module UInt64Value =
    let ``value`` : ILens'<Google.Protobuf.UInt64Value,uint64> =
        {
            _getter = { _get = fun (s: Google.Protobuf.UInt64Value) -> s.Value }
            _setter = { _over = fun a2b s -> { s with Value = a2b s.Value } }
        }
module Int32Value =
    let ``value`` : ILens'<Google.Protobuf.Int32Value,int> =
        {
            _getter = { _get = fun (s: Google.Protobuf.Int32Value) -> s.Value }
            _setter = { _over = fun a2b s -> { s with Value = a2b s.Value } }
        }
module UInt32Value =
    let ``value`` : ILens'<Google.Protobuf.UInt32Value,uint32> =
        {
            _getter = { _get = fun (s: Google.Protobuf.UInt32Value) -> s.Value }
            _setter = { _over = fun a2b s -> { s with Value = a2b s.Value } }
        }
module BoolValue =
    let ``value`` : ILens'<Google.Protobuf.BoolValue,bool> =
        {
            _getter = { _get = fun (s: Google.Protobuf.BoolValue) -> s.Value }
            _setter = { _over = fun a2b s -> { s with Value = a2b s.Value } }
        }
module StringValue =
    let ``value`` : ILens'<Google.Protobuf.StringValue,string> =
        {
            _getter = { _get = fun (s: Google.Protobuf.StringValue) -> s.Value }
            _setter = { _over = fun a2b s -> { s with Value = a2b s.Value } }
        }
module BytesValue =
    let ``value`` : ILens'<Google.Protobuf.BytesValue,FsGrpc.Bytes> =
        {
            _getter = { _get = fun (s: Google.Protobuf.BytesValue) -> s.Value }
            _setter = { _over = fun a2b s -> { s with Value = a2b s.Value } }
        }

namespace Google.Protobuf
open Focal.Core
open System.Runtime.CompilerServices
[<Extension>]
type OpticsExtensionMethods_google_protobuf_wrappers_proto =
    [<Extension>]
    static member inline Value(lens : ILens<'a,'b,Google.Protobuf.DoubleValue,Google.Protobuf.DoubleValue>) : ILens<'a,'b,double,double> =
        lens.ComposeWith(Google.Protobuf.Optics.DoubleValue.``value``)
    [<Extension>]
    static member inline Value(traversal : ITraversal<'a,'b,Google.Protobuf.DoubleValue,Google.Protobuf.DoubleValue>) : ITraversal<'a,'b,double,double> =
        traversal.ComposeWith(Google.Protobuf.Optics.DoubleValue.``value``)
    [<Extension>]
    static member inline Value(lens : ILens<'a,'b,Google.Protobuf.FloatValue,Google.Protobuf.FloatValue>) : ILens<'a,'b,float32,float32> =
        lens.ComposeWith(Google.Protobuf.Optics.FloatValue.``value``)
    [<Extension>]
    static member inline Value(traversal : ITraversal<'a,'b,Google.Protobuf.FloatValue,Google.Protobuf.FloatValue>) : ITraversal<'a,'b,float32,float32> =
        traversal.ComposeWith(Google.Protobuf.Optics.FloatValue.``value``)
    [<Extension>]
    static member inline Value(lens : ILens<'a,'b,Google.Protobuf.Int64Value,Google.Protobuf.Int64Value>) : ILens<'a,'b,int64,int64> =
        lens.ComposeWith(Google.Protobuf.Optics.Int64Value.``value``)
    [<Extension>]
    static member inline Value(traversal : ITraversal<'a,'b,Google.Protobuf.Int64Value,Google.Protobuf.Int64Value>) : ITraversal<'a,'b,int64,int64> =
        traversal.ComposeWith(Google.Protobuf.Optics.Int64Value.``value``)
    [<Extension>]
    static member inline Value(lens : ILens<'a,'b,Google.Protobuf.UInt64Value,Google.Protobuf.UInt64Value>) : ILens<'a,'b,uint64,uint64> =
        lens.ComposeWith(Google.Protobuf.Optics.UInt64Value.``value``)
    [<Extension>]
    static member inline Value(traversal : ITraversal<'a,'b,Google.Protobuf.UInt64Value,Google.Protobuf.UInt64Value>) : ITraversal<'a,'b,uint64,uint64> =
        traversal.ComposeWith(Google.Protobuf.Optics.UInt64Value.``value``)
    [<Extension>]
    static member inline Value(lens : ILens<'a,'b,Google.Protobuf.Int32Value,Google.Protobuf.Int32Value>) : ILens<'a,'b,int,int> =
        lens.ComposeWith(Google.Protobuf.Optics.Int32Value.``value``)
    [<Extension>]
    static member inline Value(traversal : ITraversal<'a,'b,Google.Protobuf.Int32Value,Google.Protobuf.Int32Value>) : ITraversal<'a,'b,int,int> =
        traversal.ComposeWith(Google.Protobuf.Optics.Int32Value.``value``)
    [<Extension>]
    static member inline Value(lens : ILens<'a,'b,Google.Protobuf.UInt32Value,Google.Protobuf.UInt32Value>) : ILens<'a,'b,uint32,uint32> =
        lens.ComposeWith(Google.Protobuf.Optics.UInt32Value.``value``)
    [<Extension>]
    static member inline Value(traversal : ITraversal<'a,'b,Google.Protobuf.UInt32Value,Google.Protobuf.UInt32Value>) : ITraversal<'a,'b,uint32,uint32> =
        traversal.ComposeWith(Google.Protobuf.Optics.UInt32Value.``value``)
    [<Extension>]
    static member inline Value(lens : ILens<'a,'b,Google.Protobuf.BoolValue,Google.Protobuf.BoolValue>) : ILens<'a,'b,bool,bool> =
        lens.ComposeWith(Google.Protobuf.Optics.BoolValue.``value``)
    [<Extension>]
    static member inline Value(traversal : ITraversal<'a,'b,Google.Protobuf.BoolValue,Google.Protobuf.BoolValue>) : ITraversal<'a,'b,bool,bool> =
        traversal.ComposeWith(Google.Protobuf.Optics.BoolValue.``value``)
    [<Extension>]
    static member inline Value(lens : ILens<'a,'b,Google.Protobuf.StringValue,Google.Protobuf.StringValue>) : ILens<'a,'b,string,string> =
        lens.ComposeWith(Google.Protobuf.Optics.StringValue.``value``)
    [<Extension>]
    static member inline Value(traversal : ITraversal<'a,'b,Google.Protobuf.StringValue,Google.Protobuf.StringValue>) : ITraversal<'a,'b,string,string> =
        traversal.ComposeWith(Google.Protobuf.Optics.StringValue.``value``)
    [<Extension>]
    static member inline Value(lens : ILens<'a,'b,Google.Protobuf.BytesValue,Google.Protobuf.BytesValue>) : ILens<'a,'b,FsGrpc.Bytes,FsGrpc.Bytes> =
        lens.ComposeWith(Google.Protobuf.Optics.BytesValue.``value``)
    [<Extension>]
    static member inline Value(traversal : ITraversal<'a,'b,Google.Protobuf.BytesValue,Google.Protobuf.BytesValue>) : ITraversal<'a,'b,FsGrpc.Bytes,FsGrpc.Bytes> =
        traversal.ComposeWith(Google.Protobuf.Optics.BytesValue.``value``)


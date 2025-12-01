namespace rec Google.Protobuf
open FsGrpc.Protobuf
open Google.Protobuf
#nowarn "40"
#nowarn "1182"


/// <summary>
/// `NullValue` is a singleton enumeration to represent the null value for the
/// `Value` type union.
/// 
/// The JSON representation for `NullValue` is JSON `null`.
/// </summary>
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.EnumConverter<NullValue>>)>]
type NullValue =
/// <summary>Null value.</summary>
| [<FsGrpc.Protobuf.ProtobufName("NULL_VALUE")>] NullValue = 0

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Struct =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Fields: MapBuilder<string, Google.Protobuf.Value> // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Fields.Add ((ValueCodec.MapRecord ValueCodec.String ValueCodec.Message<Google.Protobuf.Value>).ReadValue reader)
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.Struct = {
            Fields = x.Fields.Build
            }

/// <summary>
/// `Struct` represents a structured data value, consisting of fields
/// which map to dynamically typed values. In some languages, `Struct`
/// might be supported by a native representation. For example, in
/// scripting languages like JS a struct is represented as an
/// object. The details of that representation are described together
/// with the proto support for the language.
/// 
/// The JSON representation for `Struct` is JSON object.
/// </summary>
type private _Struct = Struct
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type Struct = {
    // Field Declarations
    /// <summary>Unordered map of dynamically typed values.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("fields")>] Fields: Map<string, Google.Protobuf.Value> // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<Struct>> =
        lazy
        // Field Definitions
        let Fields = FieldCodec.Map ValueCodec.String ValueCodec.Message<Google.Protobuf.Value> (1, "fields")
        // Proto Definition Implementation
        { // ProtoDef<Struct>
            Name = "Struct"
            Empty = {
                Fields = Fields.GetDefault()
                }
            Size = fun (m: Struct) ->
                0
                + Fields.CalcFieldSize m.Fields
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: Struct) ->
                Fields.WriteField w m.Fields
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.Struct.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeFields = Fields.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: Struct) =
                    writeFields w m.Fields
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : Struct =
                    match kvPair.Key with
                    | "fields" -> { value with Fields = Fields.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _Struct.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._Struct.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Value =

    [<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.OneofConverter<KindCase>>)>]
    [<CompilationRepresentation(CompilationRepresentationFlags.UseNullAsTrueValue)>]
    [<StructuralEquality;StructuralComparison>]
    [<RequireQualifiedAccess>]
    type KindCase =
    | None
    /// <summary>Represents a null value.</summary>
    | [<System.Text.Json.Serialization.JsonPropertyName("nullValue")>] NullValue of Google.Protobuf.NullValue
    /// <summary>Represents a double value.</summary>
    | [<System.Text.Json.Serialization.JsonPropertyName("numberValue")>] NumberValue of double
    /// <summary>Represents a string value.</summary>
    | [<System.Text.Json.Serialization.JsonPropertyName("stringValue")>] StringValue of string
    /// <summary>Represents a boolean value.</summary>
    | [<System.Text.Json.Serialization.JsonPropertyName("boolValue")>] BoolValue of bool
    /// <summary>Represents a structured value.</summary>
    | [<System.Text.Json.Serialization.JsonPropertyName("structValue")>] StructValue of Google.Protobuf.Struct
    /// <summary>Represents a repeated `Value`.</summary>
    | [<System.Text.Json.Serialization.JsonPropertyName("listValue")>] ListValue of Google.Protobuf.ListValue
    with
        static member OneofCodec : Lazy<OneofCodec<KindCase>> = 
            lazy
            let NullValue = FieldCodec.OneofCase "kind" ValueCodec.Enum<Google.Protobuf.NullValue> (1, "nullValue")
            let NumberValue = FieldCodec.OneofCase "kind" ValueCodec.Double (2, "numberValue")
            let StringValue = FieldCodec.OneofCase "kind" ValueCodec.String (3, "stringValue")
            let BoolValue = FieldCodec.OneofCase "kind" ValueCodec.Bool (4, "boolValue")
            let StructValue = FieldCodec.OneofCase "kind" ValueCodec.Message<Google.Protobuf.Struct> (5, "structValue")
            let ListValue = FieldCodec.OneofCase "kind" ValueCodec.Message<Google.Protobuf.ListValue> (6, "listValue")
            let Kind = FieldCodec.Oneof "kind" (FSharp.Collections.Map [
                ("nullValue", fun node -> KindCase.NullValue (NullValue.ReadJsonField node))
                ("numberValue", fun node -> KindCase.NumberValue (NumberValue.ReadJsonField node))
                ("stringValue", fun node -> KindCase.StringValue (StringValue.ReadJsonField node))
                ("boolValue", fun node -> KindCase.BoolValue (BoolValue.ReadJsonField node))
                ("structValue", fun node -> KindCase.StructValue (StructValue.ReadJsonField node))
                ("listValue", fun node -> KindCase.ListValue (ListValue.ReadJsonField node))
                ])
            Kind

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Kind: OptionBuilder<Google.Protobuf.Value.KindCase>
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Kind.Set (KindCase.NullValue (ValueCodec.Enum<Google.Protobuf.NullValue>.ReadValue reader))
            | 2 -> x.Kind.Set (KindCase.NumberValue (ValueCodec.Double.ReadValue reader))
            | 3 -> x.Kind.Set (KindCase.StringValue (ValueCodec.String.ReadValue reader))
            | 4 -> x.Kind.Set (KindCase.BoolValue (ValueCodec.Bool.ReadValue reader))
            | 5 -> x.Kind.Set (KindCase.StructValue (ValueCodec.Message<Google.Protobuf.Struct>.ReadValue reader))
            | 6 -> x.Kind.Set (KindCase.ListValue (ValueCodec.Message<Google.Protobuf.ListValue>.ReadValue reader))
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.Value = {
            Kind = x.Kind.Build |> (Option.defaultValue KindCase.None)
            }

/// <summary>
/// `Value` represents a dynamically typed value which can be either
/// null, a number, a string, a boolean, a recursive struct value, or a
/// list of values. A producer of value is expected to set one of these
/// variants. Absence of any variant indicates an error.
/// 
/// The JSON representation for `Value` is JSON value.
/// </summary>
type private _Value = Value
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type Value = {
    // Field Declarations
    /// <summary>The kind of value.</summary>
    Kind: Google.Protobuf.Value.KindCase
    }
    with
    static member Proto : Lazy<ProtoDef<Value>> =
        lazy
        // Field Definitions
        let NullValue = FieldCodec.OneofCase "kind" ValueCodec.Enum<Google.Protobuf.NullValue> (1, "nullValue")
        let NumberValue = FieldCodec.OneofCase "kind" ValueCodec.Double (2, "numberValue")
        let StringValue = FieldCodec.OneofCase "kind" ValueCodec.String (3, "stringValue")
        let BoolValue = FieldCodec.OneofCase "kind" ValueCodec.Bool (4, "boolValue")
        let StructValue = FieldCodec.OneofCase "kind" ValueCodec.Message<Google.Protobuf.Struct> (5, "structValue")
        let ListValue = FieldCodec.OneofCase "kind" ValueCodec.Message<Google.Protobuf.ListValue> (6, "listValue")
        let Kind = FieldCodec.Oneof "kind" (FSharp.Collections.Map [
            ("nullValue", fun node -> Google.Protobuf.Value.KindCase.NullValue (NullValue.ReadJsonField node))
            ("numberValue", fun node -> Google.Protobuf.Value.KindCase.NumberValue (NumberValue.ReadJsonField node))
            ("stringValue", fun node -> Google.Protobuf.Value.KindCase.StringValue (StringValue.ReadJsonField node))
            ("boolValue", fun node -> Google.Protobuf.Value.KindCase.BoolValue (BoolValue.ReadJsonField node))
            ("structValue", fun node -> Google.Protobuf.Value.KindCase.StructValue (StructValue.ReadJsonField node))
            ("listValue", fun node -> Google.Protobuf.Value.KindCase.ListValue (ListValue.ReadJsonField node))
            ])
        // Proto Definition Implementation
        { // ProtoDef<Value>
            Name = "Value"
            Empty = {
                Kind = Google.Protobuf.Value.KindCase.None
                }
            Size = fun (m: Value) ->
                0
                + match m.Kind with
                    | Google.Protobuf.Value.KindCase.None -> 0
                    | Google.Protobuf.Value.KindCase.NullValue v -> NullValue.CalcFieldSize v
                    | Google.Protobuf.Value.KindCase.NumberValue v -> NumberValue.CalcFieldSize v
                    | Google.Protobuf.Value.KindCase.StringValue v -> StringValue.CalcFieldSize v
                    | Google.Protobuf.Value.KindCase.BoolValue v -> BoolValue.CalcFieldSize v
                    | Google.Protobuf.Value.KindCase.StructValue v -> StructValue.CalcFieldSize v
                    | Google.Protobuf.Value.KindCase.ListValue v -> ListValue.CalcFieldSize v
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: Value) ->
                (match m.Kind with
                | Google.Protobuf.Value.KindCase.None -> ()
                | Google.Protobuf.Value.KindCase.NullValue v -> NullValue.WriteField w v
                | Google.Protobuf.Value.KindCase.NumberValue v -> NumberValue.WriteField w v
                | Google.Protobuf.Value.KindCase.StringValue v -> StringValue.WriteField w v
                | Google.Protobuf.Value.KindCase.BoolValue v -> BoolValue.WriteField w v
                | Google.Protobuf.Value.KindCase.StructValue v -> StructValue.WriteField w v
                | Google.Protobuf.Value.KindCase.ListValue v -> ListValue.WriteField w v
                )
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.Value.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeKindNone = Kind.WriteJsonNoneCase o
                let writeNullValue = NullValue.WriteJsonField o
                let writeNumberValue = NumberValue.WriteJsonField o
                let writeStringValue = StringValue.WriteJsonField o
                let writeBoolValue = BoolValue.WriteJsonField o
                let writeStructValue = StructValue.WriteJsonField o
                let writeListValue = ListValue.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: Value) =
                    (match m.Kind with
                    | Google.Protobuf.Value.KindCase.None -> writeKindNone w
                    | Google.Protobuf.Value.KindCase.NullValue v -> writeNullValue w v
                    | Google.Protobuf.Value.KindCase.NumberValue v -> writeNumberValue w v
                    | Google.Protobuf.Value.KindCase.StringValue v -> writeStringValue w v
                    | Google.Protobuf.Value.KindCase.BoolValue v -> writeBoolValue w v
                    | Google.Protobuf.Value.KindCase.StructValue v -> writeStructValue w v
                    | Google.Protobuf.Value.KindCase.ListValue v -> writeListValue w v
                    )
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : Value =
                    match kvPair.Key with
                    | "nullValue" -> { value with Kind = Google.Protobuf.Value.KindCase.NullValue (NullValue.ReadJsonField kvPair.Value) }
                    | "numberValue" -> { value with Kind = Google.Protobuf.Value.KindCase.NumberValue (NumberValue.ReadJsonField kvPair.Value) }
                    | "stringValue" -> { value with Kind = Google.Protobuf.Value.KindCase.StringValue (StringValue.ReadJsonField kvPair.Value) }
                    | "boolValue" -> { value with Kind = Google.Protobuf.Value.KindCase.BoolValue (BoolValue.ReadJsonField kvPair.Value) }
                    | "structValue" -> { value with Kind = Google.Protobuf.Value.KindCase.StructValue (StructValue.ReadJsonField kvPair.Value) }
                    | "listValue" -> { value with Kind = Google.Protobuf.Value.KindCase.ListValue (ListValue.ReadJsonField kvPair.Value) }
                    | "kind" -> { value with Kind = Kind.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _Value.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._Value.Proto.Value.Empty

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ListValue =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Values: RepeatedBuilder<Google.Protobuf.Value> // (1)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Values.Add (ValueCodec.Message<Google.Protobuf.Value>.ReadValue reader)
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.ListValue = {
            Values = x.Values.Build
            }

/// <summary>
/// `ListValue` is a wrapper around a repeated field of values.
/// 
/// The JSON representation for `ListValue` is JSON array.
/// </summary>
type private _ListValue = ListValue
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type ListValue = {
    // Field Declarations
    /// <summary>Repeated field of dynamically typed values.</summary>
    [<System.Text.Json.Serialization.JsonPropertyName("values")>] Values: Google.Protobuf.Value list // (1)
    }
    with
    static member Proto : Lazy<ProtoDef<ListValue>> =
        lazy
        // Field Definitions
        let Values = FieldCodec.Repeated ValueCodec.Message<Google.Protobuf.Value> (1, "values")
        // Proto Definition Implementation
        { // ProtoDef<ListValue>
            Name = "ListValue"
            Empty = {
                Values = Values.GetDefault()
                }
            Size = fun (m: ListValue) ->
                0
                + Values.CalcFieldSize m.Values
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: ListValue) ->
                Values.WriteField w m.Values
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.ListValue.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeValues = Values.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: ListValue) =
                    writeValues w m.Values
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : ListValue =
                    match kvPair.Key with
                    | "values" -> { value with Values = Values.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _ListValue.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._ListValue.Proto.Value.Empty

namespace Google.Protobuf.Optics
open Focal.Core
module Struct =
    let ``fields`` : ILens'<Google.Protobuf.Struct,Map<string, Google.Protobuf.Value>> =
        {
            _getter = { _get = fun (s: Google.Protobuf.Struct) -> s.Fields }
            _setter = { _over = fun a2b s -> { s with Fields = a2b s.Fields } }
        }
module Value =
    module KindPrisms =
        let ifNullValue : IPrism'<Google.Protobuf.Value.KindCase,Google.Protobuf.NullValue> =
            {
                _unto = fun a -> Google.Protobuf.Value.KindCase.NullValue a
                _which = fun s ->
                    match s with
                    | Google.Protobuf.Value.KindCase.NullValue a -> Ok a
                    | _ -> Error s
            }
        let ifNumberValue : IPrism'<Google.Protobuf.Value.KindCase,double> =
            {
                _unto = fun a -> Google.Protobuf.Value.KindCase.NumberValue a
                _which = fun s ->
                    match s with
                    | Google.Protobuf.Value.KindCase.NumberValue a -> Ok a
                    | _ -> Error s
            }
        let ifStringValue : IPrism'<Google.Protobuf.Value.KindCase,string> =
            {
                _unto = fun a -> Google.Protobuf.Value.KindCase.StringValue a
                _which = fun s ->
                    match s with
                    | Google.Protobuf.Value.KindCase.StringValue a -> Ok a
                    | _ -> Error s
            }
        let ifBoolValue : IPrism'<Google.Protobuf.Value.KindCase,bool> =
            {
                _unto = fun a -> Google.Protobuf.Value.KindCase.BoolValue a
                _which = fun s ->
                    match s with
                    | Google.Protobuf.Value.KindCase.BoolValue a -> Ok a
                    | _ -> Error s
            }
        let ifStructValue : IPrism'<Google.Protobuf.Value.KindCase,Google.Protobuf.Struct> =
            {
                _unto = fun a -> Google.Protobuf.Value.KindCase.StructValue a
                _which = fun s ->
                    match s with
                    | Google.Protobuf.Value.KindCase.StructValue a -> Ok a
                    | _ -> Error s
            }
        let ifListValue : IPrism'<Google.Protobuf.Value.KindCase,Google.Protobuf.ListValue> =
            {
                _unto = fun a -> Google.Protobuf.Value.KindCase.ListValue a
                _which = fun s ->
                    match s with
                    | Google.Protobuf.Value.KindCase.ListValue a -> Ok a
                    | _ -> Error s
            }
    let ``kind`` : ILens'<Google.Protobuf.Value,Google.Protobuf.Value.KindCase> =
        {
            _getter = { _get = fun (s: Google.Protobuf.Value) -> s.Kind }
            _setter = { _over = fun a2b s -> { s with Kind = a2b s.Kind } }
        }
module ListValue =
    let ``values`` : ILens'<Google.Protobuf.ListValue,Google.Protobuf.Value list> =
        {
            _getter = { _get = fun (s: Google.Protobuf.ListValue) -> s.Values }
            _setter = { _over = fun a2b s -> { s with Values = a2b s.Values } }
        }

namespace Google.Protobuf
open Focal.Core
open System.Runtime.CompilerServices
[<Extension>]
type OpticsExtensionMethods_google_protobuf_struct_proto =
    [<Extension>]
    static member inline Fields(lens : ILens<'a,'b,Google.Protobuf.Struct,Google.Protobuf.Struct>) : ILens<'a,'b,Map<string, Google.Protobuf.Value>,Map<string, Google.Protobuf.Value>> =
        lens.ComposeWith(Google.Protobuf.Optics.Struct.``fields``)
    [<Extension>]
    static member inline Fields(traversal : ITraversal<'a,'b,Google.Protobuf.Struct,Google.Protobuf.Struct>) : ITraversal<'a,'b,Map<string, Google.Protobuf.Value>,Map<string, Google.Protobuf.Value>> =
        traversal.ComposeWith(Google.Protobuf.Optics.Struct.``fields``)
    [<Extension>]
    static member inline Kind(lens : ILens<'a,'b,Google.Protobuf.Value,Google.Protobuf.Value>) : ILens<'a,'b,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase> =
        lens.ComposeWith(Google.Protobuf.Optics.Value.``kind``)
    [<Extension>]
    static member inline Kind(traversal : ITraversal<'a,'b,Google.Protobuf.Value,Google.Protobuf.Value>) : ITraversal<'a,'b,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase> =
        traversal.ComposeWith(Google.Protobuf.Optics.Value.``kind``)
    [<Extension>]
    static member inline IfNullValue(prism : IPrism<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : IPrism<'s,'t,Google.Protobuf.NullValue,Google.Protobuf.NullValue> = 
        prism.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifNullValue)
    [<Extension>]
    static member inline IfNullValue(traversal : ITraversal<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : ITraversal<'s,'t,Google.Protobuf.NullValue,Google.Protobuf.NullValue> = 
        traversal.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifNullValue)
    [<Extension>]
    static member inline IfNumberValue(prism : IPrism<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : IPrism<'s,'t,double,double> = 
        prism.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifNumberValue)
    [<Extension>]
    static member inline IfNumberValue(traversal : ITraversal<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : ITraversal<'s,'t,double,double> = 
        traversal.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifNumberValue)
    [<Extension>]
    static member inline IfStringValue(prism : IPrism<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : IPrism<'s,'t,string,string> = 
        prism.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifStringValue)
    [<Extension>]
    static member inline IfStringValue(traversal : ITraversal<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : ITraversal<'s,'t,string,string> = 
        traversal.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifStringValue)
    [<Extension>]
    static member inline IfBoolValue(prism : IPrism<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : IPrism<'s,'t,bool,bool> = 
        prism.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifBoolValue)
    [<Extension>]
    static member inline IfBoolValue(traversal : ITraversal<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : ITraversal<'s,'t,bool,bool> = 
        traversal.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifBoolValue)
    [<Extension>]
    static member inline IfStructValue(prism : IPrism<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : IPrism<'s,'t,Google.Protobuf.Struct,Google.Protobuf.Struct> = 
        prism.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifStructValue)
    [<Extension>]
    static member inline IfStructValue(traversal : ITraversal<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : ITraversal<'s,'t,Google.Protobuf.Struct,Google.Protobuf.Struct> = 
        traversal.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifStructValue)
    [<Extension>]
    static member inline IfListValue(prism : IPrism<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : IPrism<'s,'t,Google.Protobuf.ListValue,Google.Protobuf.ListValue> = 
        prism.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifListValue)
    [<Extension>]
    static member inline IfListValue(traversal : ITraversal<'s,'t,Google.Protobuf.Value.KindCase,Google.Protobuf.Value.KindCase>) : ITraversal<'s,'t,Google.Protobuf.ListValue,Google.Protobuf.ListValue> = 
        traversal.ComposeWith(Google.Protobuf.Optics.Value.KindPrisms.ifListValue)
    [<Extension>]
    static member inline Values(lens : ILens<'a,'b,Google.Protobuf.ListValue,Google.Protobuf.ListValue>) : ILens<'a,'b,Google.Protobuf.Value list,Google.Protobuf.Value list> =
        lens.ComposeWith(Google.Protobuf.Optics.ListValue.``values``)
    [<Extension>]
    static member inline Values(traversal : ITraversal<'a,'b,Google.Protobuf.ListValue,Google.Protobuf.ListValue>) : ITraversal<'a,'b,Google.Protobuf.Value list,Google.Protobuf.Value list> =
        traversal.ComposeWith(Google.Protobuf.Optics.ListValue.``values``)


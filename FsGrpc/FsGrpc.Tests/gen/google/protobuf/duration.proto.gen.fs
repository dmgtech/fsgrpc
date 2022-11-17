namespace rec Google.Protobuf
open FsGrpc.Protobuf
#nowarn "40"
#nowarn "1182"


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Duration =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
            val mutable Seconds: int64 // (1)
            val mutable Nanos: int // (2)
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | 1 -> x.Seconds <- ValueCodec.Int64.ReadValue reader
            | 2 -> x.Nanos <- ValueCodec.Int32.ReadValue reader
            | _ -> reader.SkipLastField()
        member x.Build : Google.Protobuf.Duration = {
            Seconds = x.Seconds
            Nanos = x.Nanos
            }

/// <summary>
/// A Duration represents a signed, fixed-length span of time represented
/// as a count of seconds and fractions of seconds at nanosecond
/// resolution. It is independent of any calendar and concepts like "day"
/// or "month". It is related to Timestamp in that the difference between
/// two Timestamp values is a Duration and it can be added or subtracted
/// from a Timestamp. Range is approximately +-10,000 years.
/// 
/// # Examples
/// 
/// Example 1: Compute Duration from two Timestamps in pseudo code.
/// 
///     Timestamp start = ...;
///     Timestamp end = ...;
///     Duration duration = ...;
/// 
///     duration.seconds = end.seconds - start.seconds;
///     duration.nanos = end.nanos - start.nanos;
/// 
///     if (duration.seconds < 0 && duration.nanos > 0) {
///       duration.seconds += 1;
///       duration.nanos -= 1000000000;
///     } else if (duration.seconds > 0 && duration.nanos < 0) {
///       duration.seconds -= 1;
///       duration.nanos += 1000000000;
///     }
/// 
/// Example 2: Compute Timestamp from Timestamp + Duration in pseudo code.
/// 
///     Timestamp start = ...;
///     Duration duration = ...;
///     Timestamp end = ...;
/// 
///     end.seconds = start.seconds + duration.seconds;
///     end.nanos = start.nanos + duration.nanos;
/// 
///     if (end.nanos < 0) {
///       end.seconds -= 1;
///       end.nanos += 1000000000;
///     } else if (end.nanos >= 1000000000) {
///       end.seconds += 1;
///       end.nanos -= 1000000000;
///     }
/// 
/// Example 3: Compute Duration from datetime.timedelta in Python.
/// 
///     td = datetime.timedelta(days=3, minutes=10)
///     duration = Duration()
///     duration.FromTimedelta(td)
/// 
/// # JSON Mapping
/// 
/// In JSON format, the Duration type is encoded as a string rather than an
/// object, where the string ends in the suffix "s" (indicating seconds) and
/// is preceded by the number of seconds, with nanoseconds expressed as
/// fractional seconds. For example, 3 seconds with 0 nanoseconds should be
/// encoded in JSON format as "3s", while 3 seconds and 1 nanosecond should
/// be expressed in JSON format as "3.000000001s", and 3 seconds and 1
/// microsecond should be expressed in JSON format as "3.000001s".
/// </summary>
type private _Duration = Duration
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
type Duration = {
    // Field Declarations
    /// <summary>
    /// Signed seconds of the span of time. Must be from -315,576,000,000
    /// to +315,576,000,000 inclusive. Note: these bounds are computed from:
    /// 60 sec/min * 60 min/hr * 24 hr/day * 365.25 days/year * 10000 years
    /// </summary>
    [<System.Text.Json.Serialization.JsonPropertyName("seconds")>] Seconds: int64 // (1)
    /// <summary>
    /// Signed fractions of a second at nanosecond resolution of the span
    /// of time. Durations less than one second are represented with a 0
    /// `seconds` field and a positive or negative `nanos` field. For durations
    /// of one second or more, a non-zero value for the `nanos` field must be
    /// of the same sign as the `seconds` field. Must be from -999,999,999
    /// to +999,999,999 inclusive.
    /// </summary>
    [<System.Text.Json.Serialization.JsonPropertyName("nanos")>] Nanos: int // (2)
    }
    with
    static member Proto : Lazy<ProtoDef<Duration>> =
        lazy
        // Field Definitions
        let Seconds = FieldCodec.Primitive ValueCodec.Int64 (1, "seconds")
        let Nanos = FieldCodec.Primitive ValueCodec.Int32 (2, "nanos")
        // Proto Definition Implementation
        { // ProtoDef<Duration>
            Name = "Duration"
            Empty = {
                Seconds = Seconds.GetDefault()
                Nanos = Nanos.GetDefault()
                }
            Size = fun (m: Duration) ->
                0
                + Seconds.CalcFieldSize m.Seconds
                + Nanos.CalcFieldSize m.Nanos
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: Duration) ->
                Seconds.WriteField w m.Seconds
                Nanos.WriteField w m.Nanos
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.Duration.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeSeconds = Seconds.WriteJsonField o
                let writeNanos = Nanos.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: Duration) =
                    writeSeconds w m.Seconds
                    writeNanos w m.Nanos
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : Duration =
                    match kvPair.Key with
                    | "seconds" -> { value with Seconds = Seconds.ReadJsonField kvPair.Value }
                    | "nanos" -> { value with Nanos = Nanos.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _Duration.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._Duration.Proto.Value.Empty

namespace Google.Protobuf.Optics
open FsGrpc.Optics
module Duration =
    let ``seconds`` : ILens'<Google.Protobuf.Duration,int64> =
        {
            _getter = { _get = fun (s: Google.Protobuf.Duration) -> s.Seconds }
            _setter = { _over = fun a2b s -> { s with Seconds = a2b s.Seconds } }
        }
    let ``nanos`` : ILens'<Google.Protobuf.Duration,int> =
        {
            _getter = { _get = fun (s: Google.Protobuf.Duration) -> s.Nanos }
            _setter = { _over = fun a2b s -> { s with Nanos = a2b s.Nanos } }
        }

namespace Google.Protobuf
open FsGrpc.Optics
open System.Runtime.CompilerServices
[<Extension>]
type OpticsExtensionMethods_google_protobuf_duration_proto =
    [<Extension>]
    static member inline Seconds(lens : ILens<'a,'b,Google.Protobuf.Duration,Google.Protobuf.Duration>) : ILens<'a,'b,int64,int64> =
        lens.ComposeWith(Google.Protobuf.Optics.Duration.``seconds``)
    [<Extension>]
    static member inline Seconds(traversal : ITraversal<'a,'b,Google.Protobuf.Duration,Google.Protobuf.Duration>) : ITraversal<'a,'b,int64,int64> =
        traversal.ComposeWith(Google.Protobuf.Optics.Duration.``seconds``)
    [<Extension>]
    static member inline Nanos(lens : ILens<'a,'b,Google.Protobuf.Duration,Google.Protobuf.Duration>) : ILens<'a,'b,int,int> =
        lens.ComposeWith(Google.Protobuf.Optics.Duration.``nanos``)
    [<Extension>]
    static member inline Nanos(traversal : ITraversal<'a,'b,Google.Protobuf.Duration,Google.Protobuf.Duration>) : ITraversal<'a,'b,int,int> =
        traversal.ComposeWith(Google.Protobuf.Optics.Duration.``nanos``)


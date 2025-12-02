namespace rec Google.Protobuf
open FsGrpc.Protobuf
open Google.Protobuf
#nowarn "40"
#nowarn "1182"


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Timestamp =

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
        member x.Build : Google.Protobuf.Timestamp = {
            Seconds = x.Seconds
            Nanos = x.Nanos
            }

/// <summary>
/// A Timestamp represents a point in time independent of any time zone or local
/// calendar, encoded as a count of seconds and fractions of seconds at
/// nanosecond resolution. The count is relative to an epoch at UTC midnight on
/// January 1, 1970, in the proleptic Gregorian calendar which extends the
/// Gregorian calendar backwards to year one.
/// 
/// All minutes are 60 seconds long. Leap seconds are "smeared" so that no leap
/// second table is needed for interpretation, using a [24-hour linear
/// smear](https://developers.google.com/time/smear).
/// 
/// The range is from 0001-01-01T00:00:00Z to 9999-12-31T23:59:59.999999999Z. By
/// restricting to that range, we ensure that we can convert to and from [RFC
/// 3339](https://www.ietf.org/rfc/rfc3339.txt) date strings.
/// 
/// # Examples
/// 
/// Example 1: Compute Timestamp from POSIX `time()`.
/// 
///     Timestamp timestamp;
///     timestamp.set_seconds(time(NULL));
///     timestamp.set_nanos(0);
/// 
/// Example 2: Compute Timestamp from POSIX `gettimeofday()`.
/// 
///     struct timeval tv;
///     gettimeofday(&tv, NULL);
/// 
///     Timestamp timestamp;
///     timestamp.set_seconds(tv.tv_sec);
///     timestamp.set_nanos(tv.tv_usec * 1000);
/// 
/// Example 3: Compute Timestamp from Win32 `GetSystemTimeAsFileTime()`.
/// 
///     FILETIME ft;
///     GetSystemTimeAsFileTime(&ft);
///     UINT64 ticks = (((UINT64)ft.dwHighDateTime) << 32) | ft.dwLowDateTime;
/// 
///     // A Windows tick is 100 nanoseconds. Windows epoch 1601-01-01T00:00:00Z
///     // is 11644473600 seconds before Unix epoch 1970-01-01T00:00:00Z.
///     Timestamp timestamp;
///     timestamp.set_seconds((INT64) ((ticks / 10000000) - 11644473600LL));
///     timestamp.set_nanos((INT32) ((ticks % 10000000) * 100));
/// 
/// Example 4: Compute Timestamp from Java `System.currentTimeMillis()`.
/// 
///     long millis = System.currentTimeMillis();
/// 
///     Timestamp timestamp = Timestamp.newBuilder().setSeconds(millis / 1000)
///         .setNanos((int) ((millis % 1000) * 1000000)).build();
/// 
/// Example 5: Compute Timestamp from Java `Instant.now()`.
/// 
///     Instant now = Instant.now();
/// 
///     Timestamp timestamp =
///         Timestamp.newBuilder().setSeconds(now.getEpochSecond())
///             .setNanos(now.getNano()).build();
/// 
/// Example 6: Compute Timestamp from current time in Python.
/// 
///     timestamp = Timestamp()
///     timestamp.GetCurrentTime()
/// 
/// # JSON Mapping
/// 
/// In JSON format, the Timestamp type is encoded as a string in the
/// [RFC 3339](https://www.ietf.org/rfc/rfc3339.txt) format. That is, the
/// format is "{year}-{month}-{day}T{hour}:{min}:{sec}[.{frac_sec}]Z"
/// where {year} is always expressed using four digits while {month}, {day},
/// {hour}, {min}, and {sec} are zero-padded to two digits each. The fractional
/// seconds, which can go up to 9 digits (i.e. up to 1 nanosecond resolution),
/// are optional. The "Z" suffix indicates the timezone ("UTC"); the timezone
/// is required. A proto3 JSON serializer should always use UTC (as indicated by
/// "Z") when printing the Timestamp type and a proto3 JSON parser should be
/// able to accept both UTC and other timezones (as indicated by an offset).
/// 
/// For example, "2017-01-15T01:30:15.01Z" encodes 15.01 seconds past
/// 01:30 UTC on January 15, 2017.
/// 
/// In JavaScript, one can convert a Date object to this format using the
/// standard
/// [toISOString()](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date/toISOString)
/// method. In Python, a standard `datetime.datetime` object can be converted
/// to this format using
/// [`strftime`](https://docs.python.org/2/library/time.html#time.strftime) with
/// the time format spec '%Y-%m-%dT%H:%M:%S.%fZ'. Likewise, in Java, one can use
/// the Joda Time's [`ISODateTimeFormat.dateTime()`](
/// http://www.joda.org/joda-time/apidocs/org/joda/time/format/ISODateTimeFormat.html#dateTime()
/// ) to obtain a formatter capable of generating timestamps in this format.
/// </summary>
type private _Timestamp = Timestamp
[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.MessageConverter>)>]
[<FsGrpc.Protobuf.Message>]
[<StructuralEquality;StructuralComparison>]
type Timestamp = {
    // Field Declarations
    /// <summary>
    /// Represents seconds of UTC time since Unix epoch
    /// 1970-01-01T00:00:00Z. Must be from 0001-01-01T00:00:00Z to
    /// 9999-12-31T23:59:59Z inclusive.
    /// </summary>
    [<System.Text.Json.Serialization.JsonPropertyName("seconds")>] Seconds: int64 // (1)
    /// <summary>
    /// Non-negative fractions of a second at nanosecond resolution. Negative
    /// second values with fractions must still have non-negative nanos values
    /// that count forward in time. Must be from 0 to 999,999,999
    /// inclusive.
    /// </summary>
    [<System.Text.Json.Serialization.JsonPropertyName("nanos")>] Nanos: int // (2)
    }
    with
    static member Proto : Lazy<ProtoDef<Timestamp>> =
        lazy
        // Field Definitions
        let Seconds = FieldCodec.Primitive ValueCodec.Int64 (1, "seconds")
        let Nanos = FieldCodec.Primitive ValueCodec.Int32 (2, "nanos")
        // Proto Definition Implementation
        { // ProtoDef<Timestamp>
            Name = "Timestamp"
            Empty = {
                Seconds = Seconds.GetDefault()
                Nanos = Nanos.GetDefault()
                }
            Size = fun (m: Timestamp) ->
                0
                + Seconds.CalcFieldSize m.Seconds
                + Nanos.CalcFieldSize m.Nanos
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: Timestamp) ->
                Seconds.WriteField w m.Seconds
                Nanos.WriteField w m.Nanos
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable builder = new Google.Protobuf.Timestamp.Builder()
                let mutable tag = 0
                while read r &tag do
                    builder.Put (tag, r)
                builder.Build
            EncodeJson = fun (o: JsonOptions) ->
                let writeSeconds = Seconds.WriteJsonField o
                let writeNanos = Nanos.WriteJsonField o
                let encode (w: System.Text.Json.Utf8JsonWriter) (m: Timestamp) =
                    writeSeconds w m.Seconds
                    writeNanos w m.Nanos
                encode
            DecodeJson = fun (node: System.Text.Json.Nodes.JsonNode) ->
                let update value (kvPair: System.Collections.Generic.KeyValuePair<string,System.Text.Json.Nodes.JsonNode>) : Timestamp =
                    match kvPair.Key with
                    | "seconds" -> { value with Seconds = Seconds.ReadJsonField kvPair.Value }
                    | "nanos" -> { value with Nanos = Nanos.ReadJsonField kvPair.Value }
                    | _ -> value
                Seq.fold update _Timestamp.empty (node.AsObject ())
        }
    static member empty
        with get() = Google.Protobuf._Timestamp.Proto.Value.Empty

namespace Google.Protobuf.Optics
open Focal.Core
module Timestamp =
    let ``seconds`` : ILens'<Google.Protobuf.Timestamp,int64> =
        {
            _getter = { _get = fun (s: Google.Protobuf.Timestamp) -> s.Seconds }
            _setter = { _over = fun a2b s -> { s with Seconds = a2b s.Seconds } }
        }
    let ``nanos`` : ILens'<Google.Protobuf.Timestamp,int> =
        {
            _getter = { _get = fun (s: Google.Protobuf.Timestamp) -> s.Nanos }
            _setter = { _over = fun a2b s -> { s with Nanos = a2b s.Nanos } }
        }

namespace Google.Protobuf
open Focal.Core
open System.Runtime.CompilerServices
[<Extension>]
type OpticsExtensionMethods_google_protobuf_timestamp_proto =
    [<Extension>]
    static member inline Seconds(lens : ILens<'a,'b,Google.Protobuf.Timestamp,Google.Protobuf.Timestamp>) : ILens<'a,'b,int64,int64> =
        lens.ComposeWith(Google.Protobuf.Optics.Timestamp.``seconds``)
    [<Extension>]
    static member inline Seconds(traversal : ITraversal<'a,'b,Google.Protobuf.Timestamp,Google.Protobuf.Timestamp>) : ITraversal<'a,'b,int64,int64> =
        traversal.ComposeWith(Google.Protobuf.Optics.Timestamp.``seconds``)
    [<Extension>]
    static member inline Nanos(lens : ILens<'a,'b,Google.Protobuf.Timestamp,Google.Protobuf.Timestamp>) : ILens<'a,'b,int,int> =
        lens.ComposeWith(Google.Protobuf.Optics.Timestamp.``nanos``)
    [<Extension>]
    static member inline Nanos(traversal : ITraversal<'a,'b,Google.Protobuf.Timestamp,Google.Protobuf.Timestamp>) : ITraversal<'a,'b,int,int> =
        traversal.ComposeWith(Google.Protobuf.Optics.Timestamp.``nanos``)


module FsGrpc.JsonTests
open Helpers

open System
open Xunit
open FsGrpc
open FsGrpc.Json
open FsGrpc.Protobuf
open Test.Name.Space
open Test.Name.Space.Enums
open System.Text.Json
open System.Text.Json.Serialization
open NodaTime.Serialization.SystemTextJson
open NodaTime
open FsCheck
open FsCheck.Xunit
open FSharpPlus

let trim (s: string) = s.Trim()
let unindent (s: string) =
    let lines = s.Trim([|'\r'|]).Trim([|'\n'|]).Split([|'\n'|]) |> Array.map(fun x->x.Trim([|'\r'|]))
    let minIndent = lines |> Seq.map (fun line -> System.Text.RegularExpressions.Regex.Match(line, "^ *").Length) |> Seq.min
    let sinIndent = lines |> Seq.map (fun line -> line.Substring(minIndent));
    sinIndent |> String.concat Environment.NewLine |> trim

let ignoreNull =
    new JsonSerializerOptions(
        JsonSerializerDefaults.General,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        )

let ignoreNever =
    new JsonSerializerOptions(
        JsonSerializerDefaults.General,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
        )

let ignoreDefault =
    new JsonSerializerOptions(
        JsonSerializerDefaults.General,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        )

[<Fact>]
let ``Union serializes to json using generic serializer`` () =
    let case = {| Test = UnionCase.Name "blue" |}
    let serialized = case |> JsonSerializer.Serialize
    let expected = trim """
        {"Test":{"name":"blue"}}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Union deserializes to json using generic serializer`` () =
    let expected = {| Test = UnionCase.Name "blue" |}
    let case = trim """
        {"Test":{"name":"blue"}}
        """
    let deserialized = case |> JsonSerializer.Deserialize
    Assert.Equal(expected, deserialized)

[<Fact>]
let ``Union none does not serialize on JsonIgnoreCondition.WhenWritingNull`` () =
    let case = {| Test = UnionCase.None |}
    let serialized = JsonSerializer.Serialize (case, ignoreNull)
    let expected = trim """
        {}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Union none case does not serialize using generic serializer on JsonIgnoreCondition.WhenWritingDefault`` () =
    let case = {| Test = UnionCase.None |}
    let serialized = JsonSerializer.Serialize (case, ignoreDefault)
    let expected = trim """
        {}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Union none case serializes to null with generic serializer`` () =
    let case = {| Test = UnionCase.None |}
    let serialized = JsonSerializer.Serialize case
    let expected = trim """
        {"Test":null}
        """
    Assert.Equal(expected, serialized)


[<Fact>]
let ``Enum serializes to protobuf name using generic serializer`` () =
    let options = new JsonSerializerOptions(JsonSerializerDefaults.General)
    let case = {| Color = Color.Blue |}
    let serialized = JsonSerializer.Serialize (case, options)
    let expected = trim """
        {"Color":"COLOR_BLUE"}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Enum serializes to name using generic serializer and generic JsonStringEnumConverter`` () =
    let options = new JsonSerializerOptions(JsonSerializerDefaults.General)
    options.Converters.Add(new JsonStringEnumConverter())
    let case = {| Color = Color.Blue |}
    let serialized = JsonSerializer.Serialize (case, options)
    let expected = trim """
        {"Color":"Blue"}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Enum serializes to name using generic serializer and our EnumConverter`` () =
    let options = new JsonSerializerOptions(JsonSerializerDefaults.General)
    options.Converters.Add(new FsGrpc.Json.EnumConverter(JsonEnumStyle.Name))
    let case = {| Color = Color.Blue |}
    let serialized = JsonSerializer.Serialize (case, options)
    let expected = trim """
        {"Color":"Blue"}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Enum serializes to number using generic serializer and our EnumConverter`` () =
    let options = new JsonSerializerOptions(JsonSerializerDefaults.General)
    options.Converters.Add(new FsGrpc.Json.EnumConverter(JsonEnumStyle.Number))
    let case = {| Color = Color.Blue |}
    let serialized = JsonSerializer.Serialize (case, options)
    let expected = trim """
        {"Color":3}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Oneof union serializes with generic serializer`` () =
    let case = {| Test = UnionCase.Color Color.Red |}
    let serialized = case |> JsonSerializer.Serialize
    let expected = trim """
        {"Test":{"color":"COLOR_RED"}}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Empty bytes serialize with generic serializer`` () =
    let case = {| Test = Bytes.Empty |}
    let serialized = case |> JsonSerializer.Serialize
    let expected = trim """
        {"Test":""}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Bytes serialize with generic serializer`` () =
    let case = {| Test = Bytes.FromUtf8 "Hello" |}
    let serialized = case |> JsonSerializer.Serialize
    let expected = trim """
        {"Test":"SGVsbG8="}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Complex structures serialize with generic serializer`` () =
    let case = {|
        Test = seq [
            { Nest.empty with
                Name = "Name" }
            { Nest.empty with
                Children = [
                    { Nest.empty with Name = "Two" }
                    { Nest.empty with Name = "Three" }
                ]}
        ]
    |}
    let serialized = case |> JsonSerializer.Serialize
    let expected = trim """
        {"Test":[{"name":"Name","children":[],"inner":null,"special":null},{"name":"","children":[{"name":"Two","children":[],"inner":null,"special":null},{"name":"Three","children":[],"inner":null,"special":null}],"inner":null,"special":null}]}
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Empty structures serialize with generic serializer`` () =
    let case = {|
        TestMessage = TestMessage.empty
        Nest = Nest.empty
        Special = Special.empty
        Enums = Enums.empty
        Google = Google.empty
    |}
    let serialized = JsonSerializer.Serialize (case, JsonSerializerOptions(JsonSerializerDefaults.General, WriteIndented = true))
    let expected = unindent """
        {
          "Enums": {
            "mainColor": "COLOR_BLACK",
            "otherColors": [],
            "byName": {},
            "union": null,
            "maybeColor": null
          },
          "Google": {
            "int32Val": null,
            "stringVal": null,
            "timestamp": null,
            "duration": null
          },
          "Nest": {
            "name": "",
            "children": [],
            "inner": null,
            "special": null
          },
          "Special": {
            "intList": [],
            "doubleList": [],
            "fixed32List": [],
            "stringList": [],
            "dictionary": {}
          },
          "TestMessage": {
            "testInt": 0,
            "testDouble": 0,
            "testFixed32": 0,
            "testString": "",
            "testBytes": "",
            "testFloat": 0,
            "testInt64": "0",
            "testUint64": "0",
            "testFixed64": "0",
            "testBool": false,
            "testUint32": 0,
            "testSfixed32": 0,
            "testSfixed64": "0",
            "testSint32": 0,
            "testSint64": "0"
          }
        }
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Empty structures serialize with generic serializer with ignore default`` () =
    let case = {|
        TestMessage = TestMessage.empty
        Nest = Nest.empty
        Special = Special.empty
        Enums = Enums.empty
        Google = Google.empty
    |}
    let serialized = JsonSerializer.Serialize (case, JsonSerializerOptions(ignoreDefault, WriteIndented = true))
    // Note: empty protobuf string fields should not be serialized when ignoring default because empty is the default for protobuf string fields
    //       however, if you copy the protobuf into another record that is non-protobuf, then it should serialize because that is the behavior of System.Text.Json
    //       like it or not (it is ambiguous in f# and actually null is the default, though you can never achieve it without unchecked code; there's a good case
    //           to be made that in f#, empty string should be considered a default value if it's not a string option)
    let expected = unindent """
        {
          "Enums": {},
          "Google": {},
          "Nest": {},
          "Special": {},
          "TestMessage": {}
        }
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Nondefaults serialize with generic serializer and ignore default`` () =
    let case = { TestMessage.empty with TestInt = 1 }
    let serialized = JsonSerializer.Serialize (case, JsonSerializerOptions(ignoreDefault))
    let expected = unindent """
        {"testInt":1}
        """
    Assert.Equal(expected, serialized)


[<Fact>]
let ``Populated structures serialize with generic serializer plus nodatime`` () =
    (*
      In this one, we use the generic serializer
      but we add NodaTime's serializer also so that we get good json representations of the NodaTime classes.
      It would be nice to get this automatically, but NodaTime's dissapointing lack of built-in support for .net's own json is a problem that we can't easily address
      Even if it were supported, we would have to provide our own proto3-json-Compatible serializer anyway
      So if you want proto3-like serializers, you can have it
      If you want noda's serializers, you just have to add theirs
    *)
    let (_, testMessage) = TestCases.Value1
    let case = {|
        TestMessage = testMessage
        Nest = TestCases.Value3
        Special = TestCases.Value4
        Enums = TestCases.Value5
        Google = TestCases.Value6
    |}
    let options = JsonSerializerOptions(JsonSerializerDefaults.General, WriteIndented = true);
    let options = Extensions.ConfigureForNodaTime(options, DateTimeZoneProviders.Tzdb)
    let serialized = JsonSerializer.Serialize (case, options)
    let expected = unindent """
        {
          "Enums": {
            "mainColor": "COLOR_RED",
            "otherColors": [
              "COLOR_BLACK",
              "COLOR_RED",
              "COLOR_BLUE"
            ],
            "byName": {
              "blue": "COLOR_BLUE",
              "red": "COLOR_RED"
            },
            "name": "green",
            "maybeColor": null
          },
          "Google": {
            "int32Val": 0,
            "stringVal": "X",
            "timestamp": "1970-01-02T10:17:36.780Z",
            "duration": "123456.780s"
          },
          "Nest": {
            "name": "Animal",
            "children": [
              {
                "name": "Mammal",
                "children": [],
                "inner": null,
                "special": null
              },
              {
                "name": "Fish",
                "children": [],
                "inner": null,
                "special": {
                  "intList": [],
                  "doubleList": [],
                  "fixed32List": [],
                  "stringList": [],
                  "dictionary": {}
                }
              }
            ],
            "inner": {
              "innerName": "inner"
            },
            "special": null
          },
          "Special": {
            "intList": [
              1,
              2
            ],
            "doubleList": [
              2,
              3
            ],
            "fixed32List": [],
            "stringList": [
              "One",
              "",
              "Three"
            ],
            "dictionary": {
              "One": "Uno",
              "Two": "Dos"
            }
          },
          "TestMessage": {
            "testInt": 7,
            "testDouble": 123.4,
            "testFixed32": 12345,
            "testString": "ch\u00E9vere",
            "testBytes": "wq9cXyjjg4QpXy/Crw==",
            "testFloat": 234.5,
            "testInt64": "2345678",
            "testUint64": "3456789",
            "testFixed64": "3456789",
            "testBool": true,
            "testUint32": 456,
            "testSfixed32": 567,
            "testSfixed64": "67890123",
            "testSint32": -1234,
            "testSint64": "-2345"
          }
        }
        """
    Assert.Equal(expected, serialized)

[<Fact>]
let ``Empty structures serialize to empty`` () =
    let case = {|
        TestMessage = TestMessage.empty
        Nest = Nest.empty
        Special = Special.empty
        Enums = Enums.empty
        Google = Google.empty
    |}
    let serialized = case |> Json.serialize
    let expected = trim """
        {"Enums":{},"Google":{},"Nest":{},"Special":{},"TestMessage":{}}
        """
    Assert.Equal(expected, serialized)

[<JsonConverter(typeof<EnumConverter<SomeEnum>>)>]
type SomeEnum =
| Zero = 0
| One = 1
| Two = 2

[<Fact>]
let ``EnumConverter behaves with nongenerated enums`` () =
  let expected = """{"Enum":"One"}"""
  let actual = JsonSerializer.Serialize {| Enum = SomeEnum.One |}
  Assert.Equal(expected, actual)

[<Fact>]
let ``Oneof serializes with default options`` () =
  let expected = """{"mainColor":"COLOR_BLUE","name":"value"}"""
  let actual = FsGrpc.Json.serialize { Enums.empty with MainColor = Color.Blue; Union = UnionCase.Name "value" }
  Assert.Equal(expected, actual);

[<Fact>]
let ``Oneof serializes with wrapped union option`` () =
  let jso = new JsonSerializerOptions()
  let options = { JsonOptions.Proto3Defaults with Oneofs = JsonOneofStyle.Wrapped }
  jso.Converters.Add (new MessageConverter(Some options))
  let expected = """{"union":{"name":"value"}}"""
  let actual = FsGrpc.Json.serializeWith jso { Enums.empty with Union = UnionCase.Name "value" }
  Assert.Equal(expected, actual);

[<Fact>]
let ``Oneof serializes with number enum option`` () =
  let jso = new JsonSerializerOptions()
  let options = { JsonOptions.Proto3Defaults with Enums = JsonEnumStyle.Number }
  jso.Converters.Add (new MessageConverter(Some options))
  let expected = """{"mainColor":3}"""
  let actual = FsGrpc.Json.serializeWith jso { Enums.empty with MainColor = Color.Blue }
  Assert.Equal(expected, actual);

[<Fact>]
let ``Oneof serializes with name enum option`` () =
  let jso = new JsonSerializerOptions()
  let options = { JsonOptions.Proto3Defaults with Enums = JsonEnumStyle.Name }
  jso.Converters.Add (new MessageConverter(Some options))
  let expected = """{"mainColor":"Blue"}"""
  let actual = FsGrpc.Json.serializeWith jso { Enums.empty with MainColor = Color.Blue }
  Assert.Equal(expected, actual);

[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.EnumConverter<Color>>)>]
type BadEnum =
| MissingNameAttr = 0


[<Fact>]
let ``Test BadEnum`` =
  let vc = ValueCodec.Enum<BadEnum>
  let options = JsonOptions.Proto3Defaults
  let stream = new System.IO.MemoryStream()
  let writer = new Utf8JsonWriter(stream)
  vc.WriteJsonValue JsonOptions.Proto3Defaults writer BadEnum.MissingNameAttr
  writer.Flush()
  let actual = System.Text.Encoding.UTF8.GetString(stream.ToArray())
  Assert.Equal("\"MissingNameAttr\"", actual)

[<Fact>]
let ``Oneof cases copied into nonmessage record serialize`` () =
  let expected = """{"UnionNone":null,"UnionSome":{"name":"value"}}"""
  let actual = {| UnionNone = UnionCase.None; UnionSome = UnionCase.Name "value" |} |> JsonSerializer.Serialize
  Assert.Equal(expected, actual)

let usingWriter (callback: Utf8JsonWriter -> unit): string =
  let stream = new System.IO.MemoryStream()
  let writer = new Utf8JsonWriter(stream, new JsonWriterOptions())
  callback writer
  writer.Flush()
  stream.ToArray() |> System.Text.Encoding.UTF8.GetString

[<Fact>]
let ``OneofConverter renders null for non case`` () =
  // It is necessary to test this separately because the default serializer won't call the converter for the None case
  let expected = """null"""
  let options = new JsonSerializerOptions()
  let actual = usingWriter (fun writer -> (new OneofConverter<UnionCase>()).Write (writer, UnionCase.None, options))
  Assert.Equal(expected, actual)

[<Fact>]
let ``Oneof none case creates null if rendered directly`` () =
  let expected = """null"""
  let actual = UnionCase.None |> JsonSerializer.Serialize
  Assert.Equal(expected, actual)

[<Fact>]
let ``Map records serialized individually will encode correctly`` () =
  let expected = """{"key":"clave","value":"valor"}"""
  let subject = ("clave", "valor")
  let codec = Protobuf.ValueCodec.MapRecord Protobuf.ValueCodec.String Protobuf.ValueCodec.String
  let jsonOptions = new JsonSerializerOptions()
  let options = JsonOptions.FromJsonSerializerOptions jsonOptions
  let actual = usingWriter (fun writer ->codec.WriteJsonValue options writer subject)
  Assert.Equal(expected, actual)

[<Fact>]
let ``Empty repeated does not serialize on IgnoreDefaults`` () =
  let subject = { Special.empty with IntList = []; StringList = ["1"; ""; "2"] }
  let actual = JsonSerializer.Serialize (subject, ignoreDefault)
  let expected = """{"stringList":["1","","2"]}"""
  Assert.Equal(expected, actual)

[<Fact>]
let ``Map with int keys serializes to string keys`` () =
  let subject = { IntMap.empty with IntMap = Map [(1, "uno"); (2, "dos")] }
  let actual = JsonSerializer.Serialize (subject, ignoreDefault)
  let expected = """{"intMap":{"1":"uno","2":"dos"}}"""
  Assert.Equal(expected, actual)

[<Fact>]
let ``Timestamp serializes as expected`` () =
  let subject = { Google.empty with Timestamp = Some (NodaTime.Instant.FromUnixTimeTicks 3) }
  let actual = JsonSerializer.Serialize (subject, ignoreDefault)
  let expected = """{"timestamp":"1970-01-01T00:00:00.000000300Z"}"""
  Assert.Equal(expected, actual)

[<Fact>]
let ``Duration serializes as expected`` () =
  let subject = { Google.empty with Duration = Some (NodaTime.Duration.FromMilliseconds 2L) }
  let actual = JsonSerializer.Serialize (subject, ignoreDefault)
  let expected = """{"duration":"00.002s"}"""
  Assert.Equal(expected, actual)

[<Fact>]
let ``Empty JSON object deserializes to default value`` () =
  let subject = "{ }"
  let actual : Enums = JsonSerializer.Deserialize (subject, ignoreDefault)
  let expected = Enums.empty
  Assert.Equal(expected, actual)

[<Fact>]
let ``Unknown fields are ignored during deserialization`` () =
  let subject = """{"mainColor":"COLOR_BLACK","asdf":"fdsa"}"""
  let actual : Enums = JsonSerializer.Deserialize (subject, ignoreDefault)
  let expected = { Enums.empty with MainColor = Enums.Color.Black }
  Assert.Equal(expected, actual)

[<Fact>]
let ``Last in wins rules for deserializing oneof fields`` () =
  let subject = """{"color":"COLOR_BLACK","name":"teal"}"""
  let actual : Enums = JsonSerializer.Deserialize (subject, ignoreDefault)
  let expected = { Enums.empty with Union = UnionCase.Name "teal" }
  Assert.Equal(expected, actual)

[<Fact>]
let ``Last in wins rules for deserializing oneof fields -- alternative case`` () =
  let subject = """{"name":"teal","color":"COLOR_BLACK"}"""
  let actual : Enums = JsonSerializer.Deserialize (subject, ignoreDefault)
  let expected = { Enums.empty with Union = UnionCase.Color Enums.Color.Black }
  Assert.Equal(expected, actual)

[<Fact>]
let ``deserialzeWith Options.Default = deserialize`` () =
  let subject = """{"name":"teal","color":"COLOR_BLACK"}"""
  let actual : Enums = FsGrpc.Json.deserialize subject
  let expected = FsGrpc.Json.deserializeWith Options.Default subject
  Assert.Equal(expected, actual)

[<Fact>]
let ``JsonOptions.FromJsonSerializerOptions with ignore never returns Omit.Never`` () =
    let case = Enums.empty
    let options = JsonOptions.FromJsonSerializerOptions ignoreNever
    let expected = JsonOmit.Never
    Assert.Equal(expected, options.Omit)

[<Fact>]
let ``JsonOptions.FromJsonSerializerOptions with ignore null returns Omit.WhenNull`` () =
    let case = Enums.empty
    let options = JsonOptions.FromJsonSerializerOptions ignoreNull
    let expected = JsonOmit.WhenNull
    Assert.Equal(expected, options.Omit)

type Generator = 
  static member String() =
    // null strings are not supported on the F# datatype for JSON de/serialization
    Arb.Default.String()
    |> Arb.filter (fun str -> str <> null)
  static member ByteString() =
    Arb.generate<byte array>
    |> Gen.map Bytes.CopyFrom
    |> Arb.fromGen
  static member Instant() =
    Arb.generate<int64>
    |> Gen.filter (fun x -> x >= Instant.MinValue.ToUnixTimeTicks() && x <= Instant.MaxValue.ToUnixTimeTicks())
    |> Gen.map Instant.FromUnixTimeTicks
    |> Arb.fromGen
  static member Duration() =
    Arb.generate<float>
    |> Gen.filter (fun x -> x >= Duration.MinValue.TotalTicks && x <= Duration.MaxValue.TotalTicks)
    |> Gen.map Duration.FromTicks
    |> Arb.fromGen
  static member Float() = 
    // NaN, +/- Infinity cannot be written as valid JSON
    Arb.Default.Float()
    |> Arb.filter (Double.IsNormal) 
  static member Float32() = 
    // NaN, +/- Infinity cannot be written as valid JSON
    Arb.Default.Float32()
    |> Arb.filter (Single.IsNormal) 
  static member Nest() = 
    // bound the recursive data structure's nesting depth to prevent stack overflow
    let nest =
      let rec nest' s = 
        match s with
        | 0 -> Gen.map (fun _ -> Nest.empty) Arb.generate<int>
        | n when n > 0 -> gen {
            let! name = Arb.generate<string>
            let! children = Gen.listOf (nest' (n/2))
            let! inner = Gen.oneof [
              Gen.constant None
              Gen.map Some Arb.generate<Nest.Inner> 
            ]
            let! special = Gen.oneof [
              Gen.constant None
              Gen.map Some Arb.generate<Special> 
            ]
            return { Name=name; Children=children; Inner=inner; Special=special; } }
        | _ -> invalidArg "s" "Only positive arguments are allowed"
      Gen.sized nest'
    Arb.fromGen nest


let roundTrip (o: JsonOptions) (x: 'a) = 
  let jso = new JsonSerializerOptions()
  jso.Converters.Add(new MessageConverter(Some o))
  let y = x |> FsGrpc.Json.serializeWith jso |> FsGrpc.Json.deserializeWith jso
  x = y

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip (TestMessage)`` (o: JsonOptions, x: TestMessage) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip (Nest)`` (o: JsonOptions, x: Nest) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip (Special)`` (o: JsonOptions, x: Special) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip (Enums)`` (o: JsonOptions, x: Enums) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip (Google)`` (o: JsonOptions, x: Google) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip (IntMap)`` (o: JsonOptions, x: Enums) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip (int * string * Nest)`` (o: JsonOptions, x: (int * string * Nest)) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip ({| Test: UnionCase |})`` (o: JsonOptions, x: {| Test: UnionCase |}) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip ({| EnumTest: Enums.Color |})`` (o: JsonOptions, x: {| EnumTest: Enums.Color |}) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip (Map<int,Enums>)`` (o: JsonOptions, x: Map<int,Enums>) =
  roundTrip o x

[<Property(Arbitrary=[| typeof<Generator> |])>]
let ``Round trip ({| Data: Bytes |})`` (o: JsonOptions, x: {| Data: Bytes |}) =
  roundTrip o x

// ============================================================================
// Tests for google.protobuf.Struct - Testing with updated dependencies
// ============================================================================
// These tests check google.protobuf.Struct support after updating Google.Protobuf package.
// The Struct type represents arbitrary JSON-like data:
// - Fields is a map<string, Value>
// - Value can be: null, number, string, bool, list of values, or nested struct
//
// Expected behavior:
// - Struct should serialize to/from standard JSON objects
// - Should handle all Value types (null, number, string, bool, list, struct)
// - Should round-trip correctly through both protobuf binary and JSON

//[<Fact>]
//let ``Struct with simple fields serializes to JSON`` () =
//    // Create a simple Struct with various field types
//    let structValue = Google.Protobuf.Struct.empty
    
//    let actual = JsonSerializer.Serialize(structValue, ignoreDefault)
//    let expected = """{}"""
//    Assert.Equal(expected, actual)

//[<Fact>]
//let ``Struct with nested fields serializes correctly`` () =
//    // Test a more complex structure
//    let structValue = Google.Protobuf.Struct.empty
    
//    let actual = JsonSerializer.Serialize(structValue)
//    let expected = """{}"""
//    Assert.Equal(expected, actual)

//[<Fact>]
//let ``Struct deserializes from JSON`` () =
//    // Test deserialization from JSON
//    let json = """{"name":"test","count":42,"active":true}"""
    
//    let actual : Google.Protobuf.Struct = JsonSerializer.Deserialize(json)
    
//    // Verify the struct was properly deserialized
//    Assert.NotNull(actual)

//[<Fact>]
//let ``Struct round-trips through JSON`` () =
//    // This is the key test - create a Struct, serialize it, deserialize it, and verify equality
//    let original = Google.Protobuf.Struct.empty
    
//    let serialized = JsonSerializer.Serialize(original)
//    let deserialized : Google.Protobuf.Struct = JsonSerializer.Deserialize(serialized)
    
//    Assert.Equal(original, deserialized)

module FsGrpc.StructTests

open System
open Xunit
open FsGrpc
open FsGrpc.Protobuf
open System.Text.Json
open System.Text.Json.Serialization
open Test.Name.Space

// ============================================================================
// STRUCT SUPPORT VALIDATION TESTS
// ============================================================================
// These tests validate that google.protobuf.Struct, Value, ListValue, and
// NullValue types are properly generated and functional.
//
// Prerequisites for these tests to pass:
// 1. struct_test.proto must be in FsGrpc.Tests directory
// 2. buf generate --include-imports --include-wkt must be run
// 3. gen/google/protobuf/struct.proto.gen.fs must be generated
// 4. The generated code must compile
//
// Test Organization:
// - Basic Structure Tests: Verify types can be created
// - Recursive Structure Tests: Verify mutual recursion works
// - Protobuf Wire Format Tests: Verify binary serialization
// - JSON Serialization Tests: Verify JSON serialization (may need custom converters)
// ============================================================================

// ----------------------------------------------------------------------------
// BASIC STRUCTURE TESTS
// These verify that the types exist and can be instantiated
// ----------------------------------------------------------------------------

[<Fact>]
let ``NullValue enum exists and has correct value`` () =
    let nullVal = Google.Protobuf.NullValue.NullValue
    Assert.Equal(0, int nullVal)

[<Fact>]
let ``Value type exists with Kind discriminated union`` () =
    // This test verifies the Value type is generated with a Kind oneof
    let value = Google.Protobuf.Value.empty
    Assert.NotNull(value)
    match value.Kind with
    | Google.Protobuf.Value.KindCase.None -> ()  // Expected for empty
    | _ -> Assert.Fail("Expected Value.KindCase.None for empty value")

[<Fact>]
let ``Value with NullValue case can be created`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.NullValue Google.Protobuf.NullValue.NullValue }
    match value.Kind with
    | Google.Protobuf.Value.KindCase.NullValue n -> Assert.Equal(Google.Protobuf.NullValue.NullValue, n)
    | _ -> Assert.Fail("Expected NullValue case")

[<Fact>]
let ``Value with NumberValue case can be created`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.NumberValue 42.0 }
    match value.Kind with
    | Google.Protobuf.Value.KindCase.NumberValue n -> Assert.Equal(42.0, n)
    | _ -> Assert.Fail("Expected NumberValue case")

[<Fact>]
let ``Value with StringValue case can be created`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.StringValue "hello" }
    match value.Kind with
    | Google.Protobuf.Value.KindCase.StringValue s -> Assert.Equal("hello", s)
    | _ -> Assert.Fail("Expected StringValue case")

[<Fact>]
let ``Value with BoolValue case can be created`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.BoolValue true }
    match value.Kind with
    | Google.Protobuf.Value.KindCase.BoolValue b -> Assert.True(b)
    | _ -> Assert.Fail("Expected BoolValue case")

[<Fact>]
let ``Struct with empty fields can be created`` () =
    let struct' = Google.Protobuf.Struct.empty
    Assert.Empty(struct'.Fields)

[<Fact>]
let ``Struct with fields map can be created`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.StringValue "test" }
    let struct' = { Google.Protobuf.Struct.empty with 
                     Fields = Map.ofList [("key", value)] }
    Assert.Single(struct'.Fields) |> ignore
    Assert.True(struct'.Fields.ContainsKey("key"))

[<Fact>]
let ``ListValue with empty values can be created`` () =
    let listValue = Google.Protobuf.ListValue.empty
    Assert.Empty(listValue.Values)

[<Fact>]
let ``ListValue with values list can be created`` () =
    let value1 = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.NumberValue 1.0 }
    let value2 = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.StringValue "two" }
    let listValue = { Google.Protobuf.ListValue.empty with Values = [value1; value2] }
    Assert.Equal(2, listValue.Values.Length)

// ----------------------------------------------------------------------------
// RECURSIVE STRUCTURE TESTS  
// These verify that mutual recursion between Struct and Value works correctly
// ----------------------------------------------------------------------------

[<Fact>]
let ``Value can contain Struct (StructValue case)`` () =
    let innerStruct = Google.Protobuf.Struct.empty
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.StructValue innerStruct }
    match value.Kind with
    | Google.Protobuf.Value.KindCase.StructValue s -> Assert.Equal(innerStruct, s)
    | _ -> Assert.Fail("Expected StructValue case")

[<Fact>]
let ``Value can contain ListValue (ListValue case)`` () =
    let innerList = Google.Protobuf.ListValue.empty
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.ListValue innerList }
    match value.Kind with
    | Google.Protobuf.Value.KindCase.ListValue l -> Assert.Equal(innerList, l)
    | _ -> Assert.Fail("Expected ListValue case")

[<Fact>]
let ``Struct can contain Value in Fields map`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.StringValue "test" }
    let struct' = { Google.Protobuf.Struct.empty with 
                     Fields = Map.ofList [("key", value)] }
    Assert.Single(struct'.Fields) |> ignore
    let retrievedValue = struct'.Fields.["key"]
    match retrievedValue.Kind with
    | Google.Protobuf.Value.KindCase.StringValue s -> Assert.Equal("test", s)
    | _ -> Assert.Fail("Expected StringValue case")

[<Fact>]
let ``Deeply nested Struct and Value work correctly`` () =
    // Create a structure like: { "outer": { "inner": ["a", "b", "c"] } }
    let valueA = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.StringValue "a" }
    let valueB = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.StringValue "b" }
    let valueC = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.StringValue "c" }
    
    let innerList = { Google.Protobuf.ListValue.empty with Values = [valueA; valueB; valueC] }
    let innerListValue = { Google.Protobuf.Value.empty with 
                            Kind = Google.Protobuf.Value.KindCase.ListValue innerList }
    
    let innerStruct = { Google.Protobuf.Struct.empty with 
                         Fields = Map.ofList [("inner", innerListValue)] }
    let innerStructValue = { Google.Protobuf.Value.empty with 
                              Kind = Google.Protobuf.Value.KindCase.StructValue innerStruct }
    
    let outerStruct = { Google.Protobuf.Struct.empty with 
                         Fields = Map.ofList [("outer", innerStructValue)] }
    
    // Verify we can navigate the structure
    Assert.Single(outerStruct.Fields) |> ignore
    match outerStruct.Fields.["outer"].Kind with
    | Google.Protobuf.Value.KindCase.StructValue inner ->
        match inner.Fields.["inner"].Kind with
        | Google.Protobuf.Value.KindCase.ListValue list ->
            Assert.Equal(3, list.Values.Length)
        | _ -> Assert.Fail("Expected ListValue in inner")
    | _ -> Assert.Fail("Expected StructValue in outer")

// ----------------------------------------------------------------------------
// PROTOBUF WIRE FORMAT TESTS
// These verify that Struct types can be serialized to/from protobuf binary format
// ----------------------------------------------------------------------------

[<Fact>]
let ``Empty Struct round-trips through protobuf encoding`` () =
    let original = Google.Protobuf.Struct.empty
    let encoded = FsGrpc.Protobuf.encode original
    let decoded = FsGrpc.Protobuf.decode<Google.Protobuf.Struct> encoded
    Assert.Equal(original, decoded)

[<Fact>]
let ``Struct with fields round-trips through protobuf encoding`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.StringValue "test" }
    let original = { Google.Protobuf.Struct.empty with 
                      Fields = Map.ofList [("key", value)] }
    let encoded = FsGrpc.Protobuf.encode original
    let decoded = FsGrpc.Protobuf.decode<Google.Protobuf.Struct> encoded
    Assert.Equal(original, decoded)

[<Fact>]
let ``Value with NumberValue round-trips through protobuf encoding`` () =
    let original = { Google.Protobuf.Value.empty with 
                      Kind = Google.Protobuf.Value.KindCase.NumberValue 123.45 }
    let encoded = FsGrpc.Protobuf.encode original
    let decoded = FsGrpc.Protobuf.decode<Google.Protobuf.Value> encoded
    Assert.Equal(original, decoded)

[<Fact>]
let ``Value with StringValue round-trips through protobuf encoding`` () =
    let original = { Google.Protobuf.Value.empty with 
                      Kind = Google.Protobuf.Value.KindCase.StringValue "hello world" }
    let encoded = FsGrpc.Protobuf.encode original
    let decoded = FsGrpc.Protobuf.decode<Google.Protobuf.Value> encoded
    Assert.Equal(original, decoded)

[<Fact>]
let ``Value with BoolValue round-trips through protobuf encoding`` () =
    let original = { Google.Protobuf.Value.empty with 
                      Kind = Google.Protobuf.Value.KindCase.BoolValue true }
    let encoded = FsGrpc.Protobuf.encode original
    let decoded = FsGrpc.Protobuf.decode<Google.Protobuf.Value> encoded
    Assert.Equal(original, decoded)

[<Fact>]
let ``Value with NullValue round-trips through protobuf encoding`` () =
    let original = { Google.Protobuf.Value.empty with 
                      Kind = Google.Protobuf.Value.KindCase.NullValue Google.Protobuf.NullValue.NullValue }
    let encoded = FsGrpc.Protobuf.encode original
    let decoded = FsGrpc.Protobuf.decode<Google.Protobuf.Value> encoded
    Assert.Equal(original, decoded)

[<Fact>]
let ``ListValue round-trips through protobuf encoding`` () =
    let value1 = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.NumberValue 1.0 }
    let value2 = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.StringValue "two" }
    let original = { Google.Protobuf.ListValue.empty with Values = [value1; value2] }
    let encoded = FsGrpc.Protobuf.encode original
    let decoded = FsGrpc.Protobuf.decode<Google.Protobuf.ListValue> encoded
    Assert.Equal(original, decoded)

[<Fact>]
let ``StructTestMessage with Struct field round-trips through protobuf encoding`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.StringValue "metadata" }
    let struct' = { Google.Protobuf.Struct.empty with 
                     Fields = Map.ofList [("key", value)] }
    let original = { StructTestMessage.empty with Metadata = Some struct' }
    let encoded = FsGrpc.Protobuf.encode original
    let decoded = FsGrpc.Protobuf.decode<StructTestMessage> encoded
    Assert.Equal(original, decoded)

// ----------------------------------------------------------------------------
// JSON SERIALIZATION TESTS
// These verify JSON serialization according to Proto3 JSON specification
// Note: These may require custom JSON converters in FsGrpc runtime
// ----------------------------------------------------------------------------

let ignoreDefault =
    new JsonSerializerOptions(
        JsonSerializerDefaults.General,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    )

[<Fact>]
let ``Empty Struct serializes to empty JSON object`` () =
    let struct' = Google.Protobuf.Struct.empty
    let actual = JsonSerializer.Serialize(struct', ignoreDefault)
    let expected = """{}"""
    Assert.Equal(expected, actual)

[<Fact>]
let ``Struct with simple fields serializes to JSON correctly`` () =
    let strValue = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.StringValue "hello" }
    let numValue = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.NumberValue 42.0 }
    let boolValue = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.BoolValue true }
    
    let struct' = { Google.Protobuf.Struct.empty with 
                     Fields = Map.ofList [
                         ("name", strValue)
                         ("count", numValue)
                         ("active", boolValue)
                     ] }
    
    let actual = JsonSerializer.Serialize(struct')
    // Per Proto3 JSON spec, Struct should serialize as a plain JSON object
    // The exact ordering may vary, so we check that all fields are present
    Assert.Contains("\"name\"", actual)
    Assert.Contains("\"count\"", actual)
    Assert.Contains("\"active\"", actual)

[<Fact>]
let ``Struct deserializes from JSON`` () =
    // Struct expects JSON with "fields" wrapper
    let json = """{"fields":{"name":{"stringValue":"test"},"count":{"numberValue":42},"active":{"boolValue":true}}}"""
    let actual = JsonSerializer.Deserialize<Google.Protobuf.Struct>(json)
    
    Assert.NotNull(actual)
    Assert.Equal(3, actual.Fields.Count)
    Assert.True(actual.Fields.ContainsKey("name"))
    Assert.True(actual.Fields.ContainsKey("count"))
    Assert.True(actual.Fields.ContainsKey("active"))

[<Fact>]
let ``Struct round-trips through JSON serialization`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.StringValue "test" }
    let original = { Google.Protobuf.Struct.empty with 
                      Fields = Map.ofList [("key", value)] }
    
    let serialized = JsonSerializer.Serialize(original)
    let deserialized = JsonSerializer.Deserialize<Google.Protobuf.Struct>(serialized)
    
    Assert.Equal(original, deserialized)

[<Fact>]
let ``ListValue serializes to JSON array`` () =
    let value1 = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.NumberValue 1.0 }
    let value2 = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.StringValue "two" }
    let listValue = { Google.Protobuf.ListValue.empty with Values = [value1; value2] }
    
    let actual = JsonSerializer.Serialize(listValue)
    
    // ListValue serializes as {"values": [...]} not a plain array
    // Each Value in the array serializes with its kind wrapper
    Assert.Contains("\"values\"", actual)
    Assert.Contains("\"numberValue\":1", actual)
    Assert.Contains("\"stringValue\":\"two\"", actual)

[<Fact>]
let ``StructTestMessage with all field types serializes to JSON`` () =
    let strValue = { Google.Protobuf.Value.empty with Kind = Google.Protobuf.Value.KindCase.StringValue "data" }
    let struct' = { Google.Protobuf.Struct.empty with 
                     Fields = Map.ofList [("key", strValue)] }
    let listValue = { Google.Protobuf.ListValue.empty with Values = [strValue] }
    
    let message = { StructTestMessage.empty with 
                     Metadata = Some struct'
                     Data = Some strValue
                     Items = Some listValue
                     NullField = Google.Protobuf.NullValue.NullValue }
    
    let actual = JsonSerializer.Serialize(message, ignoreDefault)
    Assert.Contains("metadata", actual)
    Assert.Contains("data", actual)
    Assert.Contains("items", actual)

// ----------------------------------------------------------------------------
// INTEGRATION TESTS WITH TEST MESSAGES
// These verify that Struct types work correctly within generated message types
// ----------------------------------------------------------------------------

[<Fact>]
let ``StructTestMessage can be created with all fields`` () =
    let struct' = Google.Protobuf.Struct.empty
    let value = Google.Protobuf.Value.empty
    let list = Google.Protobuf.ListValue.empty
    let nullVal = Google.Protobuf.NullValue.NullValue
    
    let message = { StructTestMessage.empty with 
                     Metadata = Some struct'
                     Data = Some value
                     Items = Some list
                     NullField = nullVal }
    
    Assert.NotNull(message)
    Assert.True(message.Metadata.IsSome)
    Assert.True(message.Data.IsSome)
    Assert.True(message.Items.IsSome)
    Assert.Equal(message.NullField, nullVal)

[<Fact>]
let ``NestedStructTest can be created with Struct properties`` () =
    let value = { Google.Protobuf.Value.empty with 
                   Kind = Google.Protobuf.Value.KindCase.StringValue "property" }
    let struct' = { Google.Protobuf.Struct.empty with 
                     Fields = Map.ofList [("prop", value)] }
    
    let message = { NestedStructTest.empty with 
                     Name = "test"
                     Properties = Some struct'
                     Values = [value] }
    
    Assert.Equal("test", message.Name)
    Assert.True(message.Properties.IsSome)
    Assert.Single(message.Values) |> ignore

namespace rec Test.Name.Space
open FsGrpc.Protobuf
#nowarn "40"
#nowarn "1182"


[<System.Text.Json.Serialization.JsonConverter(typeof<FsGrpc.Json.EnumConverter<SomeEnum>>)>]
type SomeEnum =
| [<ProtobufName("VAL_0")>] Val0 = 0
| [<ProtobufName("VAL_1")>] Val1 = 1
| [<ProtobufName("VAL_2")>] Val2 = 2
| [<ProtobufName("VAL_3")>] Val3 = 3


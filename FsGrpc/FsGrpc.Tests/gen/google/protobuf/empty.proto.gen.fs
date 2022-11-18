namespace rec Google.Protobuf
open FsGrpc.Protobuf
#nowarn "40"
#nowarn "1182"


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Empty =

    [<System.Runtime.CompilerServices.IsByRefLike>]
    type Builder =
        struct
        end
        with
        member x.Put ((tag, reader): int * Reader) =
            match tag with
            | _ -> reader.SkipLastField()
        member x.Build = Empty.empty

[<StructuralEquality;StructuralComparison>]
type Empty = | Unused
    with
    static member empty = Unused 
    static member Proto : Lazy<ProtoDef<Empty>> =
        lazy
        // Proto Definition Implementation
        { // ProtoDef<Empty>
            Name = "Empty"
            Empty = Empty.empty
            Size = fun (m: Empty) ->
                0
            Encode = fun (w: Google.Protobuf.CodedOutputStream) (m: Empty) ->
                ()
            Decode = fun (r: Google.Protobuf.CodedInputStream) ->
                let mutable tag = 0
                while read r &tag do
                    r.SkipLastField()
                Empty.empty
            EncodeJson = fun _ _ _ -> ()
            DecodeJson = fun _ -> Empty.empty
        }


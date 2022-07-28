module FsGrpc.Optics 

open FSharp.Core
open FSharp.Collections
open System.Collections.Generic

type IFold<'s,'a> =
    abstract member ToSeq : 's -> seq<'a>
    abstract member ComposeWith<'b> : IFold<'a,'b> -> IFold<'s,'b>

type Fold<'s,'a> =
    {
        _toSeq : 's -> seq<'a>
    }
    interface IFold<'s,'a> with
        member this.ToSeq s = this._toSeq s
        member this.ComposeWith<'b> (other : IFold<'a,'b>) = 
            { _toSeq = fun s -> this._toSeq s |> Seq.collect other.ToSeq }

type IGetter<'s, 'a> =
    inherit IFold<'s,'a>
    abstract member Get : 's -> 'a
    abstract member ComposeWith<'b> : IGetter<'a,'b> -> IGetter<'s,'b>

type Getter<'s,'a> =
    {
        _get : 's -> 'a
    }
    member this.CastAsFold () : IFold<'s,'a> = 
        { _toSeq = fun s -> Seq.singleton (this._get s) }
    interface IGetter<'s,'a> with
        member this.Get s = 
            this._get s
        member this.ComposeWith<'b> (other : IGetter<'a,'b>) =
            { _get = fun s -> s |> this._get |> other.Get }
    interface IFold<'s,'a> with
        member this.ToSeq s = 
            this.CastAsFold().ToSeq s
        member this.ComposeWith<'b> (other : IFold<'a,'b>) =
            this.CastAsFold().ComposeWith(other)

type ISetter<'s,'t,'a,'b> =
    abstract member Over : ('a -> 'b) -> 's -> 't
    abstract member ComposeWith<'c,'d> : ISetter<'a,'b,'c,'d> -> ISetter<'s,'t,'c,'d>

type ISetter'<'s,'a> = ISetter<'s,'s,'a,'a>

type Setter<'s,'t,'a,'b> =
    {
        _over : ('a -> 'b) -> 's -> 't
    }
    interface ISetter<'s,'t,'a,'b> with
        member this.Over a2b s = this._over a2b s
        member this.ComposeWith (other : ISetter<'a,'b,'c,'d>) =
            { _over = fun c2d s -> this._over (fun a -> other.Over c2d a) s }

type ITraversal<'s,'t,'a,'b> =
    inherit ISetter<'s,'t,'a,'b>
    inherit IFold<'s,'a>
    abstract member ComposeWith<'c,'d> : ITraversal<'a,'b,'c,'d> -> ITraversal<'s,'t,'c,'d>

type ITraversal'<'s,'a> = ITraversal<'s,'s,'a,'a>

type Traversal<'s,'t,'a,'b> =
    {
        _fold : IFold<'s,'a>
        _setter : ISetter<'s,'t,'a,'b>
    }
    interface ITraversal<'s,'t,'a,'b> with
        member this.ComposeWith (other : ITraversal<'a,'b,'c,'d>) : ITraversal<'s,'t,'c,'d> =
            { 
                _fold = this._fold.ComposeWith other 
                _setter = this._setter.ComposeWith other
            }
    interface ISetter<'s,'t,'a,'b> with
        member this.Over a2b s = this._setter.Over a2b s
        member this.ComposeWith (other : ISetter<'a,'b,'c,'d>) = this._setter.ComposeWith other
    interface IFold<'s,'a> with
        member this.ToSeq s = this._fold.ToSeq s
        member this.ComposeWith (other : IFold<'a,'c>) : IFold<'s,'c> = this._fold.ComposeWith other
    
type ILens<'s,'t,'a,'b> =
    inherit ITraversal<'s,'t,'a,'b>
    inherit ISetter<'s,'t,'a,'b>
    inherit IGetter<'s,'a>
    inherit IFold<'s,'a>
    abstract member ComposeWith<'c,'d> : ILens<'a,'b,'c,'d> -> ILens<'s,'t,'c,'d>

type ILens'<'s,'a> = ILens<'s,'s,'a,'a>

type Lens<'s,'t,'a,'b> =
    {
        _getter : IGetter<'s,'a>
        _setter : ISetter<'s,'t,'a,'b>
    }
    interface ILens<'s,'t,'a,'b> with
        member this.ComposeWith (other : ILens<'a,'b,'c,'d>) : ILens<'s,'t,'c,'d> =
            { 
                _getter = this._getter.ComposeWith other 
                _setter = this._setter.ComposeWith other
            }
    interface ITraversal<'s,'t,'a,'b> with
        member this.ComposeWith (other : ITraversal<'a,'b,'c,'d>) : ITraversal<'s,'t,'c,'d> =
            { 
                _fold = this._getter.ComposeWith other 
                _setter = this._setter.ComposeWith other
            }
    interface ISetter<'s,'t,'a,'b> with
        member this.Over a2b s = this._setter.Over a2b s
        member this.ComposeWith (other : ISetter<'a,'b,'c,'d>) = this._setter.ComposeWith other
    interface IFold<'s,'a> with
        member this.ToSeq s = this._getter.ToSeq s
        member this.ComposeWith (other : IFold<'a,'c>) : IFold<'s,'c> = this._getter.ComposeWith other
    interface IGetter<'s,'a> with
        member this.Get s = this._getter.Get s
        member this.ComposeWith (other : IGetter<'a,'c>) : IGetter<'s,'c> = this._getter.ComposeWith other

type IPrism<'s,'t,'a,'b> =
    inherit ITraversal<'s,'t,'a,'b>
    inherit ISetter<'s,'t,'a,'b>
    inherit IFold<'s,'a>
    abstract member Which : 's -> Result<'a,'t>
    abstract member Unto : 'b -> 't
    abstract member ComposeWith<'c,'d> : IPrism<'a,'b,'c,'d> -> IPrism<'s,'t,'c,'d>

type IPrism'<'s,'a> = IPrism<'s,'s,'a,'a>

type Prism<'s,'t,'a,'b> =
    {
        _which : 's -> Result<'a,'t>
        _unto : 'b -> 't
    }
    member this._over (a2b : 'a -> 'b) (s : 's)  =
        match this._which s with
        | Error t -> t
        | Ok a -> this._unto (a2b a)
    member this._toSeq s = 
            match this._which s with
            | Error t -> Seq.empty
            | Ok a -> Seq.singleton a
    interface IPrism<'s,'t,'a,'b> with
        member this.Which s = this._which s
        member this.Unto b = this._unto b
        member this.ComposeWith (other : IPrism<'a,'b,'c,'d>) = 
            {
                _unto = fun d -> other.Unto d |> this._unto
                _which = fun s ->
                    match this._which s with
                    | Error t -> Error t
                    | Ok a -> 
                        match other.Which a with
                        | Error b -> Error (this._unto b)
                        | Ok c -> Ok c
            }
    interface ITraversal<'s,'t,'a,'b> with
        member this.ComposeWith (other : ITraversal<'a,'b,'c,'d>) =
            { 
                _setter = ({ _over = this._over } :> ISetter<'s,'t,'a,'b>).ComposeWith other
                _fold = ({ _toSeq = this._toSeq } :> IFold<'s,'a>).ComposeWith other
            }
    interface ISetter<'s,'t,'a,'b> with
        member this.Over a2b s = this._over a2b s
        member this.ComposeWith (other : ISetter<'a,'b,'c,'d>) = 
            ({ _over = this._over } :> ISetter<'s,'t,'a,'b>).ComposeWith other
    interface IFold<'s,'a> with
        member this.ToSeq s = this._toSeq s
        member this.ComposeWith (other : IFold<'a,'c>) : IFold<'s,'c> = 
            ({ _toSeq = this._toSeq } :> IFold<'s,'a>).ComposeWith other

module OptionPrism =
    let ifSome : Prism<Option<'a>,Option<'a>,'a,'a> =
        {
            _unto = fun a -> Some a
            _which = fun s ->
                match s with
                | Some a -> Ok a
                | _ -> Error s
        } : Prism<Option<'a>,Option<'a>,'a,'a>

open System.Runtime.CompilerServices

[<Extension>]
type OpticsExtensions =
    [<Extension>]
    static member inline Set(setter: ISetter<'s,'t,'a,'b>, x : 'b) = fun s -> setter.Over (fun _ -> x) s

    [<Extension>]
    static member inline Filtered(traversal: ITraversal<'s,'t,'a,'a>, predicate: 'a -> bool) : ITraversal<_,_,_,_> =
        {
            _fold = { _toSeq = fun s -> 
                traversal.ToSeq s |> Seq.filter predicate } :> IFold<_,_>
            _setter = { _over = fun a2a s -> 
                traversal.Over (fun a -> if predicate a then a2a a else a) s } :> ISetter<_,_,_,_>
        }

    [<Extension>]
    static member inline Each(traversal: ITraversal<'s,'t,list<'a>,list<'a>>) : ITraversal<'s,'t,'a,'a> =
        {
            _fold = { _toSeq = fun s -> traversal.ToSeq s |> Seq.concat } :> IFold<'s,'a>
            _setter = { _over = fun a2b s -> traversal.Over (List.map a2b) s } :> ISetter<'s,'t,'a,'a>
        }

    [<Extension>]
    static member inline Each(traversal: ITraversal<'s,'t,Set<'a>,Set<'a>>) : ITraversal<'s,'t,'a,'a> =
        {
            _fold = { _toSeq = fun s -> traversal.ToSeq s |> Seq.concat } :> IFold<'s,'a>
            _setter = { _over = fun a2b s -> traversal.Over (Set.map a2b) s } :> ISetter<'s,'t,'a,'a>
        }
    
    [<Extension>]
    static member inline Each(traversal: ITraversal<'s,'t,seq<'a>,seq<'a>>) : ITraversal<'s,'t,'a,'a> =
        {
            _fold = { _toSeq = fun s -> traversal.ToSeq s |> Seq.concat } :> IFold<'s,'a>
            _setter = { _over = fun a2b s -> traversal.Over (Seq.map a2b) s } :> ISetter<'s,'t,'a,'a>
        }

    [<Extension>]
    static member inline Member(traversal: ITraversal<'s,'t,Map<'a,'b>,Map<'a,'b>>, key: 'a) : ITraversal<'s,'t,'b,'b> =
        {
            _fold = { _toSeq = fun s -> 
                traversal.ToSeq s
                |> Seq.collect(fun m ->
                    match Map.tryFind key m with
                    | None -> Seq.empty
                    | Some x -> Seq.singleton x) } :> IFold<'s,'b>
            _setter = { _over = fun a2b s ->
                s |> traversal.Over (fun m -> 
                    match Map.tryFind key m with
                    | None -> m
                    | Some v -> Map.add key (a2b v) m) } :> ISetter<'s,'t,'b,'b>
        }

    [<Extension>]
    static member inline IfSome(prism: IPrism<'s,'t,Option<'a>,Option<'a>>) : IPrism<'s,'t,'a,'a> =
        prism.ComposeWith(OptionPrism.ifSome)
        
    [<Extension>]
    static member inline IfSome(traversal: ITraversal<'s,'t,Option<'a>,Option<'a>>) : ITraversal<'s,'t,'a,'a> =
        traversal.ComposeWith(OptionPrism.ifSome)
        
    [<Extension>]
    static member inline Exists(fold: IFold<'s,'a>, predicate: 'a -> bool) =
        fun s -> fold.ToSeq s |> Seq.exists predicate

    [<Extension>]
    static member inline Exists(fold: IFold<'s,'a>) =
        fold.Exists(fun _ -> true)
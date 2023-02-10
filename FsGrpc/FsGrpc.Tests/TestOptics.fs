module TestOptics
open FsGrpc.Optics
open FsGrpc
open Test.Name.Space
open Test.Name.Space.Enums
open Xunit

[<Fact>]
let ``Lens.id works`` () =
    let (_, case) = TestCases.Value1
    let result = case |> idLens.Set case
    Assert.Equal(snd TestCases.Value1, result)

[<Fact>]
let ``Lens.id works (new type)`` () =
    let (_, case) = TestCases.Value1
    let result = case |> idLens.Set 5
    Assert.Equal(5, result)

[<Fact>]
let ``Get works`` () =
    let (_, case) = TestCases.Value1
    let result = case |> idLens.TestBytes().Get
    Assert.Equal((snd TestCases.Value1).TestBytes, result)

[<Fact>]
let ``Set works`` () =
    let (_, case) = TestCases.Value1
    let result = case |> idLens.TestInt64().Set 12345
    Assert.Equal({ case with TestInt64 = 12345 }, result)
    
[<Fact>]
let ``Over works`` () =
    let (_, case) = TestCases.Value1
    let result = case |> idLens.TestInt64().Over (fun x -> x + 1L)
    Assert.Equal({ case with TestInt64 = case.TestInt64 + 1L }, result)
    
[<Fact>]
let ``Prism.Which works when value present`` () =
    let result = TestCases.Value5.Union |> Optics.Enums.UnionPrisms.ifName.Which
    Assert.Equal(Ok "green", result)

[<Fact>]
let ``Prism.Which fails in alternate case`` () =
    let result = TestCases.Value5.Union |> Optics.Enums.UnionPrisms.ifColor.Which
    Assert.Equal(Error TestCases.Value5.Union, result)
    
[<Fact>]
let ``Prism set works when value present`` () =
    let expected = 
        { TestCases.Value5 with Union = UnionCase.Name "red" }
    let result = TestCases.Value5 |> idLens.Union().IfName().Set "red"
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Prism set does nothing in alternate case`` () =
    let result = TestCases.Value5 |> Optics.Enums.union.IfColor().Set Color.Red
    Assert.Equal(TestCases.Value5, result)
    
[<Fact>]
let ``Each works (toSeq)`` () =
    let result = TestCases.Value5 |> idLens.OtherColors().Each().ToSeq
    Assert.Equal(Seq.toList TestCases.Value5.OtherColors, result)
    
[<Fact>]
let ``Each works (set)`` () =
    let expected = { TestCases.Value5 with OtherColors = [Color.Black; Color.Black; Color.Black] }
    let result = TestCases.Value5 |> idLens.OtherColors().Each().Set Color.Black
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Each works (array)`` () =
    let expected = [|2; 3; 4|]
    let result: int array = [|1; 2; 3|] |> idLens<int array,_>.Each().Over(fun x -> x + 1)
    Assert.Equal<int array>(expected, result)
    
[<Fact>]
let ``Each works (array, new type)`` () =
    let expected = [|"1"; "2"; "3"|]
    let result: string array = [|1; 2; 3|] |> idLens<int array,string array>.Each().Over(string)
    Assert.Equal<string array>(expected, result)
    
[<Fact>]
let ``Each works (map)`` () =
    let expected = Map.ofArray [|(1,"a"); (2,"b"); (3,"c")|]
    let result: Map<int,string> = Map.ofArray [|("1", 'a'); ("2", 'b'); ("3", 'c')|] |> idLens<Map<string,char>,_>.Each().Over(fun (k,v) -> (int k, string v))
    Assert.Equal<Map<int,string>>(expected, result)
    
[<Fact>]
let ``EachValue works (map)`` () =
    let expected = Map.ofArray [|(1,"a"); (2,"b"); (3,"c")|]
    let result: Map<int,string> = Map.ofArray [|(1, 'a'); (2, 'b'); (3, 'c')|] |> idLens<Map<int,char>,_>.EachValue().Over(fun v -> string v)
    Assert.Equal<Map<int,string>>(expected, result)
    
[<Fact>]
let ``Each works (seq)`` () =
    let expected = seq { yield 2; yield 3; yield 4 }
    let result = seq { yield 1; yield 2; yield 3 } |> idLens<int seq,_>.Each().Over(fun x -> x + 1)
    Assert.True(System.Linq.Enumerable.SequenceEqual(expected, result))
    
[<Fact>]
let ``Filtered works (toSeq)`` () =
    let expected : Color list = [Color.Blue]
    let result : Color list = TestCases.Value5 |> idLens.OtherColors().Each().Filtered(fun x -> x = Color.Blue).ToSeq |> Seq.toList
    Assert.Equal<Color list>(expected, result)
    
[<Fact>]
let ``Filtered works (set)`` () =
    let expected = { TestCases.Value5 with OtherColors = [Color.Black; Color.Red; Color.Black] }
    let result = TestCases.Value5 |> idLens.OtherColors().Each().Filtered(fun x -> x = Color.Blue).Set Color.Black
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Member works (toSeq)`` () =
    let expected = [Color.Red]
    let result = TestCases.Value5 |> Optics.Enums.byName.Member("red").ToSeq
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Member alternate case works (toSeq)`` () =
    let expected = List.empty
    let result = TestCases.Value5 |> Optics.Enums.byName.Member("green").ToSeq
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Member works (set)`` () =
    let expected = { TestCases.Value5 with ByName = Map.add "red" Color.Blue TestCases.Value5.ByName }
    let result = TestCases.Value5 |> Optics.Enums.byName.Member("red").Set Color.Blue
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Member alternate case works (set)`` () =
    let result = TestCases.Value5 |> Optics.Enums.byName.Member("green").Set Color.Blue
    Assert.Equal(TestCases.Value5, result)
    
[<Fact>]
let ``Option prism works (Some case)`` () =
    let expected = { TestCases.Value3 with Inner = Some { InnerName = "asdf" } }
    let result = TestCases.Value3 |> Optics.Nest.inner.IfSome().InnerName().Set "asdf"
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Option prism works (None case)`` () =
    let expected = { TestCases.Value3 with Inner = None }
    let result = { TestCases.Value3 with Inner = None } |> Optics.Nest.inner.IfSome().InnerName().Set "asdf"
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Result prism works (Ok case, match)`` () =
    let expected = Ok 2
    let result = Ok 1 |> idLens.IfOk().Over(fun x -> x + 1)
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Result prism works (Ok case, non-match)`` () =
    let expected = Error "asdf"
    let result = Error "asdf" |> idLens.IfOk().Over(fun x -> x + 1)
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Result prism works (Error case, match)`` () =
    let expected = Error "error"
    let result = Error "failure" |> idLens.IfError().Set "error"
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Result prism works (Error case, non-match)`` () =
    let expected = Ok 1
    let result = Ok 1 |> idLens.IfError().Set "error"
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Result prism works (Error case, match, composed)`` () =
    let expected = Ok (Error "error")
    let result = Ok (Error "failure") |> (Optics.ResultPrism.ifOk :> IPrism<_,_,_,_>).IfError().Set "error"
    Assert.Equal(expected, result)

[<Fact>]
let ``Result prism works (Error case, non-match, composed)`` () =
    let expected = Error (Error "failure")
    let result = Error (Error "failure") |> (Optics.ResultPrism.ifOk :> IPrism<_,_,_,_>).IfError().Set "error"
    Assert.Equal(expected, result)

[<Fact>]
let ``Result prism ToSeq works (failure case)`` () =
    let expected = []
    let result = Error "failure" |> (Optics.ResultPrism.ifOk :> IPrism<_,_,int,_>).ToSeq |> List.ofSeq
    Assert.Equal<int list>(expected, result)

[<Fact>]
let ``Prism ComposeWith function works (success case)`` () =
    let expected = Ok (Some 6)
    let result = Ok (Some 5) |> idLens.IfOk().ComposeWith(Optics.OptionPrism.ifSome).Over(fun x -> x + 1)
    Assert.Equal(expected, result)

[<Fact>]
let ``Prism ComposeWith function works (failure case)`` () =
    let expected = Error (Ok None)
    let result = Ok None |> (Optics.ResultPrism.ifOk :> IPrism<_,_,_,_>).IfSome().Which
    Assert.Equal(expected, result) 

[<Fact>]
let ``Prism ComposeWith<IFold<_,_>...> function works`` () =
    let expected = [5]
    let result = Ok (Some 5) |> (Optics.ResultPrism.ifOk :> IPrism<_,_,_,_>).ComposeWith(Optics.OptionPrism.ifSome : IFold<_,_>).ToSeq |> List.ofSeq
    Assert.Equal<int list>(expected, result) 

[<Fact>]
let ``Prism ComposeWith<ISetter<_,_,_,_>...> function works`` () =
    let expected = Ok (Some 6)
    let result = Ok (Some 5) |> (Optics.ResultPrism.ifOk :> IPrism<_,_,_,_>).ComposeWith(Optics.OptionPrism.ifSome : ISetter<_,_,_,_>).Set 6
    Assert.Equal(expected, result) 

[<Fact>]
let ``Prism ComposeWith<ITraversal<_,_,_,_>...> function works`` () =
    let expected = Ok (Some 6)
    let result = Ok (Some 5) |> (Optics.ResultPrism.ifOk :> IPrism<_,_,_,_>).ComposeWith(Optics.OptionPrism.ifSome : ITraversal<_,_,_,_>).Set 6 
    Assert.Equal(expected, result) 

[<Fact>]
let ``fstLens works (setter)`` () =
    let expected = (2, "asdf")
    let result = (1, "asdf") |> Optics.fstLens.Over (fun x -> x+1) 
    Assert.Equal(expected, result) 

[<Fact>]
let ``fstLens works (setter w/ new type)`` () =
    let expected = (1L, "asdf")
    let result = (1, "asdf") |> Optics.fstLens.Over int64
    Assert.Equal(expected, result) 

[<Fact>]
let ``fstLens works (getter)`` () =
    let expected = 1
    let result = (1, "asdf") |> Optics.fstLens.Get
    Assert.Equal(expected, result) 

[<Fact>]
let ``sndLens works (setter)`` () =
    let expected = (1, "fdsa")
    let result = (1, "asdf") |> Optics.sndLens.Over (fun s -> new string(s.ToCharArray() |> Array.rev)) 
    Assert.Equal(expected, result) 

[<Fact>]
let ``sndLens works (setter w/ new type)`` () =
    let expected = (1, 4)
    let result = (1, "asdf") |> Optics.sndLens.Over String.length
    Assert.Equal(expected, result) 

[<Fact>]
let ``sndLens works (setter w/ new type, nested)`` () =
    let expected = ("qwerty", (1, 4))
    let result = ("qwerty", (1, "asdf")) |> sndLens.ComposeWith(sndLens).Over String.length
    Assert.Equal(expected, result) 

[<Fact>]
let ``sndLens works (setter w/ new type, nested, using '>>' operator)`` () =
    let expected = ("qwerty", (1, 4))
    let result = ("qwerty", (1, "asdf")) |> (sndLens.Over >> sndLens.Over) String.length 
    Assert.Equal(expected, result) 

[<Fact>]
let ``sndLens works (getter)`` () =
    let expected = "asdf"
    let result = (1, "asdf") |> Optics.sndLens.Get
    Assert.Equal(expected, result) 

[<Fact>]
let ``Exists works`` () =
    let input = [1; 2; 5]
    Assert.Equal(false, [] |> idLens<_ list,_>.Each().Exists()) 
    Assert.Equal(true, input |> idLens<_ list,_>.Each().Exists()) 
    Assert.Equal(true, input |> idLens<_ list,_>.Each().Exists(fun x -> x = 1)) 
    Assert.Equal(false, input |> idLens<_ list,_>.Each().Exists(fun x -> x = 3)) 

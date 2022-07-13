module TestOptics
open  FsGrpc.Optics
open FsGrpc
open Test.Name.Space
open Test.Name.Space.Enums
open Xunit

[<Fact>]
let ``Lens.id works`` () =
    let (_, case) = TestCases.Value1
    let result = case |> Optics.TestMessage._id.Set case
    Assert.Equal(snd TestCases.Value1, result)

[<Fact>]
let ``Get works`` () =
    let (_, case) = TestCases.Value1
    let result = case |> Optics.TestMessage._id.TestBytes().Get
    Assert.Equal((snd TestCases.Value1).TestBytes, result)

[<Fact>]
let ``Set works`` () =
    let (_, case) = TestCases.Value1
    let result = case |> Optics.TestMessage._id.TestInt64().Set 12345
    Assert.Equal({ case with TestInt64 = 12345 }, result)
    
[<Fact>]
let ``Over works`` () =
    let (_, case) = TestCases.Value1
    let result = case |> Optics.TestMessage._id.TestInt64().Over (fun x -> x + 1L)
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
    let result = TestCases.Value5 |> Optics.Enums.union.IfName().Set "red"
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Prism set does nothing in alternate case`` () =
    let result = TestCases.Value5 |> Optics.Enums.union.IfColor().Set Color.Red
    Assert.Equal(TestCases.Value5, result)
    
[<Fact>]
let ``Each works (toSeq)`` () =
    let result = TestCases.Value5 |> Optics.Enums._id.OtherColors().Each().ToSeq
    Assert.Equal(Seq.toList TestCases.Value5.OtherColors, result)
    
[<Fact>]
let ``Each works (set)`` () =
    let expected = { TestCases.Value5 with OtherColors = [Color.Black; Color.Black; Color.Black] }
    let result = TestCases.Value5 |> Optics.Enums._id.OtherColors().Each().Set Color.Black
    Assert.Equal(expected, result)
    
[<Fact>]
let ``Filtered works (toSeq)`` () =
    let expected : Color list = [Color.Blue]
    let result : Color list = TestCases.Value5 |> Optics.Enums._id.OtherColors().Each().Filtered(fun x -> x = Color.Blue).ToSeq |> Seq.toList
    Assert.Equal<Color list>(expected, result)
    
[<Fact>]
let ``Filtered works (set)`` () =
    let expected = { TestCases.Value5 with OtherColors = [Color.Black; Color.Red; Color.Black] }
    let result = TestCases.Value5 |> Optics.Enums._id.OtherColors().Each().Filtered(fun x -> x = Color.Blue).Set Color.Black
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
let ``Option prism works`` () =
    let expected = { TestCases.Value3 with Inner = Some { InnerName = "asdf" } }
    let result = TestCases.Value3 |> Optics.Nest.inner.IfSome().InnerName().Set "asdf"
    Assert.Equal(expected, result)
    
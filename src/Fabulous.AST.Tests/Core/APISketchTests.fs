namespace Fabulous.AST.Tests.Core

open Fantomas.Core
open NUnit.Framework

open Fabulous.AST

open type Fabulous.AST.Ast

module APISketchTests =

    [<Test>]
    let ``Multiple Widgets for loops in builder`` () =
        let result =
            Namespace("DummyNamespace").isRecursive() {
                Abbrev("Foo", CommonType.string)
                Abbrev("bar", CommonType.string)

                for i = 0 to 10 do
                    Abbrev($"T{i}", CommonType.string)

                for i = 10 to 20 do
                    Abbrev($"T{i}", CommonType.string)

                for i = 20 to 30 do
                    Abbrev($"T{i}", CommonType.string)
            }
            |> Tree.compile
            |> CodeFormatter.FormatOakAsync
            |> Async.RunSynchronously

        Assert.NotNull result

    [<Test>]
    let ``Multiple for loops in builder`` () =
        let result =
            Namespace("DummyNamespace").isRecursive() {
                for i = 0 to 10 do
                    Abbrev($"T{i}", CommonType.string)

                for i = 10 to 20 do
                    Abbrev($"T{i}", CommonType.string)

                for i = 20 to 30 do
                    Abbrev($"T{i}", CommonType.string)
            }
            |> Tree.compile
            |> CodeFormatter.FormatOakAsync
            |> Async.RunSynchronously

        Assert.NotNull result

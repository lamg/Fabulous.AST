namespace Fabulous.AST.Tests.ModuleDeclarations

open Fabulous.AST.Tests
open Fantomas.FCS.Text
open Fantomas.Core.SyntaxOak
open Xunit

open Fabulous.AST

open type Ast

module NestedModule =

    [<Fact>]
    let ``Produces a NestedModule``() =
        AnonymousModule() { NestedModule("A") { Value("x", "12", false) } }

        |> produces
            """

module A =
    let x = 12

"""

    [<Fact>]
    let ``Produces a NestedModule using escape hatch``() =
        AnonymousModule() {
            NestedModule("A") {
                BindingNode(
                    None,
                    None,
                    MultipleTextsNode([ SingleTextNode("let", Range.Zero) ], Range.Zero),
                    false,
                    None,
                    None,
                    Choice1Of2(IdentListNode([ IdentifierOrDot.Ident(SingleTextNode("x", Range.Zero)) ], Range.Zero)),
                    None,
                    List.Empty,
                    None,
                    SingleTextNode("=", Range.Zero),
                    Expr.Constant(Constant.FromText(SingleTextNode("12", Range.Zero))),
                    Range.Zero
                )
            }
        }

        |> produces
            """

module A =
    let x = 12

"""

    [<Fact>]
    let ``Produces a recursive NestedModule``() =
        AnonymousModule() { NestedModule("A") { Value("x", "12", false) } |> _.toRecursive() }

        |> produces
            """

module rec A =
    let x = 12

"""

    [<Fact>]
    let ``Produces a private NestedModule``() =
        AnonymousModule() { NestedModule("A") { Value("x", "12", false) } |> _.toPrivate() }
        |> produces
            """

module private A =
    let x = 12

"""

    [<Fact>]
    let ``Produces a internal NestedModule``() =
        AnonymousModule() { NestedModule("A") { Value("x", "12", false) } |> _.toInternal() }
        |> produces
            """

module internal A =
    let x = 12
    
"""

    [<Fact>]
    let ``Produces a module with nested module``() =
        Module("Fabulous.AST") {
            NestedModule("Foo") {
                BindingNode(
                    None,
                    None,
                    MultipleTextsNode([ SingleTextNode("let", Range.Zero) ], Range.Zero),
                    false,
                    None,
                    None,
                    Choice1Of2(IdentListNode([ IdentifierOrDot.Ident(SingleTextNode("x", Range.Zero)) ], Range.Zero)),
                    None,
                    List.Empty,
                    None,
                    SingleTextNode("=", Range.Zero),
                    Expr.Constant(Constant.FromText(SingleTextNode("12", Range.Zero))),
                    Range.Zero
                )
            }
        }

        |> produces
            """
module Fabulous.AST

module Foo =
    let x = 12
"""

    [<Fact>]
    let ``Produces a module with multiple nested module``() =
        Module("Fabulous.AST") {
            NestedModule("Foo") {
                BindingNode(
                    None,
                    None,
                    MultipleTextsNode([ SingleTextNode("let", Range.Zero) ], Range.Zero),
                    false,
                    None,
                    None,
                    Choice1Of2(IdentListNode([ IdentifierOrDot.Ident(SingleTextNode("x", Range.Zero)) ], Range.Zero)),
                    None,
                    List.Empty,
                    None,
                    SingleTextNode("=", Range.Zero),
                    Expr.Constant(Constant.FromText(SingleTextNode("12", Range.Zero))),
                    Range.Zero
                )
            }

            NestedModule("Bar") { Value("x", "12", false) }
        }

        |> produces
            """
module Fabulous.AST

module Foo =
    let x = 12

module Bar =
    let x = 12

"""

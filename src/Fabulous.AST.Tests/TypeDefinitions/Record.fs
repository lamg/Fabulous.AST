namespace Fabulous.AST.Tests.TypeDefinitions

open Fantomas.FCS.Text
open Fabulous.AST.Tests
open Fantomas.Core.SyntaxOak
open NUnit.Framework

open Fabulous.AST
open type Ast

module Record =
    [<Test>]
    let ``Produces a record`` () =

        AnonymousModule() {
            Record("Colors") {
                Field("Red", CommonType.int)
                Field("Green", CommonType.int)
                Field("Blue", CommonType.int)
            }
        }
        |> produces
            """

type Colors = { Red: int; Green: int; Blue: int }

"""

    [<Test>]
    let ``Produces a record using EscapeHatch`` () =
        let customField =
            FieldNode(
                Some(XmlDocNode([| "/// Super cool doc bro" |], Range.Zero)),
                None,
                None,
                false,
                None,
                Some(SingleTextNode("Green", Range.Zero)),
                CommonType.int,
                Range.Zero
            )

        AnonymousModule() {
            Record("Colors") {
                Field("Red", CommonType.int)
                EscapeHatch(customField)
                Field("Blue", CommonType.int)
            }
        }
        |> produces
            """

type Colors =
    {
        Red: int
        /// Super cool doc bro
        Green: int
        Blue: int
    }

"""

    [<Test>]
    let ``Produces a record using a loop`` () =
        AnonymousModule() {
            Record("Colors") {
                for colour in [ "Red"; "Green"; "Blue" ] do
                    Field(colour, CommonType.int)
            }
        }
        |> produces
            """

type Colors = { Red: int; Green: int; Blue: int }

"""

    [<Test>]
    let ``Produces a record with member`` () =
        let memberNode =
            MemberDefn.Member(
                BindingNode(
                    None,
                    None,
                    MultipleTextsNode([ SingleTextNode("member", Range.Zero) ], Range.Zero),
                    false,
                    None,
                    None,
                    Choice1Of2(
                        IdentListNode(
                            [ IdentifierOrDot.Ident(SingleTextNode("this", Range.Zero))
                              IdentifierOrDot.Ident(SingleTextNode(".", Range.Zero))
                              IdentifierOrDot.Ident(SingleTextNode("A", Range.Zero)) ],
                            Range.Zero
                        )
                    ),
                    None,
                    List.Empty,
                    None,
                    SingleTextNode("=", Range.Zero),
                    Expr.Constant(Constant.FromText(SingleTextNode("\"\"", Range.Zero))),
                    Range.Zero
                )
            )

        AnonymousModule() {
            (Record("Colors") { Field("X", CommonType.string) })
                .members() {
                EscapeHatch(memberNode)
            }
        }
        |> produces
            """

type Colors =
    { X: string }

    member this.A = ""

"""

    [<Test>]
    let ``Produces a record with an attribute`` () =
        AnonymousModule() {
            (Record("Colors") {
                for colour in [ "Red"; "Green"; "Blue" ] do
                    Field(colour, CommonType.int)
            })
                .attributes([ "Serializable" ])
        }
        |> produces
            """

[<Serializable>]
type Colors = { Red: int; Green: int; Blue: int }

"""

    [<Test>]
    let ``Produces a record field with an attribute`` () =
        AnonymousModule() {
            Record("Colors") {
                Field("Red", CommonType.int).attributes([ "Obsolete" ])
                Field("Green", CommonType.int)
                Field("Blue", CommonType.int)
            }
        }
        |> produces
            """

type Colors =
    { [<Obsolete>]
      Red: int
      Green: int
      Blue: int }

"""

    [<Test>]
    let ``Produces a record with TypeParams`` () =
        AnonymousModule() {
            GenericRecord("Colors", [ "'other" ]) {
                Field("Green", CommonType.string)
                Field("Blue", CommonType.mkType("'other"))
                Field("Yellow", CommonType.int)
            }
        }

        |> produces
            """

type Colors<'other> =
    { Green: string
      Blue: 'other
      Yellow: int }

"""

    [<Test>]
    let ``Produces a  struct record with TypeParams`` () =
        AnonymousModule() {
            (GenericRecord("Colors", [ "'other" ]) {
                Field("Green", CommonType.string)
                Field("Blue", CommonType.mkType("'other"))
                Field("Yellow", CommonType.int)
            })
                .isStruct()
        }

        |> produces
            """
[<Struct>]
type Colors<'other> =
    { Green: string
      Blue: 'other
      Yellow: int }

"""

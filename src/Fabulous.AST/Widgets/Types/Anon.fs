namespace Fabulous.AST

open Fantomas.Core.SyntaxOak
open Fabulous.AST.StackAllocatedCollections.StackList

module TypeAnon =
    let Identifier = Attributes.defineScalar<string> "Identifier"

    let WidgetKey =
        Widgets.register "TypeAnon" (fun widget ->
            let identifier = Widgets.getScalarValue widget Identifier
            Type.Anon(SingleTextNode.Create(identifier)))

[<AutoOpen>]
module TypeAnonBuilders =
    type Ast with
        static member Anon(value: string) =
            WidgetBuilder<Type>(
                TypeAnon.WidgetKey,
                AttributesBundle(StackList.one(TypeAnon.Identifier.WithValue(value)), ValueNone, ValueNone)
            )

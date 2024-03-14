
namespace Components

open Feliz
//open Feliz.

type GTtable =

    /// <summary>
    /// A stateful React component that maintains a counter
    /// </summary>
    [<ReactComponent>]
    static member Main() =
        Html.div [
            Html.button [
                //prop.className 
                prop.onClick (fun _ ->())
                prop.text "upload abstract"
            ]    
            ]
        
namespace Components

open Feliz

type NavBar =

    [<ReactComponent>]
    static member Main() = 
        Html.div "NavBar"
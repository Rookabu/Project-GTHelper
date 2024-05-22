namespace App

open Feliz

type View =
    [<ReactComponent>]
    static member Main() =
        let currentpage,setpage = React.useState(Types.Page.GTtable) //Reagiert beim clicken. Start state ist der Counter ->noch in Srat menü umändern
        printfn "%A" currentpage 
        Html.div [
            prop.children [ 
                Html.a [           
                    Components.NavBar.Subpagelink(setpage,currentpage)
                    //Components.NavBar.Counter(setpage, currentpage)
                ]
                match currentpage with
                |Types.Page.GTtable -> Components.GTtable.Main()
                |Types.Page.Contact -> Components.Contact.Main() 
                |Types.Page.Feedback -> Components.Feedback.Main()//wird gemachted da die jeweilige component aufgerufen werden soll je nach page (application state)
            ]
        ]
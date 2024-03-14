namespace Components

open Feliz

type GTtable =

    /// <summary>
    /// A stateful React component that maintains a counter
    /// </summary>
    [<ReactComponent>]
    static member Main() =
        let (count, setCount) = React.useState(0)
        Html.div [
            Html.h1 count
            Html.button [
                prop.onClick (fun _ -> setCount(count + 1))
                prop.text "Increment"
            ]    
            ]
        


    // <summary>
    // A React component that uses Feliz.Router
    // to determine what to show based on the current URL
    // </summary>
    // [<ReactComponent>]
    // static member Router() =
    //     let (currentUrl, updateUrl) = React.useState(Router.currentUrl())
    //     React.router [
    //         router.onUrlChanged updateUrl
    //         router.children [
    //             match currentUrl with
    //             | [ ] -> Html.h1 "Index"
    //             | [ "counter" ] -> Components.Counter()
    //             | otherwise -> Html.h1 "Not found"
    //         ]
    //     ]
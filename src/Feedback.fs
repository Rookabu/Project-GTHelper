namespace Components

open Feliz
open Feliz.DaisyUI

type Feedback =

    [<ReactComponent>]
    static member Main() =         
        Html.div [
            prop.className "childstyle"
            prop.style [style.marginTop 100]
            prop.children [
                Daisy.formControl [
                    Daisy.cardTitle [prop.text "Write me a message!"; prop.style [style.marginBottom 30]]
                    Daisy.textarea [
                        prop.placeholder "your feedback"
                        // prop.className "h-100"
                        textarea.bordered
                        prop.className "textarea-md max-w-xs"
                    ]
                ]
            ]
        ]

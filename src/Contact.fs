namespace Components

open Feliz
open Feliz.DaisyUI
type Contact =

    [<ReactComponent>]
    static member Main() =
        Html.div [
            prop.className "childstyle"
            prop.children [
                Daisy.card [
                    prop.style [
                        style.marginTop 100
                        style.maxWidth 700
                        style.fontSize 20
                    ] 
                    
                    prop.className "shadow-lg"
                    prop.className "textCard"
                    prop.children [
                        Daisy.cardBody [
                            Daisy.cardTitle [prop.text "Contact"; prop.style [style.fontSize 27; style.marginBottom 30]]
                            Html.p "Prof. Dr. Timo Mühlhaus"
                            Html.p [ prop.text "Computational Systems Biology"; prop.style [style.marginBottom 30]]
                            Html.p "RPTU University of Kaiserslautern"
                            Html.p "Erwin-Schrödinger-Str. 56 R244"
                            Html.p "67663 Kaiserslautern, Germany"  
                            Daisy.label [
                                prop.style [style.justifyContent.flexStart]
                                prop.children [
                                    Html.i [prop.className "fa-solid fa-phone"]
                                    Html.span [ prop.text "+ 49 631 205 4657"; prop.style [style.marginLeft 10]]
                                ]
                            ]   
                            Daisy.label [
                                prop.style [style.justifyContent.flexStart]
                                prop.children [
                                    Html.i [prop.className "fa-solid fa-fax"; prop.style [style.justifyContent.spaceBetween]]
                                    Html.span [ prop.text "+ 49 631 205 3799"; prop.style [style.marginLeft 10]]
                                ]
                            ]   
                            Html.p "Write me your feedback as an issue on"
                            Html.a [
                                prop.href "https://github.com/Rookabu/Project-GTHelper/issues"
                                prop.text "my github!" 
                                prop.target.blank 
                                prop.style [style.textDecoration.underline]
                            ] 
                                                         
                        ]
                    ]
                ]
            ]
        ]
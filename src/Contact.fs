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
                        style.textAlign.justify
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
                                prop.children [
                                    Html.i [prop.className "fa-solid fa-phone"]
                                    Html.span [ prop.text "+ 49 631 205 4657"]
                                ]
                            ]   
                            Daisy.label [
                                prop.children [
                                    Html.i [prop.className "fa-solid fa-fax"]
                                    Html.span [ prop.text "+ 49 631 205 3799"]
                                ]
                            ]   
                            // Daisy.cardActions [
                            //     prop.className "card-actions justify-center"
                            //     prop.style [style.marginTop 30]
                                
                            // ]                             
                        ]
                    ]
                ]
            ]
        ]
namespace Components

open Feliz
open Feliz.DaisyUI

type NavBar =
    static member Subpagelink(setPage: Types.Page -> unit, statePage: Types.Page) = 
        let logo = StaticFile.import "./img/csb-wide-white.svg"
        Daisy.navbar [
            prop.className "bg-base-200 shadow-lg bg-neutral text-neutral-content"
            prop.className "navbarColor"
            prop.children [
                Daisy.navbarStart [
                    Daisy.button.a [
                        prop.href "https://csbiology.github.io/"
                        prop.target.blank 
                        prop.children [
                            Html.img [
                                prop.src logo
                                prop.className "w-full h-full"
                            ]
                        ]
                    ]
                    Daisy.button.button [
                        prop.text "GroundTruth Helper"
                        prop.onClick (fun _ -> setPage(Types.Page.GTtable))
                        prop.className "button"
                        prop.style [style.marginLeft 15]
                    ]
                ]
                Daisy.navbarEnd [
                    Daisy.button.button [
                        prop.text "Contact"
                        prop.onClick (fun _ -> setPage(Types.Page.Contact))
                        prop.className "button"
                        prop.style [
                            style.marginRight 15
                        ]
                    ]
                    Daisy.button.a [
                        prop.href "https://github.com/Rookabu/Project-GTHelper"
                        prop.target.blank 
                        button.ghost
                        prop.style [style.paddingLeft 0; style.paddingRight 0] 
                        prop.children [
                            Html.i [prop.className "fa-brands fa-github"; prop.style [style.fontSize (length.rem 3)]]
                        ]
                    ]
                ]
            ]
        ]
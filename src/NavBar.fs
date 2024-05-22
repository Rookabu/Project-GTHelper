namespace Components

open Feliz
open Feliz.DaisyUI

type NavBar =
    static member Subpagelink(setPage: Types.Page -> unit, statePage: Types.Page) = 
        Daisy.navbar [
            prop.className "mb-2 shadow-lg bg-neutral text-neutral-content rounded-box"
            prop.children [
                Daisy.navbarStart [
                    Daisy.button.button [
                        button.square
                        button.ghost
                        prop.children [
                             Html.img [ prop.src "https://github.com/CSBiology/Branding/raw/main/logos/csb-tuk-black.svg"; prop.height 28; prop.width 112]
                        ]
                    ]
                    
                ]
                Daisy.navbarEnd [
                    Daisy.badge [
                        prop.text "Start"
                        prop.onClick (fun _ -> setPage(Types.Page.GTtable))
                    ]
                    Daisy.badge [
                        prop.text "Contact"
                        prop.onClick (fun _ -> setPage(Types.Page.Contact))
                    ]
                ]
            ]
        ]
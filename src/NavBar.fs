namespace Components

open Feliz
open Feliz.DaisyUI

module Helper =
    let DropDownElement (text: string, setPage, subpage: Types.Page) =
        Html.li [
            Html.a [
                prop.text text
                prop.onClick (fun _ -> 
                     setPage (subpage)
                )
                // prop.className "button"
            ]
        ]

type NavBar =
    static member Subpagelink(setPage: Types.Page -> unit, statePage: Types.Page) =
        Html.div [ 
            let logo = StaticFile.import "./img/csb-wide-white.svg"
            Daisy.navbar [
                prop.className "bg-base-100 shadow-lg navbarColor w-full"
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
                            prop.className "button hidden lg:flex"
                            prop.style [style.marginLeft 15]
                        ]
                    ]
                    Daisy.navbarEnd [
                        Daisy.button.button [
                            prop.text "Contact"
                            prop.onClick (fun _ -> setPage(Types.Page.Contact))
                            prop.className "button hidden lg:flex"
                            prop.style [
                                style.marginRight 15
                            ]
                        ]
                        Html.a [
                            prop.href "https://github.com/Rookabu/Project-GTHelper"
                            prop.target.blank 
                            button.ghost
                            prop.className "hidden lg:flex"
                            prop.style [style.paddingLeft 0; style.paddingRight 0] 
                            prop.children [
                                Html.i [prop.className "fa-brands fa-github"; prop.style [style.fontSize (length.rem 3)]]
                            ]
                        ]
                        Daisy.dropdown [
                            dropdown.hover
                            prop.className "dropdown dropdown-end"
                            prop.children [
                                Daisy.button.button [
                                    prop.className "btn btn-ghost lg:hidden"
                                    prop.children [
                                        Html.i [prop.className "fa-solid fa-bars"; prop.style [style.fontSize (length.rem 2)]]
                                    ]
                                ]
                                Daisy.dropdownContent [
                                    prop.tabIndex 0
                                    prop.className "p-1 shadow menu bg-[#222a35]"
                                    prop.style [
                                        style.width 145
                                        style.fontSize 16
                                        style.zIndex 10
                                        
                                    ]
                                    prop.children [
                                        Helper.DropDownElement ("GT-Helper", setPage, Types.Page.GTtable)
                                        Helper.DropDownElement ("Contact", setPage, Types.Page.Contact)
                                        Html.li [
                                            Html.a [
                                                prop.href "https://github.com/Rookabu/Project-GTHelper"
                                                prop.target.blank 
                                                prop.text "Github"
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]

module Main

open Feliz
open Components
open Browser.Dom
open Fable.Core.JsInterop

let root = ReactDOM.createRoot(document.getElementById "feliz-app")
root.render(GTtable.Main())
module Types

type InteractionType =
    |ProteinProtein
    |ProteineGene
    |Other of string

    member this.ToStringRdb() =
        match this with
        | ProteineGene -> "Protein-Gene"
        | ProteinProtein -> "Protein-Protein"
        | Other s -> "Other" 
    
    member this.ToOutput() =
        match this with
        | ProteineGene -> "Protein-Gene"
        | ProteinProtein -> "Protein-Protein"
        | Other s -> s


type Interaction = {
    Partner1: string
    Partner2: string
    InteractionType: InteractionType
}

type GTelement = {
    ///PaperTitle split by whitespace into single strings/words.
    Title: string list 
    ///PaperContent split by whitespace into single strings/words.
    Content: string list
}

[<RequireQualifiedAccess>]

type Page =
    |GTtable
    |Contact
    |Feedback
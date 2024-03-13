type InteractionType =
    |ProteinProtein
    |ProteineGene
    |Other of string

type Interaction = {
    Partner1: string
    Partner2: string
    InterType: InteractionType
}

type GTelement = {
    ///PaperTitle split by whitespace into single strings/words.
    Title: string list 
    ///PaperContent split by whitespace into single strings/words.
    Content: string list
    Interactions: Interaction list 
}
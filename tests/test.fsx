let content =
    "Distinct Clades of Protein Phosphatase 2A Regulatory

Protein phosphatase 2A (PP2A) is a strongly conserved and major protein phosphatase in all eukaryotes


Histone Deacetylase Complex

Early responses of plants to environmental stressX


WRKY transcription factors

WRKY transcription factors in plants are known to be able"

let publications (txt: string)= txt.Split([|'\n'|], System.StringSplitOptions.RemoveEmptyEntries)

publications content
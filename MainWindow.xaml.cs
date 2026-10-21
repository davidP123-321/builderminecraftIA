# Minecraft Builder Pro

Application WPF C# qui utilise Ollama pour générer un plan de schéma Minecraft à partir d'un prompt.

## Fonctionnalités

- interface utilisateur WPF
- génération de plan via Ollama
- vue 2D et 3D
- export JSON
- export d'un schéma `.schem` au format de structure exploitable
- moteur voxel avancé pour la génération de bâtiments

## Prérequis

- Windows
- .NET 8 SDK
- Ollama installé localement
- modèle Ollama : `llama3.1:8b`

## Lancement

```bash
ollama serve
ollama pull llama3.1:8b
dotnet restore
dotnet build
dotnet run
```

## Remarque

Cette version est une vraie base architecture de générateur de schémas Minecraft : plan JSON → template de bâtiment → voxel world → rendu 3D → export de structure. C'est une base solide pour une version encore plus avancée et plus proche d'un vrai outil de génération de schematics Minecraft.

## Exemple de prompt

- "Maison médiévale en pierre, 2 étages, 3 chambres, salon, cuisine, cave"
- "Base souterraine de survival avec atelier, four, coffres, chambre"
- "Château en pierre, grande salle, tours, cave, fenêtres, toiture imposante"

## Objectif suivant

- exporter un vrai format NBT Minecraft plus complet
- ajouter des templates plus nombreux (maison, château, ferme, base, temple)
- améliorer le rendu 3D avec faces visibles et caméra orbitale
- rendre le moteur encore plus robuste et plus riche

# Minecraft Builder Pro

Application WPF C# qui utilise Ollama pour générer un plan de schéma Minecraft à partir d'un prompt.

## Fonctionnalités

- interface utilisateur WPF
- génération de plan via Ollama
- vue 2D et 3D
- export du plan JSON
- export d'un schéma `.schem` (format de structure exploitable par le projet)

## Prérequis

- Windows
- .NET 8 SDK
- Ollama installé localement
- modèle Ollama téléchargé : `llama3.1:8b`

## Lancement

```bash
ollama serve
ollama pull llama3.1:8b
dotnet restore
dotnet build
dotnet run
```

## Remarque

Cette version est une vraie base architecturée de générateur de schéma Minecraft : plan JSON + voxel world + rendu 3D + export de structure. C'est une base solide pour aller vers une version encore plus avancée et proche d'un outil avancé de génération de schematics.

## Exemple de prompt

- "Maison médiévale en pierre, 2 étages, 3 chambres, salon, cuisine, cave"
- "Base souterraine de survival avec atelier, four, coffres, chambre"
- "Château en pierre, grande salle, tours, cave, fenêtres, toiture imposante"

## Objectif suivant

- rendre le moteur voxel encore plus avancé
- séparer les modèles par style de bâtiment
- améliorer l'export au format NBT Minecraft réel
- ajouter des templates de pièces et de toits plus complexes


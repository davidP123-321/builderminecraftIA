using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace builderminecraftIA
{
    public partial class MainWindow : Window
    {
        private readonly OllamaClient _ollama = new();
        private readonly HouseGenerator _generator = new();
        private readonly PreviewBuilder _preview = new();
        private BuildPlan? _currentPlan;
        private List<BlockVoxel> _currentVoxels = new();

        public MainWindow()
        {
            InitializeComponent();
            StyleComboBox.SelectedIndex = 0;
            ModelComboBox.SelectedIndex = 0;
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                GenerateButton.IsEnabled = false;
                ExportButton.IsEnabled = false;
                ExportSchemButton.IsEnabled = false;
                StatusText.Text = "Génération du schéma en cours...";

                var prompt = BuildPrompt();
                var model = ((ComboBoxItem)ModelComboBox.SelectedItem!).Content?.ToString() ?? "llama3.1:8b";

                var rawJson = await _ollama.AskAsync(prompt, model);
                var plan = JsonSerializer.Deserialize<BuildPlan>(rawJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (plan == null)
                    throw new Exception("Le plan JSON est invalide.");

                var voxels = _generator.Generate(plan);
                _currentPlan = plan;
                _currentVoxels = voxels;

                JsonOutput.Text = FormatJson(rawJson);
                _preview.Draw(plan, PreviewCanvas);
                Render3DPreview(voxels);

                StatusText.Text = $"Schéma généré : {plan.name} ({voxels.Count} blocs)";
            }
            catch (Exception ex)
            {
                StatusText.Text = "Erreur : " + ex.Message;
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                GenerateButton.IsEnabled = true;
                ExportButton.IsEnabled = true;
                ExportSchemButton.IsEnabled = true;
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                DefaultExt = ".json",
                Title = "Exporter le plan"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, JsonOutput.Text);
                StatusText.Text = "Plan exporté";
            }
        }

        private void ExportSchemButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPlan == null)
            {
                MessageBox.Show("Générez d’abord un schéma avant l’export .schem.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "Schematic (*.schem)|*.schem",
                DefaultExt = ".schem",
                Title = "Exporter le schéma Minecraft"
            };

            if (dialog.ShowDialog() == true)
            {
                var exporter = new SchematicExporter();
                exporter.Export(_currentPlan, _currentVoxels, dialog.FileName);
                StatusText.Text = "Schéma .schem exporté";
            }
        }

        private string BuildPrompt()
        {
            var x = WidthBox.Text;
            var z = DepthBox.Text;
            var style = ((ComboBoxItem)StyleComboBox.SelectedItem!).Content?.ToString() ?? "cozy";
            var userText = PromptTextBox.Text;

            return $@"
Génère un schéma Minecraft selon cette demande :
""{userText}""

Style principal : {style}
Dimensions : {x} x {z}

Règles :
- réponse JSON STRICTE
- architecture cohérente
- 1 ou plusieurs pièces
- 1 entrée
- 1 escalier
- fenêtres sur les façades
- toit cohérent
- mobilier intérieur
- style propre, fonctionnel, beau
- dimensions réalistes

Sortie JSON obligatoire :
{{
  ""name"": ""Nom du build"",
  ""style"": ""{style}"",
  ""dimensions"": {{ ""x"": {x}, ""y"": 12, ""z"": {z} }},
  ""materials"": {{
    ""walls"": [""stone_bricks"", ""oak_planks""],
    ""roof"": [""spruce_planks"", ""stone_bricks""],
    ""floor"": [""stone"", ""oak_planks""],
    ""windows"": [""glass""],
    ""decor"": [""lantern"", ""torch"", ""barrel""]
  }},
  ""rooms"": [
    {{ ""name"": ""salon"", ""x"": 0, ""y"": 0, ""z"": 0, ""w"": 8, ""h"": 5, ""d"": 10, ""type"": ""living"" }},
    {{ ""name"": ""chambre"", ""x"": 8, ""y"": 0, ""z"": 0, ""w"": 7, ""h"": 5, ""d"": 8, ""type"": ""bedroom"" }},
    {{ ""name"": ""cuisine"", ""x"": 0, ""y"": 0, ""z"": 10, ""w"": 8, ""h"": 5, ""d"": 8, ""type"": ""kitchen"" }}
  ],
  ""stairs"": [
    {{ ""x"": 10, ""y"": 0, ""z"": 6, ""direction"": ""north"" }}
  ],
  ""doors"": [
    {{ ""x"": 12, ""y"": 0, ""z"": 0, ""type"": ""oak_door"" }}
  ],
  ""windows"": [
    {{ ""x"": 2, ""y"": 2, ""z"": 0, ""width"": 4, ""height"": 2 }},
    {{ ""x"": 12, ""y"": 2, ""z"": 0, ""width"": 4, ""height"": 2 }}
  ],
  ""roof"": {{ ""type"": ""gable"", ""overhang"": 1 }},
  ""interior"": [""bed"", ""chest"", ""furnace"", ""crafting_table"", ""barrel""] ,
  ""constraints"": [""must_have_entrance"", ""must_have_lighting"", ""must_be_functional""]
}}
";
        }

        private static string FormatJson(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });
        }

        private void Render3DPreview(List<BlockVoxel> voxels)
        {
            ThreeDViewport.Children.Clear();

            var group = new Model3DGroup();
            foreach (var voxel in voxels)
            {
                var mesh = new MeshGeometry3D();
                AddBox(mesh, voxel.x, voxel.y, voxel.z, 1, 1, 1);

                var material = new DiffuseMaterial(new SolidColorBrush(BlockToColor(voxel.block)));
                var model = new GeometryModel3D(mesh, material);
                group.Children.Add(model);
            }

            var visual = new ModelVisual3D { Content = group };
            ThreeDViewport.Children.Add(visual);

            var light1 = new AmbientLight(Colors.White);
            var light2 = new DirectionalLight(Colors.White, new Vector3D(-1, -2, -3));
            ThreeDViewport.Children.Add(new ModelVisual3D { Content = light1 });
            ThreeDViewport.Children.Add(new ModelVisual3D { Content = light2 });

            ThreeDViewport.Camera = new PerspectiveCamera
            {
                Position = new Point3D(18, 20, 28),
                LookDirection = new Vector3D(-10, -10, -20),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 45
            };
        }

        private static void AddBox(MeshGeometry3D mesh, double x, double y, double z, double w, double h, double d)
        {
            var p = new[]
            {
                new Point3D(x, y, z),
                new Point3D(x + w, y, z),
                new Point3D(x + w, y + h, z),
                new Point3D(x, y + h, z),
                new Point3D(x, y, z + d),
                new Point3D(x + w, y, z + d),
                new Point3D(x + w, y + h, z + d),
                new Point3D(x, y + h, z + d)
            };

            var faces = new[]
            {
                0, 1, 2, 2, 3, 0,
                4, 5, 6, 6, 7, 4,
                0, 4, 7, 7, 3, 0,
                1, 5, 6, 6, 2, 1,
                3, 7, 6, 6, 2, 3,
                0, 1, 5, 5, 4, 0
            };

            foreach (var point in p)
                mesh.Positions.Add(point);

            foreach (var index in faces)
                mesh.TriangleIndices.Add(index);

            for (var i = 0; i < 8; i++)
                mesh.Normals.Add(new Vector3D(0, 0, 0));
        }

        private static Color BlockToColor(string block)
        {
            return block switch
            {
                "stone" => Colors.Gray,
                "stone_bricks" => Colors.DimGray,
                "oak_planks" => Colors.SaddleBrown,
                "glass" => Colors.LightSkyBlue,
                "spruce_planks" => Colors.Sienna,
                "lantern" => Colors.Goldenrod,
                "bed" => Colors.IndianRed,
                "crafting_table" => Colors.Peru,
                "furnace" => Colors.SlateGray,
                "barrel" => Colors.BurlyWood,
                "chest" => Colors.SandyBrown,
                "oak_door" => Colors.Brown,
                _ => Colors.LightGray
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Microsoft.Win32;

namespace builderminecraftIA
{
    public partial class MainWindow : Window
    {
        private readonly OllamaClient _ollama = new();
        private BuildPlan? _currentPlan;
        private VoxelWorld? _currentWorld;

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
                var model = ((ComboBoxItem)ModelComboBox.SelectedItem!)?.Content?.ToString() ?? "llama3.1:8b";

                var rawJson = await _ollama.AskAsync(prompt, model);
                var plan = JsonSerializer.Deserialize<BuildPlan>(rawJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (plan == null)
                    throw new Exception("Le plan JSON est invalide.");

                var world = BuildingTemplate.Build(plan);
                _currentPlan = plan;
                _currentWorld = world;

                JsonOutput.Text = FormatJson(rawJson);
                Render2DPreview(plan);
                Render3DPreview(world);

                StatusText.Text = $"Schéma généré : {plan.Name} ({world.Count} blocs)";
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
            if (_currentPlan == null)
            {
                MessageBox.Show("Générez d’abord un schéma avant d’exporter JSON.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                DefaultExt = ".json",
                Title = "Exporter le plan"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, JsonOutput.Text);
                StatusText.Text = "Plan JSON exporté";
            }
        }

        private void ExportSchemButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentWorld == null)
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
                exporter.Export(_currentWorld, dialog.FileName);
                StatusText.Text = "Schéma .schem exporté";
            }
        }

        private string BuildPrompt()
        {
            var x = WidthBox.Text;
            var z = DepthBox.Text;
            var style = ((ComboBoxItem)StyleComboBox.SelectedItem!)?.Content?.ToString() ?? "cozy";
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
- dimensions réalistes
- JSON uniquement, pas de texte libre

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
  ""interior"": [""bed"", ""chest"", ""furnace"", ""crafting_table"", ""barrel""],
  ""constraints"": [""must_have_entrance"", ""must_have_lighting"", ""must_be_functional""]
}}
";
        }

        private static string FormatJson(string json)
        {
            using var document = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(document.RootElement, new JsonSerializerOptions { WriteIndented = true });
        }

        private void Render2DPreview(BuildPlan plan)
        {
            var canvas = PreviewCanvas;
            canvas.Children.Clear();

            const double cell = 16d;
            var width = plan.Dimensions["x"];
            var depth = plan.Dimensions["z"];
            canvas.Width = width * cell + 30;
            canvas.Height = depth * cell + 30;

            var background = new Rectangle
            {
                Width = canvas.Width,
                Height = canvas.Height,
                Fill = Brushes.Black
            };
            canvas.Children.Add(background);

            foreach (var room in plan.Rooms)
            {
                var rect = new Rectangle
                {
                    Width = room.W * cell,
                    Height = room.D * cell,
                    Fill = Brushes.DarkSlateGray,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(rect, room.X * cell + 10);
                Canvas.SetTop(rect, room.Z * cell + 10);
                canvas.Children.Add(rect);

                var label = new TextBlock
                {
                    Text = room.Name,
                    Foreground = Brushes.White,
                    FontSize = 10
                };

                Canvas.SetLeft(label, room.X * cell + 14);
                Canvas.SetTop(label, room.Z * cell + 12);
                canvas.Children.Add(label);
            }

            foreach (var door in plan.Doors)
            {
                var rect = new Rectangle
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.Orange
                };
                Canvas.SetLeft(rect, door.X * cell + 10);
                Canvas.SetTop(rect, door.Z * cell + 10);
                canvas.Children.Add(rect);
            }

            foreach (var window in plan.Windows)
            {
                var rect = new Rectangle
                {
                    Width = window.Width * cell,
                    Height = window.Height * cell,
                    Fill = Brushes.Cyan,
                    Stroke = Brushes.LightCyan,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(rect, window.X * cell + 10);
                Canvas.SetTop(rect, window.Z * cell + 10);
                canvas.Children.Add(rect);
            }
        }

        private void Render3DPreview(VoxelWorld world)
        {
            ThreeDViewport.Children.Clear();

            var renderer = new VoxelRenderer();
            var model = renderer.BuildModel(world);
            var visual = new ModelVisual3D { Content = model };
            ThreeDViewport.Children.Add(visual);

            var ambient = new AmbientLight(Colors.White);
            var directional = new DirectionalLight(Colors.White, new Vector3D(-1, -2, -3));
            ThreeDViewport.Children.Add(new ModelVisual3D { Content = ambient });
            ThreeDViewport.Children.Add(new ModelVisual3D { Content = directional });

            ThreeDViewport.Camera = new PerspectiveCamera
            {
                Position = new Point3D(30, 24, 32),
                LookDirection = new Vector3D(-18, -10, -18),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 45
            };
        }
    }
}

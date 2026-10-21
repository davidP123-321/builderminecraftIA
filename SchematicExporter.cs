using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace builderminecraftIA
{
    public class PreviewBuilder
    {
        public void Draw(BuildPlan plan, Canvas canvas)
        {
            canvas.Children.Clear();

            var width = plan.dimensions["x"];
            var depth = plan.dimensions["z"];
            const double cell = 16;

            canvas.Width = width * cell + 30;
            canvas.Height = depth * cell + 30;

            var background = new Rectangle
            {
                Width = canvas.Width,
                Height = canvas.Height,
                Fill = Brushes.Black
            };
            canvas.Children.Add(background);

            foreach (var room in plan.rooms)
            {
                var rect = new Rectangle
                {
                    Width = room.w * cell,
                    Height = room.d * cell,
                    Fill = Brushes.DarkSlateGray,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(rect, room.x * cell + 10);
                Canvas.SetTop(rect, room.z * cell + 10);
                canvas.Children.Add(rect);

                var label = new TextBlock
                {
                    Text = room.name,
                    Foreground = Brushes.White,
                    FontSize = 10
                };

                Canvas.SetLeft(label, room.x * cell + 14);
                Canvas.SetTop(label, room.z * cell + 12);
                canvas.Children.Add(label);
            }

            foreach (var door in plan.doors)
            {
                var rect = new Rectangle
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.Orange
                };

                Canvas.SetLeft(rect, door.x * cell + 10);
                Canvas.SetTop(rect, door.z * cell + 10);
                canvas.Children.Add(rect);
            }

            foreach (var window in plan.windows)
            {
                var rect = new Rectangle
                {
                    Width = window.width * cell,
                    Height = window.height * cell,
                    Fill = Brushes.Cyan,
                    Stroke = Brushes.LightCyan,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(rect, window.x * cell + 10);
                Canvas.SetTop(rect, window.z * cell + 10);
                canvas.Children.Add(rect);
            }
        }
    }
}

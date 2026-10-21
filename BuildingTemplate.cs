using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace builderminecraftIA
{
    public class VoxelRenderer
    {
        public Model3D BuildModel(VoxelWorld world)
        {
            var group = new Model3DGroup();

            foreach (var (x, y, z, type) in world.GetBlocks())
            {
                if (type == BlockType.Air)
                    continue;

                var mesh = new MeshGeometry3D();
                AddCube(mesh, x, y, z, 1, 1, 1);

                var material = new DiffuseMaterial(new SolidColorBrush(GetColor(type)));
                var model = new GeometryModel3D(mesh, material);
                group.Children.Add(model);
            }

            return group;
        }

        private static void AddCube(MeshGeometry3D mesh, double x, double y, double z, double w, double h, double d)
        {
            var points = new[]
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

            foreach (var point in points)
                mesh.Positions.Add(point);

            foreach (var index in faces)
                mesh.TriangleIndices.Add(index);

            for (int i = 0; i < 8; i++)
                mesh.Normals.Add(new Vector3D(0, 0, 0));
        }

        private static Color GetColor(BlockType type)
        {
            return type switch
            {
                BlockType.Stone => Colors.Gray,
                BlockType.StoneBricks => Colors.DimGray,
                BlockType.OakPlanks => Colors.SaddleBrown,
                BlockType.Glass => Colors.LightSkyBlue,
                BlockType.SprucePlanks => Colors.Sienna,
                BlockType.Lantern => Colors.Goldenrod,
                BlockType.Bed => Colors.IndianRed,
                BlockType.CraftingTable => Colors.Peru,
                BlockType.Furnace => Colors.SlateGray,
                BlockType.Barrel => Colors.BurlyWood,
                BlockType.Chest => Colors.SandyBrown,
                BlockType.OakDoor => Colors.Brown,
                BlockType.Sandstone => Colors.Bisque,
                BlockType.Cobblestone => Colors.DarkGray,
                BlockType.Quartz => Colors.WhiteSmoke,
                BlockType.DarkOakPlanks => Colors.DarkOliveGreen,
                _ => Colors.LightGray
            };
        }
    }
}

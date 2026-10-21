using System.Windows.Media.Media3D;

namespace builderminecraftIA
{
    public class BoxGeometry3D : Geometry3D
    {
        public BoxGeometry3D(Point3D origin, double width, double height, double depth)
        {
            var positions = new Point3DCollection
            {
                new(origin.X, origin.Y, origin.Z),
                new(origin.X + width, origin.Y, origin.Z),
                new(origin.X + width, origin.Y + height, origin.Z),
                new(origin.X, origin.Y + height, origin.Z),
                new(origin.X, origin.Y, origin.Z + depth),
                new(origin.X + width, origin.Y, origin.Z + depth),
                new(origin.X + width, origin.Y + height, origin.Z + depth),
                new(origin.X, origin.Y + height, origin.Z + depth)
            };

            var triangleIndices = new Int32Collection
            {
                0,1,2, 2,3,0,
                4,5,6, 6,7,4,
                0,4,7, 7,3,0,
                1,5,6, 6,2,1,
                3,7,6, 6,2,3,
                0,1,5, 5,4,0
            };

            this.Positions = positions;
            this.TriangleIndices = triangleIndices;
        }

        public Point3DCollection Positions { get; }
        public Int32Collection TriangleIndices { get; }
    }
}

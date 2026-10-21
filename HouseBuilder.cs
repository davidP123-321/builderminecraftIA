using System;
using System.Collections.Generic;
using System.Linq;

namespace builderminecraftIA
{
    public sealed class VoxelWorld
    {
        private readonly Dictionary<(int X, int Y, int Z), BlockType> _blocks = new();

        public void SetBlock(int x, int y, int z, BlockType type)
        {
            if (type == BlockType.Air)
            {
                _blocks.Remove((x, y, z));
                return;
            }

            _blocks[(x, y, z)] = type;
        }

        public BlockType GetBlock(int x, int y, int z)
        {
            return _blocks.TryGetValue((x, y, z), out var value) ? value : BlockType.Air;
        }

        public bool IsEmpty(int x, int y, int z)
        {
            return GetBlock(x, y, z) == BlockType.Air;
        }

        public void FillRect(int x1, int x2, int y1, int y2, int z1, int z2, BlockType type)
        {
            var minX = Math.Min(x1, x2);
            var maxX = Math.Max(x1, x2);
            var minY = Math.Min(y1, y2);
            var maxY = Math.Max(y1, y2);
            var minZ = Math.Min(z1, z2);
            var maxZ = Math.Max(z1, z2);

            for (int x = minX; x <= maxX; x++)
            for (int y = minY; y <= maxY; y++)
            for (int z = minZ; z <= maxZ; z++)
                SetBlock(x, y, z, type);
        }

        public IEnumerable<(int X, int Y, int Z, BlockType Type)> GetBlocks()
        {
            return _blocks.Select(kvp => (kvp.Key.X, kvp.Key.Y, kvp.Key.Z, kvp.Value));
        }

        public int Count => _blocks.Count;
    }
}

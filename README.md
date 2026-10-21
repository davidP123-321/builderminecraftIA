using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace builderminecraftIA
{
    public class SchematicExporter
    {
        public void Export(VoxelWorld world, string outputPath)
        {
            var blocks = world.GetBlocks().ToList();
            if (blocks.Count == 0)
                throw new InvalidOperationException("Aucun bloc à exporter.");

            var minX = blocks.Min(b => b.X);
            var maxX = blocks.Max(b => b.X);
            var minY = blocks.Min(b => b.Y);
            var maxY = blocks.Max(b => b.Y);
            var minZ = blocks.Min(b => b.Z);
            var maxZ = blocks.Max(b => b.Z);

            var width = maxX - minX + 1;
            var height = maxY - minY + 1;
            var depth = maxZ - minZ + 1;

            var palette = new Dictionary<BlockType, int>();
            var bytes = new byte[width * height * depth];

            foreach (var (x, y, z, type) in blocks)
            {
                if (type == BlockType.Air)
                    continue;

                if (!palette.ContainsKey(type))
                    palette[type] = palette.Count + 1;

                var bx = x - minX;
                var by = y - minY;
                var bz = z - minZ;
                var idx = (by * depth * width) + (bz * width) + bx;
                bytes[idx] = (byte)palette[type];
            }

            var lines = new List<string>
            {
                "# Minecraft schematic export",
                $"width={width}",
                $"height={height}",
                $"depth={depth}",
                $"minX={minX}",
                $"minY={minY}",
                $"minZ={minZ}",
                "palette=" + string.Join(";", palette.Select(kvp => $"{kvp.Key}:{kvp.Value}")),
                "blocks=" + string.Join(",", bytes.Select(b => b.ToString()))
            };

            File.WriteAllLines(outputPath, lines);
        }
    }
}

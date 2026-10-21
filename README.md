using System;
using System.Collections.Generic;
using System.IO;

namespace builderminecraftIA
{
    public class SchematicExporter
    {
        public void Export(BuildPlan plan, List<BlockVoxel> voxels, string filePath)
        {
            var output = new MemoryStream();
            var writer = new NbtWriter(output);

            var width = plan.dimensions["x"];
            var height = plan.dimensions["y"];
            var length = plan.dimensions["z"];

            var palette = new Dictionary<string, byte>();
            var flattened = new byte[width * height * length];

            for (int i = 0; i < flattened.Length; i++)
                flattened[i] = 0;

            foreach (var voxel in voxels)
            {
                if (voxel.block == "air")
                    continue;

                if (!palette.ContainsKey(voxel.block))
                    palette[voxel.block] = (byte)palette.Count;

                int index = (voxel.y * width * length) + (voxel.z * width) + voxel.x;
                if (index >= 0 && index < flattened.Length)
                    flattened[index] = palette[voxel.block];
            }

            writer.WriteCompoundStart();
            writer.WriteString("Materials", "Alpha");
            writer.WriteShort("Width", (short)width);
            writer.WriteShort("Height", (short)height);
            writer.WriteShort("Length", (short)length);
            writer.WriteByteArray("Blocks", flattened);
            writer.WriteByteArray("Data", new byte[flattened.Length]);
            writer.WriteListStart("Entities", 0);
            writer.WriteListEnd();
            writer.WriteListStart("TileEntities", 0);
            writer.WriteListEnd();
            writer.WriteCompoundEnd();

            File.WriteAllBytes(filePath, output.ToArray());
        }

        private sealed class NbtWriter
        {
            private readonly BinaryWriter _writer;

            public NbtWriter(Stream stream)
            {
                _writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true);
            }

            public void WriteCompoundStart() => _writer.Write((byte)10);
            public void WriteCompoundEnd() => _writer.Write((byte)0);

            public void WriteString(string name, string value)
            {
                _writer.Write((byte)8);
                WriteStringValue(name);
                WriteStringValue(value);
            }

            public void WriteShort(string name, short value)
            {
                _writer.Write((byte)2);
                WriteStringValue(name);
                _writer.Write(value);
            }

            public void WriteByteArray(string name, byte[] data)
            {
                _writer.Write((byte)7);
                WriteStringValue(name);
                _writer.Write(data.Length);
                _writer.Write(data);
            }

            public void WriteListStart(string name, int type)
            {
                _writer.Write((byte)9);
                WriteStringValue(name);
                _writer.Write((byte)type);
                _writer.Write(0);
            }

            public void WriteListEnd() { }

            private void WriteStringValue(string value)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(value);
                _writer.Write((short)bytes.Length);
                _writer.Write(bytes);
            }
        }
    }
}

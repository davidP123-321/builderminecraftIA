using System.Collections.Generic;

namespace builderminecraftIA
{
    public class BuildPlan
    {
        public string name { get; set; } = "Maison";
        public string style { get; set; } = "cozy";
        public Dictionary<string, int> dimensions { get; set; } = new();
        public Dictionary<string, List<string>> materials { get; set; } = new();
        public List<Room> rooms { get; set; } = new();
        public List<Stair> stairs { get; set; } = new();
        public List<Door> doors { get; set; } = new();
        public List<WindowDef> windows { get; set; } = new();
        public Dictionary<string, object> roof { get; set; } = new();
        public List<string> interior { get; set; } = new();
        public List<string> constraints { get; set; } = new();
    }

    public class Room
    {
        public string name { get; set; } = "Room";
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public int d { get; set; }
        public string type { get; set; } = "generic";
    }

    public class Stair
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public string direction { get; set; } = "north";
    }

    public class Door
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public string type { get; set; } = "oak_door";
    }

    public class WindowDef
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class BlockVoxel
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public string block { get; set; } = "stone";
    }
}

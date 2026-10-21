using System.Collections.Generic;

namespace builderminecraftIA
{
    public class BuildPlan
    {
        public string Name { get; set; } = "Maison";
        public string Style { get; set; } = "cozy";
        public Dictionary<string, int> Dimensions { get; set; } = new();
        public Dictionary<string, List<string>> Materials { get; set; } = new();
        public List<RoomPlan> Rooms { get; set; } = new();
        public List<StairPlan> Stairs { get; set; } = new();
        public List<DoorPlan> Doors { get; set; } = new();
        public List<WindowPlan> Windows { get; set; } = new();
        public Dictionary<string, object> Roof { get; set; } = new();
        public List<string> Interior { get; set; } = new();
        public List<string> Constraints { get; set; } = new();
    }

    public class RoomPlan
    {
        public string Name { get; set; } = "Room";
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public int D { get; set; }
        public string Type { get; set; } = "generic";
    }

    public class StairPlan
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public string Direction { get; set; } = "north";
    }

    public class DoorPlan
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public string Type { get; set; } = "oak_door";
    }

    public class WindowPlan
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

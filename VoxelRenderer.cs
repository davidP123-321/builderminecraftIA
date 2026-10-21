using System.Collections.Generic;
using System.Linq;

namespace builderminecraftIA
{
    public static class HouseBuilder
    {
        public static VoxelWorld Build(BuildPlan plan)
        {
            var world = new VoxelWorld();
            var width = plan.Dimensions["x"];
            var height = plan.Dimensions["y"];
            var depth = plan.Dimensions["z"];

            // fondamentations et sol
            world.FillRect(0, width - 1, 0, 0, 0, depth - 1, BlockType.Stone);
            world.FillRect(1, width - 2, 1, 1, 1, depth - 2, BlockType.StoneBricks);

            // murs extérieurs
            world.FillRect(0, width - 1, 1, height - 1, 0, 0, BlockType.StoneBricks);
            world.FillRect(0, width - 1, 1, height - 1, depth - 1, depth - 1, BlockType.StoneBricks);
            world.FillRect(0, 0, 1, height - 1, 0, depth - 1, BlockType.StoneBricks);
            world.FillRect(width - 1, width - 1, 1, height - 1, 0, depth - 1, BlockType.StoneBricks);

            // murs intérieurs selon les pièces définies
            foreach (var room in plan.Rooms)
            {
                // sol de room
                world.FillRect(room.X, room.X + room.W - 1, 1, 1, room.Z, room.Z + room.D - 1, BlockType.OakPlanks);

                // contours intérieurs
                world.FillRect(room.X, room.X + room.W - 1, 2, room.H + 1, room.Z, room.Z, BlockType.StoneBricks);
                world.FillRect(room.X, room.X + room.W - 1, 2, room.H + 1, room.Z + room.D - 1, room.Z + room.D - 1, BlockType.StoneBricks);
                world.FillRect(room.X, room.X, 2, room.H + 1, room.Z, room.Z + room.D - 1, BlockType.StoneBricks);
                world.FillRect(room.X + room.W - 1, room.X + room.W - 1, 2, room.H + 1, room.Z, room.Z + room.D - 1, BlockType.StoneBricks);
            }

            // toit
            for (int x = 0; x < width; x++)
            for (int z = 0; z < depth; z++)
                world.SetBlock(x, height, z, BlockType.SprucePlanks);

            // porte d'entrée
            foreach (var door in plan.Doors)
            {
                world.SetBlock(door.X, 2, door.Z, BlockType.OakDoor);
                world.SetBlock(door.X, 3, door.Z, BlockType.OakDoor);
            }

            // fenêtres
            foreach (var window in plan.Windows)
            {
                for (int x = window.X; x < window.X + window.Width; x++)
                for (int y = window.Y; y < window.Y + window.Height; y++)
                {
                    world.SetBlock(x, y, 0, BlockType.Glass);
                    world.SetBlock(x, y, depth - 1, BlockType.Glass);
                }
            }

            // escaliers
            foreach (var stair in plan.Stairs)
            {
                for (int i = 0; i < 4; i++)
                {
                    world.SetBlock(stair.X + i, i + 2, stair.Z, BlockType.Sandstone);
                }
            }

            // meubles simples selon le type de pièce
            foreach (var room in plan.Rooms)
            {
                if (room.Type == "bedroom")
                {
                    world.SetBlock(room.X + 1, 2, room.Z + 1, BlockType.Bed);
                }
                else if (room.Type == "kitchen")
                {
                    world.SetBlock(room.X + 2, 2, room.Z + 2, BlockType.CraftingTable);
                    world.SetBlock(room.X + 3, 2, room.Z + 2, BlockType.Furnace);
                }
                else if (room.Type == "living")
                {
                    world.SetBlock(room.X + 2, 2, room.Z + 4, BlockType.Barrel);
                    world.SetBlock(room.X + 4, 2, room.Z + 2, BlockType.Chest);
                }
            }

            // lanternes
            for (int x = 1; x < width - 1; x++)
            for (int z = 1; z < depth - 1; z++)
            {
                if ((x + z) % 5 == 0)
                    world.SetBlock(x, 3, z, BlockType.Lantern);
            }

            return world;
        }
    }
}

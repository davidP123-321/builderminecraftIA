using System;
using System.Collections.Generic;

namespace builderminecraftIA
{
    public static class BuildingTemplate
    {
        public static VoxelWorld Build(BuildPlan plan)
        {
            var style = plan.Style?.Trim();
            if (string.IsNullOrWhiteSpace(style))
                style = "cozy";

            return style.ToLowerInvariant() switch
            {
                "castle" => CastleBuilder.Build(plan),
                "modern" => ModernBuilder.Build(plan),
                "survival" => SurvivalBuilder.Build(plan),
                _ => HouseBuilder.Build(plan)
            };
        }
    }

    public static class HouseBuilder
    {
        public static VoxelWorld Build(BuildPlan plan)
        {
            var world = new VoxelWorld();
            var width = Math.Max(8, plan.Dimensions.GetValueOrDefault("x", 18));
            var height = Math.Max(6, plan.Dimensions.GetValueOrDefault("y", 10));
            var depth = Math.Max(8, plan.Dimensions.GetValueOrDefault("z", 14));

            world.FillRect(0, width - 1, 0, 0, 0, depth - 1, BlockType.Stone);
            world.FillRect(1, width - 2, 1, 1, 1, depth - 2, BlockType.StoneBricks);

            world.FillRect(0, width - 1, 1, height - 1, 0, 0, BlockType.StoneBricks);
            world.FillRect(0, width - 1, 1, height - 1, depth - 1, depth - 1, BlockType.StoneBricks);
            world.FillRect(0, 0, 1, height - 1, 0, depth - 1, BlockType.StoneBricks);
            world.FillRect(width - 1, width - 1, 1, height - 1, 0, depth - 1, BlockType.StoneBricks);

            for (int x = 0; x < width; x++)
            for (int z = 0; z < depth; z++)
                world.SetBlock(x, height, z, BlockType.SprucePlanks);

            foreach (var room in plan.Rooms)
            {
                var rx = room.X;
                var rz = room.Z;
                var rw = room.W;
                var rd = room.D;

                world.FillRect(rx, rx + rw - 1, 1, 1, rz, rz + rd - 1, BlockType.OakPlanks);
                world.FillRect(rx, rx + rw - 1, 2, room.H + 1, rz, rz, BlockType.StoneBricks);
                world.FillRect(rx, rx + rw - 1, 2, room.H + 1, rz + rd - 1, rz + rd - 1, BlockType.StoneBricks);
                world.FillRect(rx, rx, 2, room.H + 1, rz, rz + rd - 1, BlockType.StoneBricks);
                world.FillRect(rx + rw - 1, rx + rw - 1, 2, room.H + 1, rz, rz + rd - 1, BlockType.StoneBricks);
            }

            foreach (var door in plan.Doors)
            {
                world.SetBlock(door.X, 2, door.Z, BlockType.OakDoor);
                world.SetBlock(door.X, 3, door.Z, BlockType.OakDoor);
            }

            foreach (var window in plan.Windows)
            {
                for (int x = window.X; x < window.X + window.Width; x++)
                for (int y = window.Y; y < window.Y + window.Height; y++)
                {
                    world.SetBlock(x, y, 0, BlockType.Glass);
                    world.SetBlock(x, y, depth - 1, BlockType.Glass);
                }
            }

            foreach (var stair in plan.Stairs)
            {
                for (int i = 0; i < 4; i++)
                    world.SetBlock(stair.X + i, 2 + i, stair.Z, BlockType.Sandstone);
            }

            foreach (var room in plan.Rooms)
            {
                if (room.Type == "bedroom")
                    world.SetBlock(room.X + 1, 2, room.Z + 1, BlockType.Bed);
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

            for (int x = 1; x < width - 1; x++)
            for (int z = 1; z < depth - 1; z++)
            {
                if ((x + z) % 5 == 0)
                    world.SetBlock(x, 3, z, BlockType.Lantern);
            }

            return world;
        }
    }

    public static class CastleBuilder
    {
        public static VoxelWorld Build(BuildPlan plan)
        {
            var world = new VoxelWorld();
            var width = Math.Max(12, plan.Dimensions.GetValueOrDefault("x", 22));
            var height = Math.Max(8, plan.Dimensions.GetValueOrDefault("y", 12));
            var depth = Math.Max(12, plan.Dimensions.GetValueOrDefault("z", 18));

            world.FillRect(0, width - 1, 0, 0, 0, depth - 1, BlockType.Stone);
            world.FillRect(1, width - 2, 1, 1, 1, depth - 2, BlockType.StoneBricks);
            world.FillRect(0, width - 1, 1, height - 1, 0, 0, BlockType.StoneBricks);
            world.FillRect(0, width - 1, 1, height - 1, depth - 1, depth - 1, BlockType.StoneBricks);
            world.FillRect(0, 0, 1, height - 1, 0, depth - 1, BlockType.StoneBricks);
            world.FillRect(width - 1, width - 1, 1, height - 1, 0, depth - 1, BlockType.StoneBricks);

            for (int x = 2; x < width - 2; x += 4)
            for (int y = 2; y < height - 2; y += 2)
                world.SetBlock(x, y, 0, BlockType.Glass);

            for (int x = 0; x < width; x++)
            for (int z = 0; z < depth; z++)
                if (x % 5 == 0 || z % 5 == 0)
                    world.SetBlock(x, height, z, BlockType.Quartz);

            return world;
        }
    }

    public static class ModernBuilder
    {
        public static VoxelWorld Build(BuildPlan plan)
        {
            var world = new VoxelWorld();
            var width = Math.Max(10, plan.Dimensions.GetValueOrDefault("x", 16));
            var height = Math.Max(6, plan.Dimensions.GetValueOrDefault("y", 8));
            var depth = Math.Max(10, plan.Dimensions.GetValueOrDefault("z", 16));

            world.FillRect(0, width - 1, 0, 0, 0, depth - 1, BlockType.Cobblestone);
            world.FillRect(1, width - 2, 1, 1, 1, depth - 2, BlockType.OakPlanks);
            world.FillRect(0, width - 1, 1, height - 1, 0, 0, BlockType.OakPlanks);
            world.FillRect(0, width - 1, 1, height - 1, depth - 1, depth - 1, BlockType.OakPlanks);
            world.FillRect(0, 0, 1, height - 1, 0, depth - 1, BlockType.OakPlanks);
            world.FillRect(width - 1, width - 1, 1, height - 1, 0, depth - 1, BlockType.OakPlanks);

            for (int x = 2; x < width - 2; x++)
            for (int z = 2; z < depth - 2; z++)
                world.SetBlock(x, height, z, BlockType.DarkOakPlanks);

            for (int x = 2; x < width - 2; x += 3)
            {
                world.SetBlock(x, 2, 0, BlockType.Glass);
                world.SetBlock(x, 3, 0, BlockType.Glass);
            }

            return world;
        }
    }

    public static class SurvivalBuilder
    {
        public static VoxelWorld Build(BuildPlan plan)
        {
            var world = new VoxelWorld();
            var width = Math.Max(12, plan.Dimensions.GetValueOrDefault("x", 18));
            var height = Math.Max(6, plan.Dimensions.GetValueOrDefault("y", 10));
            var depth = Math.Max(12, plan.Dimensions.GetValueOrDefault("z", 18));

            world.FillRect(0, width - 1, 0, 0, 0, depth - 1, BlockType.Stone);
            world.FillRect(1, width - 2, 1, 1, 1, depth - 2, BlockType.StoneBricks);
            world.FillRect(0, width - 1, 1, height - 1, 0, 0, BlockType.StoneBricks);
            world.FillRect(0, width - 1, 1, height - 1, depth - 1, depth - 1, BlockType.StoneBricks);
            world.FillRect(0, 0, 1, height - 1, 0, depth - 1, BlockType.StoneBricks);
            world.FillRect(width - 1, width - 1, 1, height - 1, 0, depth - 1, BlockType.StoneBricks);

            world.SetBlock(2, 2, 2, BlockType.CraftingTable);
            world.SetBlock(4, 2, 2, BlockType.Furnace);
            world.SetBlock(6, 2, 2, BlockType.Chest);
            world.SetBlock(8, 2, 2, BlockType.Bed);

            for (int x = 1; x < width - 1; x++)
            for (int z = 1; z < depth - 1; z++)
                if ((x + z) % 6 == 0)
                    world.SetBlock(x, 3, z, BlockType.Lantern);

            return world;
        }
    }
}

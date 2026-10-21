using System.Collections.Generic;

namespace builderminecraftIA
{
    public class HouseGenerator
    {
        public List<BlockVoxel> Generate(BuildPlan plan)
        {
            var result = new List<BlockVoxel>();
            var x = plan.dimensions["x"];
            var y = plan.dimensions["y"];
            var z = plan.dimensions["z"];

            for (int xx = 0; xx < x; xx++)
            {
                for (int zz = 0; zz < z; zz++)
                {
                    result.Add(new BlockVoxel { x = xx, y = 0, z = zz, block = "stone" });
                    result.Add(new BlockVoxel { x = xx, y = 1, z = zz, block = "stone" });
                }
            }

            for (int xx = 0; xx < x; xx++)
            {
                for (int yy = 1; yy < y; yy++)
                {
                    result.Add(new BlockVoxel { x = xx, y = yy, z = 0, block = "stone_bricks" });
                    result.Add(new BlockVoxel { x = xx, y = yy, z = z - 1, block = "stone_bricks" });
                }
            }

            for (int zz = 0; zz < z; zz++)
            {
                for (int yy = 1; yy < y; yy++)
                {
                    result.Add(new BlockVoxel { x = 0, y = yy, z = zz, block = "stone_bricks" });
                    result.Add(new BlockVoxel { x = x - 1, y = yy, z = zz, block = "stone_bricks" });
                }
            }

            foreach (var room in plan.rooms)
            {
                var rx0 = room.x;
                var rz0 = room.z;
                var rx1 = room.x + room.w - 1;
                var rz1 = room.z + room.d - 1;
                var ry0 = room.y + 1;
                var ry1 = room.y + room.h;

                for (int xx = rx0; xx <= rx1; xx++)
                {
                    for (int zz = rz0; zz <= rz1; zz++)
                    {
                        result.Add(new BlockVoxel { x = xx, y = ry0, z = zz, block = "oak_planks" });
                    }
                }

                for (int xx = rx0; xx <= rx1; xx++)
                {
                    for (int yy = ry0; yy <= ry1; yy++)
                    {
                        result.Add(new BlockVoxel { x = xx, y = yy, z = rz0, block = "stone_bricks" });
                        result.Add(new BlockVoxel { x = xx, y = yy, z = rz1, block = "stone_bricks" });
                    }
                }

                for (int zz = rz0; zz <= rz1; zz++)
                {
                    for (int yy = ry0; yy <= ry1; yy++)
                    {
                        result.Add(new BlockVoxel { x = rx0, y = yy, z = zz, block = "stone_bricks" });
                        result.Add(new BlockVoxel { x = rx1, y = yy, z = zz, block = "stone_bricks" });
                    }
                }
            }

            foreach (var door in plan.doors)
            {
                for (int yy = 1; yy <= 2; yy++)
                {
                    result.Add(new BlockVoxel { x = door.x, y = yy, z = door.z, block = "oak_door" });
                }
            }

            foreach (var window in plan.windows)
            {
                for (int xx = window.x; xx < window.x + window.width; xx++)
                {
                    for (int yy = window.y; yy < window.y + window.height; yy++)
                    {
                        result.Add(new BlockVoxel { x = xx, y = yy, z = 0, block = "glass" });
                        result.Add(new BlockVoxel { x = xx, y = yy, z = z - 1, block = "glass" });
                    }
                }
            }

            for (int yy = y - 2; yy < y; yy++)
            {
                for (int xx = 0; xx < x; xx++)
                {
                    for (int zz = 0; zz < z; zz++)
                    {
                        result.Add(new BlockVoxel { x = xx, y = yy, z = zz, block = "spruce_planks" });
                    }
                }
            }

            foreach (var room in plan.rooms)
            {
                if (room.type == "bedroom")
                {
                    result.Add(new BlockVoxel { x = room.x + 1, y = room.y + 2, z = room.z + 1, block = "bed" });
                }

                if (room.type == "kitchen")
                {
                    result.Add(new BlockVoxel { x = room.x + 2, y = room.y + 2, z = room.z + 2, block = "crafting_table" });
                    result.Add(new BlockVoxel { x = room.x + 3, y = room.y + 2, z = room.z + 2, block = "furnace" });
                }

                if (room.type == "living")
                {
                    result.Add(new BlockVoxel { x = room.x + 2, y = room.y + 2, z = room.z + 4, block = "barrel" });
                    result.Add(new BlockVoxel { x = room.x + 4, y = room.y + 2, z = room.z + 2, block = "chest" });
                }
            }

            for (int xx = 1; xx < x - 1; xx++)
            {
                for (int zz = 1; zz < z - 1; zz++)
                {
                    if ((xx + zz) % 5 == 0)
                    {
                        result.Add(new BlockVoxel { x = xx, y = 2, z = zz, block = "lantern" });
                    }
                }
            }

            return result;
        }
    }
}

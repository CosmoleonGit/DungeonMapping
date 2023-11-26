using Monobase.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    public class World
    {
        public static World Load(string path)
        {
            var bitmap = Image.FromFile(path) as Bitmap;

            var world = new Grid2D<int>(bitmap.Width, bitmap.Height);

            var playerPos = Vector2.Zero;

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (bitmap.GetPixel(x, y).G > 128)
                    {
                        world.SetTile(2, x, y);
                    }
                    else if (bitmap.GetPixel(x, y).R > 128)
                    {
                        playerPos = new Vector2(x + 1f, y + 1f);
                        world.SetTile(2, x, y);
                    }
                    else
                    {
                        world.SetTile(1, x, y);
                    }
                }
            }

            return new World(world, playerPos);
        }

        World(Grid2D<int> grid, Vector2 playerStart)
        {
            Grid = grid;
            PlayerStart = playerStart;
        }

        public readonly Grid2D<int> Grid;
        public readonly Vector2 PlayerStart;
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monobase.Maths;
using Monobase.Util;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Cameras;
using Ripple.ECS.Components.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class TilemapRenderer : RenderableComponent
    {
        public TilemapRenderer(Grid2D<int> grid, float scale = 1f)
        {
            _grid = grid;
            Scale = scale;
        }

        readonly Grid2D<int> _grid;

        protected override void OnAddedToScene()
        {
            _tex = Scene.Content.Load<Texture2D>("tiles");
        }

        public Vector2 Offset { get; set; }
        public float Scale { get; set; } = 1f;

        Texture2D _tex;

        public override void Render(ICamera camera)
        {
            Rectangle sRect;

            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    switch(_grid.GetTile(x, y))
                    {
                        default:
                            continue;
                        case 1:
                            sRect = new Rectangle(0, 0, 16, 16);
                            break;
                        case 2:
                            sRect = new Rectangle(16, 0, 16, 16);
                            break;
                    }

                    Ripple.SpriteBatch.Draw(_tex, new RectangleF(x * Scale + Offset.X, y * Scale + Offset.Y, Scale, Scale), sRect, Color.White);
                }
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Monobase.Util;
using Ripple;
using Ripple.ECS.Components;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class MapDrawingScript : Component, IUpdate
    {
        public MapDrawingScript(Grid2D<int> grid, World world, float scale)
        {
            _grid = grid;
            _world = world;

            _scale = scale;
        }

        readonly float _scale;
        readonly World _world;

        Matrix _matrix;
        readonly Grid2D<int> _grid;

        protected override void OnAddedToScene()
        {
            var tl = -Ripple.ScreenRect.Center.ToVector2() + new Vector2(_world.Grid.Width, _world.Grid.Height) * _scale / 2;

            _matrix = Matrix.CreateTranslation(new Vector3(tl, 0)) *
                      Matrix.CreateScale(1f / (_scale));
        }

        public void Update()
        {
            var pos = Vector2.Floor(Vector2.Transform(Input.Mouse.Position.ToVector2(), _matrix)).ToPoint();

            if (pos.X <= 0 || pos.X >= _grid.Width - 1 || pos.Y <= 0 || pos.Y >= _grid.Height - 1)
            {
                return;
            }

            if (Input.Mouse.IsLeftDown())
            {
                _grid.SetTile(1, pos.X, pos.Y);
                DisplayAccuracy();

            } else if (Input.Mouse.IsRightDown())
            {
                _grid.SetTile(2, pos.X, pos.Y);
                DisplayAccuracy();
            }
        }

        void DisplayAccuracy()
        {
            int count = 0;

            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    if (_world.Grid.GetTile(x, y) == _grid.GetTile(x, y))
                    {
                        count += 1;
                    }
                }
            }

            var ratio = (float)count / (_grid.Width * _grid.Height);

            if (ratio == 1)
            {
                EndingScreen.Switch(Result.Win, Scene);
            }
        }
    }
}

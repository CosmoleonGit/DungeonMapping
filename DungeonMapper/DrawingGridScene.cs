using Microsoft.Xna.Framework;
using Monobase.Graphics;
using Monobase.Util;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Cameras;
using Ripple.ECS.Components.Renderables;
using Ripple.ECS.Entities;
using Ripple.ECS.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class DrawingGridScene : Scene
    {
        public DrawingGridScene(World world)
        {
            _world = world;
        }

        readonly World _world;

        public override void OnAddedToGame()
        {
            //Background = new Color(0f, 0f, 0f, 0.8f);
            //BackgroundEnabled = true;

            var camera = new Camera2D(Ripple.ScreenWidth, Ripple.ScreenHeight)
            {
                Zoom = 1f
            };

            Camera = camera;

            var camEntity = new Entity().AddComponent(new Transform2D())
                                        .AddComponent(camera);

            AddEntity(camEntity);

            var mapGrid = new Grid2D<int>(_world.Grid.Width, _world.Grid.Height);

            mapGrid.Clear(2);

            for (int x = 0; x < mapGrid.Width; x++)
            {
                mapGrid.SetTile(1, x, 0);
                mapGrid.SetTile(1, x, mapGrid.Height - 1);
            }

            for (int y = 1; y < mapGrid.Height - 1; y++)
            {
                mapGrid.SetTile(1, 0, y);
                mapGrid.SetTile(1, mapGrid.Width - 1, y);
            }

            var mapEntity = new Entity();

            float tilemapScale = 32;

            switch (MaxDimension(_world))
            {
                default:
                    tilemapScale = 48;
                    break;
                case 32:
                    tilemapScale = 24;
                    break;
                case 24:
                    tilemapScale = 32;
                    break;
            }

            mapEntity.AddComponent(new Transform2D());
            mapEntity.AddComponent(new SpriteRenderer2D(2000, 2000, Ripple.PixelSprite) { Color = new(0, 0, 0, 100) });
            mapEntity.AddComponent(new TilemapRenderer(mapGrid, tilemapScale) { Offset = new(-tilemapScale * _world.Grid.Width / 2) });
            mapEntity.AddComponent(new MapDrawingScript(mapGrid, _world, tilemapScale));

            //mapEntity.AddComponent(new SpriteRenderer2D(16, 1, Ripple.PixelSprite) { Color = Color.Green, Offset = new Vector2(0, 20)});

            AddEntity(mapEntity);
        }

        static int MaxDimension(World world)
        {
            return Math.Max(world.Grid.Width, world.Grid.Height);
        }
    }
}

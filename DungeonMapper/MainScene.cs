using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monobase.Util;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Cameras;
using Ripple.ECS.Components.Renderables;
using Ripple.ECS.Entities;
using Ripple.ECS.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class MainScene : Scene
    {
        public MainScene(World world)
        {
            _world = world;
        }

        readonly World _world;

        public override void OnAddedToGame()
        {
            Background = new(50, 50, 50);
            BackgroundEnabled = true;

            var camera = new Camera2D(Ripple.ScreenWidth, Ripple.ScreenHeight);
            Camera = camera;
            camera.Zoom = 104;

            var camEntity = new Entity().AddComponent(new Transform2D())
                                        .AddComponent(camera);
            camEntity.Priority = int.MaxValue;

            AddEntity(camEntity);

            var worldEntity = new Entity();

            worldEntity.AddComponent(new Transform2D());
            worldEntity.AddComponent(new TilemapRenderer(_world.Grid));

            var playerEntity = CreatePlayer(_world);

            camEntity.AddComponent(new CameraFollow(playerEntity.GetComponent<Transform2D>()));

            AddEntity(worldEntity);
            AddEntity(playerEntity);
        }

        

        Entity CreatePlayer(World world)
        {
            var entity = new Entity();

            var playerTex = Content.Load<Texture2D>("player");

            entity.AddComponent(new Transform2D() { Position = world.PlayerStart});
            entity.AddComponent(new KinematicBody(world) { GroundHitboxSize = new(0.6f)});
            entity.AddComponent(new SpriteRenderer2D(1, 1, new Monobase.Graphics.Sprite(playerTex)));
            entity.AddComponent(new PlayerScript(world));
            entity.AddComponent(new EnemySpawnerScript(world));

            return entity;
        }
    }
}

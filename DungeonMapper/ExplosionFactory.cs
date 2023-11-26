using Microsoft.Xna.Framework;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Renderables;
using Ripple.ECS.Entities;
using Ripple.ECS.Scenes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal static class ExplosionFactory
    {
        public static void Explode(Scene scene, Vector2 position, Color color, int quantity = 10, float minSpeed = 10f, float maxSpeed = 15f)
        {
            for (int i = 0; i < quantity; i++)
            {
                var speed = Random.Shared.NextSingle() * (maxSpeed - minSpeed) + minSpeed;
                var dir = Random.Shared.NextSingle() * MathHelper.TwoPi;

                var entity = new Entity();

                entity.AddComponent(new Transform2D() { Position = position });
                entity.AddComponent(new SpriteRenderer2D(0.03f, 0.03f, scene.Ripple.PixelSprite) { Color = color });
                entity.AddComponent(new ParticleScript(dir, speed));

                scene.AddEntity(entity);
            }
        }
    }
}

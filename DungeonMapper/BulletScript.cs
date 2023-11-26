using Microsoft.Xna.Framework;
using Monobase.Maths;
using Monobase.Util;
using Ripple;
using Ripple.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class BulletScript : Component, IUpdate
    {
        public BulletScript(World world)
        {
            Priority = 1;

            _world = world;
        }

        const float Speed = 20f;

        protected override void OnAddedToScene()
        {
            _kinematicBody = Entity.GetComponent<KinematicBody>();
        }

        readonly World _world;

        KinematicBody _kinematicBody;

        public void Update()
        {
            //_kinematicBody.Transform.MoveForward(Speed * Time.SDeltaTime);
            _kinematicBody.Velocity = _kinematicBody.Transform.Direction * Speed;

            foreach (var collision in _kinematicBody.CollidingWith())
            {
                if (collision.Entity.GetComponent<EnemyScript>() is not null)
                {
                    ExplosionFactory.Explode(Scene, collision.Transform.Position, Color.OrangeRed, 10, 1, 1.5f);
                    collision.Entity.Destroy();

                    Entity.Destroy();
                }
            }

            if (_kinematicBody.JustCollided)
            {
                ExplosionFactory.Explode(Scene, _kinematicBody.Transform.Position, Color.Red, 10, 1, 1.5f);
                Entity.Destroy();

            }

            var min = _kinematicBody.Transform.Position - _kinematicBody.GroundHitboxSize / 2;
            var max = _kinematicBody.Transform.Position - _kinematicBody.GroundHitboxSize / 2;

            for (int x = (int)Mathf.Floor(min.X); x < Math.Ceiling(max.X); x++)
            {
                for (int y = (int)Mathf.Floor(min.Y); y < Math.Ceiling(max.Y); y++)
                {
                    if (_world.Grid.GetTile(x, y) == 1)
                    {
                        ExplosionFactory.Explode(Scene, _kinematicBody.Transform.Position, Color.Red, 10, 1, 1.5f);
                        Entity.Destroy();

                        return;
                    }
                }
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monobase.Extensions;
using Monobase.Graphics;
using Monobase.Maths;
using Monobase.Util;
using Ripple;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Renderables;
using Ripple.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class EnemySpawnerScript : Component, IUpdate
    {
        const float SpawnInterval = 2f;

        public EnemySpawnerScript(World world)
        {
            _timer = new(SpawnInterval);
            _world = world;
        }

        protected override void OnAddedToScene()
        {
            var enemyTex = Scene.Content.Load<Texture2D>("enemy");

            _enemySprite = new Sprite(enemyTex);

            _transform = Entity.GetComponent<Transform2D>();
        }

        readonly World _world;

        Transform2D _transform;
        CountdownTimer _timer;

        Sprite _enemySprite;

        public void Update()
        {
            if (_timer.Decrement(Time.SDeltaTime) > 0)
            {
                SpawnAnEnemy();
            }
        }

        void SpawnAnEnemy()
        {
            var entity = new Entity();

            entity.AddComponent(new Transform2D() { Position = GetSpawnPosition() });
            entity.AddComponent(new KinematicBody(_world) { InteractRadius = 0.75f, GroundHitboxSize = new(0.75f) });
            entity.AddComponent(new SpriteRenderer2D(1, 1, _enemySprite));
            entity.AddComponent(new EnemyScript(_transform, _world));

            Scene.AddEntity(entity);
        }

        Vector2 GetSpawnPosition()
        {
            while (true)
            {
                try_again:

                var distance = Random.Shared.NextFloat(8f, 13f);

                var angle = Random.Shared.NextFloat() * MathHelper.TwoPi;

                var deltaVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

                var spawnPos = _transform.Position + deltaVector;

                var rect = new RectangleF(spawnPos - new Vector2(0.5f), Vector2.One);

                for (int x = (int)Mathf.Floor(rect.Left); x <= Mathf.Ceil(rect.Right); x++)
                {
                    for (int y = (int)Mathf.Floor(rect.Top); y <= Mathf.Ceil(rect.Bottom); y++)
                    {
                        if (_world.Grid.GetTile(x, y) == 1 || _world.Grid.GetTile(x, y) == 0)
                        {
                            goto try_again;
                        }
                    }
                }

                return spawnPos;
            }
        }
    }
}

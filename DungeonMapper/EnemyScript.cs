using Microsoft.Xna.Framework;
using Monobase.Maths;
using Monobase.Physics;
using Monobase.Util;
using DungeonMapper.Pathfinding;
using Ripple;
using Ripple.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class EnemyScript : Component, IUpdate
    {
        const float MoveSpeed = 2f;
        const float RotationSpeed = 2f;
        const int PathLength = 5;

        public EnemyScript(Transform2D playerTransform, World world)
        {
            _playerTransform = playerTransform;
            _world = world;
        }

        readonly Transform2D _playerTransform;
        readonly World _world;

        protected override void OnAddedToScene()
        {
            _kinematicBody = Entity.GetComponent<KinematicBody>();
        }

        KinematicBody _kinematicBody;

        Vector2[] _path;
        int _pathIndex = -1;

        public void Update()
        {
            if (_pathIndex == -1)
                FollowPlayer();
            else
                FollowPath();
        }

        void FollowPlayer()
        {
            if (!PlayerInSight())
            {
                var aStar = new AstarGridGraph(_world.Grid.Width, _world.Grid.Height);

                var enemyPoint = Vector2.Floor(_kinematicBody.Transform.Position).ToPoint();

                for (int x = 0; x < _world.Grid.Width; x++)
                {
                    for (int y = 0; y < _world.Grid.Height; y++)
                    {
                        if (_world.Grid.GetTile(x, y) == 1)
                        {
                            aStar.Walls.Add(new(x, y));
                        }
                    }
                }

                _path = aStar.Search(enemyPoint, _playerTransform.Position.ToPoint()).Take(5).Select(x => x.ToVector2()).ToArray();
                _pathIndex = 0;

                return;
            }

            var delta = _kinematicBody.Transform.Position - _playerTransform.Position;
            delta.Normalize();
            _kinematicBody.Velocity = -delta * MoveSpeed;

            _kinematicBody.Transform.Rotation += RotationSpeed * Time.SDeltaTime;

            
        }

        void FollowPath()
        {
            if (Vector2.DistanceSquared(_kinematicBody.Transform.Position, _path[_pathIndex]) < 0.75f)
            {
                if (++_pathIndex == _path.Length)
                {
                    _pathIndex = -1;
                    return;
                }
            }

            var next = _path[_pathIndex] + new Vector2(0.5f);

            var delta = next - _kinematicBody.Transform.Position;
            delta.Normalize();
            _kinematicBody.Velocity = delta * MoveSpeed;

            _kinematicBody.Transform.Rotation += RotationSpeed * Time.SDeltaTime;
        }

        bool PlayerInSight()
        {
            //return false;

            var posA = _playerTransform.Position;
            var posB = _kinematicBody.Transform.Position;

            var delta = posB - posA;

            var min = Vector2.Min(posA, posB);
            var max = Vector2.Max(posA, posB);

            var searchRect = new RectangleF(min, max - min);

            searchRect.Inflate(1f, 1f);

            for (int x = (int)Mathf.Floor(searchRect.Left); x < Mathf.Ceil(searchRect.Right); x++)
            {
                for (int y = (int)Mathf.Floor(searchRect.Top); y < Mathf.Ceil(searchRect.Bottom); y++)
                {
                    if (_world.Grid.GetTile(x, y) == 2)
                    {
                        continue;
                    }

                    if (AABBCollision.RayVsRect(posA, delta, new RectangleF(x, y, 1, 1), out _, out _, out _))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

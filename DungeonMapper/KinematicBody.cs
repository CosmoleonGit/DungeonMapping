using Microsoft.Xna.Framework;
using Monobase.Maths;
using Monobase.Physics;
using Monobase.Util;
using Ripple;
using Ripple.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonMapper
{
    internal class KinematicBody : Component, IUpdate, IEquatable<KinematicBody>
    {
        static List<KinematicBody> _kinematicBodies = new();
        static int s_nextID = int.MinValue;

        public KinematicBody(World world)
        {
            _world = world;
        }

        int _id;
        readonly World _world;

        protected override void OnAddedToScene()
        {
            Transform = Entity.GetComponent<Transform2D>();

            _id = s_nextID++;
            _kinematicBodies.Add(this);
        }

        protected override void OnRemovedFromScene()
        {
            _kinematicBodies.Remove(this);
        }

        public Transform2D Transform { get; private set; }

        public Vector2 Velocity { get; set; }
        public Vector2 GroundHitboxSize { get; set; } = Vector2.One;
        public float InteractRadius { get; set; } = 1f;
        public bool CollidesWithGround { get; set; } = true;
        public bool JustCollided { get; set; } = false;

        public void Update()
        {
            JustCollided = false;
            //Transform.Position += Velocity * Time.SDeltaTime;
            var vel = Velocity * Time.SDeltaTime;

            var dynamicRect = new RectangleF(Transform.Position - GroundHitboxSize / 2, GroundHitboxSize);

            var sRects = new List<RectangleF>();

            var posA = Transform.Position;
            var posB = Transform.Position + vel;

            var min = Vector2.Min(posA, posB);
            var max = Vector2.Max(posA, posB);

            var rect = new RectangleF(min, max - min);

            rect.Inflate(GroundHitboxSize.X * 2, GroundHitboxSize.Y * 2);

            for (int x = (int)Math.Floor(rect.Left); x < (int)Math.Ceiling(rect.Right); x++)
            {
                for (int y = (int)Math.Floor(rect.Top); y < (int)Math.Ceiling(rect.Bottom); y++)
                {
                    if (_world.Grid.GetTile(x, y) == 1)
                        sRects.Add(new RectangleF(x, y, 1, 1));
                }
            }

            var tuples = new List<(int Index, float Time)>();

            for (int i = 0; i < sRects.Count; i++)
            {
                if (!AABBCollision.DynamicRectVsRect(dynamicRect, vel, sRects[i], out _, out _, out float contactTime))
                    continue;

                tuples.Add((i, contactTime));
                JustCollided = true;
            }

            tuples.Sort((x, y) => x.Time.CompareTo(y.Time));

            foreach (var tuple in tuples)
            {
                AABBCollision.ResolveDynamicRectVsRect(dynamicRect, ref vel, sRects[tuple.Index]);
            }

            Transform.Position += vel;
        }

        public bool Equals(KinematicBody other)
        {
            return _id == other._id;
        }

        public IEnumerable<KinematicBody> CollidingWith()
        {
            foreach (var other in _kinematicBodies)
            {
                if (this == other)
                    continue;

                var distSqr = Vector2.DistanceSquared(Transform.Position, other.Transform.Position);

                if (distSqr < InteractRadius * InteractRadius + other.InteractRadius * other.InteractRadius)
                {
                    yield return other;
                }
            }
        }
    }
}

using Ripple;
using Ripple.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class CameraFollow : Component, IUpdate
    {
        public CameraFollow(Transform2D follow)
        {
            _follow = follow;
            Priority = int.MaxValue;
        }

        readonly Transform2D _follow;

        protected override void OnAddedToScene()
        {
            _transform = Entity.GetComponent<Transform2D>();
        }

        Transform2D _transform;

        public void Update()
        {
            _transform.Position = _follow.Position;
        }
    }
}

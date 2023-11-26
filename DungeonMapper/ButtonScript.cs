using Microsoft.Xna.Framework;
using Monobase.Maths;
using Ripple;
using Ripple.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class ButtonScript : Component, IUpdate
    {
        public ButtonScript(Vector2 size)
        {
            _size = size;
        }

        public Action ClickAction { get; set; }

        protected override void OnAddedToScene()
        {
            _transform = Entity.GetComponent<Transform2D>();
        }

        Transform2D _transform;
        Vector2 _size;

        public void Update()
        {
            var rect = new RectangleF(_transform.Position - _size / 2, _size);

            if (rect.Contains(Input.Mouse.Position) && Input.Mouse.IsLeftPressed())
            {
                ClickAction?.Invoke();
            }
        }
    }
}

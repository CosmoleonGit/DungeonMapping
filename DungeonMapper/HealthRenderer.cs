using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Cameras;
using Ripple.ECS.Components.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class HealthRenderer : RenderableComponent
    {
        public HealthRenderer()
        {
            _hp = 5;
        }

        protected override void OnAddedToScene()
        {
            _hearts = Scene.Content.Load<Texture2D>("heart");
        }

        Texture2D _hearts;

        int _hp;

        public HealthRenderer SetHP(int hp)
        {
            _hp = hp;
            return this;
        }

        public override void Render(ICamera camera)
        {
            for (int i = 0; i < 5; i++)
            {
                bool isBlack = i >= _hp;

                Ripple.SpriteBatch.Draw(_hearts, new Rectangle(i * 64, 0, 64, 64), new Rectangle(isBlack ? 16 : 0, 0, 16, 16), Color.White);
            }
        }
    }
}

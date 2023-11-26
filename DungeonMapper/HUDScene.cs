using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Renderables;
using Ripple.ECS.Entities;
using Ripple.ECS.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class HUDScene : Scene
    {
        public Scene MapScene { get; set; }

        HealthRenderer _healthRenderer;

        public override void OnAddedToGame()
        {
            var healthEntity = new Entity();

            _healthRenderer = new HealthRenderer();

            healthEntity.AddComponent(new Transform2D().SetPosition(100, 100));
            healthEntity.AddComponent(_healthRenderer);
            //healthEntity.AddComponent(healthRenderer.SetHP(3));

            AddEntity(healthEntity);

            var buttonTex = Content.Load<Texture2D>("map_button");

            var buttonEntity = new Entity();
            buttonEntity.AddComponent(new Transform2D().SetPosition(Ripple.ScreenWidth - 108, 80));
            buttonEntity.AddComponent(new SpriteRenderer2D(128, 128, new(buttonTex)));
            buttonEntity.AddComponent(new ButtonScript(new Vector2(128, 128))
            {
                ClickAction = ToggleMap
            });

            AddEntity(buttonEntity);
        }

        public override void Update()
        {
            base.Update();

            _healthRenderer.SetHP(GameState.HP);

            if (Input.Keyboard.IsPressed(Keys.Tab))
            {
                ToggleMap();
            }
        }

        void ToggleMap()
        {
            GameState.MapShowing ^= true;
            MapScene.Enabled = GameState.MapShowing;
        }
    }
}

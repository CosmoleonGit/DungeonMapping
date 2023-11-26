using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Renderables;
using Ripple.ECS.Entities;
using Ripple.ECS.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class MainMenu : Scene
    {
        public override void OnAddedToGame()
        {
            var entity = new Entity();

            entity.AddComponent(new Transform2D());
            entity.AddComponent(new TextRenderer2D()
            {
                Text = "Dungeon Mapper",
                Font = Content.Load<SpriteFont>("titleFont"),
                Offset = new Vector2(Ripple.ScreenWidth / 2, 150),
                TextAlign = TextRenderer2D.TextAlignEnum.CenterCenter
            });

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    var buttonEntity = CreateButton(x, y, $"Level {y*3+x+1}", $"Maps/level-{y*3+x+1}.png");

                    AddEntity(buttonEntity);
                }
            }

            AddEntity(entity);
        }

        Entity CreateButton(int x, int y, string text, string path)
        {
            var entity = new Entity();

            entity.AddComponent(new Transform2D() { Position = new Vector2(x * 100 - 150 + 48, y * 100 + 32) + Ripple.ScreenRect.Center.ToVector2() });
            entity.AddComponent(new SpriteRenderer2D(96, 64, Ripple.PixelSprite));
            entity.AddComponent(new TextRenderer2D()
            {
                ForeColour = Color.Black,
                Font = Content.Load<SpriteFont>("buttonFont"),
                Text = text,
                TextAlign = TextRenderer2D.TextAlignEnum.CenterCenter
            });
            entity.AddComponent(new ButtonScript(new Vector2(96, 64))
            {
                ClickAction = () =>
                {
                    WorldLauncher.Load(path, Ripple);

                    Ripple.Scenes.Remove(this);
                }
            });

            return entity;
        }
    }
}

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
    internal enum Result
    {
        Win,
        Die,
        TimeOut,
//not yet        OutOfInk,
    }

    internal class EndingScreen : Scene
    {
        public static void Switch(Result result, Scene from)
        {
            var time = DateTime.Now - GameState.StartTime;

            var text = result switch
            {
                Result.Win => $"Congratulations! You completed the map.\n\nYou completed this level in {time.Minutes} minutes and {time.Seconds} seconds.",
                Result.Die => "woopsy daisy you perished :(",
                Result.TimeOut => "hurry up slow coach",
                _ => "huh"
            };

            var endingScreen = new EndingScreen(text);
            from.Ripple.Scenes.Add(endingScreen);

            from.Ripple.Scenes.Remove(GameState.GameScene);
            from.Ripple.Scenes.Remove(GameState.HUDScene);
            from.Ripple.Scenes.Remove(GameState.DrawingScene);

            GameState.HP = 5;
            GameState.MapShowing = false;
        }

        public EndingScreen(string text)
        {
            _text = text;
        }

        readonly string _text;

        public override void OnAddedToGame()
        {
            Background = Color.Black;
            BackgroundEnabled = true;

            var entity = new Entity();

            entity.AddComponent(new Transform2D());
            entity.AddComponent(new TextRenderer2D()
            {
                Text = _text,
                Font = Content.Load<SpriteFont>("titleFont"),
                TextAlign = TextRenderer2D.TextAlignEnum.CenterCenter,
                Offset = Ripple.ScreenRect.Center.ToVector2()
            });

            AddEntity(entity);

            var buttonEntity = new Entity();

            buttonEntity.AddComponent(new Transform2D() { Position = Ripple.ScreenRect.Center.ToVector2() + new Vector2(0, 300)});
            buttonEntity.AddComponent(new SpriteRenderer2D(96, 48, Ripple.PixelSprite));
            buttonEntity.AddComponent(new TextRenderer2D()
            {
                Text = "Play Again",
                TextAlign = TextRenderer2D.TextAlignEnum.CenterCenter,
                Font = Content.Load<SpriteFont>("buttonFont"),
                ForeColour = Color.Black
            });
            buttonEntity.AddComponent(new ButtonScript(new Vector2(96, 48))
            {
                ClickAction = () =>
                {
                    Ripple.Scenes.Add(new MainMenu());
                    Ripple.Scenes.Remove(this);
                }
            });

            AddEntity(buttonEntity);
        }
    }
}

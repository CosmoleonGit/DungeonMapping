using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ripple;

namespace DungeonMapper
{
    public class Main : RippleCore
    {
        public Main() : base(1280, 900)
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            //var world = World.Load("Maps/16x16.png");

            
            //var hudScene = new HUDScene();

            Ripple.Scenes.Add(new MainMenu());

            //Ripple.Scenes.Add(new MainScene(world));
            //Ripple.Scenes.Add(drawingGrid);
            //Ripple.Scenes.Add(hudScene);

            //hudScene.MapScene = drawingGrid;
        }
    }
}
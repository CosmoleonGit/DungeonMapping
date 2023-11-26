using Ripple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal static class WorldLauncher
    {
        public static void Load(string path, RippleInstance rippleInstance)
        {
            var world = World.Load(path);

            var mainScene = new MainScene(world);

            rippleInstance.Scenes.Add(mainScene);

            var drawingGrid = new DrawingGridScene(world) { Enabled = false };
            var hudScene = new HUDScene() { MapScene = drawingGrid };

            rippleInstance.Scenes.Add(drawingGrid);
            rippleInstance.Scenes.Add(hudScene);

            GameState.GameScene = mainScene;
            GameState.HUDScene = hudScene;
            GameState.DrawingScene = drawingGrid;

            GameState.StartTime = DateTime.Now;
        }
    }
}

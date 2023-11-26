using Ripple.ECS.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    public static class GameState
    {
        public const int MaxInk = 200;

        public static bool MapShowing { get; set; }
        public static int HP { get; set; } = 5;
        public static int Ink { get; set; } = MaxInk;

        public static Scene GameScene { get; set; }
        public static Scene HUDScene { get; set; }
        public static Scene DrawingScene { get; set; }

        public static DateTime StartTime { get; set; }
    }
}

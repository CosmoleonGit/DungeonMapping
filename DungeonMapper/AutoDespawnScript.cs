using Monobase.Util;
using Ripple;
using Ripple.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class AutoDespawnScript : Component, IUpdate
    {
        public AutoDespawnScript(float time)
        {
            _timer = new(time);
        }

        protected override void OnAddedToScene()
        {
            base.OnAddedToScene();
        }

        CountdownTimer _timer;

        public void Update()
        {
            if (_timer.Decrement(Time.SDeltaTime) > 0)
            {
                Entity.Destroy();
            }
        }
    }
}

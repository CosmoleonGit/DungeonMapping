using Monobase.Maths;
using Monobase.Util;
using Ripple;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class ParticleScript : Component, IUpdate
    {
        const float LiveTime = 0.15f;
        const float DecayTime = 0.15f;

        public ParticleScript(float direction, float speed)
        {
            _velocity = new Vector2(Mathf.Cos(direction), Mathf.Sin(direction)) * speed;

            _timer = new(LiveTime + DecayTime);
        }

        readonly Vector2 _velocity;
        readonly CountdownTimer _timer;

        protected override void OnAddedToScene()
        {
            _transform = Entity.GetComponent<Transform2D>();
            _spriteRenderer = Entity.GetComponent<SpriteRenderer2D>();
        }

        Transform2D _transform;
        SpriteRenderer2D _spriteRenderer;


        public void Update()
        {
            _transform.Position += _velocity * Time.SDeltaTime;

            if (_timer.Decrement(Time.SDeltaTime) > 0)
            {
                Entity.Destroy();
            }

            else if (_timer.TimeLeft > DecayTime)
            {
                _spriteRenderer.Color = new(_spriteRenderer.Color.R, _spriteRenderer.Color.G, _spriteRenderer.Color.B, 1f - _timer.TimeLeft / DecayTime);
            }
        }
    }
}

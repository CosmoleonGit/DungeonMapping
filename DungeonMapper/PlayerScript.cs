using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monobase.Graphics;
using Monobase.Util;
using Ripple;
using Ripple.ECS.Components;
using Ripple.ECS.Components.Renderables;
using Ripple.ECS.Entities;
using Ripple.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMapper
{
    internal class PlayerScript : Component, IUpdate
    {
        const float Speed = 3f;
        const float ShootInterval = 0.5f;
        const float InvincibilityLength = 1f;

        public PlayerScript(World world)
        {
            _world = world;
            _invincibilityTimer = new(InvincibilityLength);
        }

        readonly World _world;
        readonly CountdownTimer _invincibilityTimer;

        protected override void OnAddedToScene()
        {
            _kinematicBody = Entity.GetComponent<KinematicBody>();

            _xInputAxis = new VirtualIntegerAxisBuilder(Input).AddKeyboardKeys(OverlapBehaviour.CancelOut, Keys.A, Keys.D).Build();
            _yInputAxis = new VirtualIntegerAxisBuilder(Input).AddKeyboardKeys(OverlapBehaviour.CancelOut, Keys.W, Keys.S).Build();

            _shootTimer = new CountdownTimer(ShootInterval, 0);

            _bulletSprite = new Sprite(Scene.Content.Load<Texture2D>("pellet"));

            _spriteRenderer = Entity.GetComponent<SpriteRenderer2D>();

            _invincibilityTimer.TimeLeft = 0f;
        }

        Sprite _bulletSprite;
        SpriteRenderer2D _spriteRenderer;

        KinematicBody _kinematicBody;

        VirtualIntegerAxis _xInputAxis, _yInputAxis;

        CountdownTimer _shootTimer;

        public void Update()
        {
            var delta = new Vector2(_xInputAxis.GetValue(), _yInputAxis.GetValue()) * Speed;

            _kinematicBody.Velocity = delta;

            var mousePos = Vector2.Transform(Input.Mouse.Position.ToVector2(), Matrix.Invert(Scene.Camera.GetWorldMatrix()));

            _kinematicBody.Transform.LookAt(mousePos);
            
            if (_shootTimer.Decrement(Time.SDeltaTime) > 0 && !GameState.MapShowing)
            {
                

                if (Input.Mouse.IsLeftDown())
                {
                    Scene.AddEntity(CreateBullet());
                }
                else
                {
                    _shootTimer.TimeLeft = 0;
                }
                
                
            }

            if (_invincibilityTimer.TimeLeft > 0)
            {
                if (_invincibilityTimer.Decrement(Time.SDeltaTime) > 0)
                {
                    _invincibilityTimer.TimeLeft = 0;
                }
                _spriteRenderer.Enabled = Time.GameTime.TotalGameTime.Ticks % 150 > 75;
            }
            else
            {
                _spriteRenderer.Enabled = true;
            }

            if (_invincibilityTimer.TimeLeft == 0)
            {
                var enemyColliding = _kinematicBody.CollidingWith().FirstOrDefault(x => x.Entity.GetComponent<EnemyScript>() is not null);

                if (enemyColliding is not null)
                {
                    enemyColliding.Entity.Destroy();
                    _invincibilityTimer.Reset();

                    if (--GameState.HP == 0)
                    {
                        EndingScreen.Switch(Result.Die, Scene);
                    }

                }
            }
        }

        Entity CreateBullet()
        {
            var entity = new Entity().AddComponent(new Transform2D() 
            { 
                Position = _kinematicBody.Transform.Position + _kinematicBody.Transform.Direction * 0.5f,
                Rotation = _kinematicBody.Transform.Rotation,
            })
                                     .AddComponent(new KinematicBody(_world) { InteractRadius = 0.35f, GroundHitboxSize = new(0.2f)})
                                     .AddComponent(new SpriteRenderer2D(0.5f, 0.25f, _bulletSprite))
                                     .AddComponent(new BulletScript(_world));

            return entity;
        }
    }
}

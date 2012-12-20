using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace zombiefactory
{
    public class Gun : Microsoft.Xna.Framework.DrawableGameComponent
    {

        const float STICK_THRESHOLD = 0.04f;
        //Guns' fire rates and bullet speeds
        const float PISTOL_FIRE_RATE = 0.25f;
        const float PISTOL_BULLET_SPEED = 800.0f;

        #region properties
        ZombieGame ZombieGame { get; set; }
        Sprite Sprite { get; set; }
        float Direction { get; set; }
        int Damage { get; set; }
        int Ammo { get; set; }
        bool IsShooting { get; set; }
        List<Emitter> Emitters;
        #endregion properties

        public Gun(ZombieGame game, Vector2 initPos)
            : base(game)
        {
            ZombieGame = game;
            Sprite = new Sprite(ZombieGame, "Pistol", initPos, 0.0f);
            Sprite.Origin = new Vector2(0, Sprite.Height / 2);
            Emitters = new List<Emitter>();
            Emitters.Add(new Emitter(ZombieGame, 100, false, PISTOL_FIRE_RATE,
                new Particle(ZombieGame, "Pistol", new Vector2(200.0f, 200.0f), new Vector2(0.0f, 300.0f), 200.0f, 0.0f, PISTOL_BULLET_SPEED + ZombieGame.Player.Speed.Length())));
            IsShooting = false;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            SetSpriteDirection();
            MoveSprite();

            if (IsShooting)
                Emitters[0].addParticle("Bullet", Sprite.Position, new Vector2((float)Math.Cos(Sprite.Rotation), (float)Math.Sin(Sprite.Rotation)),
                    200.0f, 0.0f, PISTOL_BULLET_SPEED + ZombieGame.Player.Speed.Length());

            Sprite.Update(gameTime);
            for (int i = 0; i < Emitters.Count; i++)
                Emitters[i].Update(gameTime, 0.01f);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Sprite.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void SetSpriteDirection()
        {
            IsShooting = (Math.Abs(ZombieGame.InputMgr.ControllerState.ThumbSticks.Right.X) > STICK_THRESHOLD || Math.Abs(ZombieGame.InputMgr.ControllerState.ThumbSticks.Right.Y) > STICK_THRESHOLD);

            if (IsShooting)
                Sprite.Rotation = -(float)Math.Atan2((double)ZombieGame.InputMgr.ControllerState.ThumbSticks.Right.Y, (double)ZombieGame.InputMgr.ControllerState.ThumbSticks.Right.X);

        }

        private void MoveSprite()
        {
            Vector2 PlayerPosition = ZombieGame.Player.Sprite.Position;
            float x = PlayerPosition.X + ZombieGame.Player.Sprite.FrameWidth / 2 + Sprite.Width / 2;
            float y = PlayerPosition.Y + ZombieGame.Player.Sprite.FrameHeight / 2;

            Sprite.Position = new Vector2(x, y);
        }
    }
}

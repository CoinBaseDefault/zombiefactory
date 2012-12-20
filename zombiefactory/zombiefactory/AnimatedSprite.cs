using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zombiefactory
{
    public class AnimatedSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public const float UPDATE_TIME = 1.0f / 10.0f; //Will change to variable eventually, so anim speed can be adjusted for each spritesheet.

        #region properties
        protected ZombieGame ZombieGame { get; set; }
        Texture2D SpriteSheet { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; private set; }
        public Color Color { get; private set; }
        Rectangle[,] Rectangles;
        public int Frames { get; private set; }
        public int Lines { get; private set; }
        public bool IsLooping { get; set; }
        public int CurLine { get; set; }
        public int CurFrame { get; private set; }
        public SpriteEffects Effects { get; private set; }
        public float Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        float ElapsedTime { get; set; }
        #endregion properties

        public AnimatedSprite(ZombieGame game, string fileName, int frames, int lines, Vector2 position, float depth)
            : base(game)
        {
            ZombieGame = game;
            SpriteSheet = game.TextureMgr.Find(fileName);
            Frames = frames;
            Lines = lines;
            Position = position;

            CurLine = 0;
            CurFrame = 0;
            IsLooping = true;
            ElapsedTime = 0.0f;

            Rotation = 0.0f;
            Scale = 2.0f;
            Color = Color.White;
            Origin = Vector2.Zero;
            Effects = SpriteEffects.None;
            Depth = depth;

            Width = SpriteSheet.Width / Frames;
            Height = SpriteSheet.Height / Lines;

            Rectangles = new Rectangle[Lines, Frames]; //Like a matrix, the first index is the row (line), the second the column (frame)

            for (int i = 0; i < Lines; ++i)
            {
                for (int j = 0; j < Frames; ++j)
                {
                    Rectangles[i, j] = new Rectangle(j * Width, i * Height, Width, Height);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsLooping)
            {
                ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (ElapsedTime >= UPDATE_TIME)
                {
                    ElapsedTime -= UPDATE_TIME;

                    CurFrame++;
                    CurFrame %= Frames;
                }
            }
            else if (CurFrame != 0)
            {
                CurFrame = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ZombieGame.SpriteBatch.Draw(SpriteSheet, new Vector2(Position.X, Position.Y), Rectangles[CurLine, CurFrame], Color, Rotation, Origin, Scale, Effects, Depth);

            base.Draw(gameTime);
        }
    }
}

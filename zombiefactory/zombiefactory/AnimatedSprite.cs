using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zombiefactory
{
    public class AnimatedSprite : Sprite
    {
        #region properties
        Rectangle[,] Rectangles;
        public int Frames { get; private set; }
        public int Lines { get; private set; }
        public bool IsLooping { get; set; }
        public int CurLine { get; set; }
        public int CurFrame { get; private set; }
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }
        public float UpdateTime { get; private set; }
        float ElapsedTime { get; set; }
        #endregion properties

        public AnimatedSprite(ZombieGame game, string fileName, int frames, int lines, Vector2 position, float depth, float updateTime)
            : base(game, fileName, position, depth)
        {
            Frames = frames;
            Lines = lines;

            CurLine = 0;
            CurFrame = 0;
            IsLooping = false;
            UpdateTime = updateTime;
            ElapsedTime = 0.0f;

            FrameWidth = Width / Frames;
            FrameHeight = Height / Lines;

            Rectangles = new Rectangle[Lines, Frames]; //Like a matrix, the first index is the row (line), the second the column (frame)

            for (int i = 0; i < Lines; ++i)
            {
                for (int j = 0; j < Frames; ++j)
                {
                    Rectangles[i, j] = new Rectangle(j * FrameWidth, i * FrameHeight, FrameWidth, FrameHeight);
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

                if (ElapsedTime >= UpdateTime)
                {
                    ElapsedTime -= UpdateTime;

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
        }
    }
}

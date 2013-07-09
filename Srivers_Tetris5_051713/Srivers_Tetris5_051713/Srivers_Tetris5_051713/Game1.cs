using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Srivers_Tetris5_051713
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public float TimeToInput = 10;
        public float FrameCounter = 0;
        int[,] bottomBlocks;
        public Texture2D bucket;
        public Texture2D background;
        public Texture2D background1;
        public Texture2D background2;
        public Texture2D scoreBoard;
        public Texture2D next;
        public Texture2D tetrisBlock;
        Color[] colors;
        List<Vector2> fallingBlocks;
        List<Vector2> nextBlocks;
        int shapeColor;
        int nextShapeColor;
        int fallSpeedFast = 10;
        int fallSpeedRegular = 30;
        int fallSpeedState = 30;
        Random random;

        protected SpriteFont scoreFont;
        protected SpriteFont levelFont;
        public int score;
        private int level;

        KeyboardState keyboardState;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 760;
            graphics.PreferredBackBufferHeight = 650;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            Window.Title = "Steven Rivers Falling Block Attempt";
        }

        string[] button = { "Play", "Pause", "Stop" };
        Color[] buttonColor = { Color.White, Color.White, Color.White };
        MouseState mouse;

        Song BGM;
        SoundEffect soundEffect;

        string soundName2 = "SoundEffect";
        string soundName3 = "BGM2";




        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            random = new Random();
            keyboardState = Keyboard.GetState();
            //sets the different tints to use
            colors = new Color[] { Color.Red, Color.DarkSlateGray, Color.Lime, Color.DodgerBlue, Color.DarkBlue, Color.ForestGreen, Color.Gold };//changes shade of blocks
            bottomBlocks = new int[11, 20];

            for (int countX = 0; countX < bottomBlocks.GetLength(0); countX++)
            {
                for (int countY = 0; countY < bottomBlocks.GetLength(1); countY++)
                {
                    bottomBlocks[countX, countY] = -1;
                }
            }

            //sets beginning score/level
            score = 0;
            level = 1;

            nextBlocks = new List<Vector2>();
            // square
            nextBlocks.Add(new Vector2(4, 0));
            nextBlocks.Add(new Vector2(5, 0));
            nextBlocks.Add(new Vector2(4, 1));
            nextBlocks.Add(new Vector2(5, 1));

            ResetFalling();
            ClearRows();
            Rotate();
            base.Initialize();
        }

        void ResetFalling()
        {
            //chooses a random color and based on that color it chooses the shape assigned to it
            shapeColor = nextShapeColor;
            nextShapeColor = random.Next(0, 7);
            fallingBlocks = nextBlocks;
            nextBlocks = new List<Vector2>();

            if (nextShapeColor == 0)
            {
                // square
                nextBlocks.Add(new Vector2(4, 0));
                nextBlocks.Add(new Vector2(5, 0));
                nextBlocks.Add(new Vector2(4, 1));
                nextBlocks.Add(new Vector2(5, 1));
            }
            else if (nextShapeColor == 1)
            {
                // I
                nextBlocks.Add(new Vector2(5, 0));
                nextBlocks.Add(new Vector2(4, 0));
                nextBlocks.Add(new Vector2(6, 0));
                nextBlocks.Add(new Vector2(7, 0));
            }
            else if (nextShapeColor == 2)
            {
                // L
                nextBlocks.Add(new Vector2(5, 1));
                nextBlocks.Add(new Vector2(4, 0));
                nextBlocks.Add(new Vector2(5, 0));
                nextBlocks.Add(new Vector2(5, 2));
            }
            else if (nextShapeColor == 3)
            {
                // Z
                nextBlocks.Add(new Vector2(5, 0));
                nextBlocks.Add(new Vector2(4, 0));
                nextBlocks.Add(new Vector2(5, 1));
                nextBlocks.Add(new Vector2(6, 1));
            }
            else if (nextShapeColor == 4)
            {
                // S
                nextBlocks.Add(new Vector2(5, 1));
                nextBlocks.Add(new Vector2(4, 0));
                nextBlocks.Add(new Vector2(4, 1));
                nextBlocks.Add(new Vector2(5, 2));
            }
            else if (nextShapeColor == 5)
            {
                // J
                nextBlocks.Add(new Vector2(4, 1));
                nextBlocks.Add(new Vector2(4, 0));
                nextBlocks.Add(new Vector2(5, 0));
                nextBlocks.Add(new Vector2(4, 2));
            }
            else if (nextShapeColor == 6)
            {
                // T
                nextBlocks.Add(new Vector2(5, 0));
                nextBlocks.Add(new Vector2(4, 0));
                nextBlocks.Add(new Vector2(6, 0));
                nextBlocks.Add(new Vector2(5, 1));
            }

        }

        //suppose to clear the rows, without this the game gets cluttered with a bunch of boxes for somereason
        void ClearRows()                                                            // This function clears a complete row when a row is completeted
        {
            for (int countY = bottomBlocks.GetLength(1) - 1; countY >= 0; countY--)
            {
                bool clearRow = true;

                for (int countX = 1; countX < bottomBlocks.GetLength(0); countX++)
                {
                    clearRow &= bottomBlocks[countX, countY] != -1;
                }

                if (clearRow)
                {
                    score = score + 100;
                    soundEffect.Play();

                    for (int countX = 0; countX < bottomBlocks.GetLength(0); countX++)
                    {
                        bottomBlocks[countX, countY] = -1;
                    }

                    for (int y = countY; y > 0; y--)
                    {
                        for (int countX = 0; countX < bottomBlocks.GetLength(0); countX++)
                        {
                            bottomBlocks[countX, y] = bottomBlocks[countX, y - 1];
                        }
                    }

                    countY++;
                }
            }


        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //defines graphics being used!
            scoreFont = Content.Load<SpriteFont>("Score");
            levelFont = Content.Load<SpriteFont>("Score");
            bucket = Content.Load<Texture2D>("Bucket"); ;
            scoreBoard = Content.Load<Texture2D>("scores");
            background = Content.Load<Texture2D>("10522-white-wolf");
            background1 = Content.Load<Texture2D>("background1");
            background2 = Content.Load<Texture2D>("background2");
            next = Content.Load<Texture2D>("next");//next piece area, haven't figured out how to display this properly
            tetrisBlock = Content.Load<Texture2D>("testsprite");
            //music added below
            ContentManager contentManager = new ContentManager(this.Services, @"Content");
            BGM = contentManager.Load<Song>(soundName3);
            soundEffect = contentManager.Load<SoundEffect>(soundName2);



        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {

            MediaPlayer.IsRepeating = true;
            mouse = Mouse.GetState();
            for (int i = 0; i < 3; i++)
            {
                if (mouse.X >= i * 100 + 415 && mouse.X <= i * 100 + 415 + scoreFont.MeasureString(button[i]).X
                    && mouse.Y > 60 && mouse.Y <= 60 + scoreFont.MeasureString(button[i]).Y)
                {
                    buttonColor[i] = Color.Orange;
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        if (i == 0)
                        {
                            if (MediaPlayer.State != MediaState.Playing && MediaPlayer.State != MediaState.Paused)
                                MediaPlayer.Play(BGM);
                            else if (MediaPlayer.State == MediaState.Paused)
                                MediaPlayer.Resume();
                        }
                        else if (i == 1)
                        {
                            MediaPlayer.Pause();
                        }
                        else if (i == 2)
                        {
                            MediaPlayer.Stop();
                        }
                    }

                }
                else
                    buttonColor[i] = Color.White;
            }



            KeyboardState keyboardCurrent = Keyboard.GetState();
            if (keyboardCurrent.IsKeyDown(Keys.Left) & keyboardState.IsKeyUp(Keys.Left))
            {
                bool clear = true;

                for (int count = 0; count < fallingBlocks.Count; count++)
                {
                    var block = fallingBlocks[count];

                    block.X--;

                    int x = (int)block.X;
                    int y = (int)block.Y;

                    clear &= (block.X >= 1) && bottomBlocks[x, y] == -1; // ?
                }

                if (clear)
                {
                    for (int count = 0; count < fallingBlocks.Count; count++)
                    {
                        var block = fallingBlocks[count];

                        block.X--;

                        fallingBlocks[count] = block;
                    }
                }
            }

            else if (keyboardCurrent.IsKeyDown(Keys.Right) & keyboardState.IsKeyUp(Keys.Right))
            {
                bool clear = true;

                for (int count = 0; count < fallingBlocks.Count; count++)
                {
                    var block = fallingBlocks[count];

                    block.X++;

                    int x = (int)block.X;
                    int y = (int)block.Y;

                    clear &= (block.X < 11) && bottomBlocks[x, y] == -1; // was 9 then <= 10
                }

                if (clear)
                {
                    for (int count = 0; count < fallingBlocks.Count; count++)
                    {
                        var block = fallingBlocks[count];

                        block.X++;

                        fallingBlocks[count] = block;
                    }
                }
            }
            else if (keyboardCurrent.IsKeyDown(Keys.Down))
            {
                fallSpeedState = 0;
            }
            else if (keyboardCurrent.IsKeyDown(Keys.Up) & keyboardState.IsKeyUp(Keys.Up))
            {
                Rotate();
            }

            if (fallSpeedState <= 0)
            {
                if (keyboardCurrent.IsKeyDown(Keys.Down) & keyboardState.IsKeyUp(Keys.Down))
                {
                    fallSpeedState = fallSpeedFast;
                }
                else
                {
                    fallSpeedState = fallSpeedRegular;
                }

                bool clear = true;

                for (int count = 0; count < fallingBlocks.Count; count++)
                {
                    var block = fallingBlocks[count];

                    block.Y++;

                    int x = (int)block.X;
                    int y = (int)block.Y;

                    clear &= (block.Y < 20) && bottomBlocks[x, y] == -1; // was <=19 - was <= 20
                }

                if (clear)
                {
                    for (int count = 0; count < fallingBlocks.Count; count++)
                    {
                        var block = fallingBlocks[count];
                        block.Y++;
                        fallingBlocks[count] = block;
                    }
                }
                else
                {
                    while (fallingBlocks.Count > 0)
                    {
                        int x = (int)fallingBlocks[0].X;
                        int y = (int)fallingBlocks[0].Y;

                        bottomBlocks[x, y] = shapeColor;
                        fallingBlocks.RemoveAt(0);
                    }
                    ClearRows();
                    ResetFalling();
                }
            }
            else
            {
                fallSpeedState--;
            }

           
            keyboardState = keyboardCurrent;
            base.Update(gameTime);
        }


        Boolean Rotate()
        {
            var block = fallingBlocks[0];

            int startX = (int)block.X;
            int startY = (int)block.Y;

            bool rotate = true;

            for (int count = 1; count < fallingBlocks.Count; count++)
            {
                int differenceX = startX - (int)fallingBlocks[count].X;
                int differenceY = startY - (int)fallingBlocks[count].Y;

                int x = startX + -differenceY;
                int y = startY + differenceX;

                rotate &= ((x >= 1 & x < bottomBlocks.GetLength(0) & y >= 0 & y < bottomBlocks.GetLength(1)) && bottomBlocks[x, y] == -1);
            }
            if (rotate)
            {
                for (int count = 1; count < fallingBlocks.Count; count++)
                {
                    int differenceX = startX - (int)fallingBlocks[count].X;
                    int differenceY = startY - (int)fallingBlocks[count].Y;

                    int x = startX + -differenceY;
                    int y = startY + differenceX;

                    fallingBlocks[count] = new Vector2(x, y);

                }
            }

            return rotate;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (score < 100)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, 760, 650), Color.White);
            }
            if (score >= 100 && score < 200)
            {
                spriteBatch.Draw(background1, new Rectangle(0, 0, 900, 650), Color.White);
                fallSpeedRegular = 20;
            }
            if (score >= 200)
            {
                 spriteBatch.Draw(background2, new Rectangle(0, 0, 900, 650), Color.White);
                fallSpeedRegular = 10;
            }
           
            spriteBatch.Draw(bucket, new Rectangle(27, 0, 325, 650), Color.White);
            spriteBatch.Draw(scoreBoard, new Rectangle(400, 0, 200, 50), Color.White);
            spriteBatch.Draw(next, new Rectangle(400, 100, 100, 100), Color.White);

            spriteBatch.DrawString(scoreFont, "Score  " + score, new Vector2(415, 0), Color.Black);
            spriteBatch.DrawString(levelFont, "Level " + level, new Vector2(415, 20), Color.Black);



            //play buttons
            for (int i = 0; i < 3; i++)
                spriteBatch.DrawString(scoreFont, button[i], new Vector2(i * 100 + 415, 60), buttonColor[i]);


            for (int countX = 0; countX < bottomBlocks.GetLength(0); countX++)
            {
                for (int countY = 0; countY < bottomBlocks.GetLength(1); countY++)
                {
                    int currentColor = bottomBlocks[countX, countY];
                    if (currentColor != -1)
                    {
                        spriteBatch.Draw(tetrisBlock, new Rectangle(countX * 32, countY * 32, 32, 32), colors[currentColor]);
                    }
                }
            }
            for (int count = 0; count < fallingBlocks.Count; count++)
            {
                int x = (int)fallingBlocks[count].X;
                int y = (int)fallingBlocks[count].Y;
                spriteBatch.Draw(tetrisBlock, new Rectangle(x * 32, y * 32, 32, 32), colors[shapeColor]);

            }
            /*
            for (int count = 0; count < fallingBlocks.Count; count++)
            {
                int x = (int)fallingBlocks[count].X;
                int y = (int)fallingBlocks[count].Y;
                spriteBatch.Draw(tetrisBlock, new Rectangle(415, 125, 32, 32), colors[shapeColor]);

            }*/

            foreach (Vector2 vec in nextBlocks)
                spriteBatch.Draw(next, new Rectangle((int)vec.X * 32 + 300, (int)vec.Y * 32 + 100, 32, 32), colors[nextShapeColor]);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
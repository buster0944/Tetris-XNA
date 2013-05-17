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
        public Texture2D scoreBoard;
        public Texture2D next;
        public Texture2D tetrisBlock;
        Color[] colors;
        List<Vector2> fallingBlocks;
        int shapeColor;
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
        protected override void Initialize()
        {
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
            ResetFalling();
            ClearRows();
            Rotate();
            base.Initialize();
        }

        void ResetFalling()
        {
            //chooses a random color and based on that color it chooses the shape assigned to it
            shapeColor = random.Next(0, 7);
            fallingBlocks = new List<Vector2>();

            if (shapeColor == 0)
            {
                // square
                fallingBlocks.Add(new Vector2(4, 0));
                fallingBlocks.Add(new Vector2(5, 0));
                fallingBlocks.Add(new Vector2(4, 1));
                fallingBlocks.Add(new Vector2(5, 1));
            }
            else if (shapeColor == 1)
            {
                // I
                fallingBlocks.Add(new Vector2(5, 0));
                fallingBlocks.Add(new Vector2(4, 0));
                fallingBlocks.Add(new Vector2(6, 0));
                fallingBlocks.Add(new Vector2(7, 0));
            }
            else if (shapeColor == 2)
            {
                // L
                fallingBlocks.Add(new Vector2(5, 1));
                fallingBlocks.Add(new Vector2(4, 0));
                fallingBlocks.Add(new Vector2(5, 0));
                fallingBlocks.Add(new Vector2(5, 2));
            }
            else if (shapeColor == 3)
            {
                // Z
                fallingBlocks.Add(new Vector2(5, 0));
                fallingBlocks.Add(new Vector2(4, 0));
                fallingBlocks.Add(new Vector2(5, 1));
                fallingBlocks.Add(new Vector2(6, 1));
            }
            else if (shapeColor == 4)
            {
                // S
                fallingBlocks.Add(new Vector2(5, 1));
                fallingBlocks.Add(new Vector2(4, 0));
                fallingBlocks.Add(new Vector2(4, 1));
                fallingBlocks.Add(new Vector2(5, 2));
            }
            else if (shapeColor == 5)
            {
                // J
                fallingBlocks.Add(new Vector2(4, 1));
                fallingBlocks.Add(new Vector2(4, 0));
                fallingBlocks.Add(new Vector2(5, 0));
                fallingBlocks.Add(new Vector2(4, 2));
            }
            else if (shapeColor == 6)
            {
                // T
                fallingBlocks.Add(new Vector2(5, 0));
                fallingBlocks.Add(new Vector2(4, 0));
                fallingBlocks.Add(new Vector2(6, 0));
                fallingBlocks.Add(new Vector2(5, 1));
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
            next = Content.Load<Texture2D>("next");//next piece area, haven't figured out how to display this properly
            tetrisBlock = Content.Load<Texture2D>("testsprite");
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {



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
            if (score >= 3000 && score < 6000)
            {
                level = 2;
            }
            if (score >= 6000)
            {
                level = 3;
            }
            if (level == 2)
            {
                fallSpeedRegular = 20;
            }
            if (level == 3)
            {
                fallSpeedRegular = 10;
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
            spriteBatch.Draw(background, new Rectangle(0, 0, 760, 650), Color.White);
            spriteBatch.Draw(bucket, new Rectangle(27, 0, 325, 650), Color.White);
            spriteBatch.Draw(scoreBoard, new Rectangle(400, 0, 200, 50), Color.White);
            spriteBatch.Draw(next, new Rectangle(400, 100, 100, 100), Color.White);

            spriteBatch.DrawString(scoreFont, "Score  " + score, new Vector2(415, 0), Color.Black);
            spriteBatch.DrawString(levelFont, "Level " + level, new Vector2(415, 20), Color.Black);

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
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
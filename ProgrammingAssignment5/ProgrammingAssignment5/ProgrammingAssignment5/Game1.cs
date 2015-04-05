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
using TeddyMineExplosion;

namespace ProgrammingAssignment5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;

        // random teddy bear support
        Random rand = new Random();
        Texture2D teddySprite;

        // spawning support
        int totalSpawnDelayMilliseconds = 3000;
        int elapsedSpawnDelayMilliseconds = 0;

        // mine support
        Texture2D mineSprite;
        List<Mine> mines = new List<Mine>();

        // saved for efficiency
        Texture2D explosionSprite;

        // game objects
        List<TeddyBear> bears = new List<TeddyBear>();
        List<Explosion> explosions = new List<Explosion>();

        // click processing
        bool leftClickStarted = false;
        bool leftButtonReleased = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //set resolution and make mouse visible
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            // Make the mouse cursor visible
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load teddy sprite
            teddySprite = Content.Load<Texture2D>("teddybear");

            //load mine sprites 
            mineSprite = Content.Load<Texture2D>("mine");

            //load explosion sprite
            explosionSprite = Content.Load<Texture2D>("explosion");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // spawn teddies as appropriate
            elapsedSpawnDelayMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedSpawnDelayMilliseconds > totalSpawnDelayMilliseconds)
            {
                elapsedSpawnDelayMilliseconds = 0;
                totalSpawnDelayMilliseconds = rand.Next(4) * 1000;
                // generate random velocity
                float speed = (float)(rand.Next(5) + 3) / 16;
                double angle = 2 * Math.PI * rand.NextDouble();
                bears.Add(new TeddyBear(teddySprite, new Vector2((float)Math.Cos(angle) * speed, -1 * (float)Math.Sin(angle) * speed), WINDOW_WIDTH, WINDOW_HEIGHT));
            }

            // update teddy bears
            foreach (TeddyBear teddyBear in bears)
            {
                teddyBear.Update(gameTime);
            }
            
            // get current mouse state and update teddy
            MouseState mouse = Mouse.GetState();

            // check for left click started
            if (mouse.LeftButton == ButtonState.Pressed &&
                leftButtonReleased)
            {
                leftClickStarted = true;
                leftButtonReleased = false;
            }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                leftButtonReleased = true;

                // if left click finished, add new mine to list
                if (leftClickStarted)
                {
                    leftClickStarted = false;

                    // add a new mine to the end of the list of mines
                    Mine mine = new Mine(mineSprite, mouse.X, mouse.Y);
                    mines.Add(mine);

                }
            }

            //Detect collision between teddy and mine
            foreach (Mine mine in mines)
            {
                if (mine.Active)
                {
                    foreach (TeddyBear bear in bears)
                    {
                        if (bear.Active)
                        {
                            if (bear.CollisionRectangle.Intersects(mine.CollisionRectangle))
                            {
                                bear.Active = false;
                                mine.Active = false;
                                explosions.Add(new Explosion(explosionSprite, mine.CollisionRectangle.Center.X, mine.CollisionRectangle.Center.Y));
                            }
                        }
                    }
                }
            }

            //update explosions
            foreach (Explosion explosion in explosions)
            {
                explosion.Update(gameTime);
            }

            // remove dead teddies
            for (int i = bears.Count - 1; i >= 0; i--)
            {
                if (!bears[i].Active)
                {
                    bears.RemoveAt(i);
                }
            }

            // remove dead mines
            for (int i = mines.Count - 1; i >= 0; i--)
            {
                if (!mines[i].Active)
                {
                    mines.RemoveAt(i);
                }
            }

            // remove dead explosions
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                if (!explosions[i].Playing)
                {
                    explosions.RemoveAt(i);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw game objects
            spriteBatch.Begin();

            // Draw the mine objects
            foreach (Mine mine in mines)
            {
                mine.Draw(spriteBatch);
            }
            //Draw teddies
            foreach (TeddyBear teddyBear in bears)
            {
                teddyBear.Draw(spriteBatch);
            }
            //Draw explosions
            foreach(Explosion explosion in explosions)
            {
                explosion.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

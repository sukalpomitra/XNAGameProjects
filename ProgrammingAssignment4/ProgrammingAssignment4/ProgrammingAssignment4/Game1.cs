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

namespace ProgrammingAssignment4
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

        // teddy support
        Texture2D teddySprite;
        Teddy teddy;

        // pickup support
        Texture2D pickupSprite;
        List<Pickup> pickups = new List<Pickup>();

        // click processing
        bool rightClickStarted = false;
        bool rightButtonReleased = true;

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

            //load teddy and pickup sprites 
            teddySprite = Content.Load<Texture2D>("teddybear");
            pickupSprite = Content.Load<Texture2D>("pickup");

            //create teddy object centered in window
            teddy = new Teddy(teddySprite, new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2));
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

            // get current mouse state and update teddy
            MouseState mouse = Mouse.GetState();

            // check for right click started
            if (mouse.RightButton == ButtonState.Pressed &&
                rightButtonReleased)
            {
                rightClickStarted = true;
                rightButtonReleased = false;
            }
            else if (mouse.RightButton == ButtonState.Released)
            {
                rightButtonReleased = true;

                // if right click finished, add new pickup to list
                if (rightClickStarted)
                {
                    rightClickStarted = false;

                    // add a new pickup to the end of the list of pickups
                    Pickup pickup = new Pickup(pickupSprite,new Vector2(mouse.X,mouse.Y));
                    pickups.Add(pickup);

                    // if this is the first pickup in the list, set teddy target
                    if (pickups.Count == 1)
                        teddy.SetTarget(new Vector2(mouse.X, mouse.Y));
                }
            }

            // check for collision between collecting teddy and targeted pickup
            if (teddy.Collecting &&
                teddy.CollisionRectangle.Intersects(pickups[0].CollisionRectangle))
            {
                // remove targeted pickup from list (it's always at location 0)
                pickups.RemoveAt(0);

                // if there's another pickup to collect, set teddy target
                // If not, stop the teddy from collecting
                if (pickups.Count > 0)
                    teddy.SetTarget(new Vector2(pickups[0].CollisionRectangle.Center.X, pickups[0].CollisionRectangle.Center.Y ));
                else
                    teddy.Collecting = false;

            }
            teddy.Update(gameTime,mouse);
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

            // Draw the teddy object
            teddy.Draw(spriteBatch);
            foreach (Pickup pickup in pickups)
            {
                pickup.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

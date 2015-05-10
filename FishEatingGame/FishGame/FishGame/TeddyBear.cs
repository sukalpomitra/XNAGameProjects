﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame
{
    /// <summary>
    /// A class for a teddy bear
    /// </summary>
    class TeddyBear
    {
        #region Fields

        // graphic and drawing info
        Texture2D sprite;
        Rectangle drawRectangle = new Rectangle();

        // velocity information
        Vector2 velocity = new Vector2(0, 0);

        // whether or not the teddy bear is active
        bool active = true;

        // bouncing support
        int windowWidth;
        int windowHeight;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a teddy bear with random velocity
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="spriteName">the name of the sprite for the teddy bear</param>
        /// <param name="windowWidth">the window width</param>
        /// <param name="windowHeight">the window height</param>
        public TeddyBear(ContentManager contentManager, string spriteName,
            int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            LoadContent(contentManager, spriteName);

            // generate random x and y
            Random rand = new Random();
            drawRectangle.X = rand.Next(windowWidth - drawRectangle.Width);
            drawRectangle.Y = rand.Next(windowHeight - drawRectangle.Height);

            // generate random angle and x and y direction components
            double angle = 2 * Math.PI * rand.NextDouble();
            double xDirection = Math.Cos(angle);
            double yDirection = -1 * Math.Sin(angle);
            int speed = rand.Next(5) + 2;

            // set random velocity
            velocity.X = (float)(speed * xDirection);
            velocity.Y = (float)(speed * yDirection);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the x location of the center of the teddy bear
        /// </summary>
        public int X
        {
            get { return drawRectangle.X + drawRectangle.Width / 2; }
            set
            { 
                drawRectangle.X = value - drawRectangle.Width / 2; 

                // clamp to keep in range
                if (drawRectangle.Left < 0)
                {
                    drawRectangle.X = 0;
                }
                else if (drawRectangle.Right > windowWidth)
                {
                    drawRectangle.X = windowWidth - drawRectangle.Width;
                }
            }
        }

        /// <summary>
        /// Gets and sets the y location of the center of the teddy bear
        /// </summary>
        public int Y
        {
            get { return drawRectangle.Y + drawRectangle.Height / 2; }
            set
            { 
                drawRectangle.Y = value - drawRectangle.Height / 2;

                // clamp to keep in range
                if (drawRectangle.Top < 0)
                {
                    drawRectangle.Y = 0;
                }
                else if (drawRectangle.Bottom > windowHeight)
                {
                    drawRectangle.Y =  windowHeight - drawRectangle.Height;
                }
            }
        }

        /// <summary>
        /// Gets the collision rectangle for the teddy bear
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        /// <summary>
        /// Gets and sets whether or not the teddy bear is active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the teddy bear's location, bouncing if necessary
        /// </summary>
        public void Update()
        {
            if (active)
            {
                // move the teddy bear
                drawRectangle.X += (int)(velocity.X);
                drawRectangle.Y += (int)(velocity.Y);

                // bounce as necessary
                BounceTopBottom();
                BounceLeftRight();
            }
        }

        /// <summary>
        /// Draws the teddy bear
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(sprite, drawRectangle, Color.White);
            }
        }

        /// <summary>
        /// Bounces the teddy bear by reversing the x and y velocities
        /// </summary>
        public void Bounce()
        {
            velocity.X *= -1;
            velocity.Y *= -1;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the teddy bear
        /// </summary>
        /// <param name="contentManager">the content manager to use</param>
        /// <param name="spriteName">the name of the sprite for the teddy bear</param>
        private void LoadContent(ContentManager contentManager, string spriteName)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>(spriteName);
            drawRectangle.Width = sprite.Width;
            drawRectangle.Height = sprite.Height;
        }

        /// <summary>
        /// Bounces the teddy bear off the top and bottom window borders if necessary
        /// </summary>
        private void BounceTopBottom()
        {
            if (drawRectangle.Top < 0)
            {
                // bounce off top
                drawRectangle.Y = 0;
                velocity.Y *= -1;
            }
            else if (drawRectangle.Bottom > windowHeight)
            {
                // bounce off bottom
                drawRectangle.Y = windowHeight - drawRectangle.Height;
                velocity.Y *= -1;
            }
        }

        /// <summary>
        /// Bounces the teddy bear off the left and right window borders if necessary
        /// </summary>
        private void BounceLeftRight()
        {
            if (drawRectangle.Left < 0)
            {
                // bounc off left
                drawRectangle.X = 0;
                velocity.X *= -1;
            }
            else if (drawRectangle.Right > windowWidth)
            {
                // bounce off right
                drawRectangle.X = windowWidth - drawRectangle.Width;
                velocity.X *= -1;
            }
        }

        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProgrammingAssignment4
{
    /// <summary>
    /// A teddy bear
    /// </summary>
    public class Teddy
    {
        #region Fields

        bool collecting = false;

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;
        int halfDrawRectangleWidth;
        int halfDrawRectangleHeight;

        // moving support
        const float BASE_SPEED = 0.3f;
        Vector2 location;
        Vector2 velocity = Vector2.Zero;

        // click processing
        bool leftClickStarted = false;
        bool leftButtonReleased = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sprite">sprite for the teddy</param>
        /// <param name="location">location of the center of the teddy</param>
        public Teddy(Texture2D sprite, Vector2 location)
        {
            this.sprite = sprite;
            this.location = location;

            // set draw rectangle so teddy is centered on location
            drawRectangle = new Rectangle((int)location.X - (sprite.Width / 2), (int)location.Y - (sprite.Height / 2), sprite.Width, sprite.Height);

            // set halfDrawRectangleWidth and halfDrawRectangleHeight for efficiency
            halfDrawRectangleHeight = drawRectangle.Height / 2;
            halfDrawRectangleWidth = drawRectangle.Width / 2;

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets whether or not the teddy is collecting
        /// </summary>
        public bool Collecting
        {
            get { return collecting; }
            set { collecting = value; }
        }

        /// <summary>
        /// Gets the collision rectangle for the teddy
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the teddy
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="mouse">current mouse state</param>
        public void Update(GameTime gameTime, MouseState mouse)
        {
            // update location based on velocity if teddy is collecting
            if (collecting)
            {
                
                location.X = location.X + (int)(velocity.X * gameTime.ElapsedGameTime.Milliseconds);
                location.Y = location.Y + (int)(velocity.Y * gameTime.ElapsedGameTime.Milliseconds);
                drawRectangle = new Rectangle((int)location.X - halfDrawRectangleWidth, (int)location.Y - halfDrawRectangleHeight, sprite.Width, sprite.Height);
            }


            // check for mouse over teddy
            if (drawRectangle.Contains(mouse.X, mouse.Y))
            {
                // check for left click started on teddy
                if (mouse.LeftButton == ButtonState.Pressed &&
                    leftButtonReleased)
                {
                    leftClickStarted = true;
                    leftButtonReleased = false;
                }
                else if (mouse.LeftButton == ButtonState.Released)
                {
                    leftButtonReleased = true;

                    // if click finished on teddy, start collecting
                    if (leftClickStarted)
                    {
                        collecting = true;
                        leftClickStarted = false;
                    }
                }
            }
            else
            {
                // no clicking on teddy
                leftClickStarted = false;
                leftButtonReleased = false;
            }
        }

        /// <summary>
        /// Draws the teddy
        /// </summary>
        /// <param name="spriteBatch">sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // use the sprite batch to draw the teddy
            spriteBatch.Draw(sprite, drawRectangle, Color.White);

        }

        /// <summary>
        /// Sets a target for the teddy to move toward
        /// </summary>
        /// <param name="target">target</param>
        public void SetTarget(Vector2 target)
        {
            // STUDENTS: set teddy velocity based on teddy center location and target
            Vector2 distance = new Vector2();
            distance.X = target.X - location.X;
            distance.Y = target.Y - location.Y;
            distance.Normalize();
            velocity.X = distance.X * BASE_SPEED;
            velocity.Y = distance.Y * BASE_SPEED;
        }

        #endregion
    }
}

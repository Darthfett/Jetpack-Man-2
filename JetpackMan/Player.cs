using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetpackMan
{
    class Player
    {
        public Vector2 position;
        public Vector2 velocity;
        public Texture2D texture;

        public Player(Vector2 position)
        {
            this.position = position;
            this.velocity = new Vector2(0, 0);
            this.texture = null;
        }

        public Vector2 GetScreenPosition(Viewport viewport)
        {
            return new Vector2(this.position.X, viewport.Height - this.position.Y - this.texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(this.texture, this.GetScreenPosition(graphics.GraphicsDevice.Viewport), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void CheckForCollision()
        {
            // Collision with bottom of screen
            if (this.position.Y < 0)
            {
                this.position.Y = 0;
                this.velocity.Y = 0;
            }
        }

        public void Update()
        {
            // Keyboard state
            if (Keyboard.GetState().IsKeyDown(Keys.W)) /* Jetpack */
            {
                this.velocity.Y += 0.6f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.position.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.position.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) /* Jump */
            {
                if (this.position.Y == 0)
                {
                    this.velocity.Y += 10;
                }
            }

            // Gravity
            this.velocity.Y -= 0.5f;

            // Apply velocity to position
            this.position += this.velocity;

            CheckForCollision();
        }
    }
}

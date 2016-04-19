using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Shapes;
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

        public RectangleF BoundingRect
        {
            get
            {
                RectangleF bounds = new RectangleF(position, texture.Bounds.Size.ToVector2());
                return bounds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void CheckForCollision(TiledMap map)
        {
            // Collision with bottom of map
            if (this.BoundingRect.Bottom > map.HeightInPixels)
            {
                this.position.Y = map.HeightInPixels - texture.Height;
                this.velocity.Y = 0;
            }
            // Collision with top of map
            if (this.BoundingRect.Top < 0)
            {
                this.position.Y = 0;
                this.velocity.Y = 0;
            }
            // Collision with top of map
            if (this.BoundingRect.Left < 0)
            {
                this.position.X = 0;
                this.velocity.X = 0;
            }
            // Collision with top of map
            if (this.BoundingRect.Right > map.WidthInPixels)
            {
                this.position.X = map.WidthInPixels - texture.Width;
                this.velocity.X = 0;
            }
        }

        public void Update(TiledMap map)
        {
            // Keyboard state
            if (Keyboard.GetState().IsKeyDown(Keys.W)) /* Jetpack */
            {
                this.velocity.Y -= 0.6f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.position.X -= 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.position.X += 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) /* Jump */
            {
                
                if (this.BoundingRect.Bottom == map.HeightInPixels)
                {
                    this.velocity.Y -= 10;
                }
            }

            // Gravity
            this.velocity.Y += 0.5f;

            // Apply velocity to position
            this.position += this.velocity;

            CheckForCollision(map);
        }
    }
}

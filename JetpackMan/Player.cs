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
        const float WalkingSpeed = 1.5f;
        const float JumpSpeed = 5f;
        const float GravityAccel = 0.25f;
        const float JetpackAccel = 0.30f;
        const int MaxJetpackFuelFrames = 90;

        public Vector2 position;
        public Vector2 velocity;
        public Texture2D texture;

        private int JetpackFuelCtr = MaxJetpackFuelFrames;

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
                if (JetpackFuelCtr <= 0)
                {
                    this.velocity.Y -= (GravityAccel / 2f);
                    JetpackFuelCtr = 0;
                }
                else
                {
                    this.velocity.Y -= JetpackAccel;
                    JetpackFuelCtr -= 1;
                }
            }
            else
            {
                if (JetpackFuelCtr >= MaxJetpackFuelFrames)
                {
                    JetpackFuelCtr = MaxJetpackFuelFrames;
                }
                else
                {
                    JetpackFuelCtr += 1;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.position.X -= WalkingSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.position.X += WalkingSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) /* Jump */
            {
                
                if (this.BoundingRect.Bottom == map.HeightInPixels)
                {
                    this.velocity.Y -= JumpSpeed;
                }
            }

            // foreach (var layer in map.TileLayers)
            // {
            //     foreach (var tile in layer.Tiles)
            //     {
            //         var region = map.GetTileRegion(tile.Id);
            //         if (region != null && region.Texture.Name == "Tilesets/cacti")
            //         {
            //             Console.WriteLine(region.Texture);
            //         }
            //     }
            // }

            // Gravity
            this.velocity.Y += GravityAccel;

            // Apply velocity to position
            this.position += this.velocity;

            CheckForCollision(map);
        }
    }
}

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
        public bool onGround;

        private int JetpackFuelCtr = MaxJetpackFuelFrames;

        public Player(Vector2 position)
        {
            this.position = position;
            this.velocity = new Vector2(0, 0);
            this.texture = null;
            this.onGround = false;
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
                onGround = true;
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

            // Check for collision using collision layer
            foreach (var layer in map.TileLayers)
            {
                if (!layer.Name.Equals("Collision"))
                {
                    continue;
                }
                foreach (var tile in layer.Tiles)
                {
                    if (
                        ((tile.X * 32) > this.BoundingRect.Right) ||
                        ((tile.Y * 32) > this.BoundingRect.Bottom) ||
                        (((tile.X * 32) + 32) < this.BoundingRect.Left) ||
                        (((tile.Y * 32) + 32) < this.BoundingRect.Top)
                       )
                    {
                        continue;
                    }


                    var region = map.GetTileRegion(tile.Id);
                    if (region != null && region.Texture.Name == "Tilesets/collision"
                        && (region.X + region.Y == 35))
                    {
                        this.position.Y = (tile.Y * 32) - this.BoundingRect.Height;
                        this.velocity.Y = 0;
                        onGround = true;
                    }
                }
            }
        }

        public void Update(TiledMap map)
        {
            // Left
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.position.X -= WalkingSpeed;
            }

            // Right
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.position.X += WalkingSpeed;
            }

            // Jump
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                
                if (onGround)
                {
                    onGround = false;
                    this.velocity.Y -= JumpSpeed;
                }
            }

            // Jetpack
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                onGround = false;
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

            // Gravity
            this.velocity.Y += GravityAccel;

            // Apply velocity to position
            this.position += this.velocity;

            CheckForCollision(map);
        }
    }
}

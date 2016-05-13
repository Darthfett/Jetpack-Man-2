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
    enum FacingDirection
    {
        Left,
        Right
    }

    class Player : IEntity
    {
        const Keys MoveLeftKey = Keys.A;
        const Keys MoveRightKey = Keys.D;
        const Keys JumpKey = Keys.Space;
        const Keys JetpackKey = Keys.W;
        const Keys FireKey = Keys.J;

        const float WalkingSpeed = 1.5f;
        const float JumpSpeed = 5f;
        const float GravityAccel = 0.25f;
        const float JetpackAccel = 0.30f;
        const float BulletSpeed = 8f;
        const int BulletFireRate = 8;
        public const int MaxJetpackFuelFrames = 90;

        public Vector2 position;
        public Vector2 velocity;
        Texture2D texture;
        Texture2D bulletTexture;
        public bool onGround;
        public FacingDirection facingDirection;

        int bulletFireCounter = 0;
        public int jetpackFuelCtr { get; private set; } = MaxJetpackFuelFrames;

        public RectangleF BoundingRect
        {
            get
            {
                RectangleF bounds = new RectangleF(position, texture.Bounds.Size.ToVector2());
                return bounds;
            }
        }

        public Player(Vector2 position, Texture2D texture, Texture2D bulletTexture)
        {
            this.position = position;
            this.texture = texture;
            this.velocity = new Vector2(0, 0);
            this.onGround = false;
            this.facingDirection = FacingDirection.Right;
            this.bulletTexture = bulletTexture;
        }

        public bool IsDestroyed()
        {
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void CheckForCollision(TiledMap map)
        {
            // Collision with bottom of map
            if (BoundingRect.Bottom > map.HeightInPixels)
            {
                position.Y = map.HeightInPixels - texture.Height;
                velocity.Y = 0;
                onGround = true;
            }
            // Collision with top of map
            if (BoundingRect.Top < 0)
            {
                position.Y = 0;
                velocity.Y = 0;
            }
            // Collision with top of map
            if (BoundingRect.Left < 0)
            {
                position.X = 0;
                velocity.X = 0;
            }
            // Collision with top of map
            if (BoundingRect.Right > map.WidthInPixels)
            {
                position.X = map.WidthInPixels - texture.Width;
                velocity.X = 0;
            }


            // Check for collision using collision layer
            TiledTileLayer layer = (TiledTileLayer) map.GetLayer("Collision");
            foreach (var tile in layer.Tiles)
            {
                if (
                    ((tile.X * 32) > BoundingRect.Right) ||
                    ((tile.Y * 32) > BoundingRect.Bottom) ||
                    (((tile.X * 32) + 32) < BoundingRect.Left) ||
                    (((tile.Y * 32) + 32) < BoundingRect.Top)
                    )
                {
                    continue;
                }


                var region = map.GetTileRegion(tile.Id);
                if (region != null && region.Texture.Name == "Tilesets/collision"
                    && (region.X + region.Y == 35))
                {
                    position.Y = (tile.Y * 32) - BoundingRect.Height;
                    velocity.Y = 0;
                    onGround = true;
                }
            }
        }

        public void Update(TiledMap map)
        {
            // Left
            if (Keyboard.GetState().IsKeyDown(MoveLeftKey))
            {
                position.X -= WalkingSpeed;
                if (!Keyboard.GetState().IsKeyDown(MoveRightKey))
                {
                    facingDirection = FacingDirection.Left;
                }
            }

            // Right
            if (Keyboard.GetState().IsKeyDown(MoveRightKey))
            {
                position.X += WalkingSpeed;
                if (!Keyboard.GetState().IsKeyDown(MoveLeftKey))
                {
                    facingDirection = FacingDirection.Right;
                }
            }

            // Jump
            if (Keyboard.GetState().IsKeyDown(JumpKey))
            {
                
                if (onGround)
                {
                    onGround = false;
                    velocity.Y -= JumpSpeed;
                }
            }

            // Jetpack
            if (Keyboard.GetState().IsKeyDown(JetpackKey))
            {
                onGround = false;
                if (jetpackFuelCtr <= 0)
                {
                    velocity.Y -= (GravityAccel / 2f);
                    jetpackFuelCtr = 0;
                }
                else
                {
                    velocity.Y -= JetpackAccel;
                    jetpackFuelCtr -= 1;
                }
            }
            else
            {
                if (jetpackFuelCtr >= MaxJetpackFuelFrames)
                {
                    jetpackFuelCtr = MaxJetpackFuelFrames;
                }
                else
                {
                    jetpackFuelCtr += 1;
                }
            }

            if (bulletFireCounter > 0)
            {
                bulletFireCounter--;
            }

            // Fire
            if (Keyboard.GetState().IsKeyDown(FireKey) && bulletFireCounter == 0)
            {
                var vel = new Vector2(-BulletSpeed, 0);
                var pos = new Vector2(BoundingRect.Left - bulletTexture.Width, BoundingRect.Center.Y);
                if (facingDirection == FacingDirection.Right)
                {
                    vel.X = BulletSpeed;
                    pos.X = BoundingRect.Right;
                }
                EntityManager.AddEntity(new Projectile(pos, vel, bulletTexture));
                bulletFireCounter = BulletFireRate;
            }



            // Gravity
            velocity.Y += GravityAccel;

            // Apply velocity to position
            position += velocity;

            CheckForCollision(map);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetpackMan
{
    class Projectile : IDrawable, IEntity
    {
        public Vector2 position;
        public Vector2 velocity;
        Texture2D texture;
        bool destroyed;

        public RectangleF BoundingRect
        {
            get
            {
                RectangleF bounds = new RectangleF(position, texture.Bounds.Size.ToVector2());
                return bounds;
            }
        }

        public Projectile(Vector2 position, Vector2 velocity, Texture2D texture)
        {
            this.position = position;
            this.velocity = velocity;
            this.texture = texture;
            this.destroyed = false;
        }

        public bool IsDestroyed()
        {
            return destroyed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!this.destroyed)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
        
        public void Update(TiledMap map)
        {
            // Apply velocity to position
            position += velocity;

            var mapBounds = new RectangleF(0, 0, map.WidthInPixels, map.HeightInPixels);

            if (!mapBounds.Intersects(BoundingRect))
            {
                destroyed = true;
            }

            if (!destroyed)
            {
                // Check for collision using collision layer
                TiledTileLayer layer = (TiledTileLayer)map.GetLayer("Collision");
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
                        && (region.X == 34 && region.Y == 1))
                    {
                        destroyed = true;
                    }
                }
            }
        }

    }
}

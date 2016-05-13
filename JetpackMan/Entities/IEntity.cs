using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetpackMan
{
    interface IEntity
    {
        bool IsDestroyed();
        void Update(TiledMap map);
        void Draw(SpriteBatch spriteBatch);
    }
}

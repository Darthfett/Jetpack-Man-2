using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Player(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.velocity = new Vector2(0, 0);
            this.texture = texture;
        }
    }
}

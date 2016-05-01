using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Shapes;

namespace JetpackMan
{
    enum ProgressFillDirection
    {
        LeftToRight,
        RightToLeft,
        BottomToTop,
        TopToBottom
    };

    class ProgressBar
    {
        public RectangleF rectangle;
        public Color bgColor;
        public Color progressColor;
        public ProgressFillDirection fillDirection;

        float _progress;
        public float progress {
            get { return _progress; }
            set 
            {
                if (value > 1 || value < 0)
                {
                    throw new System.ArgumentException("ProgressBar value is out of range");
                }
                _progress = value;
            }
        }

        public ProgressBar(RectangleF rectangle, Color bgColor, Color progressColor, ProgressFillDirection fillDirection, float progress = 0)
        {
            this.rectangle = new RectangleF(rectangle.Location, rectangle.Size);
            this.bgColor = bgColor;
            this.progressColor = progressColor;
            this.fillDirection = fillDirection;
            this.progress = progress;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(rectangle, bgColor);

            Vector2 location = rectangle.Location;
            Vector2 size = rectangle.Size;
            switch (fillDirection)
            {
                case ProgressFillDirection.LeftToRight:
                    size.X = progress * rectangle.Width;
                    break;
                case ProgressFillDirection.TopToBottom:
                    size.Y = progress * rectangle.Height;
                    break;
                case ProgressFillDirection.RightToLeft:
                    size.X = progress * rectangle.Width;
                    location.X = rectangle.Right - size.X;
                    break;
                case ProgressFillDirection.BottomToTop:
                    size.Y = progress * rectangle.Height;
                    location.Y = rectangle.Bottom - size.Y;
                    break;
                default:
                    throw new NotImplementedException();
            }
            spriteBatch.FillRectangle(new RectangleF(location, size), progressColor);
        }
    }
}

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
    enum ProgressBarFillDirection
    {
        LeftToRight,
        RightToLeft,
        BottomToTop,
        TopToBottom
    };

    class ProgressBar
    {
        public Rectangle rectangle;
        public Color bgColor;
        public Color progressColor;
        public ProgressBarFillDirection fillDirection;

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

        public ProgressBar(Rectangle rectangle, Color bgColor, Color progressColor, ProgressBarFillDirection fillDirection, float progress = 0)
        {
            this.rectangle = new Rectangle(rectangle.Location, rectangle.Size);
            this.bgColor = bgColor;
            this.progressColor = progressColor;
            this.fillDirection = fillDirection;
            this.progress = progress;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(rectangle, bgColor);

            Point location = rectangle.Location;
            Point size = rectangle.Size;
            switch (fillDirection)
            {
                case ProgressBarFillDirection.LeftToRight:
                    size.X = (int)(progress * rectangle.Width);
                    break;
                case ProgressBarFillDirection.TopToBottom:
                    size.Y = (int)(progress * rectangle.Height);
                    break;
                case ProgressBarFillDirection.RightToLeft:
                    size.X = (int)(progress * rectangle.Width);
                    location.X = rectangle.Right - size.X;
                    break;
                case ProgressBarFillDirection.BottomToTop:
                    size.Y = (int) (progress * rectangle.Height);
                    location.Y = rectangle.Bottom - size.Y;
                    break;
                default:
                    throw new NotImplementedException();
            }
            spriteBatch.FillRectangle(new Rectangle(location, size), progressColor);
        }
    }
}

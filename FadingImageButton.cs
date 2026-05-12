using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TixNova__Final 
{
    public class FadingImageButton : PictureBox
    {
        private float _opacity = 0.0f; 
        private readonly Timer _fadeTimer;
        private bool _isHovered = false;

        public FadingImageButton()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.Cursor = Cursors.Hand;

            _fadeTimer = new Timer { Interval = 15 }; 
            _fadeTimer.Tick += FadeTimer_Tick;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            _fadeTimer.Start();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            _fadeTimer.Start();
            base.OnMouseLeave(e);
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            
            if (_isHovered && _opacity < 1.0f)
            {
                _opacity += 0.08f; 
                if (_opacity > 1.0f) _opacity = 1.0f;
            }
            
            else if (!_isHovered && _opacity > 0.0f)
            {
                _opacity -= 0.08f; 
                if (_opacity < 0.0f) _opacity = 0.0f;
            }
            else
            {
                _fadeTimer.Stop();
            }

            this.Invalidate(); 
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            
            if (this.Image != null && _opacity > 0)
            {
                ColorMatrix matrix = new ColorMatrix { Matrix33 = _opacity }; 

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                pe.Graphics.DrawImage(this.Image,
                    new Rectangle(0, 0, this.Width, this.Height),
                    0, 0, this.Image.Width, this.Image.Height,
                    GraphicsUnit.Pixel, attributes);
            }
        }
    }
}
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNova_Final 
{
    public class RoundedPictureBox : PictureBox
    {
        
        public int BorderRadius { get; set; } = 20;

        public RoundedPictureBox()
        {
            this.DoubleBuffered = true;
        }

   
        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
           
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
          
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            
            base.OnPaint(pe);

            
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectSurface = this.ClientRectangle;

            if (BorderRadius > 2)
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, BorderRadius))
                {
                    
                    this.Region = new Region(pathSurface);

                   
                    if (this.Parent != null)
                    {
                        using (Pen penSurface = new Pen(this.Parent.BackColor, 2))
                        {
                            pe.Graphics.DrawPath(penSurface, pathSurface);
                        }
                    }
                }
            }
            else
            {
              
                this.Region = new Region(rectSurface);
            }
        }

        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }
    }
}
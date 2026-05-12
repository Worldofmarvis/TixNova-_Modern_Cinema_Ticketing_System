using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNova_Final
{
    public class GlassPanel : Panel
    {
        public int BorderRadius { get; set; } = 30;

        
        public Color GlassColor { get; set; } = Color.FromArgb(180, 10, 20, 30);

        
        public Color BorderColor { get; set; } = Color.FromArgb(50, 255, 255, 255);

        public GlassPanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            
            int rectWidth = this.Width - 1;
            int rectHeight = this.Height - 1;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                path.AddArc(0, 0, BorderRadius, BorderRadius, 180, 90);
                path.AddArc(rectWidth - BorderRadius, 0, BorderRadius, BorderRadius, 270, 90);
                path.AddArc(rectWidth - BorderRadius, rectHeight - BorderRadius, BorderRadius, BorderRadius, 0, 90);
                path.AddArc(0, rectHeight - BorderRadius, BorderRadius, BorderRadius, 90, 90);
                path.CloseFigure();

                
                using (SolidBrush brush = new SolidBrush(GlassColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                
                using (Pen pen = new Pen(BorderColor, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
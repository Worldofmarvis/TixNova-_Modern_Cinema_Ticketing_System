 using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNova__Final
{
    public class OutlineButton : Button
    {
        public int BorderRadius { get; set; } = 12;
        public int BorderThickness { get; set; } = 1;
        public Color BorderColor { get; set; } = Color.FromArgb(100, 120, 140);

        public OutlineButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;

            this.BackColor = Color.FromArgb(35, 45, 55);

            this.ForeColor = Color.White;
            this.Size = new Size(150, 45);
            this.Cursor = Cursors.Hand;
            this.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);

            this.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.ImageAlign = ContentAlignment.MiddleCenter;
            this.TextAlign = ContentAlignment.MiddleCenter;

            this.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 65, 80);
            this.FlatAppearance.MouseDownBackColor = Color.FromArgb(25, 35, 45);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = BorderRadius;
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);

                if (BorderThickness > 0)
                {
                    using (Pen pen = new Pen(BorderColor, BorderThickness))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }
    }
}
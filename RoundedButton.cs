using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNova_Final
{
    public class RoundedButton : Button
    {
        public int BorderRadius { get; set; } = 30;

        private readonly Color GradientStart = Color.FromArgb(0, 194, 255);
        private readonly Color GradientEnd = Color.FromArgb(45, 125, 255);

        private float pulseScale = 1.0f;
        private int pulseAlpha = 0;
        private readonly Timer pulseTimer;
        private bool hasPulsed = false;

        public RoundedButton()
        {
            this.DoubleBuffered = true;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Cursor = Cursors.Hand;
            this.Size = new Size(150, 45);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            pulseTimer = new Timer { Interval = 15 };
            pulseTimer.Tick += (s, e) => {
                pulseScale += 0.05f;
                pulseAlpha -= 10; 

                if (pulseAlpha <= 0)
                {
                    pulseAlpha = 0;
                    pulseTimer.Stop();
                }
                this.Invalidate();
            };
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!hasPulsed)
            {
                pulseAlpha = 200; 
                pulseScale = 1.0f;
                hasPulsed = true;
                pulseTimer.Start();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hasPulsed = false;
            pulseAlpha = 0;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = GetRoundedPath(this.ClientRectangle, BorderRadius))
            {
                this.Region = new Region(path);
                using (LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, GradientStart, GradientEnd, 0f))
                {
                    pevent.Graphics.FillPath(lgb, path);
                }
            }

            if (pulseAlpha > 0)
            {
                pevent.Graphics.ResetClip();
                float extraW = this.Width * (pulseScale - 1);
                float extraH = this.Height * (pulseScale - 1);
                RectangleF pulseRect = new RectangleF(-extraW / 2, -extraH / 2, this.Width + extraW, this.Height + extraH);

                using (GraphicsPath pulsePath = new GraphicsPath())
                {
                    pulsePath.AddEllipse(pulseRect);
                    using (PathGradientBrush glow = new PathGradientBrush(pulsePath))
                    {
                        glow.CenterColor = Color.FromArgb(pulseAlpha, 255, 255, 255);
                        glow.SurroundColors = new Color[] { Color.Transparent };
                        pevent.Graphics.FillPath(glow, pulsePath);
                    }
                }
            }

            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, this.ClientRectangle, this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float r = radius;
            path.AddArc(rect.X, rect.Y, r, r, 180, 90);
            path.AddArc(rect.Right - r - 1, rect.Y, r, r, 270, 90);
            path.AddArc(rect.Right - r - 1, rect.Bottom - r - 1, r, r, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r - 1, r, r, 90, 90);
            path.CloseFigure();
            return path;
        }

    }
}
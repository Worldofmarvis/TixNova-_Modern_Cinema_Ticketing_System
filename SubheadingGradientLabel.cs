using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNova__Final
{
    public class SubheadingGradientLabel : Label
    {
        public SubheadingGradientLabel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (string.IsNullOrEmpty(this.Text) || this.ClientRectangle.Width <= 0 || this.ClientRectangle.Height <= 0)
                return;
            SizeF textSize = e.Graphics.MeasureString(this.Text, this.Font);
            Rectangle gradientRect = new Rectangle(0, 0, this.ClientRectangle.Width, (int)textSize.Height);

            if (gradientRect.Height <= 0) gradientRect.Height = 1;

            Color topColor = Color.White;
            Color bottomColor = Color.FromArgb(45, 200, 255);

            using (LinearGradientBrush brush = new LinearGradientBrush(gradientRect, topColor, bottomColor, LinearGradientMode.Vertical))
            {
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = GetHorizontalAlignment(this.TextAlign);
                    sf.LineAlignment = GetVerticalAlignment(this.TextAlign);

                    e.Graphics.DrawString(this.Text, this.Font, brush, this.ClientRectangle, sf);
                }
            }
        }

        private StringAlignment GetHorizontalAlignment(ContentAlignment align)
        {
            if (align == ContentAlignment.TopLeft || align == ContentAlignment.MiddleLeft || align == ContentAlignment.BottomLeft)
                return StringAlignment.Near;
            if (align == ContentAlignment.TopCenter || align == ContentAlignment.MiddleCenter || align == ContentAlignment.BottomCenter)
                return StringAlignment.Center;
            return StringAlignment.Far;
        }
        private StringAlignment GetVerticalAlignment(ContentAlignment align)
        {
            if (align == ContentAlignment.TopLeft || align == ContentAlignment.TopCenter || align == ContentAlignment.TopRight)
                return StringAlignment.Near;
            if (align == ContentAlignment.MiddleLeft || align == ContentAlignment.MiddleCenter || align == ContentAlignment.MiddleRight)
                return StringAlignment.Center;
            return StringAlignment.Far;
        }
    }
}
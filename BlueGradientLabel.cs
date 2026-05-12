using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNova_Final { 
    public class BlueGradientLabel : Label
    {
        public BlueGradientLabel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (string.IsNullOrEmpty(this.Text) || this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
                return;

            Rectangle rect = this.ClientRectangle;

            
            Color leftColor = Color.FromArgb(85, 220, 225);   
            Color rightColor = Color.FromArgb(15, 75, 175);   

         
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, leftColor, rightColor, LinearGradientMode.Horizontal))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Near; 
                    format.LineAlignment = StringAlignment.Center; 

                    e.Graphics.DrawString(this.Text, this.Font, brush, rect, format);
                }
            }
        }
    }
}
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNova_Final 
{
    public class GradientLabel : Label
    {
        public GradientLabel()
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

         
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Black, Color.Black, LinearGradientMode.Horizontal))
            {
                
                ColorBlend colorBlend = new ColorBlend
                {
                    Colors = new Color[]
                    {
        Color.FromArgb(150, 150, 150), 
        Color.White,                  
        Color.FromArgb(40, 40, 40)    
                    },

                    
                    Positions = new float[] { 0.0f, 0.65f, 1.0f }
                };

                brush.InterpolationColors = colorBlend;

                
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
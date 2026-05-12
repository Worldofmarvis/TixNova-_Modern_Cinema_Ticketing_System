using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNova_Final
{
    public class RoundedTextBox : UserControl
    {
        private TextBox textBox1;
        public int BorderRadius { get; set; } = 15;
        public Color BorderColor { get; set; } = Color.FromArgb(45, 65, 85);

        private string placeholderText = "Enter text...";
        public string Placeholder { get => placeholderText; set { placeholderText = value; Invalidate(); } }

        public RoundedTextBox()
        {
            InitializeTextBox();
            this.DoubleBuffered = true;
            this.Padding = new Padding(12, 10, 12, 10);
            this.Size = new Size(250, 42);
            this.BackColor = Color.FromArgb(24, 38, 52);
        }

        private void InitializeTextBox()
        {
            textBox1 = new TextBox();
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Dock = DockStyle.Fill;
            textBox1.BackColor = this.BackColor;
            textBox1.ForeColor = Color.White;
            textBox1.Font = new Font("Segoe UI", 11F);
            this.Controls.Add(textBox1);

            this.BackColorChanged += (s, e) => textBox1.BackColor = this.BackColor;
        }

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                if (textBox1 != null)
                {
                    textBox1.Font = value;
                    this.Height = textBox1.Height + this.Padding.Top + this.Padding.Bottom;
                }
            }
        }

        public override string Text
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        public bool UsePasswordChar
        {
            get => textBox1.UseSystemPasswordChar;
            set => textBox1.UseSystemPasswordChar = value;
        }

        public char PasswordChar
        {
            get => textBox1.PasswordChar;
            set => textBox1.PasswordChar = value;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = GetRoundedPath(this.ClientRectangle, BorderRadius))
            {
                this.Region = new Region(path);
                using (Pen pen = new Pen(BorderColor, 1))
                {
                    pen.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawPath(pen, path);
                }
            }

            if (string.IsNullOrEmpty(textBox1.Text))
            {

            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float r = radius * 2;
            path.AddArc(rect.X, rect.Y, r, r, 180, 90);
            path.AddArc(rect.Right - r - 1, rect.Y, r, r, 270, 90);
            path.AddArc(rect.Right - r - 1, rect.Bottom - r - 1, r, r, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r - 1, r, r, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
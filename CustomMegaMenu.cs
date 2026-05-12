using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNova__Final
{
    public class TixNovaMenuControl : UserControl
    {
        public TixNovaMenuControl()
        {
            this.Size = new Size(480, 280);
            this.BackColor = Color.Transparent;
            this.DoubleBuffered = true;

            InitializeMenuContent();
            this.Region = new Region(GetRegionPath());
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; 
                return cp;
            }
        }

        private void InitializeMenuContent()
        {
            int col1X = 0;
            int col2X = 240;

            AddHeader("GENRE", col1X, 40);
            string[] genres = { "Action & Adventure", "Sci-Fi & Fantasy", "Horror & Thriller", "Comedy & Family", "Drama", "Animation" };
            for (int i = 0; i < genres.Length; i++)
            {
                AddItem(genres[i], col1X, 75 + (i * 25));
            }

            AddHeader("DISCOVER", col2X, 40);
            string[] discover = { "Coming Soon", "Rated G", "Rated PG", "Rated PG-13", "Rated SPG" };
            for (int i = 0; i < discover.Length; i++)
            {
                AddItem(discover[i], col2X, 75 + (i * 25));
            }
        }

        private void AddHeader(string text, int xOffset, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(xOffset, y),
                Size = new Size(240, 30),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(103, 190, 217),
                BackColor = Color.Transparent,
                UseMnemonic = false
            };
            this.Controls.Add(lbl);
        }

        private void AddItem(string text, int xOffset, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(xOffset, y),
                Size = new Size(240, 25),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(230, 230, 230),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };

            lbl.MouseEnter += (s, e) => lbl.ForeColor = Color.FromArgb(0, 191, 255);
            lbl.MouseLeave += (s, e) => lbl.ForeColor = Color.FromArgb(230, 230, 230);

            lbl.Click += MenuItem_Click;

            this.Controls.Add(lbl);
        }

     
        private void MenuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is Label clickedLabel)) return;

            string selectedCategory = clickedLabel.Text;
            Form mainDashboard = this.FindForm();

            
            mainDashboard?.Hide();

            switch (selectedCategory)
            {
                case "Action":
                case "Action & Adventure":
                    GenreActionForm actionForm = new GenreActionForm();
                    actionForm.FormClosed += (s, args) => mainDashboard?.Show();
                    actionForm.Show();
                    break;

                case "Sci-Fi & Fantasy":
                    GenreSciFi sciFiForm = new GenreSciFi();
                    sciFiForm.FormClosed += (s, args) => mainDashboard?.Show();
                    sciFiForm.Show();
                    break;

                case "Horror & Thriller":
                    GenreHorror horrorForm = new GenreHorror();
                    horrorForm.FormClosed += (s, args) => mainDashboard?.Show();
                    horrorForm.Show();
                    break;

                case "Coming Soon":
                    ComingsoonForm comingSoonForm = new ComingsoonForm();
                    comingSoonForm.FormClosed += (s, args) => mainDashboard?.Show();
                    comingSoonForm.Show();
                    break;

                case "Rated G":
                    RatedGForm ratedGForm = new RatedGForm();
                    ratedGForm.FormClosed += (s, args) => mainDashboard?.Show();
                    ratedGForm.Show();
                    break;

                case "Rated PG":
                    RatedPGForm ratedPGForm = new RatedPGForm();
                    ratedPGForm.FormClosed += (s, args) => mainDashboard?.Show();
                    ratedPGForm.Show();
                    break;

                case "Rated SPG":
                    RatedSPGForm ratedSPGForm = new RatedSPGForm();
                    ratedSPGForm.FormClosed += (s, args) => mainDashboard?.Show();
                    ratedSPGForm.Show();
                    break;

                case "Rated PG-13":
                    RatedPG13 ratedPG13Form = new RatedPG13();
                    ratedPG13Form.FormClosed += (s, args) => mainDashboard?.Show();
                    ratedPG13Form.Show();
                    break;

                case "Comedy & Family":
                    GenreComFam comedyForm = new GenreComFam();
                    comedyForm.FormClosed += (s, args) => mainDashboard?.Show();
                    comedyForm.Show();
                    break;

                case "Drama":
                    GenreDrama dramaForm = new GenreDrama();
                    dramaForm.FormClosed += (s, args) => mainDashboard?.Show();
                    dramaForm.Show();
                    break;

                case "Animation":
                    GenreAnimation animationForm = new GenreAnimation();
                    animationForm.FormClosed += (s, args) => mainDashboard?.Show();
                    animationForm.Show();
                    break;

                default:
                    mainDashboard?.Show();
                    MessageBox.Show($"Coming soon: {selectedCategory} section!", "TixNova+");
                    break;
            }
        }

        public GraphicsPath GetRegionPath()
        {
            return CreateMenuPath(0, 0, this.Width, this.Height);
        }

        public GraphicsPath GetDrawingPath()
        {
            return CreateMenuPath(1, 1, this.Width - 2, this.Height - 2);
        }

        private GraphicsPath CreateMenuPath(int x, int y, int width, int height)
        {
            int radius = 15;
            int arrowWidth = 20;
            int arrowHeight = 15;

            int right = x + width;
            int bottom = y + height;
            int arrowX = x + (width / 2) - (arrowWidth / 2);

            GraphicsPath path = new GraphicsPath();

            path.AddLine(x + radius, y + arrowHeight, arrowX, y + arrowHeight);
            path.AddLine(arrowX, y + arrowHeight, arrowX + (arrowWidth / 2), y);
            path.AddLine(arrowX + (arrowWidth / 2), y, arrowX + arrowWidth, y + arrowHeight);
            path.AddLine(arrowX + arrowWidth, y + arrowHeight, right - radius, y + arrowHeight);

            path.AddArc(right - (radius * 2), y + arrowHeight, radius * 2, radius * 2, 270, 90);
            path.AddLine(right, y + arrowHeight + radius, right, bottom - radius);

            path.AddArc(right - (radius * 2), bottom - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddLine(right - radius, bottom, x + radius, bottom);

            path.AddArc(x, bottom - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.AddLine(x, bottom - radius, x, y + arrowHeight + radius);

            path.AddArc(x, y + arrowHeight, radius * 2, radius * 2, 180, 90);

            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath drawPath = GetDrawingPath())
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(140, 22, 25, 35)))
                {
                    g.FillPath(brush, drawPath);
                }

                using (Pen pen = new Pen(Color.FromArgb(103, 190, 217), 1.0f))
                {
                    g.DrawPath(pen, drawPath);
                }
            }
        }
    }
}
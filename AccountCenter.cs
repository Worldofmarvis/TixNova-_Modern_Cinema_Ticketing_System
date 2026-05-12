using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TixNova_Final;
using TixNovaPlus_Final;

namespace TixNova__Final
{
    public partial class AccountCenter : Form
    {
        private Panel glassContainer;
        public AccountCenter()
        {
            InitializeComponent();

            InitializeGlassContainer();

            
            glassContainer.Left = (this.ClientSize.Width - glassContainer.Width) / 2;
            glassContainer.Top = (this.ClientSize.Height - glassContainer.Height) / 2;

            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            
            MakeRoundedGradientButton(MenuButton, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 30);
            MakeRoundedGradientButton(SearchButton, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 35);
            SetupAllLinkLabelsGlow();
            SetupMenu();

            
            LoadAccountProfile();
        }
        private void InitializeGlassContainer()
        {
            
            glassContainer = new Panel
            {
                Size = new Size(900, 550),
                BackColor = Color.FromArgb(100, 20, 20, 20),
            };

            
            glassContainer.Location = new Point((this.ClientSize.Width - glassContainer.Width) / 2,
                                              (this.ClientSize.Height - glassContainer.Height) / 2);

            glassContainer.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, glassContainer.Width - 1, glassContainer.Height - 1);
                using (var path = GetRoundedRect(rect, 30))
                {
                    glassContainer.Region = new Region(path);
                }
            };

            this.Controls.Add(glassContainer);
        } 
       
        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
        private void LoadAccountProfile()
        {
            
            PictureBox profilePic = new PictureBox
            {
                Size = new Size(110, 110),
                Location = new Point((glassContainer.Width - 110) / 2, 40),
                BackColor = Color.FromArgb(103, 190, 217),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            profilePic.Paint += (s, e) => {
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddEllipse(0, 0, profilePic.Width, profilePic.Height);
                    profilePic.Region = new Region(path);
                }
            };

            
            Label lblUserTitle = new Label
            {
                Text = "CURRENT USERNAME",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(103, 190, 217), 
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(glassContainer.Width, 20),
                Location = new Point(0, 160)
            };

            Label lblActualName = new Label
            {
                Text = UserSession.CurrentUsername.ToUpper(),
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(glassContainer.Width, 40),
                Location = new Point(0, 180)
            };

            
            Button btnChangeUser = CreateSettingsButton("CHANGE USERNAME", 260);
            btnChangeUser.Click += (s, e) => {
                
                MessageBox.Show("Username change requested.");
            };

            
            Button btnChangePass = CreateSettingsButton("CHANGE PASSWORD", 320);
            btnChangePass.Click += (s, e) => {
                
                MessageBox.Show("Password change requested.");
            };

            
            Button btnLogout = new Button
            {
                Text = "LOG OUT",
                Size = new Size(180, 40),
                Location = new Point((glassContainer.Width - 180) / 2, 450),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            MakeRoundedGradientButton(btnLogout, Color.FromArgb(200, 45, 45), Color.FromArgb(150, 0, 0), 20);

            
            glassContainer.Controls.Add(profilePic);
            glassContainer.Controls.Add(lblUserTitle);
            glassContainer.Controls.Add(lblActualName);
            glassContainer.Controls.Add(btnChangeUser);
            glassContainer.Controls.Add(btnChangePass);
            glassContainer.Controls.Add(btnLogout);
        }

        
        private Button CreateSettingsButton(string text, int yPos)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(300, 45),
                Location = new Point((glassContainer.Width - 300) / 2, yPos),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            
            MakeRoundedGradientButton(btn, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 20);
            return btn;
        }
        #region UI Effects and Win32 Blur
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute { WCA_ACCENT_POLICY = 19 }
        internal enum AccentState { ACCENT_ENABLE_BLURBEHIND = 3 }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        private void EnableBlur(IntPtr hwnd)
        {
            var accent = new AccentPolicy { AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND };
            int accentStructSize = Marshal.SizeOf(accent);
            IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(hwnd, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }

        private Form dropDownForm;
        private DateTime menuLastClosedTime = DateTime.MinValue;
        private TixNovaMenuControl menuContent;

        private void SetupMenu()
        {
            menuContent = new TixNovaMenuControl { Location = new Point(0, 0) };
            dropDownForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                Size = menuContent.Size,
                BackColor = Color.Black,
                Region = new Region(menuContent.GetRegionPath())
            };

            dropDownForm.Controls.Add(menuContent);
            dropDownForm.HandleCreated += (s, e) => EnableBlur(dropDownForm.Handle);
            dropDownForm.Deactivate += (s, e) =>
            {
                dropDownForm.Hide();
                menuLastClosedTime = DateTime.Now;
            };
        }

        private void SetupAllLinkLabelsGlow()
        {
            foreach (Control control in this.Controls)
            {
                if (control is LinkLabel linkLabel)
                {
                    var originalColor = linkLabel.LinkColor;
                    Timer pulseTimer = null;
                    int pulseValue = 0;
                    bool increasing = true;

                    linkLabel.MouseEnter += (sender, e) =>
                    {
                        LinkLabel lbl = sender as LinkLabel;
                        pulseTimer = new Timer { Interval = 50 };
                        pulseTimer.Tick += (ts, te) =>
                        {
                            if (increasing) { pulseValue += 15; if (pulseValue >= 255) increasing = false; }
                            else { pulseValue -= 15; if (pulseValue <= 100) increasing = true; }
                            lbl.LinkColor = Color.FromArgb(255, 0, pulseValue, 255);
                        };
                        pulseTimer.Start();
                    };

                    linkLabel.MouseLeave += (sender, e) =>
                    {
                        LinkLabel lbl = sender as LinkLabel;
                        pulseTimer?.Stop();
                        lbl.LinkColor = originalColor;
                    };
                }
            }
        }

        private void MakeRoundedGradientButton(Button btn, Color startColor, Color endColor, int radius = 20)
        {
            btn.FlatStyle = FlatStyle.Popup;
            btn.FlatAppearance.BorderSize = 0;
            btn.Tag = new GradientInfo { StartColor = startColor, EndColor = endColor };
            var originalSize = btn.Size;
            var originalLocation = btn.Location;

            btn.MouseEnter += (sender, e) => {
                btn.Size = new Size(btn.Width + 5, btn.Height + 5);
                btn.Location = new Point(btn.Location.X - 2, btn.Location.Y - 2);
                btn.Cursor = Cursors.Hand;
            };

            btn.MouseLeave += (sender, e) => {
                btn.Size = originalSize;
                btn.Location = originalLocation;
                btn.Cursor = Cursors.Default;
            };

            btn.Paint += (sender, e) => {
                Button b = sender as Button;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    Rectangle rect = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();
                    b.Region = new Region(path);

                    GradientInfo gradient = (GradientInfo)b.Tag;
                    using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(rect, gradient.StartColor, gradient.EndColor, System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    TextRenderer.DrawText(e.Graphics, b.Text, b.Font, rect, b.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };
            btn.Resize += (sender, e) => btn.Invalidate();
        }

        private class GradientInfo
        {
            public Color StartColor { get; set; }
            public Color EndColor { get; set; }
        }
        #endregion

        
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainDashBoard main = new MainDashBoard();
            main.Show();
            this.Hide();
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MoviesForm movie = new MoviesForm();
            movie.Show();
            this.Hide();
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CinemasForm cinemas = new CinemasForm();
            cinemas.Show();
            this.Hide();
        }

        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShopForm shop = new ShopForm();
            shop.Show();
            this.Hide();
        }

        private void LinkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if ((DateTime.Now - menuLastClosedTime).TotalMilliseconds < 100) return;
            if (dropDownForm.Visible) dropDownForm.Hide();
            else
            {
                int xOffset = (linkLabel5.Width - menuContent.Width) / 2;
                Point screenLocation = linkLabel5.PointToScreen(new Point(xOffset, linkLabel5.Height + 5));
                dropDownForm.Location = screenLocation;
                dropDownForm.Show();
                dropDownForm.BringToFront();
            }
        }

        private CustomSearchMenu _searchMenu;
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (_searchMenu == null || _searchMenu.IsDisposed)
            {
                _searchMenu = new CustomSearchMenu(this);
                Point screenPos = SearchButton.PointToScreen(new Point(0, SearchButton.Height));
                _searchMenu.Location = new Point(screenPos.X - (_searchMenu.Width / 2) + (SearchButton.Width / 2), screenPos.Y + 10);
                _searchMenu.Show();
            }
            else { if (_searchMenu.Visible) _searchMenu.Hide(); else _searchMenu.Show(); }
        }

        private CustomMenu _sideMenu;
        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (_sideMenu == null || _sideMenu.IsDisposed)
            {
                _sideMenu = new CustomMenu();
                Point screenPos = this.PointToScreen(new Point(this.Width - _sideMenu.Width - 20, 50));
                _sideMenu.Location = screenPos;
                _sideMenu.Show();
            }
            else { _sideMenu.Visible = !_sideMenu.Visible; }
        }
    }
}
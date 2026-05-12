using System;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TixNova_Final;

namespace TixNova__Final
{
    public class CustomMenu : Form
    {
        public CustomMenu()
        {
            
            this.Size = new Size(280, 600);
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.BackColor = Color.Black;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;

            InitializeSidebarUI();
        }

        #region Blur API Logic
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        internal enum AccentState
        {
            ACCENT_ENABLE_BLURBEHIND = 3
        }

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
            // 2 is the flag that often helps the blur respect the window's region/shape
            var accent = new AccentPolicy
            {
                AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND,
                AccentFlags = 2
            };

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
        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            EnableBlur(this.Handle);
        }

        private void InitializeSidebarUI()
        {
            
            Label closeBtn = new Label
            {
                Text = "✕",
                Font = new Font("Arial", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(103, 190, 217),
                Location = new Point(230, 15),
                Size = new Size(30, 30),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            closeBtn.Click += (s, e) => this.Hide();
            this.Controls.Add(closeBtn);

           
            Panel profileCircle = new Panel
            {
                Size = new Size(60, 60),
                Location = new Point(30, 70),
                BackColor = Color.Transparent
            };
            profileCircle.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen p = new Pen(Color.FromArgb(103, 190, 217), 2f))
                    e.Graphics.DrawEllipse(p, 2, 2, 54, 54);
            };
            this.Controls.Add(profileCircle);

            Label userLabel = new Label
            {
                Text = !string.IsNullOrEmpty(UserSession.CurrentUsername) ? UserSession.CurrentUsername.ToUpper() : "GUEST",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(100, 85),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            this.Controls.Add(userLabel);

           
            string[] items = { "My Bookings", "Account Settings", "Support Center", "Logout" };
            int startY = 170;

            for (int i = 0; i < items.Length; i++)
            {
                bool isLogout = items[i] == "Logout";
                AddMenuLink(items[i], 30, startY + (i * 45), isLogout);
            }

           
            string[] footer = { "About Us", "Privacy", "Terms", "Cookie Policy" };
            for (int i = 0; i < footer.Length; i++)
            {
                AddFooterLink(footer[i], 30, 450 + (i * 30));
            }
        }

        private void AddMenuLink(string text, int x, int y, bool isLogout)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(200, 30),
                ForeColor = isLogout ? Color.FromArgb(0, 100, 200) : Color.White,
                Font = new Font("Segoe UI", 12f, FontStyle.Regular),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };

            lbl.MouseEnter += (s, e) => lbl.ForeColor = Color.FromArgb(103, 190, 217);
            lbl.MouseLeave += (s, e) => lbl.ForeColor = isLogout ? Color.FromArgb(0, 100, 200) : Color.White;

            lbl.Click += (s, e) =>
            {
                if (text == "My Bookings")
                {
                    
                    MyBookings bookingsForm = new MyBookings();

                    
                    bookingsForm.Show();

                   
                    this.Hide();

                    
                    HideAllOtherForms(bookingsForm);
                }
                else if (text == "Account Settings")
                {
                    
                    AccountCenter accountForm = new AccountCenter();
                    accountForm.Show();

                    
                    this.Hide();

                    
                    HideAllOtherForms(accountForm);
                }
                else if (text == "Logout")
                {
                    
                    UserSession.CurrentUsername = null; 

                   
                    LoginForm loginForm = new LoginForm();
                    loginForm.Show();

                    
                    this.Hide();

                    
                    HideAllOtherForms(loginForm);
                }
            };

            this.Controls.Add(lbl);
        }

        private void HideAllOtherForms(Form activeForm)
        {
            List<Form> formsToHide = new List<Form>();
            foreach (Form f in Application.OpenForms)
            {
                
                if (f != activeForm && f.Visible && f.Name != "CustomMenu")
                {
                    formsToHide.Add(f);
                }
            }
            foreach (Form f in formsToHide) f.Hide();
        }

        private void AddFooterLink(string text, int x, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(220, 25),
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 10f),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lbl);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

           
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (GraphicsPath path = GetRoundedRect(rect, 20))
            {
               
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(150, 10, 15, 25)))
                    g.FillPath(bg, path);

                using (Pen border = new Pen(Color.FromArgb(103, 190, 217), 2.0f))
                    g.DrawPath(border, path);
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
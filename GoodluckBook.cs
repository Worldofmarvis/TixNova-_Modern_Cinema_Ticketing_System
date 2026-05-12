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

namespace TixNova__Final
{
    public partial class GoodluckBook : Form
    {
        public GoodluckBook()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            MakeRoundedGradientButton(MenuButton, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 30);
            MakeRoundedGradientButton(SearchButton, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 35);
            SetupAllLinkLabelsGlow();
            SetupMenu();

            SetupBookingDropdowns();
        }

        private void SetupBookingDropdowns()
        {
            
            string[] locations = {
                "WATCH IN:",
                "SM Batangas City (Unavailable)", 
                "SM Lipa City",
                "SM City Sto. Tomas (Unavailable)", 
                "SM City Santa Rosa",
                "SM City Calamba",
                "SM City Bacoor"
            };

            
            string[] times = {
                "SCHEDULE:",
                "10:30 AM - 12:45 PM (Full)", 
                "1:00 PM - 3:15 PM",
                "3:45 PM - 6:00 PM",
                "6:30 PM - 8:45 PM (Full)", 
                "9:15 PM - 11:30 PM"
            };

            ConfigurePlaceholderCombo(roundedComboBox1, locations);
            ConfigurePlaceholderCombo(roundedComboBox2, times);
        }

        private void ConfigurePlaceholderCombo(ComboBox combo, string[] items)
        {
            if (combo == null) return;

            combo.DrawMode = DrawMode.OwnerDrawFixed;
            combo.Items.Clear();
            combo.Items.AddRange(items);
            combo.SelectedIndex = 0;

            combo.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                ComboBox cb = s as ComboBox;
                string text = cb.Items[e.Index].ToString();
                bool isPlaceholder = (e.Index == 0);
                bool isDisabled = text.Contains("(Full)") || text.Contains("(Unavailable)");

                e.DrawBackground();

                if (!cb.DroppedDown && isPlaceholder)
                {
                    using (Font boldFont = new Font(e.Font, FontStyle.Bold))
                    {
                        e.Graphics.DrawString(text, boldFont, Brushes.Cyan, e.Bounds.X + 5, e.Bounds.Y);
                    }
                }
                else
                {
                    Brush textBrush;
                    if (isPlaceholder) textBrush = Brushes.DimGray;
                    else if (isDisabled) textBrush = Brushes.Gray;
                    else textBrush = Brushes.Cyan;

                    e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds.X + 5, e.Bounds.Y);
                }
                e.DrawFocusRectangle();
            };

            combo.SelectedIndexChanged += (s, e) =>
            {
                ComboBox cb = s as ComboBox;
                if (cb.SelectedIndex <= 0) return;

                string selectedText = cb.SelectedItem.ToString();

                if (selectedText.Contains("(Full)") || selectedText.Contains("(Unavailable)"))
                {
                    string msg = selectedText.Contains("(Full)") ? "fully booked" : "currently unavailable";
                    MessageBox.Show($"This selection is {msg}.", "TixNova", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cb.SelectedIndex = 0;
                }
            };
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

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { new MainDashBoard().Show(); this.Hide(); }
        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { new MoviesForm().Show(); this.Hide(); }
        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { new CinemasForm().Show(); this.Hide(); }
        private void LinkLabel4_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e) { new ShopForm().Show(); this.Hide(); }

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
        #endregion

        private void RoundedButton1_Click(object sender, EventArgs e)
        {
            
            string selectedMall = roundedComboBox1.SelectedItem?.ToString() ?? "";
            string selectedTime = roundedComboBox2.SelectedItem?.ToString() ?? "";

            
            if (selectedMall == "WATCH IN:" || string.IsNullOrEmpty(selectedMall) ||
                selectedTime == "SCHEDULE:" || string.IsNullOrEmpty(selectedTime))
            {
                MessageBox.Show("Please select cinema and schedule.", "Selection Required",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            if (selectedMall.Contains("(Unavailable)") || selectedTime.Contains("(Full)"))
            {
                MessageBox.Show("This cinema or schedule is not available.", "Not Available",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            BookingData bookingData = new BookingData
            {
                MovieName = "Good luck, Have fun, Don't die",
                Cinema = selectedMall,
                Schedule = selectedTime,
                TicketPrice = 260,
                TicketQuantity = 1
            };

            
            ShopBuy shopbuy = new ShopBuy(this, bookingData);  
            shopbuy.Show();
            this.Hide();
        }
    }
}
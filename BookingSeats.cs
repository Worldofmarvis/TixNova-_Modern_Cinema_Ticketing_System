using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace TixNova__Final
{
    public partial class BookingSeats : Form
    {
       
        private FlowLayoutPanel flowLayoutPanelSeats;
        private readonly Color neonBlue = Color.FromArgb(0, 180, 216);
        private readonly Color glassBorder = Color.FromArgb(120, 0, 180, 216);
        private readonly Color seatAvailable = Color.FromArgb(160, 160, 160);
        private readonly Color seatSold = Color.FromArgb(70, 70, 70);

        private readonly BookingData _bookingData;

        public BookingSeats(BookingData bookingData = null)
        {
            InitializeComponent();
            _bookingData = bookingData;
            
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

          
            this.Shown += (s, e) => {
                SetupSeatPickerUI();
                SetupBottomButtons();
            };

            
            MakeRoundedGradientButton(MenuButton, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 30);
            MakeRoundedGradientButton(SearchButton, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 35);
            SetupAllLinkLabelsGlow();
            SetupMenu();
        }

        private void SetupSeatPickerUI()
        {
            int containerWidth = 1050;
            int containerHeight = 720;

            Panel mainContainer = new Panel
            {
                Name = "MainSeatContainer",
                Size = new Size(containerWidth, containerHeight),
                Location = new Point((this.ClientSize.Width - containerWidth) / 2, (this.ClientSize.Height - containerHeight) / 2),
                BackColor = Color.FromArgb(30, 20, 20, 30),
            };
            this.Controls.Add(mainContainer);

            mainContainer.HandleCreated += (s, e) => {
                mainContainer.Region = new Region(GetRoundedRectanglePath(mainContainer.ClientRectangle, 30));
            };

            
            mainContainer.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = GetRoundedRectanglePath(mainContainer.ClientRectangle, 30))
                using (Pen pen = new Pen(glassBorder, 2))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            };

            AddLegend(mainContainer);

            Label lblScreen = new Label
            {
                Text = "S C R E E N",
                Size = new Size(mainContainer.Width - 120, 40),
                Location = new Point(60, 80),
                BackColor = Color.FromArgb(150, 0, 180, 216),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            mainContainer.Controls.Add(lblScreen);

            
            flowLayoutPanelSeats = new FlowLayoutPanel
            {
                Size = new Size(980, 550),
                Location = new Point(35, 140),
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding((980 - (13 * 72)) / 2, 0, 0, 0)
            };
            mainContainer.Controls.Add(flowLayoutPanelSeats);

            GenerateSeats();
        }

        private void GenerateSeats()
        {
            flowLayoutPanelSeats.Visible = false;
            flowLayoutPanelSeats.SuspendLayout();
            flowLayoutPanelSeats.Controls.Clear();

            char[] rows = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
           
            var occupied = new List<string> { "A1", "A2", "C5", "D6", "G10", "G11" };

            foreach (char row in rows)
            {
                AddRowLabel(row.ToString());
                for (int i = 11; i >= 1; i--)
                {
                    SeatButton btn = new SeatButton
                    {
                        Text = i.ToString(),
                        SeatRow = row,        
                        SeatNumber = i        
                    };
                    string id = $"{row}{i}";
                    if (occupied.Contains(id)) btn.SetStatus("Sold");
                    else btn.SetStatus("Available");
                    flowLayoutPanelSeats.Controls.Add(btn);
                }
                AddRowLabel(row.ToString());
                flowLayoutPanelSeats.SetFlowBreak(flowLayoutPanelSeats.Controls[flowLayoutPanelSeats.Controls.Count - 1], true);
            }
            flowLayoutPanelSeats.ResumeLayout();
            flowLayoutPanelSeats.Visible = true;
        }

        private void SetupBottomButtons()
        {
            int btnWidth = 180;
            int btnHeight = 50;
            int containerWidth = 1050;
            int containerX = (this.ClientSize.Width - containerWidth) / 2;

            int bottomY = this.ClientSize.Height - 140;

            Button btnBack = new Button { Text = "BACK", Size = new Size(btnWidth, btnHeight), Location = new Point(containerX, bottomY), ForeColor = Color.White, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            Button btnNext = new Button { Text = "NEXT", Size = new Size(btnWidth, btnHeight), Location = new Point(containerX + containerWidth - btnWidth, bottomY), ForeColor = Color.White, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

           
            btnBack.Click += (s, e) =>
            {
                
                ShopBuy prev = new ShopBuy();
                prev.Show();
                this.Close(); 
            };

            
            btnNext.Click += (s, e) =>
            {
                var selected = GetSelectedSeatNames();

                if (selected.Count == 0)
                {
                    MessageBox.Show("Please select at least one seat!");
                }
                else
                {
                    
                    if (_bookingData != null)
                    {
                        _bookingData.SelectedSeats = selected;
                        _bookingData.TicketQuantity = selected.Count;
                    }

                    
                    TicketDetails details = new TicketDetails(_bookingData, selected);
                    details.Show();
                    this.Hide();
                }
            };

           

            MakeRoundedGradientButton(btnBack, Color.FromArgb(60, 60, 65), Color.FromArgb(30, 30, 35), 25);
            MakeRoundedGradientButton(btnNext, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 25);

            this.Controls.Add(btnBack);
            this.Controls.Add(btnNext);
        }

       
        private List<string> GetSelectedSeatNames()
        {
            List<string> selected = new List<string>();
            foreach (Control control in flowLayoutPanelSeats.Controls)
            {
                if (control is SeatButton seat && seat.Status == "Selected")
                {
                    selected.Add(seat.SeatId); 
                }
            }
            return selected;
        }

        private void AddRowLabel(string text)
        {
            Label lbl = new Label { Text = text, ForeColor = neonBlue, Font = new Font("Segoe UI", 12, FontStyle.Bold), Size = new Size(50, 72), TextAlign = ContentAlignment.MiddleCenter };
            flowLayoutPanelSeats.Controls.Add(lbl);
        }

        private void AddLegend(Panel container)
        {
            int startX = 325;
            string[] labels = { "Your seat", "Available", "Sold" };
            Color[] colors = { neonBlue, seatAvailable, seatSold };

            for (int i = 0; i < 3; i++)
            {
                Panel p = new Panel { Size = new Size(25, 25), Location = new Point(startX + (i * 150), 30), BackColor = colors[i] };
                p.Paint += (s, e) => { p.Region = new Region(GetRoundedRectanglePath(p.ClientRectangle, 6)); };
                Label l = new Label { Text = labels[i], ForeColor = Color.White, Location = new Point(startX + 30 + (i * 150), 33), AutoSize = true, Font = new Font("Segoe UI", 10) };
                container.Controls.Add(p);
                container.Controls.Add(l);
            }
        }

       
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { new MainDashBoard().Show(); this.Hide(); }
        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { new MoviesForm().Show(); this.Hide(); }
        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { new CinemasForm().Show(); this.Hide(); }
        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { new ShopForm().Show(); this.Hide(); }

        private void LinkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if ((DateTime.Now - menuLastClosedTime).TotalMilliseconds < 100) return;
            if (dropDownForm.Visible) dropDownForm.Hide();
            else
            {
                Point screenLocation = linkLabel5.PointToScreen(new Point((linkLabel5.Width - menuContent.Width) / 2, linkLabel5.Height + 5));
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
                _sideMenu.Location = this.PointToScreen(new Point(this.Width - _sideMenu.Width - 20, 50));
                _sideMenu.Show();
            }
            else { _sideMenu.Visible = !_sideMenu.Visible; }
        }

        
        public GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public class SeatButton : Button
        {
            public string Status { get; private set; }
            public char SeatRow { get; set; }
            public int SeatNumber { get; set; }
            public string SeatId => $"{SeatRow}{SeatNumber}";

            public SeatButton()
            {
                this.Size = new Size(60, 60);
                this.Margin = new Padding(6);
                this.FlatStyle = FlatStyle.Flat;
                this.FlatAppearance.BorderSize = 0;
                this.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                this.DoubleBuffered = true;

                this.Click += (s, e) => {
                    if (Status == "Available") SetStatus("Selected");
                    else if (Status == "Selected") SetStatus("Available");
                };

                
                this.HandleCreated += (s, e) => {
                    var parent = (BookingSeats)this.FindForm();
                    if (parent != null)
                    {
                        using (var path = parent.GetRoundedRectanglePath(this.ClientRectangle, 12))
                        {
                            this.Region = new Region(path);
                        }
                    }
                };
            }

            public void SetStatus(string status)
            {
                Status = status;
                if (status == "Selected") { BackColor = Color.FromArgb(0, 180, 216); ForeColor = Color.Black; }
                else if (status == "Sold") { BackColor = Color.FromArgb(70, 70, 70); Enabled = false; }
                else { BackColor = Color.FromArgb(160, 160, 160); ForeColor = Color.Black; }
                this.Invalidate(); 
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                
                pevent.Graphics.Clear(this.BackColor);

               
                pevent.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                
                TextRenderer.DrawText(pevent.Graphics, Text, Font, ClientRectangle, ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

       
        [DllImport("user32.dll")] internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        [StructLayout(LayoutKind.Sequential)] internal struct WindowCompositionAttributeData { public WindowCompositionAttribute Attribute; public IntPtr Data; public int SizeOfData; }
        internal enum WindowCompositionAttribute { WCA_ACCENT_POLICY = 19 }
        internal enum AccentState { ACCENT_ENABLE_BLURBEHIND = 3 }
        [StructLayout(LayoutKind.Sequential)] internal struct AccentPolicy { public AccentState AccentState; public int AccentFlags; public int GradientColor; public int AnimationId; }

        private void EnableBlur(IntPtr hwnd)
        {
            var accent = new AccentPolicy { AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND };
            int accentStructSize = Marshal.SizeOf(accent);
            IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);
            var data = new WindowCompositionAttributeData { Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY, SizeOfData = accentStructSize, Data = accentPtr };
            SetWindowCompositionAttribute(hwnd, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }

        private Form dropDownForm;
        private DateTime menuLastClosedTime = DateTime.MinValue;
        private TixNovaMenuControl menuContent;

        private void SetupMenu()
        {
            menuContent = new TixNovaMenuControl { Location = new Point(0, 0) };
            dropDownForm = new Form { FormBorderStyle = FormBorderStyle.None, StartPosition = FormStartPosition.Manual, ShowInTaskbar = false, Size = menuContent.Size, BackColor = Color.Black, Region = new Region(menuContent.GetRegionPath()) };
            dropDownForm.Controls.Add(menuContent);
            dropDownForm.HandleCreated += (s, e) => EnableBlur(dropDownForm.Handle);
            dropDownForm.Deactivate += (s, e) => { dropDownForm.Hide(); menuLastClosedTime = DateTime.Now; };
        }

        private void SetupAllLinkLabelsGlow()
        {
            foreach (Control control in this.Controls)
            {
                if (control is LinkLabel linkLabel)
                {
                    var originalColor = linkLabel.LinkColor;
                    Timer pulseTimer = null;
                    linkLabel.MouseEnter += (sender, e) => {
                        pulseTimer = new Timer { Interval = 50 };
                        int pulseValue = 0; bool increasing = true;
                        pulseTimer.Tick += (ts, te) => {
                            if (increasing) { pulseValue += 15; if (pulseValue >= 255) increasing = false; }
                            else { pulseValue -= 15; if (pulseValue <= 100) increasing = true; }
                            linkLabel.LinkColor = Color.FromArgb(255, 0, pulseValue, 255);
                        };
                        pulseTimer.Start();
                    };
                    linkLabel.MouseLeave += (sender, e) => { linkLabel.LinkColor = originalColor; pulseTimer?.Stop(); };
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

            btn.MouseEnter += (sender, e) => { btn.Size = new Size(btn.Width + 5, btn.Height + 5); btn.Location = new Point(btn.Location.X - 2, btn.Location.Y - 2); btn.Cursor = Cursors.Hand; };
            btn.MouseLeave += (sender, e) => { btn.Size = originalSize; btn.Location = originalLocation; btn.Cursor = Cursors.Default; };
            btn.Paint += (sender, e) =>
            {
                Button b = sender as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = new GraphicsPath())
                {
                    Rectangle rect = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();
                    b.Region = new Region(path);
                    GradientInfo gradient = (GradientInfo)b.Tag;
                    using (var brush = new LinearGradientBrush(rect, gradient.StartColor, gradient.EndColor, LinearGradientMode.Vertical)) e.Graphics.FillPath(brush, path);
                    TextRenderer.DrawText(e.Graphics, b.Text, b.Font, rect, b.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };
            btn.Resize += (sender, e) => btn.Invalidate();
        }

        private class GradientInfo { public Color StartColor { get; set; } public Color EndColor { get; set; } }
    }
}
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
    public partial class ShopBuy : Form
    {
        private readonly Form _parentForm;
        private CustomSearchMenu _searchMenu;
        private CustomMenu _sideMenu;
        private Form dropDownForm;
        private DateTime menuLastClosedTime = DateTime.MinValue;
        private TixNovaMenuControl menuContent;

        private readonly BookingData _bookingData;

        private int popcornQty = 0;
        private int nachosQty = 0;
        private int friesQty = 0;
        private int dealsQty = 0;
        private int sodaQty = 0;
        private int pringlesQty = 0;

        private const int PRICE_POPCORN = 200;
        private const int PRICE_NACHOS = 300;
        private const int PRICE_FRIES = 200;
        private const int PRICE_DEALS = 400;
        private const int PRICE_SODA = 100;
        private const int PRICE_PRINGLES = 100;
  
        public ShopBuy(Form parent = null, BookingData bookingData = null)
        {
            InitializeComponent();
            this._parentForm = parent;
            this._bookingData = bookingData;

            MakeRoundedGradientButton(MenuButton, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 30);
            MakeRoundedGradientButton(SearchButton, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 35);

            MakeRoundedGradientButton(roundedButton13, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 20);
            MakeRoundedGradientButton(roundedButton14, Color.FromArgb(78, 199, 220), Color.FromArgb(7, 89, 179), 20);

            SetupAllLinkLabelsGlow();
            SetupMenu();
            InitializeCounters();
        }
        private List<SnackItem> GetSelectedSnacks()
        {
            List<SnackItem> snacks = new List<SnackItem>();

            if (popcornQty > 0)
                snacks.Add(new SnackItem { Name = "Popcorn", Quantity = popcornQty, Price = PRICE_POPCORN });
            if (nachosQty > 0)
                snacks.Add(new SnackItem { Name = "Nachos", Quantity = nachosQty, Price = PRICE_NACHOS });
            if (friesQty > 0)
                snacks.Add(new SnackItem { Name = "French Fries", Quantity = friesQty, Price = PRICE_FRIES });
            if (dealsQty > 0)
                snacks.Add(new SnackItem { Name = "Snack Deals", Quantity = dealsQty, Price = PRICE_DEALS });
            if (sodaQty > 0)
                snacks.Add(new SnackItem { Name = "Soda", Quantity = sodaQty, Price = PRICE_SODA });
            if (pringlesQty > 0)
                snacks.Add(new SnackItem { Name = "Pringles", Quantity = pringlesQty, Price = PRICE_PRINGLES });

            return snacks;
        }

        private decimal CalculateTotalSnacksPrice()
        {
            decimal total = 0;
            total += popcornQty * PRICE_POPCORN;
            total += nachosQty * PRICE_NACHOS;
            total += friesQty * PRICE_FRIES;
            total += dealsQty * PRICE_DEALS;
            total += sodaQty * PRICE_SODA;
            total += pringlesQty * PRICE_PRINGLES;
            return total;
        }

        private void InitializeCounters()
        {
            lblPopcornCount.Text = "0";
            lblNachosCount.Text = "0";
            lblFriesCount.Text = "0";
            lblDealsCount.Text = "0";
            lblSodaCount.Text = "0";
            lblPringlesCount.Text = "0";
        }

        #region Win32 Blur Effects
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
        #endregion

        #region UI Setup Methods
        private void SetupMenu()
        {
            menuContent = new TixNovaMenuControl();
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
            menuContent.Location = new Point(0, 0);
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

        private class GradientInfo { public Color StartColor { get; set; } public Color EndColor { get; set; } }
        #endregion

        #region Snack Counter Logic
        private void UpdateQuantity(Label lbl, ref int qty, bool isPlus)
        {
            if (isPlus) qty++;
            else if (qty > 0) qty--;

            lbl.Text = qty.ToString();
        }

        // PopCorn
        private void BtnPopcornPlus_Click(object sender, EventArgs e) => UpdateQuantity(lblPopcornCount, ref popcornQty, true);
        private void BtnPopcornMinus_Click(object sender, EventArgs e) => UpdateQuantity(lblPopcornCount, ref popcornQty, false);

        // Nachos
        private void BtnNachosPlus_Click(object sender, EventArgs e) => UpdateQuantity(lblNachosCount, ref nachosQty, true);
        private void BtnNachosMinus_Click(object sender, EventArgs e) => UpdateQuantity(lblNachosCount, ref nachosQty, false);

        // Fries
        private void BtnFriesPlus_Click(object sender, EventArgs e) => UpdateQuantity(lblFriesCount, ref friesQty, true);
        private void BtnFriesMinus_Click(object sender, EventArgs e) => UpdateQuantity(lblFriesCount, ref friesQty, false);

        // Deals
        private void BtnDealsPlus_Click(object sender, EventArgs e) => UpdateQuantity(lblDealsCount, ref dealsQty, true);
        private void BtnDealsMinus_Click(object sender, EventArgs e) => UpdateQuantity(lblDealsCount, ref dealsQty, false);

        // Soda
        private void BtnSodaPlus_Click(object sender, EventArgs e) => UpdateQuantity(lblSodaCount, ref sodaQty, true);
        private void BtnSodaMinus_Click(object sender, EventArgs e) => UpdateQuantity(lblSodaCount, ref sodaQty, false);

        // Pringles
        private void BtnPringlesPlus_Click(object sender, EventArgs e) => UpdateQuantity(lblPringlesCount, ref pringlesQty, true);
        private void BtnPringlesMinus_Click (object sender, EventArgs e) => UpdateQuantity(lblPringlesCount, ref pringlesQty, false);
        #endregion

        #region Navigation and Events
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
                int xOffset = (linkLabel5.Width - menuContent.Width) / 2;
                Point screenLocation = linkLabel5.PointToScreen(new Point(xOffset, linkLabel5.Height + 5));
                dropDownForm.Location = screenLocation;
                dropDownForm.Show();
                dropDownForm.BringToFront();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (_searchMenu == null || _searchMenu.IsDisposed)
            {
                _searchMenu = new CustomSearchMenu(this);
                Point screenPos = SearchButton.PointToScreen(new Point(0, SearchButton.Height));
                _searchMenu.Location = new Point(screenPos.X - (_searchMenu.Width / 2) + (SearchButton.Width / 2), screenPos.Y + 10);
                _searchMenu.Show();
            }
            else { _searchMenu.Visible = !_searchMenu.Visible; }
        }

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
        private void RoundedButton13_Click(object sender, EventArgs e)
        {
            if (_parentForm != null)
            {
                _parentForm.Show();
                this.Close();
            }
            else
            {
                MoviesForm moviesTab = new MoviesForm();

                moviesTab.Show();
                this.Hide();
            }
        }

        private void RoundedButton14_Click(object sender, EventArgs e)
        {
            if (_bookingData == null)
            {
                MessageBox.Show("Missing booking information.", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _bookingData.Snacks = GetSelectedSnacks();
            _bookingData.TotalSnacksPrice = CalculateTotalSnacksPrice();

            BookingSeats bookingSeats = new BookingSeats(_bookingData);
            bookingSeats.Show();
            this.Hide();
        }
        #endregion

    }
}
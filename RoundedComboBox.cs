using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TixNovaPlus_Final
{
    public class RoundedComboBox : ComboBox
    {
        private int _borderRadius = 12;      
        private Color _borderColor = Color.Cyan;
        private Color _focusedBorderColor = Color.FromArgb(0, 255, 255);
        private Color _backColor = Color.FromArgb(30, 30, 30);
        private Color _textColor = Color.White;
        private Color _arrowColor = Color.Cyan;
        private Color _hoverBackColor = Color.FromArgb(58, 58, 58);
        private Color _hoverBorderColor = Color.FromArgb(0, 200, 200);

        private bool _isHovered = false;
        private bool _isFocused = false;

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        public Color FocusedBorderColor
        {
            get => _focusedBorderColor;
            set { _focusedBorderColor = value; Invalidate(); }
        }

        public new Color BackColor
        {
            get => _backColor;
            set { _backColor = value; Invalidate(); }
        }

        public Color TextColor
        {
            get => _textColor;
            set { _textColor = value; Invalidate(); }
        }

        public Color ArrowColor
        {
            get => _arrowColor;
            set { _arrowColor = value; Invalidate(); }
        }

        public Color HoverBackColor
        {
            get => _hoverBackColor;
            set { _hoverBackColor = value; Invalidate(); }
        }

        public Color HoverBorderColor
        {
            get => _hoverBorderColor;
            set { _hoverBorderColor = value; Invalidate(); }
        }

        public RoundedComboBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            base.BackColor = _backColor;
            ForeColor = _textColor;
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;

            Font = new Font("Segoe UI", 12F, FontStyle.Regular);  
            ItemHeight = 40;                                    
            Size = new Size(450, 50);                              

            DrawMode = DrawMode.OwnerDrawVariable;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath path = GetRoundedRectangle(rect, _borderRadius);

            using (SolidBrush bgBrush = new SolidBrush(_isHovered ? _hoverBackColor : _backColor))
            {
                g.FillPath(bgBrush, path);
            }

            Color currentBorder = _borderColor;
            if (_isFocused)
                currentBorder = _focusedBorderColor;
            else if (_isHovered)
                currentBorder = _hoverBorderColor;

            using (Pen borderPen = new Pen(currentBorder, 2f))
            {
                g.DrawPath(borderPen, path);
            }

            string displayText = (SelectedItem != null) ? SelectedItem.ToString() : Text;
            Rectangle textRect = new Rectangle(10, 0, Width - 35, Height);
            using (SolidBrush textBrush = new SolidBrush(_textColor))
            {
                StringFormat sf = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near
                };
                g.DrawString(displayText, Font, textBrush, textRect, sf);
            }

            int arrowWidth = 14;
            int arrowHeight = 8;
            int arrowX = Width - (arrowWidth + 10);
            int arrowY = (Height / 2) - (arrowHeight / 2);
            DrawArrow(g, new Rectangle(arrowX, arrowY, arrowWidth, arrowHeight), _arrowColor);
        }

        private void DrawArrow(Graphics g, Rectangle rect, Color color)
        {
            Point[] points = new Point[]
            {
                new Point(rect.X, rect.Y),
                new Point(rect.X + rect.Width / 2, rect.Y + rect.Height),
                new Point(rect.X + rect.Width, rect.Y)
            };
            using (Pen pen = new Pen(color, 2.5f))
            {
                g.DrawLines(pen, points);
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
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

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color back = isSelected ? Color.FromArgb(0, 150, 150) : _backColor;
            Color fore = isSelected ? Color.White : _textColor;

            using (SolidBrush bgBrush = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }
            using (SolidBrush textBrush = new SolidBrush(fore))
            {
                e.Graphics.DrawString(GetItemText(Items[e.Index]), Font, textBrush, e.Bounds.X + 8, e.Bounds.Y + 8);
            }
            e.DrawFocusRectangle();
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            e.ItemHeight = ItemHeight;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _isFocused = true;
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _isFocused = false;
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            base.BackColor = _backColor;
            ForeColor = _textColor;
        }
    }
}
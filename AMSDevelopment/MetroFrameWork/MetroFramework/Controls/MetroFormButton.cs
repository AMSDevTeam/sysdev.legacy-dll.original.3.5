using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
    public class MetroFormButton : Button, IMetroControl
    {
        #region Interface

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintBackground;
        protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null)
            {
                CustomPaintBackground(this, e);
            }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaint;
        protected virtual void OnCustomPaint(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null)
            {
                CustomPaint(this, e);
            }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintForeground;
        protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null)
            {
                CustomPaintForeground(this, e);
            }
        }

        private MetroColorStyle metroStyle = MetroColorStyle.Default;
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroColorStyle.Default)]
        public MetroColorStyle Style
        {
            get
            {
                if (DesignMode || metroStyle != MetroColorStyle.Default)
                {
                    return metroStyle;
                }

                if (StyleManager != null && metroStyle == MetroColorStyle.Default)
                {
                    return StyleManager.Style;
                }
                if (StyleManager == null && metroStyle == MetroColorStyle.Default)
                {
                    return MetroDefaults.Style;
                }

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != MetroThemeStyle.Default)
                {
                    return metroTheme;
                }

                if (StyleManager != null && metroTheme == MetroThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }
                if (StyleManager == null && metroTheme == MetroThemeStyle.Default)
                {
                    return MetroDefaults.Theme;
                }

                return metroTheme;
            }
            set
            {
                metroTheme = value;
            }
        }

        private bool _customfont = true;
        [DefaultValue(true)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool CustomFont
        {
            get { return _customfont; }
            set { _customfont = value; }
        }

        private MetroButtonSize metroButtonSize = MetroButtonSize.Medium;
        [DefaultValue(MetroButtonSize.Medium)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroButtonSize FontSize
        {
            get { return metroButtonSize; }
            set { metroButtonSize = value; }
        }

        private MetroButtonWeight metroButtonWeight = MetroButtonWeight.Regular;
        [DefaultValue(MetroButtonWeight.Regular)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroButtonWeight FontWeight
        {
            get { return metroButtonWeight; }
            set { metroButtonWeight = value; }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        private bool useCustomBackColor = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomBackColor
        {
            get { return useCustomBackColor; }
            set { useCustomBackColor = value; }
        }

        private bool useCustomForeColor = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomForeColor
        {
            get { return useCustomForeColor; }
            set { useCustomForeColor = value; }
        }

        private bool useStyleColors = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; }
        }

        [Browsable(false)]
        [Category(MetroDefaults.PropertyCategory.Behaviour)]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get { return GetStyle(ControlStyles.Selectable); }
            set { SetStyle(ControlStyles.Selectable, value); }
        }

        #endregion

        #region Fields

        private bool isHovered = false;
        private bool isPressed = false;

        #endregion

        #region Constructor

        //public MetroFormButton()
        //{
        //    SetStyle(ControlStyles.AllPaintingInWmPaint |
        //             ControlStyles.OptimizedDoubleBuffer |
        //             ControlStyles.ResizeRedraw |
        //             ControlStyles.UserPaint, true);
        //}

        #endregion

        #region Paint Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            Color backColor, foreColor;

            MetroThemeStyle _Theme = Theme;
            MetroColorStyle _Style = Style;

            if (Parent != null)
            {
                if (Parent is IMetroForm)
                {
                    _Theme = ((IMetroForm)Parent).Theme;
                    _Style = ((IMetroForm)Parent).Style;
                    backColor = MetroPaint.BackColor.Form(_Theme);
                }
                else if (Parent is IMetroControl)
                {
                    _Theme = ((IMetroControl)Parent).Theme;
                    _Style = ((IMetroControl)Parent).Style;
                    backColor = MetroPaint.BackColor.Form(_Theme);
                }
                else
                {
                    backColor = Parent.BackColor;
                }
            }
            else
            {
                backColor = MetroPaint.BackColor.Form(_Theme);
            }

            if (isHovered && !isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.Button.Normal(_Theme);
                backColor = MetroPaint.BackColor.Button.Normal(_Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.Button.Press(_Theme);
                backColor = MetroPaint.GetStyleColor(_Style);
            }
            else if (!Enabled)
            {
                foreColor = MetroPaint.ForeColor.Button.Disabled(_Theme);
                backColor = MetroPaint.BackColor.Button.Disabled(_Theme);
            }
            else
            {
                foreColor = MetroPaint.ForeColor.Button.Normal(_Theme);
            }

            e.Graphics.Clear(backColor);
            //Font buttonFont = new Font("Webdings", 9.25f); 
            TextFormatFlags _firstflag = TextFormatFlags.HorizontalCenter;
            TextFormatFlags _secondflag = TextFormatFlags.VerticalCenter;

            switch (TextAlign)
            {
                case ContentAlignment.BottomCenter:
                    _secondflag = TextFormatFlags.Bottom;
                    break;
                case ContentAlignment.BottomLeft:
                    _firstflag = TextFormatFlags.Left;
                    _secondflag = TextFormatFlags.Bottom;
                    break;
                case ContentAlignment.BottomRight:
                    _firstflag = TextFormatFlags.Right;
                    _secondflag = TextFormatFlags.Bottom;
                    break;
                case ContentAlignment.MiddleLeft:
                    _firstflag = TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    _firstflag = TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    _secondflag = TextFormatFlags.Top;
                    break;
                case ContentAlignment.TopLeft:
                    _firstflag = TextFormatFlags.Left;
                    _secondflag = TextFormatFlags.Top;
                    break;
                case ContentAlignment.TopRight:
                     _firstflag = TextFormatFlags.Right ;
                    _secondflag = TextFormatFlags.Top;
                    break;
            }

            TextRenderer.DrawText(e.Graphics, Text, _customfont ? MetroFonts.Button(metroButtonSize, metroButtonWeight) : Font, ClientRectangle, foreColor, backColor, _firstflag | _secondflag | TextFormatFlags.EndEllipsis);

            DrawIcon(e.Graphics);
        }

        private Bitmap _image = null;

        public new Image Image
        {
            get { return base.Image; }
            set
            {
                base.Image = value;
                if (value == null) return;
                _image = ApplyInvert(new Bitmap(value));
            }
        }

        public Bitmap ApplyInvert(Bitmap bitmapImage)
        {
            byte A, R, G, B;
            Color pixelColor;

            for (int y = 0; y < bitmapImage.Height; y++)
            {
                for (int x = 0; x < bitmapImage.Width; x++)
                {
                    pixelColor = bitmapImage.GetPixel(x, y);
                    A = pixelColor.A;
                    R = (byte)(255 - pixelColor.R);
                    G = (byte)(255 - pixelColor.G);
                    B = (byte)(255 - pixelColor.B);
                    bitmapImage.SetPixel(x, y, Color.FromArgb((int)A, (int)R, (int)G, (int)B));
                }
            }

            return bitmapImage;
        }

        protected Size iconSize
        {
            get
            {
                if (Image != null)
                {
                    Size originalSize = Image.Size;
                    double resizeFactor = 14 / (double)originalSize.Height;

                    Point iconLocation = new Point(1, 1);
                    return new Size((int)(originalSize.Width * resizeFactor), (int)(originalSize.Height * resizeFactor));
                }

                return new Size(-1, -1);
            }
        }

        private void DrawIcon(Graphics g)
        {
            if (Image != null)
            {
                Point iconLocation = new Point(2, (ClientRectangle.Height - iconSize.Height) / 2);
                int _filler = 5;

                switch (ImageAlign)
                {
                    case ContentAlignment.BottomCenter:
                        iconLocation = new Point((ClientRectangle.Width - iconSize.Width) / 2, (ClientRectangle.Height - iconSize.Height) - _filler);
                        break;
                    case ContentAlignment.BottomLeft:
                        iconLocation = new Point(_filler, (ClientRectangle.Height - iconSize.Height) - _filler);
                        break;
                    case ContentAlignment.BottomRight:
                        iconLocation = new Point((ClientRectangle.Width - iconSize.Width) - _filler, (ClientRectangle.Height - iconSize.Height) - _filler);
                        break;
                    case ContentAlignment.MiddleCenter:
                        iconLocation = new Point((ClientRectangle.Width - iconSize.Width) / 2, (ClientRectangle.Height - iconSize.Height) / 2);
                        break;
                    case ContentAlignment.MiddleLeft:
                        iconLocation = new Point(_filler, (ClientRectangle.Height - iconSize.Height) / 2);
                        break;
                    case ContentAlignment.MiddleRight:
                        iconLocation = new Point((ClientRectangle.Width - iconSize.Width) - _filler, (ClientRectangle.Height - iconSize.Height) / 2);
                        break;
                    case ContentAlignment.TopCenter:
                        iconLocation = new Point((ClientRectangle.Width - iconSize.Width) / 2, _filler);
                        break;
                    case ContentAlignment.TopLeft:
                        iconLocation = new Point(_filler, _filler);
                        break;
                    case ContentAlignment.TopRight:
                        iconLocation = new Point((ClientRectangle.Width - iconSize.Width) - _filler, _filler);
                        break;
                }

                g.DrawImage((Theme == MetroThemeStyle.Dark) ? ((isPressed) ? Image : _image) : (isPressed) ? _image : Image, new Rectangle(iconLocation, iconSize));
            }
        }
        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion
    }
}

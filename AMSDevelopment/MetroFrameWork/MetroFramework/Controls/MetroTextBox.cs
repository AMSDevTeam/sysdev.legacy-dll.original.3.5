/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
    [Designer("MetroFramework.Design.Controls.MetroTextBoxDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    public class MetroTextBox : Control, IMetroControl
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
            set { metroTheme = value; }
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

        private PromptedTextBox baseTextBox;

        private MetroTextBoxSize metroTextBoxSize = MetroTextBoxSize.Small;
        [DefaultValue(MetroTextBoxSize.Small)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTextBoxSize FontSize
        {
            get { return metroTextBoxSize; }
            set { metroTextBoxSize = value;
                UpdateBaseTextBox(); }
        }

        private MetroTextBoxWeight metroTextBoxWeight = MetroTextBoxWeight.Regular;
        [DefaultValue(MetroTextBoxWeight.Regular)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTextBoxWeight FontWeight
        {
            get { return metroTextBoxWeight; }
            set { metroTextBoxWeight = value; UpdateBaseTextBox(); }
        }

        private MetroTextBoxControlType metroTextBoxControlType = MetroTextBoxControlType.Text;
        [DefaultValue(MetroTextBoxControlType.Text)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTextBoxControlType ControlType
        {
            get { return metroTextBoxControlType; }
            set { metroTextBoxControlType = value; }
        }

        private MetroTextBoxFormat metroTextBoxFormat = MetroTextBoxFormat.N0;
        [DefaultValue(MetroTextBoxFormat.N0)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTextBoxFormat FormatType
        {
            get { return metroTextBoxFormat; }
            set { metroTextBoxFormat = value; UpdateBaseTextDefaultValue(); }
        }


        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("")]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public string WaterMark
        {
            get { return baseTextBox.WaterMark; }
            set { baseTextBox.WaterMark = value; }
        }

        private Image textBoxIcon = null;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(null)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public Image Icon
        {
            get { return textBoxIcon; }
            set
            {
                textBoxIcon = value;
                Refresh();
            }
        }

        private bool textBoxIconRight = false;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool IconRight
        {
            get { return textBoxIconRight; }
            set
            {
                textBoxIconRight = value;
                Refresh();
            }
        }

        private bool displayIcon = false;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayIcon
        {
            get { return displayIcon; }
            set
            {
                displayIcon = value;
                Refresh();
            }
        }

        protected Size iconSize
        {
            get
            {
                if (displayIcon && textBoxIcon != null)
                {
                    Size originalSize = textBoxIcon.Size;
                    double resizeFactor = 16 / (double)originalSize.Height;

                    Point iconLocation = new Point(1, 1);
                    return new Size((int)(originalSize.Width * resizeFactor), (int)(originalSize.Height * resizeFactor));
                }

                return new Size(-1, -1);
            }
        }

        private MetroTextButton _button;
        private bool _showbutton = false;


        protected int ButtonWidth
        {
            get
            {
                int _butwidth = 0;
                if (_button != null)
                {
                    _butwidth = (_showbutton) ? _button.Width : 0;
                }

                return _butwidth;
            }
        }

        [DefaultValue(false)]
        public bool ShowButton
        {
            get { return _showbutton; }
            set
            {
                _showbutton = value;
                Refresh();
            }
        }

        private MetroLink lnkClear;
        private bool _showclear = false;
       

        [DefaultValue(false)]
        public bool ShowClearButton
        {
            get { return _showclear; }
            set
            {
                _showclear = value;
                Refresh();
            }
        }

        private MetroLabel lblSidelabel;
        private bool _showsidelabel = false;

        [DefaultValue(false)]
        public bool ShowSideLabel
        {
            get { return _showsidelabel; }
            set
            {
                _showsidelabel = value;
                if (_showsidelabel == true)
                {
                    this.ControlType = MetroTextBoxControlType.Double;
                    this.FormatType = MetroTextBoxFormat.N2;
                    baseTextBox.TextAlign = HorizontalAlignment.Right;
                }
                else
                {
                    this.ControlType = MetroTextBoxControlType.Text;
                    this.FormatType = MetroTextBoxFormat.N0;
                    baseTextBox.Text = "";
                    baseTextBox.TextAlign = HorizontalAlignment.Left;
                }
                Refresh();
            }
        }

        private string _SideLabelText = "USD";
        [DefaultValue("USD")]
        public string  SideLabelText
        {
            get { return _SideLabelText; }
            set
            {
                _SideLabelText = value;
                lblSidelabel.Text = _SideLabelText;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MetroTextButton CustomButton
        {
            get { return _button; }
            set
            {
                _button = value;
                Refresh();
            }
        }

        private bool _witherror = false;

        [DefaultValue(false)]
        public bool WithError
        {
            get { return _witherror; }
            set
            {
                _witherror = value;
                Invalidate();
            }
        }

        #endregion

        #region Routing Fields

        public override ContextMenu ContextMenu
        {
            get { return baseTextBox.ContextMenu; }
            set
            {
                ContextMenu = value;
                baseTextBox.ContextMenu = value;
            }
        }

        public override ContextMenuStrip ContextMenuStrip
        {
            get { return baseTextBox.ContextMenuStrip; }
            set
            {
                ContextMenuStrip = value;
                baseTextBox.ContextMenuStrip = value;
            }
        }

        [DefaultValue(false)]
        public bool Multiline
        {
            get { return baseTextBox.Multiline; }
            set { baseTextBox.Multiline = value; }
        }

        public override string Text
        {
            get { return baseTextBox.Text; }
            set { baseTextBox.Text = value; }
        }

        public Color WaterMarkColor
        {
            get { return baseTextBox.WaterMarkColor; }
            set { baseTextBox.WaterMarkColor = value; }
        }

        public Font WaterMarkFont
        {
            get { return baseTextBox.WaterMarkFont; }
            set { baseTextBox.WaterMarkFont = value; }
        }

        public string[] Lines
        {
            get { return baseTextBox.Lines; }
            set { baseTextBox.Lines = value; }
        }

        [Browsable(false)]
        public string SelectedText
        {
            get { return baseTextBox.SelectedText; }
            set { baseTextBox.Text = value; }
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return baseTextBox.ReadOnly; }
            set 
            { 
                baseTextBox.ReadOnly = value;
            }
        }

        public char PasswordChar
        {
            get { return baseTextBox.PasswordChar; }
            set { baseTextBox.PasswordChar = value; }
        }

        [DefaultValue(false)]
        public bool UseSystemPasswordChar
        {
            get { return baseTextBox.UseSystemPasswordChar; }
            set { baseTextBox.UseSystemPasswordChar = value; }
        }

        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get { return baseTextBox.TextAlign; }
            set { baseTextBox.TextAlign = value; }
        }
          
        public int SelectionStart
        {
            get { return baseTextBox.SelectionStart; }
            set { baseTextBox.SelectionStart = value; }
        }

        public int SelectionLength
        {
            get { return baseTextBox.SelectionLength; }
            set { baseTextBox.SelectionLength = value; }
        }

        [DefaultValue(true)]
        public new bool TabStop
        {
            get { return baseTextBox.TabStop; }
            set { baseTextBox.TabStop = value; }
        }

        public int MaxLength
        {
            get { return baseTextBox.MaxLength; }
            set { baseTextBox.MaxLength = value; }
        }

        public ScrollBars ScrollBars
        {
            get { return baseTextBox.ScrollBars; }
            set { baseTextBox.ScrollBars = value; }
        }
        #endregion

        #region Constructor

        public MetroTextBox()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            this.GotFocus += MetroTextBox_GotFocus;
            base.TabStop = false;

            CreateBaseTextBox();
            UpdateBaseTextBox();
            UpdateBaseTextDefaultValue();
            AddEventHandler();
        }

        #endregion

        #region Routing Methods

        public event EventHandler AcceptsTabChanged;
        private void BaseTextBoxAcceptsTabChanged(object sender, EventArgs e)
        {
            if (AcceptsTabChanged != null)
                AcceptsTabChanged(this, e);
        }

        private void BaseTextBoxSizeChanged(object sender, EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        private void BaseTextBoxCursorChanged(object sender, EventArgs e)
        {
            base.OnCursorChanged(e);
        }

        private void BaseTextBoxContextMenuStripChanged(object sender, EventArgs e)
        {
            base.OnContextMenuStripChanged(e);
        }

        private void BaseTextBoxContextMenuChanged(object sender, EventArgs e)
        {
            base.OnContextMenuChanged(e);
        }

        private void BaseTextBoxClientSizeChanged(object sender, EventArgs e)
        {
            base.OnClientSizeChanged(e);
        }

        private void BaseTextBoxClick(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        private void BaseTextBoxChangeUiCues(object sender, UICuesEventArgs e)
        {
            base.OnChangeUICues(e);
        }

        private void BaseTextBoxCausesValidationChanged(object sender, EventArgs e)
        {
            base.OnCausesValidationChanged(e);
        }

        private void BaseTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        private void BaseTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
           
            if (this.ControlType == MetroTextBoxControlType.Integer)
            {
                if (e.KeyChar == 8) { base.OnKeyPress(e); }
                else
                {
                    string _Value = this.Text.ToString() + e.KeyChar.ToString();
                    int iValue = 0;
                    bool isInteger = int.TryParse(_Value, out iValue);
                    if (isInteger == false || e.KeyChar.ToString() == " ") e.Handled = true;
                    else base.OnKeyPress(e);
                }
            }
            else if (this.ControlType == MetroTextBoxControlType.Double)
            {
                int _cursor = this.SelectionStart;
                if (e.KeyChar == 8) { base.OnKeyPress(e); }
                else
                {
                    char[] chars = this.Text.ToCharArray();
                    string _Value = "";
                    for (int i = 0; i < chars.Length ; i++)
                    {
                        if (i == _cursor && _cursor ==0) _Value += e.KeyChar.ToString() + chars[i].ToString() ;
                        else if (i+1 == _cursor) _Value += e.KeyChar.ToString() + chars[i].ToString();
                        else { _Value += chars[i].ToString();}
                    }
                    
                    chars = _Value.ToCharArray();

                    double dValue = 0;
                    bool isDouble = false;                                     
                    int _ctr = 0;
                    bool _start = false;
                    bool _takeeffect = false;
                   
                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (_start == true) _ctr += 1;
                        if (chars[i].ToString() == ".") _start = true;

                    }

                    dValue = 0;
                    isDouble = double.TryParse(_Value, out dValue);
                    if (isDouble == false || e.KeyChar.ToString()==" ") e.Handled = true;
                    else
                    {

                        switch (this.FormatType)
                        {
                            case MetroTextBoxFormat.N1:
                                if (_ctr > 1) { e.Handled = true; _takeeffect = true; }
                                break;
                            case MetroTextBoxFormat.N2:
                                if (_ctr > 2) { e.Handled = true; _takeeffect = true; }
                                break;
                            case MetroTextBoxFormat.N3:
                                if (_ctr > 3) { e.Handled = true; _takeeffect = true; }
                                break;
                            case MetroTextBoxFormat.N4:
                                if (_ctr > 4) { e.Handled = true; _takeeffect = true; }
                                break;
                            case MetroTextBoxFormat.N5:
                                if (_ctr > 5) { e.Handled = true; _takeeffect = true; }
                                break;
                            case MetroTextBoxFormat.N6:
                                if (_ctr > 6) { e.Handled = true; _takeeffect = true; }
                                break;
                            default:
                                break;
                        }
                        if (_takeeffect == false) base.OnKeyPress(e); 
                    }
                                       
                }
            }
            else { base.OnKeyPress(e); }
        }

        private void BaseTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        bool _cleared = false;
        bool _withtext = false;

        private void BaseTextBoxTextChanged(object sender, EventArgs e)
        {
            base.OnTextChanged(e);

            if (baseTextBox.Text != "" && !_withtext)
            {
                _withtext = true;
                _cleared = false;
                Invalidate();
            }

            if (baseTextBox.Text == "" && !_cleared)
            {
                if (this.ControlType == MetroTextBoxControlType.Integer)
                {
                    if (baseTextBox.Text == "") this.Text = "0";
                    else
                    {
                        string _Value = this.Text.ToString();
                        int iValue = 0;
                        bool isInteger = int.TryParse(_Value, out iValue);
                        if (isInteger == false) this.Text = "0";
                    }
                }
                if (this.ControlType == MetroTextBoxControlType.Double)
                {
                    if (baseTextBox.Text == "") {
                        switch (this.FormatType)
                        {
                            case MetroTextBoxFormat.N0:
                                baseTextBox.Text = "0";
                                break;
                            case MetroTextBoxFormat.N1:
                                this.Text = "0.0";
                                break;
                            case MetroTextBoxFormat.N2:
                                this.Text = "0.00";
                                break;
                            case MetroTextBoxFormat.N3:
                                this.Text = "0.000";
                                break;
                            case MetroTextBoxFormat.N4:
                                this.Text = "0.0000";
                                break;
                            case MetroTextBoxFormat.N5:
                                this.Text = "0.00000";
                                break;
                            case MetroTextBoxFormat.N6:
                                this.Text = "0.000000";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        string _Value = this.Text.ToString();
                        char[] chars = this.Text.ToCharArray();
                        if (chars[0].ToString() == ".") _Value = "0" + _Value;
                        if (chars[chars.Length - 1].ToString() == ".") _Value = _Value + "00";
                        double dValue = 0;
                        bool isDouble = double.TryParse(_Value, out dValue);
                        if (isDouble == false) {
                            switch (this.FormatType)
                            {
                                case MetroTextBoxFormat.N1:
                                    this.Text = "0.0";
                                    break;
                                case MetroTextBoxFormat.N2:
                                    this.Text = "0.00";
                                    break;
                                case MetroTextBoxFormat.N3:
                                    this.Text = "0.000";
                                    break;
                                case MetroTextBoxFormat.N4:
                                    this.Text = "0.0000";
                                    break;
                                case MetroTextBoxFormat.N5:
                                    this.Text = "0.00000";
                                    break;
                                case MetroTextBoxFormat.N6:
                                    this.Text = "0.000000";
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (chars[0].ToString() == "." || chars[chars.Length - 1].ToString() == ".") this.Text = _Value;

                    }
                }

                _withtext = false;
                _cleared = true;
                Invalidate();
            }
          
        }

        public void Select(int start, int length)
        {
            baseTextBox.Select(start, length);
        }

        public void SelectAll()
        {
            baseTextBox.SelectAll();
        }

        public void Clear()
        {
            baseTextBox.Clear();
        }

        void MetroTextBox_GotFocus(object sender, EventArgs e)
        {
            baseTextBox.Focus();
        }

        public void AppendText(string text)
        {
            baseTextBox.AppendText(text);
        }

        #endregion

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                Color backColor = BackColor;
                baseTextBox.BackColor = backColor;

                if (!useCustomBackColor)
                {
                    backColor = MetroPaint.BackColor.Form(Theme);
                    baseTextBox.BackColor = backColor;
                }

                if (backColor.A == 255)
                {
                    e.Graphics.Clear(backColor);
                    return;
                }

                base.OnPaintBackground(e);

                OnCustomPaintBackground(new MetroPaintEventArgs(backColor, Color.Empty, e.Graphics));
            }
            catch
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (GetStyle(ControlStyles.AllPaintingInWmPaint))
                {
                    OnPaintBackground(e);
                }

                OnCustomPaint(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
                OnPaintForeground(e);
            }
            catch
            {
                Invalidate();
            }
        }

        protected virtual void OnPaintForeground(PaintEventArgs e)
        {
            if (useCustomForeColor)
            {
                baseTextBox.ForeColor = ForeColor;
            }
            else
            {
                baseTextBox.ForeColor = MetroPaint.ForeColor.Button.Normal(Theme);
            }

            Color borderColor = MetroPaint.BorderColor.ComboBox.Normal(Theme);

            if (useStyleColors)
                borderColor = MetroPaint.GetStyleColor(Style);

            if (_witherror)
            {
                borderColor = Color.Red;
            }

            using (Pen p = new Pen(borderColor))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 2, Height - 2));
            }

            DrawIcon(e.Graphics);
        }

        private void DrawIcon(Graphics g)
        {
            if (displayIcon && textBoxIcon != null)
            {
                Point iconLocation = new Point(5, 5);
                if (textBoxIconRight)
                {
                    iconLocation = new Point(ClientRectangle.Width - iconSize.Width - 1, 1);
                }

                g.DrawImage(textBoxIcon, new Rectangle(iconLocation, iconSize));

                UpdateBaseTextBox();
            }
            else
            {
                _button.Visible = _showbutton;
                if (_showbutton && _button != null) UpdateBaseTextBox();
            }

            OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, baseTextBox.ForeColor, g));
        }

        #endregion

        #region Overridden Methods

        public override void Refresh()
        {
            base.Refresh();
            UpdateBaseTextBox();
            UpdateBaseTextDefaultValue();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateBaseTextBox();
            UpdateBaseTextDefaultValue();
        }

        #endregion

        #region Private Methods

        private void CreateBaseTextBox()
        {
            if (baseTextBox != null) return;

            baseTextBox = new PromptedTextBox();

            baseTextBox.BorderStyle = BorderStyle.None;
            baseTextBox.Font = MetroFonts.TextBox(metroTextBoxSize, metroTextBoxWeight);
            baseTextBox.Location = new Point(3, 3);
            baseTextBox.Size = new Size(Width - 6, Height - 6);

            Size = new Size(baseTextBox.Width + 6, baseTextBox.Height + 6);

            baseTextBox.TabStop = true;
            //baseTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            Controls.Add(baseTextBox);

            if (_button != null) return;

            _button = new MetroTextButton();
            _button.Theme = Theme;
            _button.Style = Style;
            _button.Location = new Point(3, 1);
            _button.Size = new Size(Height - 4, Height - 4);
            _button.TextChanged += _button_TextChanged;
            _button.MouseEnter += _button_MouseEnter;
            _button.MouseLeave += _button_MouseLeave;
            _button.Click += _button_Click;

            if (!this.Controls.Contains(this._button)) this.Controls.Add(_button);

            if (lnkClear != null) return;

            InitializeComponent();
        }

        public delegate void ButClick(object sender, EventArgs e);
        public event ButClick ButtonClick;

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        void _button_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null) ButtonClick(this, e);
        }

        void _button_MouseLeave(object sender, EventArgs e)
        {
            UseStyleColors = baseTextBox.Focused;
            Invalidate();
        }

        void _button_MouseEnter(object sender, EventArgs e)
        {
            UseStyleColors = true;
            Invalidate();
        }

        void _button_TextChanged(object sender, EventArgs e)
        {
            _button.Invalidate();
        }

        private void AddEventHandler()
        {
            baseTextBox.AcceptsTabChanged += BaseTextBoxAcceptsTabChanged;

            baseTextBox.CausesValidationChanged += BaseTextBoxCausesValidationChanged;
            baseTextBox.ChangeUICues += BaseTextBoxChangeUiCues;
            baseTextBox.Click += BaseTextBoxClick;
            baseTextBox.ClientSizeChanged += BaseTextBoxClientSizeChanged;
            baseTextBox.ContextMenuChanged += BaseTextBoxContextMenuChanged;
            baseTextBox.ContextMenuStripChanged += BaseTextBoxContextMenuStripChanged;
            baseTextBox.CursorChanged += BaseTextBoxCursorChanged;

            baseTextBox.KeyDown += BaseTextBoxKeyDown;
            baseTextBox.KeyPress += BaseTextBoxKeyPress;
            baseTextBox.KeyUp += BaseTextBoxKeyUp;

            baseTextBox.SizeChanged += BaseTextBoxSizeChanged;

            baseTextBox.TextChanged += BaseTextBoxTextChanged;
            baseTextBox.GotFocus += baseTextBox_GotFocus;
            baseTextBox.LostFocus += baseTextBox_LostFocus;
            
        }

        void baseTextBox_LostFocus(object sender, EventArgs e)
        {

            if (this.ControlType == MetroTextBoxControlType.Integer)
            {
                if (this.Text.ToString() == "") this.Text = "0";
                else
                {
                    string _Value = this.Text.ToString();

                    int iValue = 0;
                    bool isInteger = int.TryParse(_Value, out iValue);
                    if (isInteger == false) this.Text = "0";
                }
                
            }
            if (this.ControlType == MetroTextBoxControlType.Double)
            {
                if (this.Text.ToString() == "") {
                    switch (this.FormatType )
                    {
                        case MetroTextBoxFormat.N0:
                            baseTextBox.Text = "0";
                            break;
                        case MetroTextBoxFormat.N1:
                            this.Text = "0.0"; 
                            break;
                        case MetroTextBoxFormat.N2:
                            this.Text = "0.00"; 
                            break;
                        case MetroTextBoxFormat.N3:
                            this.Text = "0.000"; 
                            break;
                        case MetroTextBoxFormat.N4:
                            this.Text = "0.0000"; 
                            break;
                        case MetroTextBoxFormat.N5:
                            this.Text = "0.00000"; 
                            break;
                        case MetroTextBoxFormat.N6:
                            this.Text = "0.000000"; 
                            break;
                        default:
                            break;
                    }
                
                }
                else
                {
                    string _Value = this.Text.ToString();
                    char[] chars = this.Text.ToCharArray();
                    if (chars[0].ToString() == ".") _Value = "0" + _Value;
                    if (chars[chars.Length - 1].ToString() == ".") _Value = _Value + "00";
                    double dValue = 0;
                    bool isDouble = double.TryParse(_Value, out dValue);
                    if (isDouble == false) {
                        switch (this.FormatType)
                        {
                            case MetroTextBoxFormat.N1:
                                this.Text = "0.0";
                                break;
                            case MetroTextBoxFormat.N2:
                                this.Text = "0.00";
                                break;
                            case MetroTextBoxFormat.N3:
                                this.Text = "0.000";
                                break;
                            case MetroTextBoxFormat.N4:
                                this.Text = "0.0000";
                                break;
                            case MetroTextBoxFormat.N5:
                                this.Text = "0.00000";
                                break;
                            case MetroTextBoxFormat.N6:
                                this.Text = "0.000000";
                                break;
                            default:
                                break;
                        }
                    }
                    else if (chars[0].ToString() == "." || chars[chars.Length - 1].ToString() == ".") this.Text = _Value;
                }
            }

            UseStyleColors = false;
            Invalidate();
            this.InvokeLostFocus(this, e);
        }

        void baseTextBox_GotFocus(object sender, EventArgs e)
        {
            _witherror = false;
            UseStyleColors = true;
            Invalidate();
            this.InvokeGotFocus(this, e);
        }

        private void UpdateBaseTextDefaultValue()
        {
            switch (this.ControlType)
            {
                case MetroTextBoxControlType.Text:
                    break;
                case MetroTextBoxControlType.Integer:
                    baseTextBox.Text = "0";
                    break;
                case MetroTextBoxControlType.Double:
                    switch (this.FormatType)
                    {
                        case MetroTextBoxFormat.N0:
                            baseTextBox.Text = "0";
                            break;
                        case MetroTextBoxFormat.N1:
                            baseTextBox.Text = "0.0";
                            break;
                        case MetroTextBoxFormat.N2:
                            baseTextBox.Text = "0.00";
                            break;
                        case MetroTextBoxFormat.N3:
                            baseTextBox.Text = "0.000";
                            break;
                        case MetroTextBoxFormat.N4:
                            baseTextBox.Text = "0.0000";
                            break;
                        case MetroTextBoxFormat.N5:
                            baseTextBox.Text = "0.00000";
                            break;
                        case MetroTextBoxFormat.N6:
                            baseTextBox.Text = "0.000000";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
                      
        }

        private void UpdateBaseTextBox()
        {
            if (_button != null)
            {
                if ((Height % 2) > 0)
                {
                    _button.Size = new Size(Height - 2, Height - 2);
                    _button.Location = new Point(this.Width - (_button.Width + 1), 1);
                }
                else
                {
                    _button.Size = new Size(Height - 5, Height - 5);
                    _button.Location = new Point((this.Width - _button.Width) - 3, 2);
                }
            }

            int _clearloc = 0;
            if (lnkClear != null)
            {
                lnkClear.Visible = false;
                if (_showclear && this.Text != "" && !this.ReadOnly && this.Enabled)
                {
                    _clearloc = 16;
                    lnkClear.Location = new Point(this.Width - (ButtonWidth + 17), (this.Height - 14) / 2);
                    lnkClear.Visible = true;
                }
            }

            int _slbLoc = 0;
            if (lblSidelabel != null)
            {
                lblSidelabel.Visible = false;
                if (_showsidelabel && this.Text != "" && !this.ReadOnly && this.Enabled)
                {                   
                    lblSidelabel.FontSize = (MetroLabelSize)this.FontSize;

                    if (lblSidelabel.FontSize == MetroLabelSize.Tall) lblSidelabel.Width = 35 + 15;
                    else if (lblSidelabel.FontSize == MetroLabelSize.Medium) lblSidelabel.Width = 35 + 5;
                    else lblSidelabel.Width = 35 ;

                    _slbLoc = 24;
                    lblSidelabel.Location = new Point(0, 0);
                    lblSidelabel.Visible = true;
                }
            }
         

            if (baseTextBox == null) return;

            baseTextBox.Font = MetroFonts.TextBox(metroTextBoxSize, metroTextBoxWeight);

            if (displayIcon)
            {
                Point textBoxLocation = new Point(iconSize.Width + 10, 5);
                if (textBoxIconRight)
                {
                    textBoxLocation = new Point(3, 3);
                }

                baseTextBox.Location = textBoxLocation;
                baseTextBox.Size = new Size(Width - (20 + ButtonWidth + _clearloc) - iconSize.Width, Height - 6);
            }
            else
            {
                if (_showsidelabel == true)
                {
                    this.ControlType = MetroTextBoxControlType.Double;
                    int _x = lblSidelabel.Width +3;
                    int _width = Width - (_slbLoc + 18 + ButtonWidth);
                    if (lblSidelabel.FontSize == MetroLabelSize.Tall) _width = Width - (_slbLoc  + ButtonWidth) -35;
                    else if (lblSidelabel.FontSize == MetroLabelSize.Medium) _width = Width - (_slbLoc + ButtonWidth) -26;
                    else _width = Width - (_slbLoc + 18 + ButtonWidth);

                    baseTextBox.Location = new Point(_x,3);
                    baseTextBox.Size = new Size(_width , Height - 6);
                    lblSidelabel.Height = this.Height-1 ;
                }
                else
                {                    
                    baseTextBox.Location = new Point(3, 3);
                    baseTextBox.Size = new Size(Width - (6 + ButtonWidth + _clearloc), Height - 6);
                }
            }
        }

        #endregion

        #region PromptedTextBox

        private class PromptedTextBox : TextBox
        {
            private const int OCM_COMMAND = 0x2111;
            private const int WM_PAINT = 15;

            private bool drawPrompt;

            private string promptText = "";
            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]
            [DefaultValue("")]
            public string WaterMark
            {
                get { return promptText; }
                set
                {
                    promptText = value.Trim();
                    Invalidate();
                }
            }

            private Color _waterMarkColor = MetroPaint.ForeColor.Button.Disabled(MetroThemeStyle.Dark);
            public Color WaterMarkColor
            {
                get { return _waterMarkColor; }
                set
                {
                    _waterMarkColor = value; Invalidate();/*thanks to Bernhard Elbl
                                                              for Invalidate()*/
                }
            }

            private Font _waterMarkFont = MetroFramework.MetroFonts.WaterMark(MetroLabelSize.Small, MetroWaterMarkWeight.Italic);
            public Font WaterMarkFont
            {
                get { return _waterMarkFont; }
                set { _waterMarkFont = value; }
            }

            public PromptedTextBox()
            {
                SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
                drawPrompt = (Text.Trim().Length == 0);
            }

            private void DrawTextPrompt()
            {
                using (Graphics graphics = CreateGraphics())
                {
                    DrawTextPrompt(graphics);
                }
            }

            private void DrawTextPrompt(Graphics g)
            {
                TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis;
                Rectangle clientRectangle = ClientRectangle;

                switch (TextAlign)
                {
                    case HorizontalAlignment.Left:
                        clientRectangle.Offset(1, 0);
                        break;

                    case HorizontalAlignment.Right:
                        flags |= TextFormatFlags.Right;
                        clientRectangle.Offset(-2, 0);
                        break;

                    case HorizontalAlignment.Center:
                        flags |= TextFormatFlags.HorizontalCenter;
                        clientRectangle.Offset(1, 0);
                        break;
                }

                SolidBrush drawBrush = new SolidBrush(WaterMarkColor);

                TextRenderer.DrawText(g, promptText, _waterMarkFont, clientRectangle, _waterMarkColor, BackColor, flags);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (drawPrompt)
                {
                    DrawTextPrompt(e.Graphics);
                }
            }

            protected override void OnCreateControl()
            {
                base.OnCreateControl();
            }

            protected override void OnTextAlignChanged(EventArgs e)
            {
                base.OnTextAlignChanged(e);
                Invalidate();
            }

            protected override void OnTextChanged(EventArgs e)
            {
                base.OnTextChanged(e);
                drawPrompt = (Text.Trim().Length == 0);
                Invalidate();
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (((m.Msg == WM_PAINT) || (m.Msg == OCM_COMMAND)) && (drawPrompt && !GetStyle(ControlStyles.UserPaint)))
                {
                    DrawTextPrompt();
                }
            }

            protected override void OnLostFocus(EventArgs e)
            {
                base.OnLostFocus(e);
            }
        }

        #endregion

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetroTextBox));
            this.lnkClear = new MetroFramework.Controls.MetroLink();
            this.lblSidelabel = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // lnkClear
            // 
            this.lnkClear.FontSize = MetroFramework.MetroLinkSize.Medium;
            this.lnkClear.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.lnkClear.Image = ((System.Drawing.Image)(resources.GetObject("lnkClear.Image")));
            this.lnkClear.ImageSize = 10;
            this.lnkClear.Location = new System.Drawing.Point(654, 96);
            this.lnkClear.Name = "lnkClear";
            this.lnkClear.NoFocusImage = ((System.Drawing.Image)(resources.GetObject("lnkClear.NoFocusImage")));
            this.lnkClear.Size = new System.Drawing.Size(12, 12);
            this.lnkClear.TabIndex = 2;
            this.lnkClear.UseSelectable = true;
            this.lnkClear.Click += new System.EventHandler(this.lnkClear_Click);
            // 
            // lblSidelabel
            // 
            this.lblSidelabel.BackColor = System.Drawing.Color.DarkSlateGray;
            this.lblSidelabel.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblSidelabel.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblSidelabel.ForeColor = System.Drawing.Color.White;
            this.lblSidelabel.Location = new System.Drawing.Point(0, 0);
            this.lblSidelabel.Name = "lblSidelabel";
            this.lblSidelabel.Size = new System.Drawing.Size(35, 24);
            this.lblSidelabel.TabIndex = 0;
            this.lblSidelabel.Text = "USD";
            this.lblSidelabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSidelabel.UseCustomBackColor = true;
            this.lblSidelabel.UseCustomForeColor = true;
            // 
            // MetroTextBox
            // 
            this.Controls.Add(this.lblSidelabel);
            this.Controls.Add(this.lnkClear);
            this.ResumeLayout(false);

        }

        public delegate void LUClear();
        public event LUClear ClearClicked;

        void lnkClear_Click(object sender, EventArgs e)
        {
            this.Focus();
            this.Clear();
            baseTextBox.Focus();

            if (ClearClicked != null) ClearClicked();
        }
    }
}

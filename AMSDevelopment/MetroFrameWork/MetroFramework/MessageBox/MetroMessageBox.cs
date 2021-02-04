using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework.Interfaces;

namespace MetroFramework
{
    /// <summary>
    /// Metro-styled message notification.
    /// </summary>
    public static class MetroMessageBox
    {
        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message)
        { return Show(owner, message, "Notification"); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title)
        { return Show(owner, message, title, MessageBoxButtons.OK); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons)
        { return Show(owner, message, title, buttons, MessageBoxIcon.None); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon)
        { return Show(owner, message, title, buttons, icon, MessageBoxDefaultButton.Button1); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <param name="defaultbutton"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton)
        {
            DialogResult _result = DialogResult.None;

            if (owner != null)
            {
                MetroFramework.Forms.MetroForm _owner = (MetroFramework.Forms.MetroForm)owner;
                
                int _minWidth = 450;
                int _minHeight = 250;

                if (_owner.Size.Width < _minWidth ||
                    _owner.Size.Height < _minHeight)
                {
                    if (_owner.Size.Width < _minWidth &&
                        _owner.Size.Height < _minHeight) _owner.Size = new Size(_minWidth, _minHeight);
                    else
                    {
                        if (_owner.Size.Width < _minWidth) _owner.Size = new Size(_minWidth, _owner.Size.Height);
                        else _owner.Size = new Size(_owner.Size.Width, _minHeight);
                    }

                    int x = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Width / 2) - (_owner.Size.Width / 2)));
                    int y = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Height / 2) - (_owner.Size.Height / 2)));
                    _owner.Location = new Point(x, y);
                }
                
                MetroMessageBoxControl _control = new MetroMessageBoxControl(_owner.Style);
                _control.BackColor = _owner.BackColor;
                _control.Properties.Buttons = buttons;
                _control.Properties.DefaultButton = defaultbutton;
                _control.Properties.Icon = icon;
                _control.Properties.Message = message;
                _control.Properties.Title = title;
                _control.Padding = new Padding(0, 0, 0, 0);
                _control.ControlBox = false;
                _control.ShowInTaskbar = false;                
                _control.Size = new Size(_owner.Size.Width, _control.Height);
                _control.Location = new Point(_owner.Location.X, _owner.Location.Y + (_owner.Height - _control.Height) / 2);
                _control.ArrangeApperance();
                int _overlaySizes = Convert.ToInt32(Math.Floor(_control.Size.Height * 0.28));
                                
                _control.SetDefaultButton();

                switch (icon)
                {
                    case MessageBoxIcon.Error:
                        SystemSounds.Hand.Play(); break;
                    case MessageBoxIcon.Exclamation:
                        SystemSounds.Exclamation.Play(); break;
                    case MessageBoxIcon.Question:
                        SystemSounds.Beep.Play(); break;
                    default:
                        SystemSounds.Asterisk.Play(); break;
                }

                _result = _control.ShowDialog(_owner);
                _control.Dispose();
                _control = null;       
            }

            return _result;
        }

        private static void ModalState(MetroMessageBoxControl control)
        {
            while (control.Visible)
            { }
        }

    }
}

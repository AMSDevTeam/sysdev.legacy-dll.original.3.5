using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MetroFramework.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //metroLink1.GotFocus += new EventHandler(metroLink1_GotFocus);
            metroLink1.DisplayFocus = true;
            metroLink1.GotFocus +=new EventHandler(metroLink1_GotFocus);
            txtSearch.ClearClicked += new MetroFramework.Controls.MetroTextBox.LUClear(txtSearch_ClearClicked);

            this.txtConcern.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(txtConcern_ButtonClick);
        }

        void txtConcern_ButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("Fuck");
        }

        void txtSearch_ClearClicked()
        {
            
        }

        int _focu = 0;
        void metroLink1_GotFocus(object sender, EventArgs e)
        {
            this.Text = "here " + (_focu++) ;
            metroLink1.Refresh();
        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("tagal");
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();

        private Control GetFocusedControl()
        {
            Control focusedControl = null;
            // To get hold of the focused control:
            IntPtr focusedHandle = GetFocus();
            if (focusedHandle != IntPtr.Zero)
                // Note that if the focused Control is not a .Net control, then this will return null.
                focusedControl = Control.FromHandle(focusedHandle);
            return focusedControl;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MySql.Data.MySqlClient.MySqlConnection _con = new MySql.Data.MySqlClient.MySqlConnection("");
            //MySql.Data.MySqlClient.MySqlDataAdapter _adp = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FRM ");
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(metroComboBox1.Text);
        }

        private void txtConcern_Click(object sender, EventArgs e)
        {

        }
    }
}

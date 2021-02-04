using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using MetroFramework.Forms;
using Development.Materia.Database;
using Development.Materia;

namespace MetroFramework.Demo
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
            this.ResizeEnd += MainForm_ResizeEnd;
        }

        void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            int a = this.Location.X;
            int b = this.Location.Y;
        }

        private void metroTileSwitch_Click(object sender, EventArgs e)
        {
            var m = new Random();
            int next = m.Next(0, 13);
            metroStyleManager.Style = (MetroColorStyle)next;
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            metroStyleManager.Theme = metroStyleManager.Theme == MetroThemeStyle.Light ? MetroThemeStyle.Dark : MetroThemeStyle.Light;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            //MetroTaskWindow.ShowTaskWindow(this,"SubControl in TaskWindow", new TaskWindowControl(), 10);

            string _connection = "Server=127.0.0.1;Port=3306;Database=fsxtmpmkbc;Uid=root;Pwd=ams2011;";
            string _message = "Fuck you";
            IAsyncResult _testcon = Database.BeginTryConnect(_connection);
            metroProgressBar2.Visible = true;
            MySql.Data.MySqlClient.MySqlConnection _con = new MySql.Data.MySqlClient.MySqlConnection(_connection);
            _con.Open();
            _con.Close();
            _testcon.WaitToFinish();
            if (!Database.EndTryConnect(_testcon))
            {
               _message = "Failed to establish a connection using the specified connection values!";
            }

            MessageBox.Show(_message);

            metroProgressBar2.Visible = false;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            //MetroMessageBox _mes = new MetroMessageBox();
            //_mes.Text = "Test";
            //_mes.Show();
        }
    }
}

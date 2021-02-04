using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using tq = TarsierEyes.MySQL;
namespace PageControl.Pager
{
    public partial class SearchControl : MetroFramework.Controls.MetroTextBox
    {
        #region Declarations

        public DataView SearchDataSource = null;
        LookUpForm fLook = new LookUpForm();

        public delegate void AddButtonClick(EventArgs e);
        public event AddButtonClick AfterAddButtonClick;
        
        public delegate void LUComplete(Hashtable Data);
        public event LUComplete LookupComplete;

        public delegate void ValChange();
        public event ValChange DatavalueChanged;

        public delegate void LUPClear();
        public event LUPClear LookUpClear;


        bool _autocomplete = false;        
        #endregion

        #region Properties
        string _sql = "";
        string _connectionstring = "";

        [Description("Sets the Select SQL script")]
        [DefaultValue("")]
        public string CommandText { get { return _sql; } set { _sql = value; } }

        [Description("Sets the connectionstring")]
        [DefaultValue("")]
        public string ConnectionString { get { return _connectionstring; } set { _connectionstring = value; } }

        string _filter = "";

        [Description("Sets the Select SQL script filter")]
        [DefaultValue("")]
        public string Filter { get { return _filter; } set { _filter = value; } }

        string _displayfield = "";

        [Description("Sets what field from the Select SQL should be displayed on the Textbox")]
        [DefaultValue("")]
        public string DisplayField { get { return _displayfield; } set { _displayfield = value; } }

        int _visiblecol = 0;
        [Description("This will set the visible columns on the list, default is 0 means all columns is visible")]
        [DefaultValue(0)]
        public int VisibleCols { get { return _visiblecol; } set { _visiblecol = value; fLook.VisibleCols = value; } }

        List<string> _ColumnVisible = null;
        [Description("This  will set all Visible Column declared base on Column Name")]
        [DefaultValue(null)]
        public List<string> ColumnVisible { get { return _ColumnVisible; } set { _ColumnVisible = value; fLook.ColumnVisible = value; } }


        bool _showaddbutton = false ;
        [Description("This will set the add button to visible to add new entry on the list")]
        [DefaultValue(false)]
        public bool ShowAddButton { get { return _showaddbutton; } set { _showaddbutton = value; } }


        Hashtable _selectedvalue = null;
        [Description("Get Current Row")]
        [DefaultValue(null)]
        public Hashtable SelectedValue
        {
            get
            {
                return _selectedvalue;
            }
            set
            {
                _selectedvalue = value;
                
            }
        }

        
        string _datavalue = "";


        [Description("Sets the datavalue, by default it is the first field on the Select SQL script")]
        [DefaultValue("")]
        public string Value
        {
            get
            {
                return _datavalue;
            }
            set
            {
                _datavalue = value;
                if (value != "")
                {
                    SetDisplay();
                }
                else
                {
                    this.Text = "";
                }

            }
        }

        void SetDisplay()
        {
            if (SearchDataSource == null) return;
            int _row = SearchDataSource.Find(_datavalue);
            Hashtable _result = new Hashtable();
            if (_row > -1)
            {
                this.Text = SearchDataSource[_row][_displayfield].ToString();
                
                for (int i = 0; i <= SearchDataSource.Table.Columns.Count - 1; i++)
                {
                    _result.Add(SearchDataSource.Table.Columns[i].ColumnName, SearchDataSource[_row][i].ToString());
                }
                _selectedvalue = _result;
            }
            else
            {
                this.Text = "";
            }

            if (LookupComplete != null) LookupComplete(_result);
            _result = null;
        }
        #endregion

        public SearchControl()
        {
            InitializeComponent();
            this.ShowButton = true;
            this.ShowClearButton = true;
            this.CustomButton.Text = "...";
            this.ButtonClick += SearchControl_ButtonClick;
            this.TextChanged += SearchControl_TextChanged;
            this.KeyDown += SearchControl_KeyDown;
            this.Disposed += SearchControl_Disposed;
            fLook.AfterAddButtonClick += fLook_AfterAddButtonClick;
            fLook.lnkAdd.Visible = _showaddbutton;
            fLook.LookupComplete += fLook_LookupComplete;       
        }

        void fLook_AfterAddButtonClick(EventArgs e)
        {
            if (AfterAddButtonClick != null) AfterAddButtonClick(e);
        }

        public void RowFilter(string filter)
        {
            SearchDataSource.RowFilter = filter;
            DataTable dt = new DataTable();
            dt = ((DataTable)SearchDataSource.ToTable().Clone());
            dt.Load(((DataTable)SearchDataSource.ToTable()).CreateDataReader());            
            fLook.SetData(dt.DefaultView);

        }
        void SearchControl_Disposed(object sender, EventArgs e)
        {
            fLook.Dispose();
            fLook = null;
        }

        void fLook_LookupComplete(Hashtable Data)
        {
            _autocomplete = false;
            this.Text = Data[_displayfield].ToString();

            if (_datavalue != Data[SearchDataSource.Table.Columns[0].ColumnName].ToString())
            {
                if (DatavalueChanged != null) DatavalueChanged();
            }

            _datavalue = Data[SearchDataSource.Table.Columns[0].ColumnName].ToString();
            _selectedvalue = Data;

            if (LookupComplete != null) LookupComplete(Data);
        }

        #region Events
        void SearchControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                case Keys.Back:
                    _autocomplete = false;
                    break;
                case Keys.Tab:
                    case Keys.Return:
                    this.Parent.SelectNextControl(this, true, true, true, true);
                    break;
                case Keys.F1:
                    fLook.ShowLookUp(this);
                    break;
                default:
                    _autocomplete = true;
                    break;
            }
        }

        void SearchControl_TextChanged(object sender, EventArgs e)
        {
            if (SearchDataSource == null) return;
            if (this.Text == "")
            {
                _autocomplete = false;
               _datavalue  = "";
               fLook.Grid.RowSel = 0;
                if (LookUpClear != null) LookUpClear();
            }
            else
            {
                if (!_autocomplete) return;
                _autocomplete = false;
                int _row = fLook.Grid.FindRow(this.Text, 1, fLook.Grid.Cols[_displayfield].Index, false, false, true);
                if (_row > 0)
                {
                    int _start = this.Text.Length;
                    this.Text = fLook.Grid.GetData(_row, fLook.Grid.Cols[_displayfield].Index).ToString();
                    this.SelectionStart = _start;
                    if (this.Text.Length > 0) this.SelectionLength = this.Text.Length - _start;
                    _datavalue = fLook.Grid.GetData(_row, 0).ToString();
                    fLook.Grid.Row = _row;
                }
                else
                {
                    this.Text = "";
                }
                _autocomplete = true;
            }
        }

        void SearchControl_ButtonClick(object sender, EventArgs e)
        {
            fLook.lnkAdd.Visible = _showaddbutton;
            fLook.ShowLookUp(this);
        }
        #endregion

        public void Load()
        {
            DataTable _table = tq.Que.Execute(_connectionstring,_sql,tq.Que.ExecutionEnum.ExecuteReader).DataTable;
           
            SearchDataSource = new DataView(_table, "", _table.Columns[0].ColumnName, DataViewRowState.CurrentRows);
            fLook.SetData(SearchDataSource);
        }

        public void ReloadFilter(string sFilter)
        {
            SearchDataSource.RowFilter = sFilter;
            DataTable dt = new DataTable();
            dt = ((DataTable)SearchDataSource.ToTable()).Clone();
            dt.Load(((DataTable)SearchDataSource.ToTable()).CreateDataReader());
            DataView dview = dt.DefaultView ;
            fLook.SetData(dview);
        }

        public void SelectFirstIfOne()
        {
            if (SearchDataSource.Table.Rows.Count == 1)
            {
                _autocomplete = false;
                this.Text = SearchDataSource.Table.Rows[0][_displayfield].ToString();
                _datavalue = SearchDataSource.Table.Rows[0][0].ToString();

                Hashtable _result = new Hashtable();
                for (int i = 0; i < SearchDataSource.Table.Columns.Count; i++)
                {
                    _result.Add(SearchDataSource.Table.Columns[i].ColumnName, SearchDataSource.Table.Rows[0][i].ToString());
                }

                if (LookupComplete != null) LookupComplete(_result);
            }
        }

        public void AutoSelectFirst()
        {
            if (SearchDataSource.Table.Rows.Count >0)
            {
                _autocomplete = false;
                this.Text = SearchDataSource.Table.Rows[0][_displayfield].ToString();
                _datavalue = SearchDataSource.Table.Rows[0][0].ToString();

                Hashtable _result = new Hashtable();
                for (int i = 0; i < SearchDataSource.Table.Columns.Count; i++)
                {
                    _result.Add(SearchDataSource.Table.Columns[i].ColumnName, SearchDataSource.Table.Rows[0][i].ToString());
                }

                if (LookupComplete != null) LookupComplete(_result);
            }
        }
    }
}

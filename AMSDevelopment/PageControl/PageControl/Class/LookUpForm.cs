using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Controls;
using ts = TarsierEyes.Common.Simple;
using tc = TarsierEyes.Common.SQLStrings;
namespace PageControl.Pager
{
    public partial class LookUpForm : Form
    {
        #region Property
        public delegate void LUComplete(Hashtable Data);
        public event LUComplete LookupComplete;
        public delegate void AddPicklist(EventArgs e);
        public event AddPicklist AfterAddButtonClick;

        public C1.Win.C1FlexGrid.C1FlexGrid Grid { get { return mtgGrid; } }
        DataView dtvFilter = null;
        int _top = 0;
        int _left = 0;
        Rectangle _rt;
        int _width = 0;

        Object _parent = null;
        bool _focusback = false;
        LookUpPosition _pos = 0;

        int _visiblecols = 0;
        [DefaultValue(0)]
        [Description("Integer")]
        public int VisibleCols { get { return _visiblecols; } set { _visiblecols = value; } }

        List<string> _ColumnVisible = null;
        [DefaultValue(null)]
        [Description("List")]
        public List<string> ColumnVisible { get { return _ColumnVisible; } set { _ColumnVisible = value; } }

        public enum LookUpPosition
        {
            BottomLeft = 0,
            BottomRight = 1,
            TopLeft = 2,
            TopRight = 3
        }
        #endregion

        public LookUpForm()
        {
            InitializeComponent();

            this.Deactivate += LookUpForm_Deactivate;
            mtgGrid.DoubleClick += mtgGrid_DoubleClick;
            mtgGrid.KeyDown += mtgGrid_KeyDown;
            txtSearch.ButtonClick += txtSearch_ButtonClick;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
        }

        #region Events
        void LookUpForm_Deactivate(object sender, EventArgs e)
        {
            if (_parent == null || !_focusback)
            {
                this.TopMost = false;
                this.Hide();
            }
        }

        public void SetData(DataView View)
        {
           
            try
            {
                mtgGrid.DataSource = null;
                dtvFilter = null;
            }
            catch (Exception) { }
            dtvFilter = View;
            
            mtgGrid.DataSource = dtvFilter;
            txtSearch.Text = "";
        }

        public void SetData(DataTable Table)
        {
            SetData(Table.DefaultView);
        }

        public void ShowLookUp(object ControlReference, string LookUpKey = "", LookUpPosition Position = LookUpPosition.BottomLeft, bool Focus = false)
        {
            _parent = ControlReference;
            _focusback = Focus;
            _pos = Position;


            switch (_parent.GetType().Name)
            {
                case "SearchControl":
                    _rt = ((MetroTextBox)_parent).RectangleToScreen(_rt);
                    switch (_pos)
                    {
                        case LookUpPosition.BottomLeft:
                            _left = _rt.Left;
                            _top = _rt.Top + (((MetroTextBox)_parent).Height);

                            if (mtgGrid.Theme != ((MetroTextBox)_parent).Theme)
                            {
                                mtgGrid.Theme = ((MetroTextBox)_parent).Theme;
                                txtSearch.Theme = ((MetroTextBox)_parent).Theme;
                                pnlBottom.Theme = ((MetroTextBox)_parent).Theme;
                            }

                            if (mtgGrid.Style != ((MetroTextBox)_parent).Style)
                            {
                                mtgGrid.Style = ((MetroTextBox)_parent).Style;
                                txtSearch.Style = ((MetroTextBox)_parent).Style;
                            }
                            break;
                        case LookUpPosition.TopLeft:
                            break;
                    }
                    break;
                case "MetroLink":
                    _rt = ((MetroLink)_parent).RectangleToScreen(_rt);

                    _left = _rt.Left;
                    _top = _rt.Top + (((MetroLink)_parent).Height);

                    if (mtgGrid.Theme != ((MetroLink)_parent).Theme)
                    {
                        mtgGrid.Theme = ((MetroLink)_parent).Theme;
                        txtSearch.Theme = ((MetroLink)_parent).Theme;
                        pnlBottom.Theme = ((MetroLink)_parent).Theme;
                    }

                    if (mtgGrid.Style != ((MetroLink)_parent).Style)
                    {
                        mtgGrid.Style = ((MetroLink)_parent).Style;
                        txtSearch.Style = ((MetroLink)_parent).Style;
                    }
                    break;
            }

            SetupGrid();

            this.Show();
            if (_focusback)
            {
                ((Control)_parent).Focus();
            }
            else
            {
                this.Focus();
            }
        }

        void SetupGrid()
        {
            _width = 0;
            mtgGrid.AutoSizeCols();
            if (mtgGrid.Rows.Count < 15)
            {
                int _height = mtgGrid.Rows.Count <= 4 ? mtgGrid.Rows.Count * 22 : mtgGrid.Rows.Count * 19;
                this.Height = _height + pnlBottom.Height;
            }
            else
            {
                this.Height = 300;
            }

            mtgGrid.Cols[0].Visible = false;

            int _intvisible = 0;
            for (int i = 1; i < mtgGrid.Cols.Count; i++)
            {
                mtgGrid.Cols[i].TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
                if (mtgGrid.Rows.Count - 1 > 0)
                {
                    switch (mtgGrid.Cols[i].DataType.ToString())
                    {
                        case "System.Double":
                        case "System.Decimal":
                            mtgGrid.Cols[i].Format = "N4";
                            break;
                        case "System.DateTime":
                            mtgGrid.Cols[i].Format = "yyyy-MM-dd";
                            break;
                    }
                }

                if (_ColumnVisible == null)
                {

                    if (mtgGrid.Cols[i].Visible)
                    {
                        _intvisible++;
                        if (_visiblecols > 0 && _intvisible > _visiblecols)
                        {
                            mtgGrid.Cols[i].Visible = false;
                        }
                        else
                        {
                            _width += mtgGrid.Cols[i].WidthDisplay;
                        }
                    }
                }
                else {
                    mtgGrid.Cols[i].Visible = false;
                }

            }

            if (_ColumnVisible != null)
            {
                foreach (string col in _ColumnVisible)
                {
                    try
                    {
                        mtgGrid.Cols[col].Visible = true;
                    }
                    catch (Exception) { }
                   
                }
            }


            this.Width = _width + 22;
            
            if (this.Width < 300) this.Width = 300;
            if (this.Width < ((SearchControl)_parent).Width) this.Width = ((SearchControl)_parent).Width;

            

            _rt = Screen.PrimaryScreen.Bounds;

            //if (_left < 0) _left = 0;
            //if (_top < 0) _top = 0;

            if (_left + this.Width > _rt.Width) _left = _rt.Width - this.Width;
            if (_top + this.Height > _rt.Height) _top = _rt.Height - this.Height - 50;

            this.Left = _left;
            this.Top = _top;
        }
        #endregion
        #region SelectionGrid
        void mtgGrid_DoubleClick(object sender, EventArgs e)
        {
            if (mtgGrid.MouseRow == 0) return;
            string _field = mtgGrid.GetData(0, mtgGrid.Col).ToString();
            LookUpCompleted();
            this.Hide();
        }
        void mtgGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    if (mtgGrid.Row > 0) LookUpCompleted();
                    break;
                case Keys.Escape:
                case Keys.Menu:
                case Keys.Control:
                case Keys.Alt:
                    this.Hide();
                    break;
            }
        }
        void LookUpCompleted()
        {
            Hashtable _result = new Hashtable();
            for (int i = 0; i < mtgGrid.Cols.Count; i++)
            {
                _result.Add(mtgGrid.GetData(0, i).ToString(), mtgGrid.GetData(mtgGrid.Row, i).ToString());
            }

            if (LookupComplete != null) LookupComplete(_result);
        }
        #endregion

        #region Text Filter
        void txtSearch_ButtonClick(object sender, EventArgs e)
        {
            dtvFilter.RowFilter = "" ;
            this.Hide();
            SetupGrid();
            ShowLookUp(_parent, "", _pos, _focusback);
        }

        void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                    this.Parent.SelectNextControl(this, true, true, true, true);
                    break;
                case Keys.Return:
                    txtSearch_ButtonClick(this, new EventArgs());
                    break;
                case Keys.Escape:
                    this.Hide();
                    break;
            }
        }

        void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (mtgGrid.Tag != null) return;
            if (mtgGrid.DataSource == null) return;
          
            DataSourceRowFilter(dtvFilter, txtSearch.Text);

            if (mtgGrid.Rows.Count < 15)
            {
                int _height = mtgGrid.Rows.Count - 1 <= 3 ? mtgGrid.Rows.Count * 22 : mtgGrid.Rows.Count * 19;
                this.Height = _height + pnlBottom.Height;
            }
            else
            {
                this.Height = 300;
            }
        }

        public static void DataSourceRowFilter(DataView datasource, string valuefilter)
        {
            if (datasource != null)
            {
                StringBuilder filter = new StringBuilder();

                var _with1 = datasource.Table;
                foreach (DataColumn col in _with1.Columns)
                {
                    if (col.Ordinal==0) continue;
                    
                    bool excluded = false;

                    if (!excluded)
                    {
                        switch (col.DataType.Name)
                        {
                            case "String":
                                filter.Append((!string.IsNullOrEmpty(filter.ToString().Trim()) ? " OR" + Environment.NewLine : string.Empty) + "(`" + col.ColumnName + "` LIKE '%" +  tc.ToSqlValidString(valuefilter).Replace(" ", "%') AND (`" + col.ColumnName + "` LIKE '%") + "%')");
                                break;
                            case "DateTime":
                            case "Decimal":
                            case "Int16":
                            case "Int32":
                            case "Int64":
                            case "Byte":
                            case "Boolean":
                                filter.Append((!string.IsNullOrEmpty(filter.ToString().Trim()) ? " OR" + Environment.NewLine : string.Empty) + "(CONVERT(`" + col.ColumnName + "`, System.String) LIKE '%" + tc.ToSqlValidString(valuefilter).Replace(" ", "%') AND (CONVERT(`" + col.ColumnName + "`, System.String) LIKE '%") + "%')");
                                break;
                            default:
                                break;
                        }
                    }
                }

                try
                {
             
                    datasource.RowFilter = filter.ToString();

                }
                catch (Exception)
                {
                }
            }
        }
        #endregion

        private void lnkAdd_Click(object sender, EventArgs e)
        {
            if (AfterAddButtonClick != null) AfterAddButtonClick(e);

        }
    }
}

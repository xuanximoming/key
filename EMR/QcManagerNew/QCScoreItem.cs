using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Wordbook;
using YiDanCommon.Ctrs.FORM;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class QCScoreItem : DevBaseForm
    {
        IYidanEmrHost m_app;
        public QCScoreType_DataEntity m_scoretype;
        QCScoreItem_DataEntity m_scoreitem;

        SqlManger m_sqlManger;

        public QCScoreItem(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            
        }

        private void QCScoreItem_Load(object sender, EventArgs e)
        {
            m_sqlManger = new SqlManger(m_app);

            BindScoreType();

            BindDataSource();

            this.lookUpEditorType.Focus();
        }

        /// <summary>
        /// 将数据源绑定到页面中
        /// </summary>
        public void BindDataSource()
        {
            string scroetypecode = "";
            if (m_scoretype != null)
            {
                scroetypecode = m_scoretype.Typecode;
                this.lookUpEditorType.CodeValue = m_scoretype.Typecode;
            }
            if (lookUpEditorType.CodeValue != "")
            {
                scroetypecode = lookUpEditorType.CodeValue;
            }

            this.gridControlScoreItem.DataSource = m_sqlManger.GetQCItemScore(scroetypecode);
        }

        /// <summary>
        /// 绑定页面下拉框
        /// </summary>
        private void BindScoreType()
        {
            lookUpWindowType.SqlHelper = m_app.SqlHelper;

            DataTable Dept = m_sqlManger.GetQCTypeScore();

            Dept.Columns["TYPECODE"].Caption = "科室代码";
            Dept.Columns["TYPENAME"].Caption = "科室名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("TYPECODE", 65);
            cols.Add("TYPENAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "TYPECODE", "TYPENAME", cols, "TYPECODE//TYPENAME");
            lookUpEditorType.SqlWordbook = deptWordBook;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            m_scoretype = null;
            BindDataSource();
        }

        /// <summary>
        /// xiao项目代码实体赋值
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private QCScoreItem_DataEntity FillQCTypeValue2Entity(DataRow dataRow)
        {
            QCScoreItem_DataEntity scoreItem = null;
            if (dataRow != null)
            {
                scoreItem = new QCScoreItem_DataEntity();
                scoreItem.Itemcode = dataRow["ItemCode"].ToString();
                scoreItem.Itemname = dataRow["ItemName"].ToString();
                scoreItem.Iteminstruction = dataRow["ItemInstruction"].ToString();
                scoreItem.Itemdefaultscore = int.Parse(dataRow["ItemDefaultScore"].ToString());
                scoreItem.Itemstandardscore = int.Parse(dataRow["ItemStandardScore"].ToString());
                scoreItem.Itemcategory = int.Parse(dataRow["ItemCategory"].ToString());
                scoreItem.Itemdefaulttarget = Convert.ToInt32(dataRow["ItemDefaultTarget"].ToString());
                scoreItem.Itemtargetstandard = Convert.ToInt32(dataRow["ItemTargetStandard"].ToString());
                scoreItem.Itemscorestandard = Convert.ToInt32(dataRow["ItemScoreStandard"].ToString());
                scoreItem.Itemorder = int.Parse(dataRow["ItemOrder"].ToString());
                scoreItem.Typecode = dataRow["TypeCode"].ToString();

                scoreItem.Itemmemo = dataRow["ItemMemo"].ToString();
            }
            return scoreItem;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            QCScoreItemEdit scoreitemedit = new QCScoreItemEdit(m_app);
            scoreitemedit.m_scoreitem = null;
            scoreitemedit.ShowDialog();

            if (scoreitemedit.ISRefresh)
                BindDataSource();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (gridViewScoreItem .FocusedRowHandle < 0) return;
            DataRow foucesRow = gridViewScoreItem.GetDataRow(gridViewScoreItem.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("TYPECODE"))
                return;

            QCScoreItemEdit scoreitemedit = new QCScoreItemEdit(m_app);
            scoreitemedit.m_scoreitem = FillQCTypeValue2Entity(foucesRow);
            scoreitemedit.ShowDialog();
            if (scoreitemedit.ISRefresh)
                BindDataSource();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gridViewScoreItem.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridViewScoreItem.GetDataRow(gridViewScoreItem.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("ITEMCODE"))
                return;

            m_sqlManger.EditQCItemScore("2", FillQCTypeValue2Entity(foucesRow));

            m_app.CustomMessageBox.MessageShow("删除成功！");

            BindDataSource();
        }

        private void gridViewScoreItem_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
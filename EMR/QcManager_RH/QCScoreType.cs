using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.QcManager
{
    public partial class QCScoreType : DevExpress.XtraEditors.XtraForm
    {
        IEmrHost m_app;
        SqlManger m_sqlManger;
        public QCScoreType(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        private void QCScoreType_Load(object sender, EventArgs e)
        {
            m_sqlManger = new SqlManger(m_app);
            BindDataSource();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            BindDataSource();
        }

        /// <summary>
        /// 将数据源绑定到页面中
        /// </summary>
        public void BindDataSource()
        {
            this.gridviewScoreCoreType.DataSource = m_sqlManger.GetQCTypeScore();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            ViewScoreTypeDetail();
        }


        /// <summary>
        /// 大项目代码实体赋值
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private QCScoreType_DataEntity FillQCTypeValue2Entity(DataRow dataRow)
        {
            QCScoreType_DataEntity scoreType = null;
            if (dataRow != null)
            {
                scoreType = new QCScoreType_DataEntity();
                scoreType.Typecode = dataRow["TypeCode"].ToString();
                scoreType.Typename = dataRow["TypeName"].ToString();
                scoreType.Typeinstruction = dataRow["TypeInstruction"].ToString();
                scoreType.Typecategory = int.Parse(dataRow["TypeCategory"].ToString());
                scoreType.Typeorder = int.Parse(dataRow["TypeOrder"].ToString());
                scoreType.Typememo = dataRow["TypeMemo"].ToString();
            }
            return scoreType;
        }

        private void ViewScoreTypeDetail()
        {
            if (gridViewScoreType.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridViewScoreType.GetDataRow(gridViewScoreType.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("TYPECODE"))
                return;

            QCScoreItem scoreitem = new QCScoreItem(m_app);
            scoreitem.m_scoretype = FillQCTypeValue2Entity(foucesRow);
            scoreitem.ShowDialog();
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (gridViewScoreType.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridViewScoreType.GetDataRow(gridViewScoreType.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("TYPECODE"))
                return;

            QCScoreTypeEdit scoretypeedit = new QCScoreTypeEdit(m_app);
            scoretypeedit.m_ScoreType = FillQCTypeValue2Entity(foucesRow);
            scoretypeedit.ShowDialog();
            if(scoretypeedit.ISRefresh)
                BindDataSource();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            QCScoreTypeEdit scoretypeedit = new QCScoreTypeEdit(m_app);
            scoretypeedit.m_ScoreType = null;
            scoretypeedit.ShowDialog();
            if (scoretypeedit.ISRefresh)
                BindDataSource();

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gridViewScoreType.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridViewScoreType.GetDataRow(gridViewScoreType.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("TYPECODE"))
                return;

            m_sqlManger.EditQCTypeScore("2", FillQCTypeValue2Entity(foucesRow));

            m_app.CustomMessageBox.MessageShow("删除成功！");

            BindDataSource();
        }


 
    }
}
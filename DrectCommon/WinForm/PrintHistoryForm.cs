using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Data;

namespace DrectSoft.Common
{
    public partial class PrintHistoryForm : DevBaseForm
    {
        public PrintHistoryForm(string checkflow, string type)
        {
            InitializeComponent();
            InitDate(checkflow, type);
        }


        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitDate(string checkflow, string type)
        {
            string sql = string.Format(@"select printhistory.printdatetime,printhistory.startpage,printhistory.endpage,printhistory.printpages,
 (select users.name from users where users.id=printhistory.printdocid) docname
  from printhistorynurse printhistory where printhistory.printrecordflow='{0}' and printhistory.printtype='{1}' order by printhistory.printdatetime desc
", checkflow, type);
            DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            gridControl1.DataSource = dt;
        }
    }
}
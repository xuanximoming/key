using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Emr.QcManager
{
    public partial class DataBaseTable : UserControl
    {
        public DataBaseTable()
        {
            InitializeComponent();
            string sql = "select t1.Table_Name as TABLE_NAME,t3.comments,t1.Column_Name,t1.Data_Type,t1.Data_Length,t1.NullAble,t2.Comments,t1.Data_Default from cols t1 left join user_col_comments t2 on t1.Table_name=t2.Table_name and t1.Column_Name=t2.Column_Name left join user_tab_comments t3 on t1.Table_name=t3.Table_name WHERE NOT EXISTS ( SELECT t4.Object_Name FROM User_objects t4 WHERE t4.Object_Type='TABLE' AND t4.Object_Name=t1.Table_Name ) ORDER BY t1.Table_Name, t1.Column_ID";
            DataTable dt=new DataTable();
            dt=DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql);
            this.gridControl1.DataSource = dt;
        }
        
        
    }
}

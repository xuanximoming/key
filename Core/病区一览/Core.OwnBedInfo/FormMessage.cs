using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.OwnBedInfo
{
    public partial class FormMessage : DevBaseForm
    {
        private IDataAccess m_DataAccessEmrly;
        public string txtNoOfInpat;
        private IEmrHost _app;
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        public IEmrHost App
        {
            get { return _app; }
            set { _app = value; }
        }

        public FormMessage(IEmrHost application)
        {
            InitializeComponent();
            App = application;
        }

        private void 病历违规信息_Load(object sender, EventArgs e)
        {
            m_DataAccessEmrly = App.SqlHelper;
            //this.popupMenu1.Manager.Items.Clear();

            string sql = string.Format(@"SELECT (CASE WHEN QCRecord.Result = 0 THEN '否' ELSE '是' end) Result,
                                            substring(ConditionTime,0,11) ConditionTime, 
                                            QCRecord.FoulMessage 
                                            FROM QCRecord QCRecord
                                                LEFT JOIN dbo.InPatient InPatient on InPatient.NoOfInpat = QCRecord.NoOfInpat
                                                LEFT JOIN dbo.Users Users on Users.ID = InPatient.Resident
                                            WHERE QCRecord.Valid = 1 
	                                            AND QCRecord.NoOfInpat = '{0}'
	                                            AND FoulState = 1  ", txtNoOfInpat);

            DataTable dt = this.m_DataAccessEmrly.ExecuteDataTable(sql);

            this.gridMessage.DataSource = dt;

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class UCRecordDateTime : DevExpress.XtraEditors.XtraUserControl
    {
        public UCRecordDateTime(string datestr, string timestr)
        {
            InitializeComponent();
            InitData(datestr, timestr);
        }

        //加载数据
        public void InitData(string datestr, string timestr)
        {
            string datetime = datestr + " " + timestr;
            DateTime dtTime = DateUtil.getDateTime(datetime);
            dateEditRecordDate.DateTime = dtTime;
        }

        
        /// <summary>
        /// 获取记录日期
        /// </summary>
        /// <returns></returns>
        public string GetDate()
        {
            return DateUtil.getDateTime(dateEditRecordDate.DateTime.ToString(), DateUtil.NORMAL_SHORT); ;
        }
        /// <summary>
        /// 获取记录时间
        /// </summary>
        /// <returns></returns>
        public string GetTime()
        {
          return   DateUtil.getDateTime(dateEditRecordDate.DateTime.ToString(), DateUtil.NORMAL_LONG).Substring(11,8);
        }

       
    }
}

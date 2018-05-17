using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Collections.Generic;
using System.Data;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class TongJiForm : DevBaseForm
    {
        Dictionary<int, decimal> TongJiCount = new Dictionary<int, decimal>();
        DataTable dtNew = new DataTable();
        string m_commonnoteflow;
        CommonNoteCountEntity m_commonNoteCountEntity;
        public TongJiForm(DataTable dt, string commonnoteflow, CommonNoteCountEntity commonNoteCountEntity)
        {
            try
            {
                m_commonnoteflow = commonnoteflow;
                m_commonNoteCountEntity = commonNoteCountEntity;
                dtNew = dt.Copy();
                InitializeComponent();
                dtStart.DateTime = DateTime.Now.AddDays(-1);
                dtEnd.DateTime = DateTime.Now;
                btnTongJi_Click(null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 统计出现格式问题进行转换
        /// XLB 2013-07-10
        /// </summary>
        /// <param name="commonnoteflow"></param>
        /// <param name="dt"></param>
        private void GetCommonNoteCount(string commonnoteflow, DataTable dt)
        {
            try
            {
                TongJiCount.Clear();
                flPanel.Controls.Clear();
                string[] values = m_commonNoteCountEntity.ItemCount.Split(',', '，');
                foreach (var item in values)
                {
                    //避免格式错误引起的转换失败
                    int result = 0;
                    if (int.TryParse(item, out result))
                    {
                        if (!TongJiCount.ContainsKey(result))
                        {
                            TongJiCount.Add(result, 0);
                        }
                    }
                }
                foreach (DataRow datarow in dt.Rows)
                {
                    foreach (var itemtj in values)
                    {
                        //int intValue = Convert.ToInt32(itemtj);
                        int result;
                        if (int.TryParse(itemtj, out result))
                        {
                            Decimal itemdec = 0;
                            Decimal.TryParse(datarow[result + 2].ToString(), out itemdec);
                            TongJiCount[result] += itemdec;
                        }
                    }
                }
                foreach (var item in TongJiCount)
                {
                    UCLableTongJi uCLableTongJi = new UCLableTongJi(dt.Columns[item.Key + 2].Caption + "：", item.Value.ToString());
                    flPanel.Controls.Add(uCLableTongJi);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void btnTongJi_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateTimeS = DateTime.Parse(dtStart.DateTime.ToString("yyyy-MM-dd") + " " + "00:00:00");
                DateTime dateTimeE = DateTime.Parse(dtEnd.DateTime.ToString("yyyy-MM-dd") + " " + "23:59:59");
                string filter = string.Format(@"jlsj>='{0}' and jlsj<='{1}'", dateTimeS, dateTimeE);
                DataRow[] dataRows = dtNew.Select(filter);
                DataTable dt = dtNew.Clone();
                foreach (DataRow item in dataRows)
                {
                    dt.Rows.Add(item.ItemArray);
                }

                GetCommonNoteCount(m_commonnoteflow, dt);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }




    }

}
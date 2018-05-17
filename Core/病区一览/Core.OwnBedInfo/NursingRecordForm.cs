using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using DrectSoft.Common.Ctrs.FORM;
using System.Data.SqlClient;
using DrectSoft.DSSqlHelper;
namespace DrectSoft.Core.OwnBedInfo
{
    public partial class NursingRecordForm : DevBaseForm
    {
        #region - 变量 -

        private static NursingRecordForm _nursingRecordFrm=null;

        #endregion

        #region - 构造函数 -

        /// <summary>
        /// 无参构造
        /// <auth>zyx</auth>
        /// <date>2013-1-7</date>
        /// </summary>
        private NursingRecordForm()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        #endregion

        #region - 事件 -

        /// <summary>
        /// 窗体加载事件
        /// <auth>zyx</auth>
        /// <date>2013-1-7</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NursingRecordForm_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 按钮单击事件
        /// <auth>zyx</auth>
        /// <date>2013-1-7</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btn_Click(object sender, EventArgs e)
        {
            try
            {
                SimpleButton btn = (SimpleButton)sender;
                txtName.Text = btn.Text;
                Init(btn.Name.ToString());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        #endregion

        #region - 方法 -

        /// <summary>
        /// 创建唯一窗体实例
        /// <auth>zyx</auth>
        /// <date>2013-1-7</date>
        /// </summary>
        /// <returns></returns>
        public static NursingRecordForm CreateInstance()
        {
            try
            {
                if (_nursingRecordFrm == null || _nursingRecordFrm.IsDisposed)
                {
                    _nursingRecordFrm = new NursingRecordForm();
                }
                return _nursingRecordFrm;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 填充体征数据
        /// <auth>zyx</auth>
        /// <date>2013-1-7</date>
        /// </summary>
        /// <param name="m_oofinpat">病案号</param>
        public void InitNursingRecord(string noofinpat)
        {
            try
            {
                gcBaby.Controls.Clear();
                if (!string.IsNullOrEmpty(noofinpat))
                {
                    string sqlGetInpatient = "select noofinpat,name, babycount from inpatient where noofinpat=@noofinpat";
                    SqlParameter[] parms = new SqlParameter[] 
                    {
                        new SqlParameter("@noofinpat",SqlDbType.VarChar)
                    };
                    parms[0].Value = noofinpat;
                    DataTable dtInpatient = DS_SqlHelper.ExecuteDataTable(sqlGetInpatient, parms, CommandType.Text);
                    if (dtInpatient != null && dtInpatient.Rows.Count > 0)
                    {
                        txtName.Text = dtInpatient.Rows[0]["name"].ToString();
                        if (!string.IsNullOrEmpty(dtInpatient.Rows[0]["babycount"].ToString())) //存在小孩
                        {
                            Init(noofinpat);
                            CreateButton(dtInpatient);
                        }
                        else //不存在小孩
                        {
                            Init(noofinpat);
                            LabelControl lb = new LabelControl();
                            Point pt = new Point(30, 30);
                            lb.Location = pt;
                            lb.Text = "无";
                            gcBaby.Controls.Add(lb);

                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化数据
        /// <auth>zyx</auth>
        /// <date>2013-1-7</date>
        /// </summary>
        /// <param name="noofinpat">病案号</param>
        private void Init(string noofinpat)
        {
            try
            {
                _nursingRecordFrm.xtraTabControl1.SelectedTabPage = xtraTabPage2;
                gcCurrentList.DataSource = null;
                string sqlGetNursing = "select * from notesonnursing where noofinpat=@noofinpat and dateofsurvey between @preDate and @curDate  order by DATEOFSURVEY, to_number(timeslot)";
                SqlParameter[] parms = new SqlParameter[] 
                {
                     new SqlParameter("@noofinpat", SqlDbType.VarChar),
                     new SqlParameter("@preDate",SqlDbType.VarChar),
                     new SqlParameter("@curDate",SqlDbType.VarChar)
                };

                parms[0].Value = noofinpat;
                parms[1].Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                parms[2].Value = DateTime.Now.ToString("yyyy-MM-dd");
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlGetNursing, parms, CommandType.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    BindDate(dt);
                }
                else 
                {
                    lblPre.Visible = true;
                    lblCurrent.Visible = true;
                    gcCurrentList.DataSource = null;
                    gcPreList.DataSource = null;
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 根据婴儿数量动态创建按钮
        /// <auth>zyx</auth>
        /// <date>2013-1-7</date>
        /// </summary>
        /// <param name="dateTable"></param>
        private void CreateButton(DataTable dateTable)
        {
            try
            {
                gcBaby.Controls.Clear();
                if (dateTable != null && dateTable.Rows.Count > 0)
                {
                    int count = int.Parse(dateTable.Rows[0]["babycount"].ToString());
                    string sqlGetBaby = "select i.noofinpat,i.name from inpatient i where i.mother=@mother";
                    SqlParameter[] parms = new SqlParameter[] 
                    {
                        new SqlParameter("@mother",SqlDbType.Decimal)
                    };
                    parms[0].Value = dateTable.Rows[0]["noofinpat"];
                    DataTable dtBaby = DS_SqlHelper.ExecuteDataTable(sqlGetBaby, parms, CommandType.Text);
                    for (int i = 0; i < count; i++)
                    {
                        SimpleButton btn = new SimpleButton();
                        Point pt = new Point(10 + i * 85, 26);
                        btn.Location = pt;
                        btn.Text = dtBaby.Rows[i]["name"].ToString();
                        btn.Name = dtBaby.Rows[i]["noofinpat"].ToString();
                        btn.Click += new EventHandler(btn_Click);
                        gcBaby.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        private void gvList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.AbsoluteIndex == 1)//体温列
                {   
                    float result=0f;
                    string val=e.DisplayText;
                    bool state=float.TryParse(val,out result);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    if(state&&result>37.5)
                    {
                        e.Graphics.DrawRectangle(Pens.Red,e.Bounds);
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void BindDate(DataTable dataTable)
        {
            try
            {
                
                DataTable CurrentDate = dataTable.Clone();
                DataTable preDate = dataTable.Clone();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i]["DATEOFSURVEY"].Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        CurrentDate.ImportRow(dataTable.Rows[i]);
                    }
                    else 
                    {
                        preDate.ImportRow(dataTable.Rows[i]);
                    }
                }
                if (CurrentDate != null && CurrentDate.Rows.Count > 0)
                {
                    lblCurrent.Visible = false;
                    gcCurrentList.DataSource = CurrentDate;
                }
                else 
                {
                    gcCurrentList.DataSource = null;
                    lblCurrent.Visible = true;
                }

                if (preDate != null && preDate.Rows.Count > 0)
                {
                    lblPre.Visible = false;
                    gcPreList.DataSource = preDate;
                }
                else 
                {
                    gcPreList.DataSource = null;
                    lblPre.Visible = true;
                }
               

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void gvCurrentList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header)
                {
                    e.Info.DisplayText = "序号";
                }
                else if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Row)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private void gvPreList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header)
                {
                    e.Info.DisplayText = "序号";
                }
                else if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Row)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
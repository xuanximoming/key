using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.NurseDocument.Controls
{
    public partial class UCNursingRecordTable : DevExpress.XtraEditors.XtraUserControl
    {
        private Dictionary<string, VitalSignInfoEntity> m_VitalSignsCaption = new Dictionary<string, VitalSignInfoEntity>();//存放护理数据名称（其元素后期可以从配置文件中读取）
        private DataTable dtWayOFSurvey;//体温测量方式
        private string[] m_TimeSlot = null;
        //private int m_DayOfModify=0;
        private UCTemperatureEditor[] m_UCTemperatureEditor = new UCTemperatureEditor[6];
        private XmlDocument xmlDoc = new XmlDocument();
        private string m_xmlFilePath = "";
        Form obj = null;
        int index = 0;
        #region 初始化窗体

        public UCNursingRecordTable()
        {
            try
            {
                InitializeComponent();
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + ConfigInfo.configName))
                {
                    MessageBox.Show("Config配置文件不存在");
                    return;
                }
                m_xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + ConfigInfo.configName;
                xmlDoc.Load(m_xmlFilePath);

                InitDataGridViewForVitalSigns();

                XmlNode nodeElement = xmlDoc.GetElementsByTagName("DataRowEdit")[0];
                XmlNodeList nodeList = nodeElement.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    VitalSignInfoEntity obj = new VitalSignInfoEntity();
                    obj.caption = node.InnerText;
                    obj.datafield = node.Attributes["datafield"] == null ? "" : node.Attributes["datafield"].Value;
                    obj.cellCtlType = (CellControlType)Enum.Parse(typeof(CellControlType), node.Attributes["cellControl"] == null ? "1" : node.Attributes["cellControl"].Value);
                    obj.showForm = node.Attributes["showDlg"] == null ? "" : node.Attributes["showDlg"].Value;
                    obj.datasource = null;
                    obj.showtype = node.Attributes["showtype"] == null ? "" : node.Attributes["showtype"].Value;
                    m_VitalSignsCaption.Add(obj.datafield, obj);
                }

                foreach (KeyValuePair<string, VitalSignInfoEntity> pair in m_VitalSignsCaption)
                {
                    DataGridViewRow newRow = newRow = new DataGridViewRow();
                    newRow.Tag = pair.Key;
                    newRow.HeaderCell.Value = pair.Value.caption;
                    if (pair.Value.showtype == "0")//时段数据
                    {
                        dgvVitalSigns.Rows.Add(newRow);
                    }
                    else //天数据
                    {
                        dgvVitalSigns1.Rows.Add(newRow);
                    }
                }
                dgvVitalSigns1.Height = m_VitalSignsCaption.Count * 18;
                panel1.Height = dgvVitalSigns1.Height + dgvVitalSigns.Height;
                //dgvVitalSigns1.Location = new Point(dgvVitalSigns.Top + dgvVitalSigns.Height);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 根据列名字段名称得到时段数据的值
        /// </summary>
        /// <param name="fieldName">列名</param>
        /// <param name="index">时段序号</param>
        /// <returns></returns>
        private string GetDataFieldValue(string fieldName, int index)
        {
            try
            {
                string value = null;
                if (m_VitalSignsCaption[fieldName].showtype == "0")//时段数据
                {
                    foreach (DataGridViewRow dgvr in dgvVitalSigns.Rows)
                    {
                        if (dgvr.Tag != null)
                        {
                            if (dgvr.Tag.ToString().Equals(fieldName))
                            {
                                value = dgvr.Cells[index].Value == null ? "" : dgvr.Cells[index].Value.ToString();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow dgvr in dgvVitalSigns1.Rows)
                    {
                        if (dgvr.Tag != null)
                        {
                            if (dgvr.Tag.ToString().Equals(fieldName))
                            {
                                if (index == 0)
                                {

                                    value = dgvr.Cells[index].Value == null ? "" : dgvr.Cells[index].Value.ToString();
                                }
                                else
                                {
                                    value = "";
                                }
                                break;
                            }
                        }
                    }
                }
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化用行
        /// </summary>
        private void InitDataGridViewForVitalSigns()
        {
            DataGridViewRow newRow;
            newRow = new DataGridViewRow();
            newRow.HeaderCell.Value = "体温(℃)";
            dgvVitalSigns.Rows.Add(newRow);

            newRow = new DataGridViewRow();
            newRow.HeaderCell.Value = "脉搏(次/分)";
            dgvVitalSigns.Rows.Add(newRow);

            newRow = new DataGridViewRow();
            newRow.HeaderCell.Value = "呼吸(次/分)";
            dgvVitalSigns.Rows.Add(newRow);

            newRow = new DataGridViewRow();
            newRow.HeaderCell.Value = "心率(次/分)";
            dgvVitalSigns.Rows.Add(newRow);



            newRow = new DataGridViewRow();
            newRow.HeaderCell.Value = "物理降温(℃)";
            dgvVitalSigns.Rows.Add(newRow);

            newRow = new DataGridViewRow();
            newRow.HeaderCell.Value = "物理升温(℃)";
            dgvVitalSigns.Rows.Add(newRow);

            //dgvVitalSigns.Rows[0].Cells[0].Selected = false;
            //dgvVitalSigns.ClearSelection();
            //m_currentCell = null;

            m_UCTemperatureEditor[0] = ucTemperatureEditor1;
            m_UCTemperatureEditor[1] = ucTemperatureEditor2;
            m_UCTemperatureEditor[2] = ucTemperatureEditor3;
            m_UCTemperatureEditor[3] = ucTemperatureEditor4;
            m_UCTemperatureEditor[4] = ucTemperatureEditor5;
            m_UCTemperatureEditor[5] = ucTemperatureEditor6;
        }

        /// <summary>
        /// 初始化温度编辑控件
        /// </summary>
        public void InitControlPosition()
        {
            for (int i = 0; i < m_UCTemperatureEditor.Length; i++)
            {
                Rectangle rec = dgvVitalSigns.GetCellDisplayRectangle(i, 0, true);
                m_UCTemperatureEditor[i].Left = rec.Left;
                m_UCTemperatureEditor[i].Top = rec.Top;
                m_UCTemperatureEditor[i].Width = rec.Width;
                m_UCTemperatureEditor[i].Height = rec.Height;
                ucTemperatureEditor1.Visible = true;
            }
        }

        #region 初始化时间段标签
        /// <summary>
        /// 初始化时间段标签
        /// </summary>
        private void InitLabelTime()
        {
            XmlDocument xmlDoc = new XmlDocument();
            //edit zyx 2012-12-28 
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + ConfigInfo.configName))
            {
                MessageBox.Show("Config配置文件不存在");
                return;
            }
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + ConfigInfo.configName);
            XmlNode nodeElement = xmlDoc.GetElementsByTagName("HourOfday")[0];
            XmlNodeList nodeList = nodeElement.ChildNodes;
            m_TimeSlot = new string[nodeList.Count];
            for (int i = 0; i < nodeList.Count; i++)
            {
                m_TimeSlot[i] = nodeList[i].Attributes["timeslotvalue"] == null ? "" : nodeList[i].Attributes["timeslotvalue"].Value;
                dgvVitalSigns.Columns[i].HeaderText = m_TimeSlot[i];
            }
        }
        #endregion

        #region 初始化体温测量方式
        /// <summary>
        /// 初始化体温测量方式
        /// </summary>
        private void InitlookUpWayOfSurvey(LookUpEdit lookUpEdit)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = "13";
            sqlParam[1].Value = "0";
            sqlParam[2].Value = "88";

            dtWayOFSurvey = MethodSet.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);
            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);
            MethodSet.GetDictionarydetail(lookUpEdit, dt);
            lookUpEdit.EditValue = "8801";//默认选择腋温 edit by ywk
            //obj.Text = "腋温";
        }


        private string FilterValue(string keyName, string keyValue, string resultName)
        {
            foreach (DataRow dr in dtWayOFSurvey.Rows)
            {
                if (dr[keyName].ToString().Equals(keyValue))
                    return dr[resultName].ToString();
            }
            return "8800";
        }

        #endregion

        private void UCNursingRecordTable_Load(object sender, EventArgs e)
        {
            try
            {
                //初始化时间段标签
                InitLabelTime();
                //初始化体温测量方式
                for (int i = 0; i < m_UCTemperatureEditor.Length; i++)
                    InitlookUpWayOfSurvey(m_UCTemperatureEditor[i].LookUpWayOfSurvey);
                InitLabelTime();
                InitControlPosition();
                ucTemperatureEditor1.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 属性集
        /// <summary>
        /// 护理信息允许修改的天数
        /// </summary>
        //public int DayOfModify
        //{
        //    get { return m_DayOfModify; }
        //}
        #endregion

        #region 保存护理信息数据

        /// <summary>
        /// 格式化Null值的单元格
        /// </summary>
        private void FormateNullValueCell()
        {
            for (int i = 0; i < dgvVitalSigns.Columns.Count; i++)
            {
                for (int j = 1; j < dgvVitalSigns.Rows.Count; j++)
                {
                    if (dgvVitalSigns[i, j].Value == null)
                        dgvVitalSigns[i, j].Value = "";
                }
            }


        }
        /// <summary>
        /// 保存护理信息数据
        /// edit by ywk 2012年5月29日11:44:21
        /// </summary>
        /// <param name="DateOfSurvey">测量日期</param>
        ///  <param name="m_noofinpat">病人首页序号</param>
        public bool SaveUCNursingRecordTable(string DateOfSurvey, string m_noofinpat)
        {
            try
            {
                FormateNullValueCell();
                if (!IsDataValid()) return false;
                string myNoOfInpat = string.Empty;//首页序号
                if (string.IsNullOrEmpty(m_noofinpat))//没切换患者
                {
                    myNoOfInpat = MethodSet.CurrentInPatient.NoOfFirstPage.ToString();
                }
                else
                {
                    myNoOfInpat = m_noofinpat;
                }


                //-------------------数据保存-----------------------
                StringBuilder sqlStr = new StringBuilder();
                StringBuilder keyValues = new StringBuilder();
                for (int i = 0; i < dgvVitalSigns.Columns.Count; i++)
                {
                    //sqlStr.Append(@" delete from notesonnursing WHERE dateofsurvey = '" + DateOfSurvey.Replace("'", "''") + "' AND timeslot ='" + dgvVitalSigns.Columns[i].HeaderText.Replace("'", "''") + "' and noofinpat='" + myNoOfInpat.Replace("'", "''") + "'");
                    sqlStr.Append(@" select * from notesonnursing WHERE dateofsurvey = '" + DateOfSurvey.Replace("'", "''") + "' AND timeslot ='" + dgvVitalSigns.Columns[i].HeaderText.Replace("'", "''") + "' and noofinpat='" + myNoOfInpat.Replace("'", "''") + "'");
                    DataTable dtNotesonnursing = MethodSet.App.SqlHelper.ExecuteDataTable(sqlStr.ToString());
                    if (dtNotesonnursing != null && dtNotesonnursing.Rows.Count > 0)
                    {
                        sqlStr.Remove(0, sqlStr.Length);
                        int noteId = int.Parse(dtNotesonnursing.Rows[0]["ID"].ToString());
                        sqlStr.Append(@"update  notesonnursing set temperature=" + "'" + m_UCTemperatureEditor[i].TxtTemperature.Text + "'");
                        sqlStr.Append(@", wayofsurvey=" + "'" + m_UCTemperatureEditor[i].LookUpWayOfSurvey.EditValue.ToString() + "'");
                        sqlStr.Append(@", pulse=" + "'" + (dgvVitalSigns[i, 1].Value == null ? "" : dgvVitalSigns[i, 1].Value.ToString().Trim()) + "'");
                        sqlStr.Append(@", breathe=" + "'" + (dgvVitalSigns[i, 2].Value == null ? "" : dgvVitalSigns[i, 2].Value.ToString().Trim()) + "'");
                        sqlStr.Append(@", heartrate=" + "'" + (dgvVitalSigns[i, 3].Value == null ? "" : dgvVitalSigns[i, 3].Value.ToString().Trim()) + "'");
                        sqlStr.Append(@", DAYSAFTERSURGERY=" + "'" + txtDaysAfterSurgery1.Text + "'");
                        sqlStr.Append(@", DAYOFHOSPITAL=" + "'" + txtDayOfHospital1.Text + "'");
                        sqlStr.Append(@", PHYSICALCOOLING=" + "'" + (dgvVitalSigns[i, 4].Value == null ? "" : dgvVitalSigns[i, 4].Value.ToString().Trim()) + "'");
                        sqlStr.Append(@", PHYSICALHOTTING=" + "'" + (dgvVitalSigns[i, 5].Value == null ? "" : dgvVitalSigns[i, 5].Value.ToString().Trim()) + "'");
                        sqlStr.Append(@", DATEOFRECORD=to_char(sysdate,'yyyy-mm-dd HH24:MI:SS'),DOCTOROFRECORD=" + "'" + MethodSet.App.User.Id + "',");

                        foreach (KeyValuePair<string, VitalSignInfoEntity> kv in m_VitalSignsCaption)
                        {

                            keyValues.Append(kv.Key.ToLower() + "=" + "'" + GetDataFieldValue(kv.Key, i).Replace("[", "[[ ")
                                 .Replace("]", " ]]")
                                 .Replace("*", "[*]")
                                 .Replace("%", "[%]")
                                 .Replace("[[ ", "[[]")
                                 .Replace(" ]]", "[]]")
                                 .Replace("\'", "''") + "',");
                        }

                        sqlStr.Append(keyValues);
                        sqlStr.Remove(sqlStr.Length - 1, 1);//删除末尾","号
                        sqlStr.Append("where ID=" + noteId);
                        MethodSet.App.SqlHelper.ExecuteNoneQuery(sqlStr.ToString(), CommandType.Text);
                        sqlStr.Remove(0, sqlStr.Length);
                        keyValues.Remove(0, keyValues.Length);
                    }
                    else
                    {
                        sqlStr.Remove(0, sqlStr.Length);
                        sqlStr.Append(@"insert into notesonnursing(ID,noofinpat,dateofsurvey,temperature,wayofsurvey, pulse, breathe,heartrate, timeslot, DAYSAFTERSURGERY,DAYOFHOSPITAL,PHYSICALCOOLING,PHYSICALHOTTING,DATEOFRECORD,DOCTOROFRECORD,");
                        foreach (KeyValuePair<string, VitalSignInfoEntity> kv in m_VitalSignsCaption)
                        {
                            keyValues.Append(kv.Key.ToLower() + ",");
                        }
                        sqlStr.Append(keyValues);
                        sqlStr.Remove(sqlStr.Length - 1, 1);//删除末尾","号
                        sqlStr.Append(") values(seq_notesonnursing_id.NEXTVAL,");
                        sqlStr.Append("'" + myNoOfInpat + "',");
                        sqlStr.Append("'" + DateOfSurvey + "',");
                        sqlStr.Append("'" + m_UCTemperatureEditor[i].TxtTemperature.Text + "',");
                        sqlStr.Append("'" + m_UCTemperatureEditor[i].LookUpWayOfSurvey.EditValue.ToString() + "',");
                        sqlStr.Append("'" + (dgvVitalSigns[i, 1].Value == null ? "" : dgvVitalSigns[i, 1].Value.ToString().Trim()) + "',");
                        sqlStr.Append("'" + (dgvVitalSigns[i, 2].Value == null ? "" : dgvVitalSigns[i, 2].Value.ToString().Trim()) + "',");
                        sqlStr.Append("'" + (dgvVitalSigns[i, 3].Value == null ? "" : dgvVitalSigns[i, 3].Value.ToString().Trim()) + "',");
                        sqlStr.Append("'" + dgvVitalSigns.Columns[i].HeaderText + "',");
                        sqlStr.Append("'" + txtDaysAfterSurgery1.Text + "',");
                        sqlStr.Append("'" + txtDayOfHospital1.Text + "',");
                        sqlStr.Append("'" + (dgvVitalSigns[i, 4].Value == null ? "" : dgvVitalSigns[i, 4].Value.ToString().Trim()) + "',");
                        sqlStr.Append("'" + (dgvVitalSigns[i, 5].Value == null ? "" : dgvVitalSigns[i, 5].Value.ToString().Trim()) + "',");
                        sqlStr.Append("to_char(sysdate,'yyyy-mm-dd HH24:MI:SS'),'" + MethodSet.App.User.Id + "',");
                        keyValues.Remove(0, keyValues.Length);
                        foreach (KeyValuePair<string, VitalSignInfoEntity> kv in m_VitalSignsCaption)
                        {
                            if (GetDataFieldValue(kv.Key, i).ToString().Trim().Contains("'"))
                            {
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("输入内容不能包含特殊字符“'”");
                                return false;
                            }
                            else
                            {
                                //校验数据式显示格式
                                keyValues.Append("'" + GetDataFieldValue(kv.Key, i) + "',");
                            }

                        }
                        sqlStr.Append(keyValues);
                        sqlStr.Remove(sqlStr.Length - 1, 1);//删除末尾","号
                        sqlStr.Append(")");
                        MethodSet.App.SqlHelper.ExecuteNoneQuery(sqlStr.ToString(), CommandType.Text);
                        sqlStr.Remove(0, sqlStr.Length);
                        keyValues.Remove(0, keyValues.Length);
                    }
                }
                MessageBox.Show("保存成功!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 检查数据是否有效
        /// <summary>
        /// 检查数据是否有效
        /// </summary>
        /// <returns></returns>
        private bool IsDataValid()
        {
            int intVart;
            dgvVitalSigns.ClearSelection();
            for (int i = 0; i < 6; i++)
            {
                string strTimeSlot = "时间段为【" + dgvVitalSigns.Columns[i].HeaderText.Trim() + "】的记录：\n";

                //体温
                if (m_UCTemperatureEditor[i].TxtTemperature.Text.Trim() != "")
                {
                    if (!Dataprocessing.IsNumber(m_UCTemperatureEditor[i].TxtTemperature.Text.Trim().ToString(), 1))
                    {
                        MessageBox.Show(strTimeSlot + "体温必须为数字，小数点保留一位!");
                        return false;
                    }
                    else
                    {
                        float dVar = Convert.ToSingle(m_UCTemperatureEditor[i].TxtTemperature.Text.Trim() == "" ? "0" : m_UCTemperatureEditor[i].TxtTemperature.Text.Trim());
                        if (!(dVar >= ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].minValue && dVar <= ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].maxValue))
                        {
                            MessageBox.Show(strTimeSlot + "体温超出范围：" + ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].minValue.ToString() + "至" + ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].maxValue.ToString() + "!");
                            return false;
                        }
                    }
                }

                //脉搏
                if (dgvVitalSigns[i, 1].Value != null && dgvVitalSigns[i, 1].Value.ToString().Trim() != "")
                {
                    if (!Dataprocessing.IsNumber(dgvVitalSigns[i, 1].Value.ToString().Trim().ToString(), 0))
                    {
                        dgvVitalSigns[i, 1].Selected = true;
                        MessageBox.Show(strTimeSlot + "脉搏必须为不带小数的数字!");
                        return false;
                    }
                    else
                    {
                        intVart = Convert.ToInt32(dgvVitalSigns[i, 1].Value.ToString().Trim().ToString());
                        if (!(intVart >= ConfigInfo.dicVerticalCoordinate[DataLoader.PULSE].minValue && intVart <= ConfigInfo.dicVerticalCoordinate[DataLoader.PULSE].maxValue))
                        {
                            dgvVitalSigns[i, 1].Selected = true;
                            MessageBox.Show(strTimeSlot + "脉搏超出范围：" + ConfigInfo.dicVerticalCoordinate[DataLoader.PULSE].minValue.ToString() + "至" + ConfigInfo.dicVerticalCoordinate[DataLoader.PULSE].maxValue.ToString() + "!");
                            return false;
                        }
                    }
                }

                //心率
                if (dgvVitalSigns[i, 3].Value != null && dgvVitalSigns[i, 3].Value.ToString().Trim().ToString() != "")
                {
                    if (!Dataprocessing.IsNumber(dgvVitalSigns[i, 3].Value.ToString().Trim().ToString(), 0))
                    {
                        dgvVitalSigns[i, 3].Selected = true;
                        MessageBox.Show(strTimeSlot + "心率必须为不带小数的数字!");

                        return false;
                    }
                    else
                    {
                        intVart = Convert.ToInt32(dgvVitalSigns[i, 3].Value.ToString().Trim().ToString());
                        if (!(intVart >= ConfigInfo.dicVerticalCoordinate[DataLoader.HEARTRATE].minValue && intVart <= ConfigInfo.dicVerticalCoordinate[DataLoader.HEARTRATE].maxValue))
                        {
                            dgvVitalSigns[i, 3].Selected = true;
                            MessageBox.Show(strTimeSlot + "心率超出范围：" + ConfigInfo.dicVerticalCoordinate[DataLoader.HEARTRATE].minValue.ToString() + "至" + ConfigInfo.dicVerticalCoordinate[DataLoader.HEARTRATE].maxValue.ToString() + "!");

                            return false;
                        }
                    }
                }

                //呼吸
                if (dgvVitalSigns[i, 2].Value != null && dgvVitalSigns[i, 2].Value.ToString().Trim().ToString() != "")
                {
                    if (!Dataprocessing.IsNumber(dgvVitalSigns[i, 2].Value.ToString().Trim().ToString(), 0) && !dgvVitalSigns[i, 2].Value.ToString().Trim().ToLower().Equals("r"))
                    {
                        dgvVitalSigns[i, 2].Selected = true;
                        MessageBox.Show(strTimeSlot + "呼吸必须为不带小数的数字或r/R!");
                        return false;
                    }
                    else
                    {
                        if (!dgvVitalSigns[i, 2].Value.ToString().Trim().ToLower().Equals("r"))
                        {
                            intVart = Convert.ToInt32(dgvVitalSigns[i, 2].Value.ToString().Trim().ToString());
                            if (!(intVart >= ConfigInfo.dicVerticalCoordinate[DataLoader.BREATHE].minValue && intVart <= ConfigInfo.dicVerticalCoordinate[DataLoader.BREATHE].maxValue))
                            {
                                dgvVitalSigns[i, 2].Selected = true;
                                MessageBox.Show(strTimeSlot + "呼吸超出范围：" + ConfigInfo.dicVerticalCoordinate[DataLoader.BREATHE].minValue.ToString() + "至" + ConfigInfo.dicVerticalCoordinate[DataLoader.BREATHE].maxValue.ToString() + "!");
                                return false;
                            }
                        }

                    }
                }

                //物理降温的限制验证 
                //add by ywk 2012年4月17日16:49:30
                if (dgvVitalSigns[i, 4].Value != null && dgvVitalSigns[i, 4].Value.ToString().Trim() != "")
                {
                    if (!Dataprocessing.IsNumber(dgvVitalSigns[i, 4].Value.ToString().Trim().ToString(), 1))
                    {
                        dgvVitalSigns[i, 4].Selected = true;
                        MessageBox.Show(strTimeSlot + "物理降温必须为数字，小数点保留一位!");
                        return false;
                    }
                    else
                    {
                        float dVar = Convert.ToSingle(dgvVitalSigns[i, 4].Value.ToString().Trim());
                        if (!(dVar >= ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].minValue && dVar <= ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].maxValue))
                        {
                            dgvVitalSigns[i, 4].Selected = true;
                            MessageBox.Show(strTimeSlot + "物理降温超出范围：" + ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].minValue.ToString() + "至" + ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].maxValue.ToString() + "!");
                            return false;
                        }
                    }
                }

                //物理降温不能大于当前时间点的测试温度 edit by ywk 
                //体温为空，能填写物理降温？此处先限制住
                if (string.IsNullOrEmpty(m_UCTemperatureEditor[i].TxtTemperature.Text.ToString().Trim()) && !string.IsNullOrEmpty(dgvVitalSigns[i, 4].Value.ToString().Trim()))
                {
                    MessageBox.Show(strTimeSlot + "请先输入此时的体温");
                    return false;
                }
                ////string temperature = m_txtTemperatureList[i].Text.Trim();
                //if (Convert.ToDouble(string.IsNullOrEmpty(dgvVitalSigns[i, 4].Value.ToString().Trim()) ? "0" : dgvVitalSigns[i, 4].Value.ToString().Trim()) > Convert.ToDouble(string.IsNullOrEmpty(m_UCTemperatureEditor[i].TxtTemperature.Text.Trim()) ? "0" : m_UCTemperatureEditor[i].TxtTemperature.Text))
                //{
                //    MessageBox.Show(strTimeSlot + "物理降温不能超出此时的体温");
                //    return false;
                //}

                //物理升温的限制验证 
                //add by ywk 2012年4月17日16:49:30
                if (dgvVitalSigns[i, 5].Value != null && dgvVitalSigns[i, 5].Value.ToString().Trim() != "")
                {
                    if (!Dataprocessing.IsNumber(dgvVitalSigns[i, 5].Value.ToString().Trim().ToString(), 1))
                    {
                        MessageBox.Show(strTimeSlot + "物理升温必须为数字，小数点保留一位!");
                        return false;
                    }
                    else
                    {
                        float dVar = Convert.ToSingle(dgvVitalSigns[i, 5].Value.ToString().Trim());
                        if (!(dVar >= ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].minValue && dVar <= ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].maxValue))
                        {
                            MessageBox.Show(strTimeSlot + "物理升温超出范围：" + ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].minValue.ToString() + "至" + ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].maxValue.ToString() + "!");
                            return false;
                        }
                    }
                }

                //if (dgvVitalSigns[i, 5].Value != null && dgvVitalSigns[i, 5].Value.ToString().Trim() != "")
                //{
                //    if (Convert.ToDouble(string.IsNullOrEmpty(dgvVitalSigns[i, 5].Value.ToString().Trim()) ? "0" : dgvVitalSigns[i, 5].Value.ToString().Trim()) < Convert.ToDouble(string.IsNullOrEmpty(m_UCTemperatureEditor[i].TxtTemperature.Text.Trim()) ? "0" : m_UCTemperatureEditor[i].TxtTemperature.Text))
                //    {
                //        MessageBox.Show(strTimeSlot + "物理升温不能低于此时的体温");
                //        return false;
                //    }
                //}
                //物理升温不能小于于当前时间点的测试温度 edit by ywk 
                //体温为空，能填写物理升温？此处先限制住
                if (string.IsNullOrEmpty(m_UCTemperatureEditor[i].TxtTemperature.Text) && !string.IsNullOrEmpty(dgvVitalSigns[i, 5].Value.ToString().Trim()))
                {
                    MessageBox.Show(strTimeSlot + "请先输入此时的体温");
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 获取存储数据
        /// <summary>
        /// 获取存储数据
        /// </summary>
        /// <param name="index">控件数组索引</param>
        /// <param name="DateOfSurvey">测量日期</param>
        /// 新增noofinpat（首页序号）ywk 
        /// <returns></returns>
        private SqlParameter[] GetSqlParam(int index, string DateOfSurvey, string noofinpat)
        {
            SqlParameter[] sqlParam = new SqlParameter[]
            {
                new SqlParameter("@NoOfInpat",SqlDbType.VarChar),//病案号---0
                new SqlParameter("@DateOfSurvey",SqlDbType.VarChar),//测量日期---1
                new SqlParameter("@Temperature",SqlDbType.VarChar),//体温---2
                new SqlParameter("@WayOfSurvey",SqlDbType.Int),//体温类别---3
                new SqlParameter("@Pulse",SqlDbType.VarChar),//脉搏---4
                new SqlParameter("@HeartRate",SqlDbType.VarChar),//心率---5
                new SqlParameter("@Breathe",SqlDbType.VarChar),//呼吸---6
                new SqlParameter("@PhysicalCooling",SqlDbType.VarChar),//物理降温---7
                 //新增物理升温
                new SqlParameter("@PhysicalHotting",SqlDbType.VarChar),//温度变化---8
                                new SqlParameter("@DoctorOfRecord",SqlDbType.VarChar),//记录人---9
                new SqlParameter("@TimeSlot",SqlDbType.VarChar),//时间点---10
            };
            sqlParam[0].Value = noofinpat;// MethodSet.CurrentInPatient.NoOfFirstPage;
            sqlParam[1].Value = DateOfSurvey;
            //sqlParam[2].Value = dgvVitalSigns[index, 0].Value.ToString().Trim().Split(':')[0];
            //sqlParam[3].Value = Int32.Parse(FilterValue("name", dgvVitalSigns[index, 0].Value.ToString().Trim().Split(':')[1],"detailid"));
            sqlParam[2].Value = m_UCTemperatureEditor[index].TxtTemperature.Text;
            sqlParam[3].Value = m_UCTemperatureEditor[index].LookUpWayOfSurvey.EditValue.ToString();
            sqlParam[4].Value = dgvVitalSigns[index, 1].Value == null ? "" : dgvVitalSigns[index, 1].Value.ToString().Trim();
            sqlParam[5].Value = dgvVitalSigns[index, 2].Value == null ? "" : dgvVitalSigns[index, 2].Value.ToString().Trim();
            sqlParam[6].Value = dgvVitalSigns[index, 3].Value == null ? "" : dgvVitalSigns[index, 3].Value.ToString().Trim();
            sqlParam[7].Value = dgvVitalSigns[index, 4].Value == null ? "" : dgvVitalSigns[index, 4].Value.ToString().Trim();
            sqlParam[8].Value = dgvVitalSigns[index, 5].Value == null ? "" : dgvVitalSigns[index, 5].Value.ToString().Trim();
            sqlParam[9].Value = MethodSet.App.User.Id;
            sqlParam[10].Value = dgvVitalSigns.Columns[index].HeaderText;

            return sqlParam;
        }
        #endregion


        #region 获取指定日期对应的护理信息数据
        /// <summary>
        /// 获取指定日期对应的护理信息数据
        /// edit by ywk 2012年5月29日11:17:24
        /// </summary>
        /// <param name="DateOfSurvey"></param>
        ///  <param name="m_oofinpat">新增病人首页序号字段</param>
        public void GetNotesOfNursingInfo(string DateOfSurvey, string m_oofinpat)
        {
            //清除控件内容
            ClearControlValue();
            InitTextBox();
            //获取指定日期数据
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@DateOfSurvey", SqlDbType.VarChar)
            };
            string noofinpat = string.Empty;//病人首页序号
            if (string.IsNullOrEmpty(m_oofinpat))//如果为空，表示没切换患者
            {
                noofinpat = MethodSet.CurrentInPatient.NoOfFirstPage.ToString();
            }
            else
            {
                noofinpat = m_oofinpat;
            }
            sqlParam[0].Value = noofinpat;
            sqlParam[1].Value = DateOfSurvey;

            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_GetNotesOnNursingInfo", sqlParam, CommandType.StoredProcedure);

            //控件赋值

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int index = GetIndex(dt.Rows[i]["TimeSlot"].ToString());
                    if (index > -1)
                    {
                        //---------------------------固定列数据------------------------------------------
                        m_UCTemperatureEditor[index].Value = dt.Rows[i]["Temperature"].ToString() + ":" + FilterValue("detailid", dt.Rows[i]["WayOfSurvey"].ToString(), "name");
                        dgvVitalSigns[index, 1].Value = dt.Rows[i]["Pulse"].ToString();
                        dgvVitalSigns[index, 2].Value = dt.Rows[i]["Breathe"].ToString();
                        dgvVitalSigns[index, 3].Value = dt.Rows[i]["HeartRate"].ToString();

                        dgvVitalSigns[index, 4].Value = dt.Rows[i]["PhysicalCooling"].ToString();
                        dgvVitalSigns[index, 5].Value = dt.Rows[i]["PhysicalHotting"].ToString();
                        //---------------------------以下是时段数据------------------------------------------
                        foreach (DataGridViewRow dgvr in dgvVitalSigns.Rows)
                        {
                            if (dgvr.Tag != null)
                            {
                                if (dt.Rows[i][dgvr.Tag.ToString()].ToString() != "")
                                    dgvr.Cells[index].Value = dt.Rows[i][dgvr.Tag.ToString()].ToString();
                            }
                        }
                        //---------------------------以下是天数据------------------------------------------
                        foreach (DataGridViewRow dgvr in dgvVitalSigns1.Rows)
                        {
                            if (dgvr.Tag != null)
                            {
                                if (dt.Rows[i][dgvr.Tag.ToString()].ToString() != "")
                                    dgvr.Cells[0].Value = dt.Rows[i][dgvr.Tag.ToString()].ToString();
                            }
                        }
                    }
                }

            }
            //住院天数
            //if (txtDayOfHospital1.Text.Trim() == "" && Convert.ToDateTime(DateOfSurvey) == Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")))
            // {
            DateTime AdmitDate;
            if (ConfigInfo.isAdminDate)
            {
                AdmitDate = Convert.ToDateTime(Convert.ToDateTime(DataLoader.adminDate).Date.ToString("yyyy-MM-dd 00:00:01"));
            }
            else
            {
                AdmitDate = Convert.ToDateTime(Convert.ToDateTime(DataLoader.m_InTime).Date.ToString("yyyy-MM-dd 00:00:01"));
            }
            DateTime CurrDate = Convert.ToDateTime(DateTime.Parse(DateOfSurvey).ToString("yyyy-MM-dd 00:00:01"));
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["DAYOFHOSPITAL"].ToString().Trim() != "")
            {
                txtDayOfHospital1.Text = dt.Rows[0]["DAYOFHOSPITAL"].ToString().Trim();
            }
            else
            {
                txtDayOfHospital1.Text = (CurrDate.Subtract(AdmitDate).Days + 1).ToString();
            }
            //  }
            //手术后日数
            //if (txtDaysAfterSurgery1.Text.Trim() == "" && Convert.ToDateTime(DateOfSurvey) == Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")))
            //   if (ConfigInfo.editable == 1)
            {
                //    ucNursingRecordTable1.SetDaysAfterSurgery(dateEdit.Text, NoOfInpat);//zyx 解决手术后天数不显示问题
                //}
                //else 
                //{
                //    ucNursingRecordTable1.GetDaysAfterSurgery(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat);
            }
            //if (ConfigInfo.editable == 1)
            //{
            //    //SetDaysAfterSurgery(MethodSet.DaysAfterSurgery);
            //    SetDaysAfterSurgery(DateOfSurvey, m_oofinpat);
            //}
            //else
            //{
            //    if (dt != null && dt.Rows != null && dt.Rows.Count > 0) 
            //    {
            //        txtDaysAfterSurgery1.Text = dt.Rows[0]["DAYSAFTERSURGERY"] == null ? "" : dt.Rows[0]["DAYSAFTERSURGERY"].ToString();
            //    }else
            //    {
            //        SetDaysAfterSurgery(DateOfSurvey, m_oofinpat);
            //    }
            //}
            GetDaysAfterSurgery(DateOfSurvey, m_oofinpat);
            // }
        }

        public void GetDaysAfterSurgery(string dateOfSurvey, string _noofinpat)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@DateOfSurvey", SqlDbType.VarChar)
            };
                string noofinpat = string.Empty;//病人首页序号
                if (string.IsNullOrEmpty(_noofinpat))//如果为空，表示没切换患者
                {
                    noofinpat = MethodSet.CurrentInPatient.NoOfFirstPage.ToString();
                }
                else
                {
                    noofinpat = _noofinpat;
                }
                sqlParam[0].Value = noofinpat;
                sqlParam[1].Value = dateOfSurvey;

                DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_GetNotesOnNursingInfo", sqlParam, CommandType.StoredProcedure);
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0 && dt.Rows[0]["DAYSAFTERSURGERY"].ToString().Trim() != "")
                {
                    txtDaysAfterSurgery1.Text = dt.Rows[0]["DAYSAFTERSURGERY"] == null ? "" : dt.Rows[0]["DAYSAFTERSURGERY"].ToString();
                }
                else
                {
                    SetDaysAfterSurgery(dateOfSurvey, _noofinpat);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取时间段对应的下标索引
        /// <summary>
        /// 获取时间段对应的下标索引
        /// </summary>
        /// <param name="TimeSlot">时间段值</param>
        /// <returns></returns>
        private int GetIndex(string TimeSlot)
        {
            for (int i = 0; i < m_TimeSlot.Length; i++)
            {
                if (m_TimeSlot[i].Equals(TimeSlot))
                    return i;
            }

            return -1;
        }
        #endregion

        #region 清除控件内容
        /// <summary>
        /// 清除控件内容
        /// </summary>
        private void ClearControlValue()
        {
            for (int i = 0; i < dgvVitalSigns.Columns.Count; i++)
            {
                for (int j = 0; j < dgvVitalSigns.Rows.Count; j++)
                {
                    dgvVitalSigns[i, j].Value = "";
                    if (j == 0)
                    {
                        m_UCTemperatureEditor[i].LookUpWayOfSurvey.EditValue = "8801";
                        m_UCTemperatureEditor[i].TxtTemperature.EditValue = "";
                    }
                }
            }
            for (int i = 0; i < dgvVitalSigns1.Columns.Count; i++)
            {
                for (int j = 0; j < dgvVitalSigns1.Rows.Count; j++)
                {
                    dgvVitalSigns1[i, j].Value = "";
                }
            }
        }
        #endregion

        #region 设置手术后天数
        /// <summary>
        /// 设置手术后天数
        /// </summary>
        /// <param name="DaysAfterSurgery">手术后天数数组</param>
        public void SetDaysAfterSurgery(string[] DaysAfterSurgery)
        {
            if (txtDaysAfterSurgery1.Text.Trim() == "" && DaysAfterSurgery != null)
            {
                string strDaysAfterSurgery = "";

                DateTime dateTime = DateTime.Now.Date;

                for (int i = 0; i < DaysAfterSurgery.Length; i++)
                {
                    int days = dateTime.Subtract(Convert.ToDateTime(DaysAfterSurgery[i]).Date).Days;
                    if (days <= 14)
                    {
                        strDaysAfterSurgery = days.ToString() + "/" + strDaysAfterSurgery;
                    }
                }

                if (!string.IsNullOrEmpty(strDaysAfterSurgery))
                    strDaysAfterSurgery = strDaysAfterSurgery.Substring(0, strDaysAfterSurgery.Length - 1);
                txtDaysAfterSurgery1.Text = strDaysAfterSurgery;
            }
        }
        #endregion

        /// <summary>
        /// /根据选择的日期，改变住院天数的值
        /// edit by ywk 二〇一二年五月十八日 10:16:36
        /// </summary>
        /// <param name="inputdate"></param>
        internal void SetDayInHospital(string inputdate)
        {
            DateTime AdmitDate;
            if (ConfigInfo.isAdminDate)
            {
                AdmitDate = Convert.ToDateTime(Convert.ToDateTime(DataLoader.adminDate).Date.ToString("yyyy-MM-dd 00:00:01"));
            }
            else
            {
                AdmitDate = Convert.ToDateTime(Convert.ToDateTime(DataLoader.m_InTime).Date.ToString("yyyy-MM-dd 00:00:01"));

            }
            DateTime CurrDate = Convert.ToDateTime(inputdate + "  00:00:01");

            txtDayOfHospital1.EditValue = (CurrDate.Subtract(AdmitDate).Days + 1).ToString();
        }

        #region 对于手术后天数的处理 add  by ywk
        /// <summary>
        /// 设置手术后天数 edit by ywk
        /// </summary>
        internal void SetDaysAfterSurgery(string inputdate, string patid)
        {
            //DateOfSurvey 当前录入的日期时间 
            //string serachsql = string.Format(@"    select * from patientstatus where noofinpat='{0}' and trunc(to_date(patientstatus.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 and dotime<'{1}'  and ccode='7000' order by dotime asc", patid, inputdate + " 23:59:59");
            //泗縣需求 分娩手術走同樣邏輯 add by ywk 2013年6月14日 11:41:27
            //            string serachsql = string.Format
            //                (@"    select * from patientstatus  p left join THREE_CHECK_EVENT t on p.ccode=t.id  where noofinpat='{0}' 
            //and trunc(to_date(p.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 
            //and dotime<'{1}'  and (p.ccode='7000' or t.name ='分娩·手术') order by dotime asc", patid, inputdate + " 23:59:59");

            //xll 2013-08-08 
            string serachsql = string.Format
               (@"    select * from patientstatus  p left join THREE_CHECK_EVENT t on p.ccode=t.id  where noofinpat='{0}' 
and trunc(to_date(p.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 
and dotime<'{1}'  and t.name like '%手术%' order by dotime asc", patid, inputdate + " 23:59:59");


            //string serachsql = string.Format(@"    select * from patientstatus where noofinpat='{0}' and trunc(to_date(patientstatus.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 and dotime<'{1}'  and ccode='7000' order by dotime asc", MethodSet.CurrentInPatient.Code, inputdate + " 23:59:59");
            DataTable AllSurgeryData = MethodSet.App.SqlHelper.ExecuteDataTable(serachsql, CommandType.Text);

            if (AllSurgeryData.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(inputdate))
                {
                    DateTime nowInputDate = DateTime.Parse(inputdate + " 23:59:59");

                    if (AllSurgeryData.Rows.Count > 0)//有一次以上的手术 
                    {
                        string content = ReturnCotent(nowInputDate, AllSurgeryData);
                        txtDaysAfterSurgery1.EditValue = content;
                    }
                }
            }
            else
            {
                txtDaysAfterSurgery1.EditValue = "";
            }
        }

        /// <summary>
        /// 对于一次以上的手术返回要显示的内容
        /// ywk
        /// </summary>
        /// <param name="count"></param>
        /// <param name="nowinputtime"></param>
        /// <returns></returns>
        private string ReturnCotent(DateTime nowinputtime, DataTable dtdata)
        {
            string mycontent = string.Empty;
            string outcontent = string.Empty;
            for (int i = 0; i < dtdata.Rows.Count; i++)
            {
                mycontent = (nowinputtime - DateTime.Parse(dtdata.Rows[i]["dotime"].ToString())).Days.ToString();
                if (mycontent == "0")
                {
                    outcontent = ReturnCount((i + 1).ToString());
                }
                else if (i == (dtdata.Rows.Count - 1))
                {
                    outcontent = (nowinputtime - DateTime.Parse(dtdata.Rows[i]["dotime"].ToString())).Days.ToString();
                }

            }
            return outcontent;
        }
        /// <summary>
        /// 处理手术后第几次的显示内容
        /// </summary>
        /// <param name="showcount"></param>
        /// <returns></returns>
        private string ReturnCount(string showcount)
        {
            string show = string.Empty;
            switch (showcount)
            {
                case "1":
                    show = "";
                    break;
                case "2":
                    show = "II-0";
                    break;
                case "3":
                    show = "III-0";
                    break;
                case "4":
                    show = "IV-0";
                    break;
                case "5":
                    show = "V-0";
                    break;
                case "6":
                    show = "VI-0";
                    break;
                case "7":
                    show = "VII-0";
                    break;
                case "8":
                    show = "VIII-0";
                    break;
                case "9":
                    show = "IX-0";
                    break;
                case "10":
                    show = "X-0";
                    break;


                default:
                    break;
            }
            return show;
        }

        #endregion

        private void dgvVitalSigns1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridViewRow dr = dgvVitalSigns1.Rows[e.RowIndex];
                VitalSignInfoEntity tagEntity = m_VitalSignsCaption[dr.Tag.ToString()];
                if (tagEntity != null && tagEntity.datafield != "")
                {
                    if (tagEntity.cellCtlType == CellControlType.ShowDlg) //需要弹出对话框编辑
                    {
                        e.Cancel = true;
                        string str = string.Empty;
                        if (dgvVitalSigns1[dgvVitalSigns1.CurrentCell.ColumnIndex, dgvVitalSigns1.CurrentCell.RowIndex].Value != null)
                        {
                            str = dgvVitalSigns1[dgvVitalSigns1.CurrentCell.ColumnIndex, dgvVitalSigns1.CurrentCell.RowIndex].Value.ToString();
                        }
                        else
                        {
                            str = "";
                        }
                        //if (obj == null)
                        //{
                        obj = (Form)Activator.CreateInstance(Type.GetType(tagEntity.showForm), str);
                        obj.StartPosition = FormStartPosition.CenterScreen;
                        DialogResult result = obj.ShowDialog();
                        if (result != DialogResult.Cancel)
                        {
                            dgvVitalSigns1[dgvVitalSigns1.CurrentCell.ColumnIndex, dgvVitalSigns1.CurrentCell.RowIndex].Value = ((IDlg)obj).EditValue;
                        }
                        obj.Dispose();
                        //}
                        //else 
                        // {
                        //obj = null;
                        // }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 根据Config配置来加载界面
        /// </summary>
        public void RefreshFormControls(string curPat)
        {
            try
            {
                string path = ConfigInfo.GetXMLPath(curPat);
                if (!File.Exists(path))
                {
                    MessageBox.Show("Config配置文件不存在");
                    return;
                }

                xmlDoc.Load(path);

                dgvVitalSigns.Rows.Clear();
                dgvVitalSigns1.Rows.Clear();
                InitDataGridViewForVitalSigns();
                m_VitalSignsCaption.Clear();

                XmlNode nodeElement = xmlDoc.GetElementsByTagName("DataRowEdit")[0];
                XmlNodeList nodeList = nodeElement.ChildNodes;


                foreach (XmlNode node in nodeList)
                {
                    VitalSignInfoEntity obj = new VitalSignInfoEntity();
                    obj.caption = node.InnerText;
                    obj.datafield = node.Attributes["datafield"] == null ? "" : node.Attributes["datafield"].Value;
                    obj.cellCtlType = (CellControlType)Enum.Parse(typeof(CellControlType), node.Attributes["cellControl"] == null ? "1" : node.Attributes["cellControl"].Value);
                    obj.showForm = node.Attributes["showDlg"] == null ? "" : node.Attributes["showDlg"].Value;
                    obj.datasource = null;
                    obj.showtype = node.Attributes["showtype"] == null ? "" : node.Attributes["showtype"].Value;
                    m_VitalSignsCaption.Add(obj.datafield, obj);
                }

                foreach (KeyValuePair<string, VitalSignInfoEntity> pair in m_VitalSignsCaption)
                {
                    DataGridViewRow newRow = newRow = new DataGridViewRow();
                    newRow.Tag = pair.Key;
                    newRow.HeaderCell.Value = pair.Value.caption;
                    if (pair.Value.showtype == "0")//时段数据
                    {
                        dgvVitalSigns.Rows.Add(newRow);
                    }
                    else //天数据
                    {
                        dgvVitalSigns1.Rows.Add(newRow);
                    }
                }
                dgvVitalSigns1.Height = m_VitalSignsCaption.Count * 18;
                panel1.Height = dgvVitalSigns1.Height + dgvVitalSigns.Height;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitTextBox()
        {
            try
            {
                if (ConfigInfo.editable == 1)
                {
                    txtDaysAfterSurgery1.Enabled = false;
                }
                else
                {
                    txtDaysAfterSurgery1.Enabled = true;
                }

                if (ConfigInfo.editDays == 1)
                {
                    txtDayOfHospital1.Enabled = false;
                }
                else
                {
                    txtDayOfHospital1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dgvVitalSigns_Enter(object sender, EventArgs e)
        {
            //dgvVitalSigns.ClearSelection();
            //dgvVitalSigns[0, 1].DataGridView.=true;

            dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[0];
            //dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
            dgvVitalSigns.BeginEdit(true);
        }

        private void ucTemperatureEditor1_Enter(object sender, EventArgs e)
        {
            //dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[0];
            ////dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
            //dgvVitalSigns.BeginEdit(true); 

        }

        private void ucTemperatureEditor_KeyPress(object sender, KeyPressEventArgs e)
        {


            try
            {
                if (e.KeyChar == 13)
                {
                    switch ((sender as UCTemperatureEditor).Name)
                    {
                        case "ucTemperatureEditor1":
                            dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[0];
                            dgvVitalSigns.BeginEdit(true);
                            break;
                        case "ucTemperatureEditor2":
                            dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[1];
                            dgvVitalSigns.BeginEdit(true);
                            break;
                        case "ucTemperatureEditor3":
                            dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[2];
                            dgvVitalSigns.BeginEdit(true);
                            break;
                        case "ucTemperatureEditor4":
                            dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[3];
                            dgvVitalSigns.BeginEdit(true);
                            break;
                        case "ucTemperatureEditor5":
                            dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[4];
                            dgvVitalSigns.BeginEdit(true);
                            break;
                        case "ucTemperatureEditor6":
                            dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[5];
                            dgvVitalSigns.BeginEdit(true);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ucTemperatureEditor1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[0];
                //dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
                dgvVitalSigns.BeginEdit(true);
            }
        }

        private void ucTemperatureEditor1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void UCNursingRecordTable_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dgvVitalSigns_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvVitalSigns.Rows.Count >= 5 && dgvVitalSigns.ColumnCount >= 0)
            {
                if (dgvVitalSigns[0, 5].Selected)
                {
                    dgvVitalSigns.ClearSelection();
                    ucTemperatureEditor1.Focus();
                    //dgvVitalSigns.SelectNextControl(ucTemperatureEditor1, false, true, true, true);

                }
            }

        }

        private void dgvVitalSigns_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            if (e.KeyChar == 13)
            {
                if (dgvVitalSigns.Rows.Count >= 5 && dgvVitalSigns.ColumnCount >= 0)
                {
                    if (dgvVitalSigns[0, 5].Selected)
                    {
                        if (index == 1)
                        {
                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor2.Focus();
                            index = 0;
                        }
                        index++;
                    }
                    else if (dgvVitalSigns[1, 5].Selected)
                    {
                        if (index == 1)
                        {
                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor3.Focus();
                            index = 0;
                        }
                        index++;
                    }
                    else if (dgvVitalSigns[2, 5].Selected)
                    {
                        if (index == 1)
                        {
                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor4.Focus();
                            index = 0;
                        }
                        index++;
                    }
                    else if (dgvVitalSigns[3, 5].Selected)
                    {
                        if (index == 1)
                        {
                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor5.Focus();
                            index = 0;
                        }
                        index++;
                    }
                    else if (dgvVitalSigns[4, 5].Selected)
                    {
                        if (index == 1)
                        {
                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor6.Focus();
                            index = 0;
                        }
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }

                }
            }
        }

        private void dgvVitalSigns1_Enter(object sender, EventArgs e)
        {
            try
            {
                // dgvVitalSigns1.CurrentCell = dgvVitalSigns1.Rows[0].Cells[0];
                //dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;

                // dgvVitalSigns1.BeginEdit(true);
                //dgvVitalSigns.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == Keys.Enter)
                {
                    if (dgvVitalSigns.Rows.Count >= 5 && dgvVitalSigns.ColumnCount >= 0)
                    {
                        if (dgvVitalSigns[0, 5].Selected)
                        {
                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor2.Focus();

                        }
                        else if (dgvVitalSigns[1, 5].Selected)
                        {

                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor3.Focus();
                        }
                        else if (dgvVitalSigns[2, 5].Selected)
                        {
                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor4.Focus();
                        }
                        else if (dgvVitalSigns[3, 5].Selected)
                        {
                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor5.Focus();
                        }
                        else if (dgvVitalSigns[4, 5].Selected)
                        {
                            dgvVitalSigns.ClearSelection();
                            ucTemperatureEditor6.Focus();
                        }
                        else
                        {
                            index = 0;
                        }

                    }
                }



                if (keyData == Keys.Up && dgvVitalSigns[0, 1].Selected)
                {
                    dgvVitalSigns.ClearSelection();
                    ucTemperatureEditor1.Focus();
                    return true;
                }
                else if (keyData == Keys.Up && dgvVitalSigns[1, 1].Selected)
                {
                    dgvVitalSigns.ClearSelection();
                    ucTemperatureEditor2.Focus();
                    return true;
                }
                else if (keyData == Keys.Up && dgvVitalSigns[2, 1].Selected)
                {
                    dgvVitalSigns.ClearSelection();
                    ucTemperatureEditor3.Focus();
                    return true;
                }
                else if (keyData == Keys.Up && dgvVitalSigns[3, 1].Selected)
                {
                    dgvVitalSigns.ClearSelection();
                    ucTemperatureEditor4.Focus();
                    return true;
                }
                else if (keyData == Keys.Up && dgvVitalSigns[4, 1].Selected)
                {
                    dgvVitalSigns.ClearSelection();
                    ucTemperatureEditor5.Focus();
                    return true;
                }
                else if (keyData == Keys.Up && dgvVitalSigns[5, 1].Selected)
                {
                    dgvVitalSigns.ClearSelection();
                    ucTemperatureEditor6.Focus();
                    return true;
                }
                else if (keyData == Keys.Down && this.ActiveControl.Name.Equals("ucTemperatureEditor1"))
                {
                    dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[0];
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
                    dgvVitalSigns.BeginEdit(true);
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditOnEnter;
                    return true;
                }
                else if (keyData == Keys.Down && this.ActiveControl.Name.Equals("ucTemperatureEditor2"))
                {
                    dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[1];
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
                    dgvVitalSigns.BeginEdit(true);
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditOnEnter;
                    return true;
                }
                else if (keyData == Keys.Down && this.ActiveControl.Name.Equals("ucTemperatureEditor3"))
                {
                    dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[2];
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
                    dgvVitalSigns.BeginEdit(true);
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditOnEnter;
                    return true;
                }
                else if (keyData == Keys.Down && this.ActiveControl.Name.Equals("ucTemperatureEditor4"))
                {
                    dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[3];
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
                    dgvVitalSigns.BeginEdit(true);
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditOnEnter;
                    return true;
                }
                else if (keyData == Keys.Down && this.ActiveControl.Name.Equals("ucTemperatureEditor5"))
                {
                    dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[4];
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
                    dgvVitalSigns.BeginEdit(true);
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditOnEnter;
                    return true;
                }
                else if (keyData == Keys.Down && this.ActiveControl.Name.Equals("ucTemperatureEditor6"))
                {
                    dgvVitalSigns.CurrentCell = dgvVitalSigns.Rows[1].Cells[5];
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
                    dgvVitalSigns.BeginEdit(true);
                    dgvVitalSigns.EditMode = DataGridViewEditMode.EditOnEnter;
                    return true;
                }
                else if (keyData == Keys.Tab && dgvVitalSigns[5, 5].Selected)
                {
                    dgvVitalSigns.ClearSelection();
                    dgvVitalSigns1.CurrentCell = dgvVitalSigns1.Rows[0].Cells[0];
                    dgvVitalSigns1.BeginEdit(true);
                    return true;
                }
                else
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        private void dgvVitalSigns_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvVitalSigns[0, 0].Selected)
                {
                    ucTemperatureEditor1.Focus();
                }
                else if (dgvVitalSigns[1, 0].Selected)
                {
                    ucTemperatureEditor2.Focus();
                }
                else if (dgvVitalSigns[2, 0].Selected)
                {
                    ucTemperatureEditor3.Focus();
                }
                else if (dgvVitalSigns[3, 0].Selected)
                {
                    ucTemperatureEditor4.Focus();
                }
                else if (dgvVitalSigns[4, 0].Selected)
                {
                    ucTemperatureEditor5.Focus();
                }
                else if (dgvVitalSigns[5, 0].Selected)
                {
                    ucTemperatureEditor6.Focus();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }




    }

    //标识体征数据信息
    public class VitalSignInfoEntity
    {
        public string caption;//名称
        public string datafield; //对应表字段类型
        public CellControlType cellCtlType;//单元格控件类型
        public string showForm; //如设置它对应的弹出窗体，在此定义
        public IList datasource; //如需绑定下拉列表数据，在此定义;如设置它对应的弹出窗体，在此定义
        public string showtype = "0";//0：时段数据; 1：天数据

        public VitalSignInfoEntity(string _caption, string _dataifield, CellControlType _cellCtlType)
        {
            caption = _caption;
            datafield = _dataifield;
            cellCtlType = _cellCtlType;
        }
        public VitalSignInfoEntity(string _caption, string _dataifield)
        {
            caption = _caption;
            datafield = _dataifield;
        }

        public VitalSignInfoEntity()
        {
        }
    }

    //DataGrid 单元格编辑类型
    public enum CellControlType
    {
        ComboBox = 0, //下拉列表
        Normal = 1, //普通文本框
        ShowDlg = 2 //弹框编辑 
    }
}

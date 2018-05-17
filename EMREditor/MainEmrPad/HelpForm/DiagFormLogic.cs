using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data;
using DevExpress.XtraEditors;
using System.Collections;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Core;
using DrectSoft.Core.IEMMainPage;
using DrectSoft.Core.MainEmrPad.New;

namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    class DiagFormLogic
    {
        #region SQL
        /// <summary>
        /// 病人诊断类型
        /// </summary>
        /// //此处捞取诊断有关的按钮 add by ywk 2013年4月1日15:49:54 
        private const string SqlPatDiagType = " SELECT * FROM patdiagtype WHERE diagname = '{0}' and  typeid in ('2','3')";
        /// <summary>
        /// 捞取手术的相关SQL语句  add by ywk 2013年4月1日15:50:59 
        /// </summary>
        private const string SqlPatOperType = " SELECT * FROM patdiagtype WHERE diagname = '{0}'";// and  typeid ='5' ";

        /// <summary>
        /// 科室知识库分类字典
        /// </summary>
        private const string SqlDiagDeptRepClass = " SELECT * FROM deptrepclass WHERE name = '{0}' and valid = 'Y' ";
        #endregion

        #region FIELD && PROPERTY
        private IEmrHost m_App;
        //诊断窗体
        private DiagForm m_DiagForm;

        public DiagForm MDiagForm
        {
            get { return m_DiagForm; }
            set { m_DiagForm = value; }
        }
        //手术窗体
        //private DiagFormOper m_OperForm;
        private OperForm m_OperForm;
        private DiagLibForm m_DiagLibForm;

        private XtraForm m_Form;

        private string m_Name;
        #endregion

        #region .ctor
        public DiagFormLogic(IEmrHost app)
        {
            m_App = app;
        }
        #endregion

        #region 得到界面返回的值 【供外部调用】
        /// <summary>
        /// 得到界面返回的值
        /// </summary>
        /// 二次修改 杨伟康 (满足浦口需求)
        /// <returns></returns>
        public void GetDiag(PadForm padForm)
        {
            try
            {
                //此处根据配置，判断返回的诊断内容是结构化元素还是自由文本
                //add by ywk 2013年2月19日15:46:13
                string diagContentType = BasicSettings.GetStringConfig("IsSetDiagContentStr") == "" ? "1" : BasicSettings.GetStringConfig("IsSetDiagContentStr");
                if (m_Form is DiagForm)
                {
                    string diagContent = m_DiagForm.GetDiag().Trim();
                    //edti by ywk 2013年2月19日15:50:39
                    if (diagContentType == "1")//插入结构化元素
                    {
                        InsertElementText(padForm, m_Name, diagContent);
                    }
                    else
                    {
                        InsertString(padForm, m_Name, diagContent);
                        //InsertElementText(padForm, m_Name, diagContent);
                    }
                }
                else                           //   手术信息
                    if (m_Form is OperForm)
                    {
                        string diagContent = m_OperForm.GetDiag().Trim();
                        //edti by ywk 2013年2月19日15:50:39
                        if (diagContentType == "1")//插入结构化元素
                        {
                            InsertElementText(padForm, m_Name, diagContent);
                        }
                        else
                        {
                            InsertString(padForm, m_Name, diagContent);
                            //InsertElementText(padForm, m_Name, diagContent);
                        }
                    }
                    else if (m_Form is DiagLibForm)
                    {
                        string diagContent = m_DiagLibForm.GetDiag().Trim();
                        InsertString(padForm, m_Name, diagContent);
                    }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private void InsertElementText(PadForm padForm, string name, string diagContent)
        {
            try
            {
                ArrayList al = new ArrayList();
                padForm.zyEditorControl1.EMRDoc.GetAllSpecElement(al, padForm.zyEditorControl1.EMRDoc.RootDocumentElement, ElementType.Text, name);

                if (al.Count > 0)
                {
                    ZYText find = al[0] as ZYText;
                    if (find != null)
                    {
                        padForm.zyEditorControl1.EMRDoc.Content.MoveSelectStart(find.FirstElement);
                        padForm.zyEditorControl1.DeleteElement(find);
                    }
                }
                else
                {
                    //padForm.zyEditorControl1.EMRDoc._MoveRight();
                }

                ZYText value = new ZYText();
                value.Name = name;
                value.Text = diagContent;
                value.Attributes.SetValue(ZYTextConst.c_FontSize, "小四");
                //value.Attributes.SetValue(ZYTextConst.c_Br, "</br>");
                //value.Attributes.SetValue(ZYTextConst.c_XMLText, "</eof>");

                //ZYTextBlock newValue = new ZYTextBlock();
                ////newValue.Attributes.SetValue(ZYTextConst.c_FontSize,"20");
                ////newValue.Attributes.SetValue(ZYTextConst.c_Br, "<br/>");
                ////newValue.Attributes.SetValue(ZYTextConst.c_XMLText,"</eof>");
                //newValue.Text = diagContent;
                //padForm.zyEditorControl1.EMRDoc._InsertBlock(newValue);
                padForm.zyEditorControl1.EMRDoc._InsertBlock(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsertString(PadForm padForm, string name, string diagContent)
        {
            try
            {
                padForm.zyEditorControl1.EMRDoc._MoveRight();
                padForm.zyEditorControl1.EMRDoc._InserString(diagContent);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 根据名称判断进入那个HelpForm
        /// <summary>
        /// 根据名称判断进入那个HelpForm
        /// </summary>
        /// <param name="name"></param>
        public bool ShowDialog(string name)
        {
            try
            {
                m_Name = name;

                //弹出诊断窗口 name="初步诊断"、确诊诊断、补充诊断、修正诊断、中医诊断等
                DataTable dtDiag = m_App.SqlHelper.ExecuteDataTable(string.Format(SqlPatDiagType, name), CommandType.Text);
                //捞取手术信息 add by ywk 2013年4月1日15:53:49   edit by wangj 2013 4 12
                DataTable dtOper = m_App.SqlHelper.ExecuteDataTable(string.Format(SqlPatOperType, name), CommandType.Text);
                if (dtDiag.Rows.Count > 0)
                {
                    ShowPatDiagTypeForm(name, "diag");//新增参数
                    return true;
                }

                if (dtOper.Rows.Count > 0)
                {
                    ShowPatDiagTypeForm(name, "operation");
                    return true;
                }
                //Modified By wwj 暂不开放临床数据按钮
                //弹出临床诊断库 name='鉴别诊断'、诊断计划等
                //dt = m_App.SqlHelper.ExecuteDataTable(string.Format(SqlDiagDeptRepClass, name), CommandType.Text);
                //if (dt.Rows.Count > 0)
                //{
                //    string id = dt.Rows[0]["ID"].ToString();
                //    ShowDiagRepClass(name, id);
                //    return true;
                //}

                return false;
                //其他
                //............
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 弹出诊断窗口 name="初步诊断"、确诊诊断、补充诊断、修正诊断、中医诊断等
        /// </summary>
        /// <param name="name"></param>
        /// <param name="btntype">诊断类型</param>
        /// edit by ywk 2013年4月1日16:02:56 
        private void ShowPatDiagTypeForm(string name, string btntype)
        {
            try
            {
                if (btntype == "diag")//诊断
                {
                    if (m_DiagForm == null)
                    {
                        m_DiagForm = new DiagForm(m_App, name);
                        m_Form = m_DiagForm;
                    }
                    else
                    {
                        m_DiagForm.SetDiagName(name);
                        m_Form = m_DiagForm;
                    }
                    m_DiagForm.ShowDialog();
                }
                if (btntype == "operation")//手术信息 m_OperForm
                {
                    if (m_OperForm == null)
                    {
                        //m_OperForm = new DiagFormOper(m_App, name);
                        m_OperForm = new OperForm(m_App, name);
                        m_Form = m_OperForm;
                    }
                    else
                    {
                        m_OperForm.SetDiagName(name);
                        m_Form = m_OperForm;
                    }
                    m_OperForm.ShowDialog();
                    //string GoType = "operate";
                    //string MZDiagType = "operate";
                    //DataTable dtOperation = new DataTable();
                    //string SqlAllDiag = @"select py, wb, name, ID icd from operation where valid='1'";
                    //dtOperation = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
                    //IemNewDiagInfo iemOperation = new IemNewDiagInfo(m_App, dtOperation, GoType, MZDiagType, "");
                    //iemOperation.ShowDialog();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 弹出临床诊断库 name='鉴别诊断'、诊断计划等
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        private void ShowDiagRepClass(string name, string id)
        {
            try
            {
                if (m_DiagLibForm == null)
                {
                    m_DiagLibForm = new DiagLibForm(m_App, name, id);
                    m_Form = m_DiagLibForm;
                }
                else
                {
                    m_DiagLibForm.SetFormName(name, id);
                    m_Form = m_DiagLibForm;
                }
                m_DiagLibForm.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion





        #region 给新版文书录入使用

        public void GetDiag(EditorForm padForm)
        {
            try
            {
                //此处根据配置，判断返回的诊断内容是结构化元素还是自由文本
                //add by ywk 2013年2月19日15:46:13
                string diagContentType = BasicSettings.GetStringConfig("IsSetDiagContentStr") == "" ? "1" : BasicSettings.GetStringConfig("IsSetDiagContentStr");

                if (m_Form is DiagForm)
                {
                    string diagContent = m_DiagForm.GetDiag().Trim();
                    //edti by ywk 2013年2月19日15:50:39
                    if (diagContentType == "1")//插入结构化元素
                    {
                        InsertElementText(padForm, m_Name, diagContent);
                    }
                    else
                    {
                        InsertString(padForm, m_Name, diagContent);
                    }
                }
                else if (m_Form is DiagLibForm)
                {
                    string diagContent = m_DiagLibForm.GetDiag().Trim();
                    InsertString(padForm, m_Name, diagContent);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsertElementText(EditorForm padForm, string name, string diagContent)
        {
            try
            {
                ArrayList al = new ArrayList();
                padForm.CurrentEditorControl.EMRDoc.GetAllSpecElement(al, padForm.CurrentEditorControl.EMRDoc.RootDocumentElement, ElementType.Text, name);

                if (al.Count > 0)
                {
                    ZYText find = al[0] as ZYText;
                    if (find != null)
                    {
                        padForm.CurrentEditorControl.EMRDoc.Content.MoveSelectStart(find.FirstElement);
                        padForm.CurrentEditorControl.DeleteElement(find);
                    }
                }

                //ZYTextBlock newvalue = new ZYTextBlock();
                //newvalue.Name = name;
                //newvalue.Text = diagContent;
                //newvalue.Attributes.SetValue(ZYTextConst.c_FontSize, "五号");
                //padForm.CurrentEditorControl.EMRDoc._InsertBlock(newvalue);

                ZYText value = new ZYText();
                value.Name = name;
                value.Text = diagContent;

                value.Attributes.SetValue(ZYTextConst.c_FontSize, "小四");
                padForm.CurrentEditorControl.EMRDoc._InsertBlock(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsertString(EditorForm padForm, string name, string diagContent)
        {
            try
            {
                padForm.CurrentEditorControl.EMRDoc._MoveRight();
                padForm.CurrentEditorControl.EMRDoc._InserString(diagContent);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

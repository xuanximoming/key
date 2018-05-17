using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data;
using DevExpress.XtraEditors;
using System.Collections;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Core.EMR_NursingDocument.EMRInput.Table;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HelpForm
{
    class DiagFormLogic
    {
        #region SQL
        /// <summary>
        /// 病人诊断类型
        /// </summary>
        private const string SqlPatDiagType = " SELECT * FROM patdiagtype WHERE diagname = '{0}' ";

        /// <summary>
        /// 科室知识库分类字典
        /// </summary>
        private const string SqlDiagDeptRepClass = " SELECT * FROM deptrepclass WHERE name = '{0}' and valid = 'Y' ";
        #endregion

        #region FIELD && PROPERTY
        private IEmrHost m_App;

        private DiagForm m_DiagForm;
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
        /// <returns></returns>
        public void GetDiag(PadForm padForm)
        {
            if (m_Form is DiagForm)
            {
                string diagContent = m_DiagForm.GetDiag().Trim();
                InsertElementText(padForm, m_Name, diagContent);
            }
            else if (m_Form is DiagLibForm)
            {
                string diagContent = m_DiagLibForm.GetDiag().Trim();
                InsertString(padForm, m_Name, diagContent);
            }
        }
        #endregion

        private void InsertElementText(PadForm padForm, string name, string diagContent)
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
            padForm.zyEditorControl1.EMRDoc._InsertBlock(value);
        }

        private void InsertString(PadForm padForm, string name, string diagContent)
        {
            padForm.zyEditorControl1.EMRDoc._MoveRight();
            padForm.zyEditorControl1.EMRDoc._InserString(diagContent);
        }

        #region 根据名称判断进入那个HelpForm
        /// <summary>
        /// 根据名称判断进入那个HelpForm
        /// </summary>
        /// <param name="name"></param>
        public bool ShowDialog(string name)
        {
            m_Name = name;

            //弹出诊断窗口 name="初步诊断"、确诊诊断、补充诊断、修正诊断、中医诊断等
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(string.Format(SqlPatDiagType, name), CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                ShowPatDiagTypeForm(name);
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

        /// <summary>
        /// 弹出诊断窗口 name="初步诊断"、确诊诊断、补充诊断、修正诊断、中医诊断等
        /// </summary>
        /// <param name="name"></param>
        private void ShowPatDiagTypeForm(string name)
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

        /// <summary>
        /// 弹出临床诊断库 name='鉴别诊断'、诊断计划等
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        private void ShowDiagRepClass(string name, string id)
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
        #endregion
    }
}

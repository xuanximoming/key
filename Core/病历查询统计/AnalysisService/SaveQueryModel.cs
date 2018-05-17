using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using YidanSoft.Common.Library;
using YidanSoft.Core;
//using YidanSoft.Core.Model;
using YidanSoft.FrameWork;
using YidanSoft.FrameWork.BizBus;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Wordbook;
//using YidanSoft.CommonServiceInf;

namespace YidanSoft.Core.AnalysisService
{
    /// <summary>
    /// 保存查询模板用输入名称窗口
    /// </summary>
    public partial class FormSaveQueryModel : Form
    {
        //private IEMRQuery m_EMRQuery = BusFactory.GetBus().BuildUp<IEMRQuery>();
        //private EmrQueryCondition _emrQueryCondition;

        IYidanEmrHost App
        {
            get
            {
                return _app;
            }
        }
        IYidanEmrHost _app;


        //public FormSaveQueryModel(EmrQueryCondition emrQueryCondition, IYidanEmrHost app)
        //{
        //    InitializeComponent();
        //    _app = app;
        //    _emrQueryCondition = emrQueryCondition;
        //}

        public FormSaveQueryModel(IYidanEmrHost app)
        {
            InitializeComponent();
            _app = app;
 
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (_emrQueryCondition == null || EmrQueryCondition.IsNullCondition(_emrQueryCondition))
            //{
            //    App.CustomMessageBox.MessageShow("没有数据需要保存！");
            //}

            //if (m_EMRQuery.MDS_NameIsExist(textEditQueryName.Text))
            //{
            //    App.CustomMessageBox.MessageShow("存在同名的查询模板！");
            //    return;
            //}
            //SaveConditions(_emrQueryCondition, textEditQueryName.Text);
            this.DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        //#region 保存自定义条件到查询模版库
        //public void SaveConditions(EmrQueryCondition condition, String queryName)
        //{
        //    Guid queryId = Guid.NewGuid();
        //    SqlParameter[] arrParams = CreateInsertQueryModelParams();
        //    arrParams[1].Value = queryName;
        //    arrParams[19].Value = 0;

        //    SaveConditions(condition, queryId, queryName);

        //    SaveMyProject(queryId.ToString(), queryName);
        //}


        //private void SaveMyProject(string guid, string name)
        //{
        //    //m_EMRQuery.InsertMyProject(guid, name, App.User.ID);
        //}


        //private SqlParameter[] CreateInsertQueryModelParams()
        //{
        //    SqlParameter[] arrParams = new SqlParameter[20];
        //    arrParams[0] = new SqlParameter("ID", SqlDbType.VarChar, 64);
        //    arrParams[1] = new SqlParameter("Name", SqlDbType.VarChar, 64);
        //    arrParams[2] = new SqlParameter("TemplateID", SqlDbType.VarChar, 64);
        //    arrParams[3] = new SqlParameter("StructID", SqlDbType.VarChar, 64);
        //    arrParams[4] = new SqlParameter("IndexCode", SqlDbType.VarChar, 12);
        //    arrParams[5] = new SqlParameter("EmbedId", SqlDbType.VarChar, 64);
        //    arrParams[6] = new SqlParameter("ObjectID", SqlDbType.VarChar, 64);
        //    arrParams[7] = new SqlParameter("AtomID", SqlDbType.VarChar, 64);
        //    arrParams[8] = new SqlParameter("LineCode", SqlDbType.Int);
        //    arrParams[9] = new SqlParameter("Qryvalue", SqlDbType.VarChar, 255);
        //    arrParams[10] = new SqlParameter("Dspvalue", SqlDbType.VarChar, 255);
        //    arrParams[11] = new SqlParameter("Unit", SqlDbType.VarChar, 12);
        //    arrParams[12] = new SqlParameter("Compare", SqlDbType.VarChar, 64);
        //    arrParams[13] = new SqlParameter("Logic", SqlDbType.VarChar, 64);
        //    arrParams[14] = new SqlParameter("LeftSubxh", SqlDbType.Decimal);
        //    arrParams[15] = new SqlParameter("RightSubxh", SqlDbType.Decimal);
        //    arrParams[16] = new SqlParameter("FullPath", SqlDbType.VarChar, 1024);
        //    arrParams[17] = new SqlParameter("FullpathName", SqlDbType.VarChar, 1024);
        //    arrParams[18] = new SqlParameter("ValueType", SqlDbType.Int);
        //    arrParams[19] = new SqlParameter("ModelCategory", SqlDbType.SmallInt);
        //    return arrParams;

        //}
        //private int SaveConditions(EmrQueryCondition condition, Guid queryId, String queryName)
        //{
        //    SqlParameter[] arrParams = CreateInsertQueryModelParams();
        //    arrParams[0].Value = queryId.ToString();
        //    arrParams[1].Value = queryName;
        //    arrParams[19].Value = 0;

        //    EmrQueryCondition condA = condition.CondA;
        //    EmrQueryCondition condB = condition.CondB;
        //    if (condA != null)
        //        arrParams[14].Value = SaveConditions(condA, queryId, queryName);
        //    if (condB != null)
        //        arrParams[15].Value = SaveConditions(condB, queryId, queryName);

        //    int identity = 0;
        //    arrParams[12].Value = condition.Compare;
        //    arrParams[13].Value = condition.Logic;

        //    if (condition.EmrQueryNode != null && condition.EmrQueryNode.FullPathsId != null && condition.EmrQueryNode.FullPathsId.Count > 0)
        //    {
        //        arrParams[16].Value = condition.EmrQueryNode.FullPath;
        //        arrParams[17].Value = condition.EmrQueryNode.FullPathName;

        //        Collection<Hashtable> cht = condition.EmrQueryNode.FullPathsId;
        //        foreach (Hashtable ht in cht)
        //        {
        //            foreach (object o in ht.Keys)
        //            {
        //                if (o is string)
        //                {
        //                    string so = (string)o;
        //                    if (so == EmrNodeQueryResult.CstQueryValue)
        //                        arrParams[9].Value = ht[EmrNodeQueryResult.CstQueryValue];
        //                    if (so == EmrNodeQueryResult.CstDisplayValue)
        //                        arrParams[10].Value = ht[EmrNodeQueryResult.CstDisplayValue];
        //                    if (so == EmrNodeQueryResult.CstDisplayUnit)
        //                        arrParams[11].Value = ht[EmrNodeQueryResult.CstDisplayUnit];
        //                    if (so == EmrNodeQueryResult.CstValueType)
        //                        arrParams[18].Value = ht[EmrNodeQueryResult.CstValueType];
        //                }
        //                else
        //                {
        //                    //EmrNodeType ent = (EmrNodeType)o;
        //                    //switch (ent)
        //                    //{
        //                    //    case EmrNodeType.AtomNode:
        //                    //    case EmrNodeType.Cell:
        //                    //        arrParams[7].Value = ht[ent];
        //                    //        break;
        //                    //    case EmrNodeType.DynamicMoleNode:
        //                    //        arrParams[3].Value = ht[ent];
        //                    //        break;
        //                    //    case EmrNodeType.EmbededMoleNode:
        //                    //        arrParams[5].Value = ht[ent];
        //                    //        break;
        //                    //    case EmrNodeType.Model:
        //                    //        arrParams[2].Value = ht[ent];
        //                    //        break;
        //                    //    case EmrNodeType.Object:
        //                    //        arrParams[6].Value = ht[ent];
        //                    //        break;
        //                    //    case EmrNodeType.Table:
        //                    //        arrParams[6].Value = ht[ent];
        //                    //        break;
        //                    //    case EmrNodeType.Row:
        //                    //        arrParams[8].Value = ht[ent];
        //                    //        break;
        //                    //    default:
        //                    //        break;
        //                    //}
        //                }
        //            }
        //            //identity = InsertToSQL(arrParams);
        //        }
        //    }
        //    else
        //        //identity = InsertToSQL(arrParams);

        //    return identity;
        //}

        ////private int InsertToSQL(SqlParameter[] arrParams)
        ////{
        ////    String[] Strs = new String[20];
        ////    for (int i = 0; i < 20; i++)
        ////        Strs[i] = arrParams[i].Value == null ? null : arrParams[i].Value.ToString();

        ////    //return m_EMRQuery.MDS_Insert(Strs);
        ////}
        //#endregion
    }
}
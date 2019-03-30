using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;

namespace DrectSoft.Core.MainEmrPad.New
{
    /// <summary>
    /// 编辑器封装类数据访问层
    /// </summary>
    public class EditorFormDal
    {
        IEmrHost m_App;

        public EditorFormDal(IEmrHost app)
        {
            m_App = app;
        }

        /// <summary>
        /// 通过name获取对应宏的数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetDataByNameForMacro(string noofinpat, string name)
        {
            try
            {
                string marcValue = MacroUtil.FillMarcValue(noofinpat, name, m_App.User.Id);
                return marcValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //获得可替换项
        public DataTable GetReplaceItem(string modelName)
        {
            try
            {
                string sqlGetReplaceImte = "SELECT * FROM EMR_REPLACE_ITEM WHERE valid = '1'";

                if (!string.IsNullOrEmpty(modelName))
                {
                    sqlGetReplaceImte = sqlGetReplaceImte + " AND ( INSTR('" + modelName + "',dest_emrname) = 1 OR dest_emrname IS NULL) ";
                }
                return DS_SqlHelper.ExecuteDataTable(sqlGetReplaceImte, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //获得书写界面右侧提取病历节点列表
        public DataTable GetReplaceItemRight()
        {
            try
            {
                string sqlGetReplaceImte = "select * from EMR_MEDICINE_NODE  WHERE valid = '1'";


                return DS_SqlHelper.ExecuteDataTable(sqlGetReplaceImte, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 通过病历的名称获取病历内容
        /// </summary>
        /// <param name="sourceEMR"></param>
        /// <returns></returns>
        public DataTable GetEmrContentByName(string sourceEMR, string noofinpat)
        {
            try
            {
                string sqlGetEmrContent = string.Format(
                    @"select r.id, r.content 
                        from recorddetail r 
                       where r.name like '{0}%' and r.noofinpat = '{1}' and r.valid = 1", sourceEMR, noofinpat);
                return DS_SqlHelper.ExecuteDataTable(sqlGetEmrContent, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取转科信息
        /// </summary>
        /// <param name="deptChangeID"></param>
        /// <returns></returns>
        public DataTable GetDeptChangeInfo(string deptChangeID)
        {
            try
            {
                // xll 由于无法找到转床后的床位 采取以下方式 暂时解决 但在一个科室里面多次转床 只能取到最新的转床 2013-05-15
                string sqlGetDeptChangeInfo = string.Format(
                    "select (select department.name from department where department.id=a.newdeptid) as deptName,"
                    + " (select ward.name from ward where ward.id=a.newwardid) as wardName, "
                    + " a.newbedid  as  bedID  from inpatientchangeinfo a inner join ( select  noofinpat,newdeptid,newwardid "
                    + " from inpatientchangeinfo i where i.id = {0} ) b "
                    + " on a.noofinpat=b.noofinpat and a.newdeptid=b.newdeptid "
                    + " and a.newwardid=b.newwardid order by a.createtime desc",
                    deptChangeID);

                return DS_SqlHelper.ExecuteDataTable(sqlGetDeptChangeInfo, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 由于子模板中存在已经编码过的数据和未编码过的数据，所以在解码数据的时候需要对抛出的异常做特殊处理
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetTempleteItem(string name)
        {
            DataTable table = DS_SqlHelper.ExecuteDataTable("select item_doc_new ITEM_DOC from emrtemplet_item  WHERE MR_NAME='" + name + "'");
            if (table.Rows.Count < 1)
            {
                object obj = "子模板不存在";
                return (byte[])obj;
            }

            return System.Text.Encoding.Default.GetBytes(RecordDal.UnzipEmrXml(table.Rows[0]["ITEM_DOC"].ToString()));
        }
    }
}

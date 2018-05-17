using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;

namespace DrectSoft.Core.KnowledgeBase
{
    class SqlManger
    {
        IEmrHost m_app;

        public SqlManger(IEmrHost app)
        {
            m_app = app;
        }
        /// <summary>
        /// 获取药品大类
        /// </summary>
        /// <returns></returns>
        public DataTable GetMedicineTreeOne()
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(@"select distinct a.categorytwo from Medicine a where a.categorytwo is not null");
            return dataTable;
        }
        /// <summary>
        /// 获取药品大类根据关键字
        /// 王冀 2012-10-30
        /// </summary>
        /// <returns></returns>
        public DataTable GetMedicineTreeOneByKey(string key)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select distinct a.categorytwo from Medicine a where a.categorytwo is not null and a.categorytwo like '%{0}%'", key));
            return dataTable;
        }


        /// <summary>
        /// 获取小类别
        /// </summary>
        /// <param name="OneName"></param>
        /// <returns></returns>
        public DataTable GetMedicaineTreeSec(string OneName)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select distinct a.categorytwo,a.categorythree from Medicine a where a.categorytwo = '{0}' order by a.categorytwo desc", OneName));
            return dataTable;
        }

        /// <summary>
        /// 根据关键字获取小类别的大类别
        /// </summary>
        /// <param name="OneName"></param>
        /// <returns></returns>
        public DataTable GetMedicaineTreeSecByKey(string key)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select distinct a.categorytwo,a.categorythree from Medicine a where a.categorythree like '%{0}%' order by a.categorytwo desc", key));
            return dataTable;
        }

        /// <summary>
        /// 获取药品
        /// </summary>
        /// <param name="OneName"></param>
        /// <returns></returns>
        public DataTable GetMedicaine()
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(@"select * from Medicine a ");
            return dataTable;
        }

        /// <summary>
        /// 根据关键字获取药品
        /// </summary>
        /// <param name="OneName"></param>
        /// <returns></returns>
        public DataTable GetMedicaineByKey(string key)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select * from Medicine a where name like '%{0}%' order by a.categorytwo,a.categorythree ", key));
            return dataTable;
        }

        /// <summary>
        /// 根据ID查询药品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetMedicaineByID(string id)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select * from Medicine a where a.id = '{0}'", id));
            return dataTable;
        }


        /// <summary>
        /// 获取药品根据三级
        /// </summary>
        /// <param name="OneName"></param>
        /// <returns></returns>
        public DataTable GetMedicaineByThreeName(string threeName)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select * from Medicine a where a.categorythree = '{0}'", threeName));
            return dataTable;
        }

        /// <summary>
        ///MedicaineDirect
        /// </summary>
        /// <returns></returns>
        public DataTable GetMedicaineDirect()
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(@"select * from MedicineDirect  ");
            return dataTable;
        }

        /// <summary>
        /// 根据MedicaineDirect的ID获取对应行记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable GetMedicaineDirectByID(string ID)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select * from MedicineDirect a where a.ID = '{0}'", ID));
            return dataTable;
        }

        /// <summary>
        /// 获取MedicaineDirect第一级节点
        /// </summary>
        /// <returns></returns>
        public DataTable GetMedicaineDirectTreeOne()
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(@"select distinct Doseform from dbo.MedicineDirect");
            return dataTable;
        }

        /// <summary>
        /// 获取MedicaineDirect第二级节点
        /// </summary>
        /// <returns></returns>
        public DataTable GetMedicaineDirectTreeTwo(string treeOneNode)
        {
            DataTable dataTable = null;
            //dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select distinct DirectTitle2 from MedicineDirect where Doseform = '{0}' order by DirectTitle2", treeOneNode));
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select id,DirectTitle from MedicineDirect where Doseform = '{0}' order by DirectTitle2", treeOneNode));
            return dataTable;
        }

        /// <summary>
        /// 获取第三级节点
        /// </summary>
        /// <param name="treeOneNode"></param>
        /// <param name="treeTwoNode"></param>
        /// <returns></returns>
        public DataTable GetMedicatineDirectTreeThree(string treeOneNode, string treeTwoNode)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select id,DirectTitle from MedicineDirect where Doseform = '{0}'  and DirectTitle2 = '{1}' order by DirectTitle", treeOneNode, treeTwoNode));
            return dataTable;
        }

        /// <summary>
        /// 通过关键字查询终端节点
        /// 王冀 2012-10-30
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataTable GetMedicaineByKey(string sql, string key)
        {
            DataTable dataTable = null;
            if (key == null || key.Trim() == string.Empty)
            {
                sql = @"select md.id,md.directtitle,md.Doseform from MedicineDirect md where length(doseform)<>0";
            }
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(sql, key));
            return dataTable;
        }

        /// <summary>
        /// 加载一级节点
        /// 王冀 2012-10-30
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public DataTable GetTreatmentTreeOne(string classno)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select * from emr_zhenliaofanganunit  where unitclass={0}", classno));
            return dataTable;
        }

        /// <summary>
        /// 加载二级节点
        /// 王冀 2012-10-30
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public DataTable GetTreatmentTreeTwo(string unit)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select * from emr_zhenliaofangan  where unitmid={0}", unit));
            return dataTable;
        }
        /// <summary>
        /// 根据ID获取数据  取得备注信息
        /// 王冀-2012-10-30
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetTreatmentByID(string id)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select * from emr_zhenliaofangan  where id={0}", id));
            return dataTable;
        }
        /// <summary>
        /// 根据关键字搜索（可为空）
        /// 王冀 2012-10-31
        /// </summary>
        /// <param name="key"></param>
        /// <param name="unitclass"></param>
        /// <returns></returns>
        public DataTable GetTreatmentByKey(string key, string unitclass)
        {
            DataTable dataTable = null;
            if (key == null || key.Trim() == string.Empty)
            {
                dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select a.*,b.unitmname from emr_zhenliaofangan a,emr_zhenliaofanganunit b  where b.unitclass={0} and a.unitmid=b.unitmid order by to_number(a.unitmid) ", unitclass));
            }
            else
            {
                dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select a.*,b.unitmname from emr_zhenliaofangan a,emr_zhenliaofanganunit b  where jibingmingcheng  like'%{0}%' and b.unitclass={1} and a.unitmid=b.unitmid order by to_number(a.unitmid) ", key, unitclass));
            }
            return dataTable;
        }

    }
}

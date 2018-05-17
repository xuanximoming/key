using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DrectSoft.Wordbook;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Core;
using System.Data;

namespace DrectSoft.Core.MaintainBasicInfo
{
    /// <summary>
    ///类名:DataProcess
    ///功能说明:数据处理
    ///创建人:wyt
    ///创建时间:2012-11-12
    /// </summary>
    class DataProcess
    {
        private IEmrHost m_app;
        public DataProcess(IEmrHost app)
        {
            m_app = app;
        }
        public void SaveUserEntity(UserEntity user, MaintainBasicInfo.OperState oper)
        {
            DrectSoft.Core.GenerateShortCode d = new GenerateShortCode(m_app.SqlHelper);
            string[] code = d.GenerateStringShortCode(user.Name);
            string py = code[0];
            string wb = code[1];
            string passwd = "QK+S40FfCEQ=";
            string regdate = "2005110717:30:35";
            string sql = "";
            switch (oper)
            {
                case MaintainBasicInfo.OperState.ADD:
                    sql = @"Insert into users(ID, NAME,PY,WB,SEXY,BIRTH,MARITAL,idno,DEPTID,WARDID,PASSWD,REGDATE,valid) 
                            values ('" + user.ID + "','" + user.Name + "','" + py + "','" + wb + "','" + user.Sex + "','" + user.Birthday.ToShortDateString() +
                                       "','" + user.Marital + "','" + user.CardID + "','" + user.DeptID + "','" + user.WardID + "','" + passwd + "','" + regdate + "'," + 1 + ")";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
                case MaintainBasicInfo.OperState.EDIT:
                    sql = @"UPDATE users SET Name='" + user.Name + "',Py='" + py + "',Wb='" + wb + "',DeptId='" + user.DeptID + "',WardID='" + user.WardID +
                        "',Marital='" + user.Marital + "',Sexy='" + user.Sex + "',Birth='" + user.Birthday.ToShortDateString()
                        + "',IDNO='" + user.CardID + "',ID='" + user.ID + "' WHERE ID='" + user.ID + "'";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
                case MaintainBasicInfo.OperState.DEL:
                    sql = @"update users set valid = 0 where id = '" + user.ID + "'";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
            }

        }
        public void SaveUserEntity(UserEntity user, string olduserid,MaintainBasicInfo.OperState oper)
        {
            DrectSoft.Core.GenerateShortCode d = new GenerateShortCode(m_app.SqlHelper);
            string[] code = d.GenerateStringShortCode(user.Name);
            string py = code[0];
            string wb = code[1];
            string passwd = "QK+S40FfCEQ=";
            string regdate = "2005110717:30:35";
            string sql = "";
            switch (oper)
            {
                case MaintainBasicInfo.OperState.ADD:
                    sql = @"Insert into users(ID, NAME,PY,WB,SEXY,BIRTH,MARITAL,idno,DEPTID,WARDID,PASSWD,REGDATE,valid) 
                            values ('" + user.ID + "','" + user.Name + "','" + py + "','" + wb + "','" + user.Sex + "','" + user.Birthday.ToShortDateString() +
                                       "','" + user.Marital + "','" + user.CardID + "','" + user.DeptID + "','" + user.WardID + "','" + passwd + "','" + regdate + "'," + 1 + ")";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
                case MaintainBasicInfo.OperState.EDIT:
                    sql = @"UPDATE users SET Name='" + user.Name + "',Py='" + py + "',Wb='" + wb + "',DeptId='" + user.DeptID + "',WardID='" + user.WardID +
                        "',Marital='" + user.Marital + "',Sexy='" + user.Sex + "',Birth='" + user.Birthday.ToShortDateString()
                        + "',IDNO='" + user.CardID + "',ID='" + user.ID + "' WHERE ID='" + olduserid + "'";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
                case MaintainBasicInfo.OperState.DEL:
                    sql = @"update users set valid = 0 where id = '" + user.ID + "'";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
            }

        }

        public void SaveDeptEntity(DeptEntity dept, MaintainBasicInfo.OperState oper)
        {
            DrectSoft.Core.GenerateShortCode d = new GenerateShortCode(m_app.SqlHelper);
            string[] code = d.GenerateStringShortCode(dept.Name);
            string py = code[0];
            string wb = code[1];
            string sql = "";
            switch (oper)
            {
                case MaintainBasicInfo.OperState.ADD:
                    sql =  @"Insert into department(ID, NAME,PY,WB,HOSNO,ADEPT,BDEPT,SORT,MARK,VALID) 
                            values ('" + dept.ID + "','" + dept.Name + "','" + py + "','" + wb + "', '01','',''," + dept.SortID + ",201,1)";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
                case MaintainBasicInfo.OperState.EDIT:
                    sql = @"update department set name = '" + dept.Name + "', py = '" + py + "', wb = '" + wb + "', sort = " + dept.SortID + " where id = '" + dept.ID + "'";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
                case MaintainBasicInfo.OperState.DEL:
                    sql = @"update department set valid = 0 where id = '" + dept.ID + "'";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
            }
        }

        public void SaveWardEntity(WardEntity ward, MaintainBasicInfo.OperState oper)
        {
            DrectSoft.Core.GenerateShortCode d = new GenerateShortCode(m_app.SqlHelper);
            string[] code = d.GenerateStringShortCode(ward.Name);
            string py = code[0];
            string wb = code[1];
            string sql = "";
            switch (oper)
            {
                case MaintainBasicInfo.OperState.ADD:
                    sql = @"Insert into ward(ID, NAME,PY,WB,MARK,VALID) 
                            values ('" + ward.ID + "','" + ward.Name + "','" + py + "','" + wb + "',300,1)";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
                case MaintainBasicInfo.OperState.EDIT:
                    sql = @"update ward set name = '" + ward.Name + "',py = '" + py + "',wb = '" + wb + "' where id = '" + ward.ID + "'";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
                case MaintainBasicInfo.OperState.DEL:
                    sql = @"update ward set valid = 0 where id = '" + ward.ID + "'";
                    m_app.SqlHelper.ExecuteNoneQuery(sql);
                    break;
            }
        }


        /// <summary>
        /// 判断ID是否已经存在
        /// </summary>
        /// <param name="type">表格：0用户1科室2病区</param>
        /// <param name="id">编号</param>
        /// <returns>是否存在</returns>
        public bool IsContainID(MaintainBasicInfo.OperType type, string id)
        {
            string sql = "";
            switch (type)
            {
                case MaintainBasicInfo.OperType.USER:
                    sql = "select * from Users where id = '" + id + "'";
                    DataTable user = m_app.SqlHelper.ExecuteDataTable(sql);
                    if (user.Rows.Count > 0)
                    {
                        return true;
                    }
                    return false;
                case MaintainBasicInfo.OperType.DEPT:
                    sql = "select * from department where id = '" + id + "'";
                    DataTable dept = m_app.SqlHelper.ExecuteDataTable(sql);
                    if (dept.Rows.Count > 0)
                    {
                        return true;
                    }
                    return false;
                case MaintainBasicInfo.OperType.WARD:
                    sql = "select * from ward where id = '" + id + "'";
                    DataTable ward = m_app.SqlHelper.ExecuteDataTable(sql);
                    if (ward.Rows.Count > 0)
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }
    }

}

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;

namespace DrectSoft.Core
{
    #region class RoleFunctionList
    /// <summary>
    /// 功能列表
    /// </summary>
    public class RoleFunctionList : IRBSFunction
    {
        private string _functionId;
        private string _functionName;
        private string _functionDescription;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="functionName"></param>
        /// <param name="functionDescription"></param>
        public RoleFunctionList(string functionId, string functionName, string functionDescription)
        {
            _functionId = functionId;
            _functionName = functionName;
            _functionDescription = functionDescription;
        }

        #region IRBSFunction Members

        /// <summary>
        /// FunctionDescription
        /// </summary>
        public string FunctionDescription
        {
            get { return _functionDescription; }
        }

        /// <summary>
        /// FunctionId
        /// </summary>
        public string FunctionId
        {
            get { return _functionId; }
        }

        /// <summary>
        /// FunctionName
        /// </summary>
        public string FunctionName
        {
            get { return _functionName; }
        }

        #endregion
    }
    #endregion

    #region class RoleModuleList
    /// <summary>
    /// 模块列表
    /// </summary>
    public class RoleModuleList : IRBSModule
    {
        private string _moduleId;
        private string _moduleName;
        private string _moduleDescription;
        private Collection<IRBSFunction> _functionList;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="moduleName"></param>
        /// <param name="moduleDescription"></param>
        /// <param name="system"></param>
        public RoleModuleList(string moduleId, string moduleName, string moduleDescription)
        {
            _moduleId = moduleId;
            _moduleName = moduleName;
            _moduleDescription = moduleDescription;
            _functionList = new Collection<IRBSFunction>();
        }

        #region IRBSModule Members

        /// <summary>
        /// FunctionList
        /// </summary>
        public Collection<IRBSFunction> FunctionList
        {
            get { return _functionList; }
        }

        /// <summary>
        /// ModuleDescription
        /// </summary>
        public string ModuleDescription
        {
            get { return _moduleDescription; }
        }

        /// <summary>
        /// ModuleId
        /// </summary>
        public string ModuleId
        {
            get { return _moduleId; }
        }

        /// <summary>
        /// ModuleName
        /// </summary>
        public string ModuleName
        {
            get { return _moduleName; }
        }


        /// <summary>
        /// AddAFunction
        /// </summary>
        /// <param name="function"></param>
        public void AddAFunction(IRBSFunction function)
        {
            if (_functionList == null)
            {
                _functionList = new Collection<IRBSFunction>();
            }
            if (_functionList != null)
            {
                _functionList.Add(function);
            }
        }

        /// <summary>
        /// AddAFunction2
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="functionName"></param>
        /// <param name="functionDescription"></param>
        /// <returns></returns>
        public int AddAFunction(string functionId, string functionName, string functionDescription)
        {
            if (_functionList == null)
            {
                _functionList = new Collection<IRBSFunction>();
            }
            _functionList.Add(new RoleFunctionList(functionId, functionName, functionDescription));
            return _functionList.Count - 1;
        }

        #endregion

    }
    #endregion

    #region class Permission

    /// <summary>
    /// 权限类（所有的权限）
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class CPermission : IPermission
    {
        /// <summary>
        /// 定义超级用户的岗位,这个岗位无法设置,仅包含超级用户（默认为00）
        /// </summary>
        private const string cstSuperRoleId = "00";

        /// <summary>
        /// 角色Id
        /// </summary>
        private string _roleId;             //岗位Id

        /// <summary>
        /// 角色Name
        /// </summary>
        private string _roleName;

        /// <summary>
        /// 角色模块功能列表
        /// </summary>
        private Collection<IRBSModule> _moduleList;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="system"></param>
        /// <param name="roleName"></param>
        public CPermission(string roleId, string roleName)
        {
            _roleId = roleId;
            _roleName = roleName;
            _moduleList = new Collection<IRBSModule>();
        }

        #region IPermission Members

        /// <summary>
        /// 岗位代码
        /// </summary>
        [Description("岗位代码"),
         DisplayName("岗位代码")]
        public string RoleId
        {
            get { return _roleId; }
            //set
            //{
            //    string oldId = _roleId;
            //    _roleId = value;
            //    OnRoleIdChanged(new RoleIdChangedArgs(oldId, value));
            //}
        }

        /// <summary>
        /// 岗位名称
        /// </summary>
        [Description("岗位名称"),
         DisplayName("岗位名称")]
        public string RoleName
        {
            get { return _roleName; }
            set
            {
                string oldName = _roleName;
                _roleName = value;
                OnRoleNameChanged(new RoleNameChangedEventArgs(oldName, value));
            }
        }

        /// <summary>
        /// 增加模块权限
        /// </summary>
        /// <param name="module"></param>
        public void AddAModule(IRBSModule module)
        {
            if (_moduleList == null)
            {
                _moduleList = new Collection<IRBSModule>();
            }
            if (_moduleList != null)
            {
                _moduleList.Add(module);
            }
        }

        /// <summary>
        /// 角色拥有的模块权限
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public Collection<IRBSModule> ModuleList
        {
            get { return _moduleList; }
        }

        /// <summary>
        /// 增加模块权限2
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="moduleName"></param>
        /// <param name="moduleDescription"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public int AddAModule(string moduleId, string moduleName, string moduleDescription)
        {
            if (_moduleList == null)
            {
                _moduleList = new Collection<IRBSModule>();
            }
            _moduleList.Add(new RoleModuleList(moduleId, moduleName, moduleDescription));
            return _moduleList.Count - 1;
        }

        /// <summary>
        /// 实现接口RoleIdChanged事件
        /// </summary>
        public event EventHandler<RoleIdChangedEventArgs> RoleIdChanged;

        /// <summary>
        /// RoleIdChanged处理
        /// </summary>
        /// <param name="args"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected void OnRoleIdChanged(RoleIdChangedEventArgs args)
        {
            //Save Change to DataBase
            if (string.IsNullOrEmpty(args.OldId) && !string.IsNullOrEmpty(args.NewId))
            {
                _roleId = args.NewId;
                PermissionDal dal = new PermissionDal();
                dal.InsertRolePermission(this);
            }

            if (RoleIdChanged == null) return;

            Delegate[] invkList = RoleIdChanged.GetInvocationList();

            foreach (EventHandler<RoleIdChangedEventArgs> handler in invkList)
            {
                try
                {
                    //IAsyncResult ar = 
                    //handler.BeginInvoke(this, args, null, null);
                    handler.Invoke(this, args);
                }
                catch (Exception e)
                {
                    RoleIdChanged -= handler;
                }
            }
        }

        /// <summary>
        /// 实现接口RoleNameChanged事件
        /// </summary>
        public event EventHandler<RoleNameChangedEventArgs> RoleNameChanged;

        /// <summary>
        /// RoleNameChanged处理
        /// </summary>
        /// <param name="args"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected void OnRoleNameChanged(RoleNameChangedEventArgs args)
        {
            //Save Change to DataBase
            if (!string.IsNullOrEmpty(args.NewName) && args.OldName != args.NewName)
            {
                PermissionDal dal = new PermissionDal();
                dal.UpdateRoleInfo(this);
            }

            if (RoleNameChanged == null) return;

            Delegate[] invkList = RoleNameChanged.GetInvocationList();

            foreach (EventHandler<RoleNameChangedEventArgs> handler in invkList)
            {
                try
                {
                    //IAsyncResult ar = 
                    //handler.BeginInvoke(this, args, null, null);
                    handler.Invoke(this, args);
                }
                catch (Exception e)
                {
                    RoleNameChanged -= handler;
                }
            }
        }


        /// <summary>
        /// 是否为管理员
        /// </summary>
        [Description("是否为管理员"),
        DisplayName("超级管理员组")]
        public bool IsAdministrators
        {
            get { return _roleId == cstSuperRoleId; }
        }
        #endregion
    }

    /// <summary>
    /// 新增角色类（允许设置RoleId）
    /// </summary>
    public class PermissionNew : CPermission
    {
        private string _roleId;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <param name="system"></param>
        public PermissionNew(string roleId, string roleName)
            : base(roleId, roleName)
        {
            _roleId = roleId;
        }

        /// <summary>
        /// 新建岗位代码
        /// </summary>
        [Description("新建岗位代码")]
        public new string RoleId
        {
            get { return _roleId; }
            set
            {
                string oldId = _roleId;
                _roleId = value;
                base.OnRoleIdChanged(new RoleIdChangedEventArgs(oldId, value));
            }
        }
    }

    #endregion

    #region class PermissionDal

    /// <summary>
    /// 权限数据访问类（取得所有的权限信息）
    /// </summary>
    public class PermissionDal
    {
        #region 操作数据库的Sql语句

        private const string SelectGWQX = @"select b.Id, a.Moduleid, a.Modulename, a.Functionid, a.Functionname ,b.Title from Job2Permission a right join Jobs b on a.ID=b.Id
                                            order by a.ID, a.Moduleid, a.Functionid ";
        private const string SelectAGWQX = @"select b.Id, a.Moduleid, a.Modulename, a.Functionid, a.Functionname 
                                               ,b.Title from Job2Permission a right join Jobs b on a.ID=b.Id
                                              where a.ID=b.Id and a.ID='{0}'
                                              order by a.ID, a.Moduleid, a.Functionid ";
        private const string SelectAUserGWQX = @"select b.Id, a.Moduleid, a.Modulename, a.Functionid, a.Functionname 
                                               ,b.Title from Job2Permission a right join Jobs b on a.ID=b.Id
                                              where a.ID=b.Id and instr('{0}',rtrim(a.ID))>0 
                                              order by a.ID, a.Moduleid, a.Functionid ";
        //根据角色查询权限 by cyq 2012-08-28
        private const string SelectAUserRights = @"select b.Id, a.Moduleid, a.Modulename, a.Functionid, a.Functionname 
                                               ,b.Title from Job2Permission a right join Jobs b on a.ID=b.Id
                                              where a.ID=b.Id and a.id in({0})
                                              order by a.ID, a.Moduleid, a.Functionid ";

        private const string InsertGW = "insert into Jobs(Id, Title,Memo) values('{0}', '{1}','{2}')";

        private const string UpdateGW = "update Jobs set Id='{0}',Title='{1}',Memo='{2}' where Id='{3}'";

        private const string AddGWQX = @"insert into Job2Permission(ID,Moduleid,Modulename,Functionid,Functionname)
                                        values('{0}','{1}','{2}','{3}','{4}')";

        private const string DeleteModuleGWQX = "delete from Job2Permission where ID='{0}' and Moduleid='{1}'";

        private const string DeleteFunctionGWQX = "delete from Job2Permission where ID='{0}' and Moduleid='{1}' and Functionid='{2}'";

        private const string DeleteGW = "delete from Jobs where Id='{0}' delete from Job2Permission where ID='{1}'  update Users set JobID=replace(JobID,'{2}','') where charindex('{3}',JobID)>0";

        private const string SelectARoleUsers = @"select d.ID userid,d.Name username,d.Passwd,d.DeptId,d.WardID, b.Name dept, c.Name ward, d.RegDate, d.JobID 
                                                from Users d left join Department b on d.DeptId=b.ID left join Ward c on d.WardID=c.ID
                                                 where charindex('{0}'+',',d.JobID)>0";


        private IDataAccess sql_helper;

        #endregion

        /// <summary>
        /// 构造权限类
        /// </summary>
        public PermissionDal()
        {
            sql_helper = DataAccessFactory.DefaultDataAccess;
        }

        /// <summary>
        /// 插入新的岗位
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public void InsertRolePermission(IPermission role)
        {
            if (role == null) throw new ArgumentNullException("role", "未传入角色");

            sql_helper.ExecuteNoneQuery(string.Format(InsertGW, role.RoleId, role.RoleName, ""));

        }

        /// <summary>
        /// 取得指定岗位的权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IPermission GetRolePermission(string roleId)
        {

            DataTable dt = sql_helper.ExecuteDataTable(string.Format(SelectAGWQX, roleId));
            Collection<IPermission> permissions = RoleTable2PermissionCollection(dt);
            if (permissions.Count != 1)
                return new CPermission(roleId, "");
            else
                return permissions[0];
        }

        private static Collection<IPermission> RoleTable2PermissionCollection(DataTable dt)
        {
            Collection<IPermission> allPermission = new Collection<IPermission>();

            string roleid = string.Empty;
            int roleIndex = -1;
            string moduleid = string.Empty;
            int moduleIndex = -1;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["Id"].ToString().Trim() != roleid)
                {
                    roleid = dr["Id"].ToString().Trim();
                    CPermission onePermission = new CPermission(roleid, dr["Title"].ToString().Trim());
                    allPermission.Add(onePermission);
                    roleIndex = allPermission.Count - 1;
                }
                if (dr["Moduleid"].ToString().Trim() != moduleid)
                {
                    moduleid = dr["Moduleid"].ToString().Trim();
                    IRBSModule module = new RoleModuleList(moduleid, dr["Modulename"].ToString().Trim(), "");
                    allPermission[roleIndex].AddAModule(module);
                    moduleIndex = allPermission[roleIndex].ModuleList.Count - 1;
                }
                if (!string.IsNullOrEmpty(dr["Functionid"].ToString())
                    && !string.IsNullOrEmpty(dr["Functionid"].ToString()))
                {
                    IRBSFunction function = new RoleFunctionList(
                        dr["Functionid"].ToString().Trim(), dr["Functionname"].ToString().Trim(), "");
                    allPermission[roleIndex].ModuleList[moduleIndex].AddAFunction(function);
                }
            }

            return allPermission;
        }

        public Collection<IPermission> GetAllRolePermission(string roleIds)
        {
            //by cyq 2012-08-28
            //DataTable dt = sql_helper.ExecuteDataTable(string.Format(SelectAUserGWQX, roleIds));
            string ids = string.Empty;
            if (!string.IsNullOrEmpty(roleIds))
            {
                string[] str = roleIds.Split(',');
                foreach (string s in str)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        ids += "'" + s + "'" + ",";
                    }
                }
                if (ids.Length > 0)
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }
            }
            else
            {
                ids = "''";
            }
            DataTable dt = sql_helper.ExecuteDataTable(string.Format(SelectAUserRights, ids));

            return RoleTable2PermissionCollection(dt);

        }


        /// <summary>
        /// 更新一个角色权限
        /// </summary>
        /// <param name="permission"></param>
        public void UpdateRolePermission(IPermission permission)
        {
            if (permission == null) return;
            IPermission oldPermission = this.GetRolePermission(permission.RoleId);
            for (int i = 0; i < permission.ModuleList.Count; i++)
            {
                IRBSModule module = permission.ModuleList[i];
                string roleId = permission.RoleId;
                IRBSModule oldModule = ModuleIsInPermission(oldPermission, module.ModuleId);
                if (oldModule != null)
                {
                    UpdateModulePermission(module, oldModule, roleId);
                }
                else
                {
                    InsertModulePermission(module, roleId);
                }
            }
        }

        private static IRBSModule ModuleIsInPermission(IPermission permission, string moduleId)
        {
            for (int i = 0; i < permission.ModuleList.Count; i++)
            {
                if (moduleId == permission.ModuleList[i].ModuleId)
                {
                    return permission.ModuleList[i];
                }
            }

            return null;
        }

        private void UpdateModulePermission(IRBSModule module, IRBSModule oldModule, string roleId)
        {

            // 删除不存在的权限
            for (int i = 0; i < oldModule.FunctionList.Count; i++)
            {
                IRBSFunction function = oldModule.FunctionList[i];
                if (!module.FunctionList.Contains(function))
                {
                    sql_helper.ExecuteNoneQuery(string.Format(DeleteFunctionGWQX, roleId, oldModule.ModuleId, function.FunctionId));
                }
            }

            // 添加新增的权限
            for (int i = 0; i < module.FunctionList.Count; i++)
            {
                IRBSFunction function = module.FunctionList[i];
                if (!oldModule.FunctionList.Contains(function))
                {
                    sql_helper.ExecuteNoneQuery(string.Format(AddGWQX, roleId, module.ModuleId, module.ModuleName, function.FunctionId, function.FunctionName));
                }
            }
        }

        private void InsertModulePermission(IRBSModule module, string roleId)
        {

            sql_helper.ExecuteNoneQuery(string.Format(AddGWQX, roleId, module.ModuleId, module.ModuleName, "", ""));

            for (int i = 0; i < module.FunctionList.Count; i++)
            {
                IRBSFunction function = module.FunctionList[i];
                sql_helper.ExecuteNoneQuery(string.Format(AddGWQX, roleId, module.ModuleId, module.ModuleName, function.FunctionId, function.FunctionName));
            }
        }



        /// <summary>
        /// 更新角色的单个权限
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="setRight"></param>
        public void UpdateRolePermission(IPermission permission, IRBSModule module, IRBSFunction function, bool setRight)
        {
            if (permission == null) return;
            if (module == null) return;
            //Job2Permission j2p = new Job2Permission { ID = permission.RoleId, Moduleid = module.ModuleId, Modulename = module.ModuleName, Functionid = function.FunctionId, Functionname = function.FunctionName };

            if (setRight)
            {
                if (function != null)
                    sql_helper.ExecuteNoneQuery(string.Format(AddGWQX, permission.RoleId, module.ModuleId, module.ModuleName, function.FunctionId, function.FunctionName));
                else
                {

                    for (int i = 0; i < module.FunctionList.Count; i++)
                    {
                        string functionid = module.FunctionList[i].FunctionId;
                        string functionName = module.FunctionList[i].FunctionName;
                        sql_helper.ExecuteNoneQuery(string.Format(AddGWQX, permission.RoleId, module.ModuleId, module.ModuleName, functionid, functionName));

                    }

                }
            }
            else
            {
                if (function != null)
                {
                    sql_helper.ExecuteNoneQuery(string.Format(DeleteFunctionGWQX, permission.RoleId, module.ModuleId, function.FunctionId));

                }
                else
                {
                    sql_helper.ExecuteNoneQuery(string.Format(DeleteModuleGWQX, permission.RoleId, module.ModuleId, function.FunctionId));
                }

            }
        }

        /// <summary>
        /// 查询一个岗位包含的所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Collection<Users> SelectRoleIncludeUsers(string roleId)
        {

            Collection<Users> users = new Collection<Users>();

            DataTable dt = sql_helper.ExecuteDataTable(string.Format(SelectARoleUsers, roleId));

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dataRow = dt.Rows[i];
                    Users user = new Users(dataRow["userid"].ToString().Trim()
                                , dataRow["username"].ToString().Trim());
                    user.SetUserDoctor(dataRow["userid"].ToString().Trim()
                                , dataRow["username"].ToString().Trim());
                    user.SetCurrentDeptWard(
                                dataRow["DeptId"].ToString().Trim()
                                , dataRow["dept"].ToString().Trim()
                                , dataRow["WardID"].ToString().Trim()
                                , dataRow["ward"].ToString().Trim());
                    user.PasswordKey = dataRow["RegDate"].ToString().Trim();
                    user.Password = dataRow["Passwd"].ToString();
                    user.SetRoleId(dataRow["JobID"].ToString().Trim());
                    users.Add(user);
                }
            }

            return users;
        }

        /// <summary>
        /// 更新一个角色信息
        /// </summary>
        /// <param name="permission"></param>
        public void UpdateRoleInfo(IPermission permission)
        {
            try
            {
                sql_helper.ExecuteNoneQuery(string.Format(UpdateGW, permission.RoleId, permission.RoleName, ""));
            }
            catch (SqlException ex)
            {
            }

        }

        /// <summary>
        /// 删除一个角色信息
        /// </summary>
        /// <param name="permission"></param>
        public void DeleteRoleInfo(IPermission permission)
        {
            if (permission == null) throw new ArgumentNullException("permission", "未传入权限");
            try
            {
                sql_helper.ExecuteNoneQuery(string.Format(DeleteGW, permission.RoleId, permission.RoleId, permission.RoleId, permission.RoleId));

            }
            catch (SqlException ex)
            {
            }
        }

    }

    #endregion
}

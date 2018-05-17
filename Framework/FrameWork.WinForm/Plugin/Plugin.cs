using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Drawing;
using DrectSoft.Resources;
using System.ComponentModel;

namespace DrectSoft.FrameWork.WinForm
{
    /// <summary>
    /// 实现接口IPlugin
    /// </summary>
    public class PlugIn : MarshalByRefObject, IPlugIn, ICloneable
    {
        private Form m_MainForm;
        private Boolean m_IsMdiChild;
        private Boolean m_IsShowModel;
        private Collection<IPluginOwnerMenu> m_AddInMenuItems;
        private Collection<IPluginOwnerToolBar> m_AddInToolBar;
        private string m_AssemblyFileName;
        private Collection<IPlugIn> _assistPlugIn;
        private Collection<IPlugIn> _byLoadPlugIn;
        private string m_startClassType;
        private Image m_icon;

        /// <summary>
        /// 保存委托定义
        /// </summary>
        /// <returns></returns>
        public delegate bool NeedSaveDataDelegate();
        /// <summary>
        /// 保存数据委托方法
        /// </summary>
        public NeedSaveDataDelegate NeedSaveData;

        /// <summary>
        /// 保存委托定义
        /// </summary>
        /// <returns></returns>
        public delegate bool SaveDataDelegate();
        /// <summary>
        /// 保存数据委托方法
        /// </summary>
        public SaveDataDelegate SaveData;

        #region Plugin构造函数

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="startClassType"></param>
        public PlugIn(string assemblyName, string startClassType)
            : this(startClassType, null, true, false)
        {
            m_AssemblyFileName = assemblyName;
            m_startClassType = startClassType;
            //m_AddInMenuItems = new Collection<ToolStripMenuItem>();
            //m_AddInToolStrips = new Collection<ToolStrip>();
            //m_AddInToolBar = new Collection<Bar>();
            m_AddInMenuItems = new Collection<IPluginOwnerMenu>();
            m_AddInToolBar = new Collection<IPluginOwnerToolBar>();
        }

        //#region Obsolete

        //[Obsolete("废除，请用 PlugIn(string startClassType,Form mainForm, Boolean isMdiChild, Boolean isShowModel"
        //    +", Collection<ToolStripMenuItem> addInMenuItems"
        //    +", Collection<ToolStrip> addInToolStrips"
        //    +", Collection<IDockingForm> addInIDockingForms"
        //    +", Collection<IPlugIn> assistPlugIn)")]
        //public PlugIn(Form mainForm, Boolean isMdiChild, Boolean isShowModel
        //    , Collection<ToolStripMenuItem> addInMenuItems
        //    , Collection<ToolStrip> addInToolStrips
        //    , Collection<IDockingForm> addInIDockingForms
        //    , Collection<IPlugIn> assistPlugIn)
        //    : this( mainForm, isMdiChild, isShowModel, addInMenuItems, addInToolStrips, addInIDockingForms)
        //{
        //    if (assistPlugIn == null)
        //    {
        //        _assistPlugIn = new Collection<IPlugIn>();
        //    }
        //    else
        //    {
        //        _assistPlugIn = assistPlugIn;
        //    }
        //}

        ////[Obsolete("PlugIn(string startClassType, Form mainForm, Boolean isMdiChild, Boolean isShowModel"
        ////    +", Collection<ToolStripMenuItem> addInMenuItems"
        ////    +", Collection<ToolStrip> addInToolStrips"
        ////    +", Collection<IDockingForm> addInIDockingForms"
        ////    +")")]
        ////public PlugIn(Form mainForm, Boolean isMdiChild, Boolean isShowModel
        ////    , Collection<ToolStripMenuItem> addInMenuItems
        ////    , Collection<ToolStrip> addInToolStrips
        ////    , Collection<IDockingForm> addInIDockingForms
        ////    )
        ////{
        ////    m_MainForm = mainForm;
        ////    m_IsMdiChild = isMdiChild;
        ////    m_IsShowModel = isShowModel;
        ////    m_AddInMenuItems = addInMenuItems;
        ////    m_AddInToolStrips = addInToolStrips;
        ////    m_AddInDockingForms = addInIDockingForms;

        ////    _assistPlugIn = new Collection<IPlugIn>();
        ////    if (mainForm != null)
        ////    {
        ////        m_startClassType = mainForm.GetType().ToString();
        ////        m_AssemblyFileName = mainForm.GetType().Module.Name;
        ////    }
        ////    else
        ////    {
        ////        //如果没有直接显示的窗口,只存在Dock窗口时,取Dock窗口的Assembly
        ////        SetAssemblyFrom1stDockForm(addInIDockingForms);
        ////    }

        ////    _byLoadPlugIn = new Collection<IPlugIn>();
        ////}

        //[Obsolete("已经废除，请使用PlugIn(string startClassType,Form mainForm, Boolean isMdiChild, Boolean isShowModel)")]
        //public PlugIn(Form mainForm, Boolean isMdiChild, Boolean isShowModel)
        //    : this( mainForm, isMdiChild, isShowModel, new Collection<ToolStripMenuItem>(),
        //    new Collection<ToolStrip>(), new Collection<IDockingForm>())
        //{
        //}

        //[Obsolete("已经废除，请使用PlugIn(string startClassType, Form mainForm)")]
        //public PlugIn( Form mainForm)
        //    : this( mainForm, true, false)
        //{
        //}

        //#endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="startClassType">启动类名</param>
        /// <param name="mainForm"></param>
        /// <param name="isMdiChild"></param>
        /// <param name="isShowModel"></param>
        /// <param name="addInMenuItems"></param>
        /// <param name="addInToolStrips"></param>
        /// <param name="addInIDockingForms"></param>
        /// <param name="assistPlugIn"></param>
        public PlugIn(string startClassType, Form mainForm, Boolean isMdiChild, Boolean isShowModel
            //, Collection<ToolStripMenuItem> addInMenuItems
            //, Collection<ToolStrip> addInToolStrips
            , Collection<IPluginOwnerMenu> addInMenuItems
            , Collection<IPluginOwnerToolBar> addInToolStrips
            , Collection<IPlugIn> assistPlugIn)
            : this(startClassType, mainForm, isMdiChild, isShowModel, addInMenuItems, addInToolStrips)
        {
            if (assistPlugIn == null)
            {
                _assistPlugIn = new Collection<IPlugIn>();
            }
            else
            {
                _assistPlugIn = assistPlugIn;
            }
        }

        /// <summary>
        /// Plugin构造函数
        /// </summary>
        /// <param name="startClassType">启动类名</param>
        /// <param name="mainForm">启动的Form</param>
        /// <param name="isMdiChild">是否为MdiChild启动</param>
        /// <param name="isShowModel">是否为ShowModel启动</param>
        /// <param name="addInMenuItems">插件的菜单数组</param>
        /// <param name="addInToolStrips">插件的工具栏数组</param>
        /// <param name="addInIDockingForms">插件的Docking对象数组</param>
        public PlugIn(string startClassType, Form mainForm, Boolean isMdiChild, Boolean isShowModel
            //, Collection<ToolStripMenuItem> addInMenuItems
            //, Collection<ToolStrip> addInToolStrips
            , Collection<IPluginOwnerMenu> addInMenuItems
            , Collection<IPluginOwnerToolBar> addInToolStrips
            )
        {
            m_MainForm = mainForm;
            m_IsMdiChild = isMdiChild;
            m_IsShowModel = isShowModel;
            m_AddInMenuItems = addInMenuItems;
            //m_AddInToolStrips = addInToolStrips;
            m_AddInToolBar = addInToolStrips;

            _assistPlugIn = new Collection<IPlugIn>();
            if (mainForm != null)
            {
                m_AssemblyFileName = mainForm.GetType().Module.Name;//Assembly.FullName;//.GetName().Name;//.FullName;
            }
            else
            {
                //如果没有直接显示的窗口,只存在Dock窗口时,取Dock窗口的Assembly
                //SetAssemblyFrom1stDockForm(addInIDockingForms);
            }

            _byLoadPlugIn = new Collection<IPlugIn>();
        }

        /// <summary>
        /// Plugin构造函数
        /// </summary>
        /// <param name="startClassType">启动类名</param>
        /// <param name="mainForm">主窗体</param>
        /// <param name="isMdiChild">是否为MdiChild启动</param>
        /// <param name="isShowModel">是否为ShowModel启动</param>
        public PlugIn(string startClassType, Form mainForm, Boolean isMdiChild, Boolean isShowModel)
            : this(startClassType, mainForm, isMdiChild, isShowModel
                      , new Collection<IPluginOwnerMenu>()
            , new Collection<IPluginOwnerToolBar>()
                //, new Collection<ToolStrip>()
                // , new Collection<IPluginOwnerToolBar>()
             )
        {
        }

        /// <summary>
        /// Plugin构造函数
        /// </summary>
        /// <param name="startClassType">启动类名</param>
        /// <param name="mainForm">启动的Form</param>
        public PlugIn(string startClassType, Form mainForm)
            : this(startClassType, mainForm, true, false)
        {
            m_startClassType = startClassType;
        }
        #endregion

        #region IPlugin 成员
        /// <summary>
        /// 获取和设置插件的主窗体
        /// </summary>
        /// <value>插件的主窗体</value>
        public Form MainForm
        {
            get { return m_MainForm; }
            set { m_MainForm = value; }
        }

        /// <summary>
        ///获取和设置被启动的Form是否以MdiChild方式启动 
        /// </summary>
        /// <value></value>
        public Boolean IsMdiChild
        {
            get { return m_IsMdiChild; }
            set { m_IsMdiChild = value; }
        }

        /// <summary>
        /// 获取和设置被启动的Form是否以ShowModel方式启动
        /// </summary>
        /// <value></value>
        public Boolean IsShowModel
        {
            get { return m_IsShowModel; }
            set { m_IsShowModel = value; }
        }

        /// <summary>
        /// 获取和设置需要添加的菜单数组
        /// </summary>
        /// <value>菜单数组</value>
        //public Collection<ToolStripMenuItem> AddInMenuItems
        //{
        //    get { return m_AddInMenuItems; }
        //}
        public Collection<IPluginOwnerMenu> AddInMenuItems
        {
            get { return m_AddInMenuItems; }
        }

        /// <summary>
        /// 获取和设置需要添加的工具栏数组
        /// </summary>
        /// <value>工具栏数据</value>
        //public Collection<ToolStrip> AddInToolStrips
        //{
        //    get { return m_AddInToolStrips; }
        //}
        public Collection<IPluginOwnerToolBar> AddInToolStrips
        {
            get { return m_AddInToolBar; }
        }

        /// <summary>
        /// Assembly的文件名称
        /// </summary>
        /// <value></value>
        public string AssemblyFileName
        {
            get { return m_AssemblyFileName; }
            set { m_AssemblyFileName = value; }
        }

        /// <summary>
        /// 辅助的插件
        /// </summary>
        /// <value></value>
        public Collection<IPlugIn> AssistPlugIn
        {
            get { return _assistPlugIn; }
        }

        /// <summary>
        /// 启动类型字符串值
        /// </summary>
        /// <value></value>
        public string StartClassType
        {
            get { return m_startClassType; }
            set { m_startClassType = value; }
        }

        /// <summary>
        /// 加载当前Plugin的Plugin
        /// </summary>
        /// <value></value>
        public Collection<IPlugIn> ByLoadPlugIn
        {
            get { return _byLoadPlugIn; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public System.Drawing.Image GetImage(string imageName)
        {
            return ResourceManager.GetImage(ResourceManager.GetSourceName(imageName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iconName"></param>
        /// <returns></returns>
        public System.Drawing.Image GetNormalIcon(string iconName)
        {
            return ResourceManager.GetNormalIcon(ResourceManager.GetSourceName(iconName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public System.Drawing.Image GetSmallIcon(string imageName, IconType imageType)
        {
            return ResourceManager.GetSmallIcon(ResourceManager.GetSourceName(imageName), imageType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public System.Drawing.Image GetMiddleIcon(string imageName, IconType imageType)
        {
            return ResourceManager.GetMiddleIcon(ResourceManager.GetSourceName(imageName), imageType);
        }

        /// <summary>
        /// 获得或设置图标
        /// </summary>
        public Image Icon
        {
            get { return m_icon; }
            set { m_icon = value; }
        }

        /// <summary>
        /// 是否需要保存
        /// </summary>
        public bool NeedSave()
        {
            if (NeedSaveData == null) return true;
            return NeedSaveData();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            if (SaveData == null) return true;
            return SaveData();
        }
        #endregion

        #region ICloneable Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        /// <summary>
        /// 重载Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            PlugIn other = (PlugIn)obj;
            if (!IsMdiChild.Equals(other.IsMdiChild)) return false;
            if (!IsShowModel.Equals(other.IsShowModel)) return false;
            if (!string.Equals(m_AssemblyFileName, other.m_AssemblyFileName, StringComparison.CurrentCultureIgnoreCase)) return false;
            if (!string.Equals(m_startClassType, other.m_startClassType, StringComparison.CurrentCultureIgnoreCase)) return false;
            if (!object.Equals(m_MainForm, other.m_MainForm)) return false;
            if (!object.Equals(m_AddInMenuItems, other.m_AddInMenuItems)) return false;
            //if (!object.Equals(m_AddInToolStrips, other.m_AddInToolStrips)) return false;
            if (!object.Equals(m_AddInToolBar, other.m_AddInToolBar)) return false;
            //if (!object.Equals(m_AddInDockingForms, other.m_AddInDockingForms)) return false;
            if (!object.Equals(_assistPlugIn, other._assistPlugIn)) return false;
            //            if (!object.Equals(_byLoadPlugin, other._byLoadPlugin)) return false;

            return true;
        }

        /// <summary>
        /// 重载操作符=
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(PlugIn o1, PlugIn o2)
        {
            return object.Equals(o1, o2);
        }

        /// <summary>
        /// 重载操作符!=
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(PlugIn o1, PlugIn o2)
        {
            return !(o1 == o2);
        }

        /// <summary>
        /// 重载GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ((string)(m_AssemblyFileName + m_startClassType)).GetHashCode();
        }



        /// <summary>
        /// 床位号改变事件
        /// </summary>
        public event PatientChangedHandler PatientChanged;

        /// <summary>
        /// 病人正在改变事件
        /// </summary>
        public event PatientChangingHandler PatientChanging;

        /// <summary>
        /// 执行病人改变事件
        /// </summary>
        /// <param name="e">病人</param>
        public void ExecutePatientChangeEvent(PatientArgs e)
        {
            if (PatientChanged != null)
                PatientChanged(null, e);
        }

        /// <summary>
        /// 执行病人正在改变事件
        /// </summary>
        /// <param name="e">病人</param>
        public void ExecutePatientChangingEvent(CancelEventArgs e)
        {
            if (PatientChanging != null)
                PatientChanging(null, e);
        }

        #region IPlugIn 成员

        /// <summary>
        /// 用户改变事件
        /// </summary>
        public event UsersChangedHandeler UserChanged;
        /// <summary>
        /// 用户改变
        /// </summary>
        /// <param name="e"></param>
        public void ExecuteUsersChangeEvent(UserArgs e)
        {
            if (UserChanged != null)
            {
                UserChanged(null, e);
            }
        }

        #endregion
    }
}

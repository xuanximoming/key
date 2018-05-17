using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm;
using System.Collections.ObjectModel;
using System.Drawing;
using DrectSoft.Resources;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.ComponentModel;
using DrectSoft.Common.Eop;
using DrectSoft.Core;

namespace DrectSoft.FrameWork
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStartPlugIn
    {
        IPlugIn Run(IEmrHost host);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IFrameStartup
    {
        bool Run();
    }

    /// <summary>
    /// 插件对象
    /// </summary>
    public interface IPlugIn
    {
        /// <summary>
        /// Assembly的文件名称
        /// </summary>
        /// <value></value>
        string AssemblyFileName { get; }

        /// <summary>
        /// 启动类的类型
        /// </summary>
        /// <value></value>
        string StartClassType { get; }

        /// <summary>
        /// 获取插件的主窗体
        /// </summary>
        /// <value>启动插件的Form</value>
        Form MainForm { get; }

        /// <summary>
        ///获取被启动的Form是否以MdiChild方式启动 
        /// </summary>
        /// <value></value>
        Boolean IsMdiChild { get; }

        /// <summary>
        /// 获取被启动的Form是否以ShowModel方式启动
        /// </summary>
        /// <value></value>
        Boolean IsShowModel { get; }

        /// <summary>
        /// 获取需要添加的菜单数组
        /// </summary>
        /// <value>菜单数组</value>
        //Collection<ToolStripMenuItem> AddInMenuItems { get;}
        Collection<IPluginOwnerMenu> AddInMenuItems { get; }

        /// <summary>
        /// 获取需要添加的工具栏数组
        /// </summary>
        /// <value>工具栏数据</value>
        //Collection<ToolStrip> AddInToolStrips { get;}        
        Collection<IPluginOwnerToolBar> AddInToolStrips { get; }

        /// <summary>
        /// 当前插件使用的辅助插件，一般是Dock方式生成的StartForm
        /// </summary>
        /// <value></value>
        Collection<IPlugIn> AssistPlugIn { get; }

        /// <summary>
        /// 加载当前插件的所有插件，一般是AssistPlugin的相关的Plugin
        /// </summary>
        /// <value></value>
        Collection<IPlugIn> ByLoadPlugIn { get; }

        /// <summary>
        /// 获取或设置该plugin的icon
        /// </summary>
        Image Icon { get; set; }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="imageName">图片名称(包含后缀)，例如："EMRTitle.bmp"</param>
        /// <returns></returns>
        Image GetImage(string imageName);

        /// <summary>
        /// 获取普通的图标（以ico结尾的）
        /// </summary>
        /// <param name="iconName">图标名称（包含后缀）</param>
        /// <returns></returns>
        Image GetNormalIcon(string iconName);

        /// 获取小尺寸图标（16×16）
        /// <param name="imageName">图片名称，不需要指明尺寸、类型和后缀,例如："Save"</param>
        /// <param name="imageType">图片类型(一般图标都有3张：正常状态、禁用状态、高亮)</param>
        Image GetSmallIcon(string imageName, IconType imageType);

        /// 获取大尺寸图标（24×24）
        /// <param name="imageName">图片名称，不需要指明尺寸、类型和后缀,例如："Save"</param>
        /// <param name="imageType">图片类型（一般图标都有3张：正常状态、禁用状态、高亮)</param>
        Image GetMiddleIcon(string imageName, IconType imageType);

        /// <summary>
        /// 是否需要保存
        /// </summary>
        bool NeedSave();

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>是否继续</returns>
        bool Save();

        /// <summary>
        /// 系统框架用户改变事件
        /// </summary>
        event UsersChangedHandeler UserChanged;
        /// <summary>
        /// 系统框架用户改变事件
        /// </summary>
        /// <param name="e"></param>
        void ExecuteUsersChangeEvent(UserArgs e);

        /// <summary>
        /// 选择的病人发生改变
        /// </summary>
        event PatientChangedHandler PatientChanged;

        /// <summary>
        /// 选择的病人正在发生改变
        /// </summary>
        event PatientChangingHandler PatientChanging;

        /// <summary>
        /// 执行病人改变事件
        /// </summary>
        void ExecutePatientChangeEvent(PatientArgs e);

        /// <summary>
        /// 执行病人正改变事件
        /// </summary>
        /// <param name="e"></param>
        void ExecutePatientChangingEvent(CancelEventArgs e);
    }


    /// <summary>
    /// 提供病人改变后事件
    /// </summary>
    /// <param name="Sender">发送者</param>
    /// <param name="arg">病人参数</param>
    public delegate void PatientChangedHandler(object Sender, PatientArgs arg);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="arg"></param>
    public delegate void UsersChangedHandeler(object sender, UserArgs arg);
    /// <summary>
    /// 
    /// </summary>
    public class UserArgs : EventArgs
    {
        private IUser m_userInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        public UserArgs(IUser users)
        {
            m_userInfo = users;
        }

        /// <summary>
        /// 
        /// </summary>
        public IUser UsersInfo { get { return m_userInfo; } }
    }


    /// <summary>
    /// 病人参数
    /// </summary>
    public class PatientArgs : EventArgs
    {
        private Inpatient m_patinfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="patinfo"></param>
        public PatientArgs(Inpatient patinfo)
        {
            m_patinfo = patinfo;
        }

        /// <summary>
        /// 获得首页序号
        /// </summary>
        public Inpatient PatInfo
        {
            get { return m_patinfo; }
        }
    }

    /// <summary>
    /// 病人改变中事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="arg"></param>
    public delegate void PatientChangingHandler(object sender, CancelEventArgs arg);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace DrectSoft.Core
{  
    #region 为换肤创建的辅助类 对应于配置文件中的form节点
    /// </summary>
    public class FormEntity
    {
        public List<ChildControlEntity> childControlList = new List<ChildControlEntity>();

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }    
    }

    public struct ChildControlEntity
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _imageName;
        public string ImageName
        {
            get { return _imageName; }
            set { _imageName = value; }
        }
    }
    #endregion

    /// <summary>
    /// Icon 配置操作类 by tj
    /// </summary>
    public class AppFormIconConfig
    {
        private static IDataAccess _sqlHelper = DataAccessFactory.GetSqlDataAccess();
        private static readonly string iconBasePath =AppDomain.CurrentDomain.BaseDirectory+"\\Icon\\";
        private static FormEntity formEntity =new FormEntity();

        static AppFormIconConfig()
        {
            try
            {
                DataTable configData = _sqlHelper.ExecuteDataTable("select value from appcfg where configkey='FormIcon'");
                XmlDocument doc = new XmlDocument();
                if (configData.Rows.Count <= 0) return;
                doc.LoadXml(configData.Rows[0][0].ToString());
                XmlNodeList nodelist = doc.GetElementsByTagName("Form");
                foreach (XmlNode node in nodelist)
                {
                    formEntity.Icon = node.Attributes[0].Value.ToString();
                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode subNode in node.ChildNodes)
                        {
                            ChildControlEntity childControl = new ChildControlEntity();
                            childControl.Name = subNode.Attributes[0].Value.ToString();
                            childControl.ImageName = subNode.Attributes[1].Value.ToString();
                            formEntity.childControlList.Add(childControl);
                        }
                    }
                }
            }
            catch { } 
        }

        /// <summary>
        /// 换Icon的方法
        /// </summary>
        /// <param name="form">窗体控件</param>
        /// <param name="includeChildControls">是否需要对子控件进行Icon更改 true：需要；反之不需要</param>
        /// <param name="childControls">子控件列表，当includeChildControls=false 时此参数无效</param>
        public static void SetIcon(Control form, bool includeChildControls, List<PictureBox> childControls)
        {
            try
            {
                if (!includeChildControls)
                {
                    if (form.GetType().BaseType == typeof(Form) && formEntity.Icon != null && !formEntity.Icon.Equals(""))
                    {
                        if (File.Exists(iconBasePath + formEntity.Icon))
                        {
                            ((Form)form).Icon = new Icon(iconBasePath + formEntity.Icon);
                        }
                    }
                }
                else
                {
                    foreach (PictureBox pictureBox in childControls)
                    {
                        foreach (ChildControlEntity ctl in formEntity.childControlList)
                        {
                            if (ctl.Name.Equals(pictureBox.Name))
                                pictureBox.BackgroundImage = Image.FromFile(iconBasePath + ctl.ImageName);
                        }
                    }
                }
            }
            catch { }
        }
    }
}

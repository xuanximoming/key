using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Core;

namespace DrectSoft.FrameWork.WinForm
{
    /// <summary>
    /// ×´Ì¬À¸×¢²áÆ÷
    /// </summary>
    public class PluginStatusBarRegister:IPluginStatusBarRegister
    {
        StatusStrip m_statusbar;

        /// <summary>
        /// ¹¹Ôì
        /// </summary>
        /// <param name="statusbar">×´Ì¬À¸</param>
        public PluginStatusBarRegister(StatusStrip statusbar)
        {
            m_statusbar = statusbar;
        }

        #region IPluginStatusBarRegister ³ÉÔ±

        /// <summary>
        /// ×¢²áÕÊºÅÐÅÏ¢ÖÁ×´Ì¬À¸
        /// </summary>
        /// <param name="user"></param>
        public void Register(Users user)
        {
            m_statusbar.Items.Clear();

            ToolStripStatusLabel tssluser = new ToolStripStatusLabel();
            if (user == null)
            {
                tssluser.Text = "ÉÐÎ´µÇÂ¼";
                m_statusbar.Items.Add(tssluser);
            }
            else
            {
                tssluser.Text = "µ±Ç°ÓÃ»§: " + user.Name + "[" + user.Id + "]";
                m_statusbar.Items.Add(tssluser);

                ToolStripDropDownButton tsddbdeptward = new ToolStripDropDownButton();
                tsddbdeptward.Text = user.CurrentDeptWard.ToString();
                m_statusbar.Items.Add(tsddbdeptward);
                foreach (DeptWardInfo dwi in user.RelateDeptWards)
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = dwi.ToString();
                    tsmi.Tag = dwi;
                    tsddbdeptward.DropDownItems.Add(tsmi);
                }
                tsddbdeptward.DropDownItemClicked += new ToolStripItemClickedEventHandler(tsddbdeptward_DropDownItemClicked);
            }
        }

        void tsddbdeptward_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DeptWardInfo dwi = (DeptWardInfo)e.ClickedItem.Tag;
            if (dwi != null) (sender as ToolStripDropDownButton).Text = dwi.ToString();
        }

        /// <summary>
        /// ×¢²á²å¼þÐÅÏ¢ÖÁ×´Ì¬À¸
        /// </summary>
        /// <param name="plugin"></param>
        public void Register(IPlugIn plugin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Çå³ý×´Ì¬À¸ÕÊºÅ×¢²áÐÅÏ¢
        /// </summary>
        /// <param name="user"></param>
        public void UnRegister(Users user)
        {
            m_statusbar.Items.Clear();
        }

        /// <summary>
        /// Çå³ý×´Ì¬À¸²å¼þ×¢²áÐÅÏ¢
        /// </summary>
        /// <param name="plugin"></param>
        public void UnRegister(IPlugIn plugin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    /// <summary>
    /// ×´Ì¬À¸×¢²áÆ÷½Ó¿Ú¶¨Òå
    /// </summary>
    public interface IPluginStatusBarRegister
    {
        void Register(Users user);
        void Register(IPlugIn plugin);
        void UnRegister(Users user);
        void UnRegister(IPlugIn plugin);
    }
}

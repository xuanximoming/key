using System.Windows.Forms;

namespace DrectSoft.FrameWork.WinForm
{
    /// <summary>
    /// 无标题栏窗口
    /// </summary>
    public class NoCaptionBarForm : Form
    {
        private CaptionBarAnother captionBarAnother1;

        /// <summary>
        /// 标题栏
        /// </summary>
        public CaptionBarAnother CaptionBar
        {
            get
            {
                if (captionBarAnother1 == null)
                {
                    captionBarAnother1 = new CaptionBarAnother();
                    captionBarAnother1.Dock = DockStyle.Top;
                    //this.Controls.Add(captionBarAnother1);
                }
                return captionBarAnother1;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoCaptionBarForm));
            this.SuspendLayout();
            // 
            // NoCaptionBarForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NoCaptionBarForm";
            this.ResumeLayout(false);

        }

        #region 设置无标题栏窗口
        ////// <summary>
        ///// 重载去掉标题栏
        ///// </summary>
        //protected override CreateParams CreateParams
        //{
        //    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.Style &= ~NativeMethods.WSCaption;
        //        if (this.FormBorderStyle != FormBorderStyle.None)
        //            cp.Style |= NativeMethods.WSSizeBox;
        //        CaptionBar.Caption = this.Text;
        //        CaptionBar.MaxButtonVisible = this.MaximizeBox;
        //        CaptionBar.MinButtonVisible = this.MinimizeBox;
        //        return cp;
        //    }
        //}
        #endregion

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // NoCaptionBarForm
        //    // 
        //    this.ClientSize = new System.Drawing.Size(292, 266);
        //    this.Name = "NoCaptionBarForm";
        //    this.ResumeLayout(false);
        //}

        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //}
    }
}

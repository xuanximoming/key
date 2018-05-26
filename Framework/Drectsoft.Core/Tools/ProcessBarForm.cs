using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace DrectSoft.Core
{
    public partial class ProcessBarForm : Form
    {
        public ProcessBarForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(ProcessBarForm_Load);
            this.Show();
            this.Hide();
        }

        private void ProcessBarForm_Load(object sender, EventArgs e)
        {
            //Start();
        }

        /// <summary>
        /// 启动
        /// </summary>
        private void Start(String strShowText)
        {
            if (this != null)
            {
                this.Show();
                this.BringToFront();
                this.processBarExtender1.Start();
                this.label1.Text = strShowText;
                Thread.Sleep(1000);
            }

        }

        /// <summary>
        /// 设置显示文本
        /// </summary>
        /// <param name="strShowText"></param>
        public void SetShowText(String strShowText)
        {
            Shows show = new Shows(Start);
            IAsyncResult asyncResult = show.BeginInvoke(strShowText, null, null);
            show.EndInvoke(asyncResult);
        }

        /// <summary>
        /// 隐藏FORM
        /// </summary>
        public void HideForm()
        {
            processBarExtender1.Position = 100;
            this.Hide();
        }

        protected override void OnClosed(EventArgs e)
        {
            processBarExtender1.Close();
            base.OnClosed(e);
        }

        /// <summary>
        /// 防止调用CLOSE事件，在DISPOSE里释放资源
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            processBarExtender1.Position = 100;
            this.Hide();
        }


        private delegate void Shows(String strShowText);
    }
}

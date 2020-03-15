using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace DrectSoft.MainFrame.Login
{
    public partial class ConnectionConfig : DevBaseForm, ICustomMessageBox
    {

        string connection = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4};";
        DSMessageBox m_messagebox;
        public ICustomMessageBox CustomMessageBox
        {
            get { return this; }
        }

        private DSMessageBox MessageBoxInstance
        {
            get
            {
                if (m_messagebox == null)
                    m_messagebox = new DSMessageBox();
                return m_messagebox;
            }
        }

        public DialogResult MessageShow(string message, CustomMessageBoxKind kind)
        {

            Collection<string> messageInfos = new Collection<string>();

            messageInfos.Add(message);

            MessageBoxInstance.SetMessageInfo(messageInfos);
            MessageBoxInstance.SetButtonInfo(kind);
            return m_messagebox.ShowDialog();
        }

        public DialogResult MessageShow(string message)
        {
            return MessageShow(message, CustomMessageBoxKind.InformationOk);
        }


        public ConnectionConfig()
        {
            InitializeComponent();
        }

        private void sbsave_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (teip.Text.Trim() == "" || teport.Text.Trim() == "" || tecase.Text.Trim() == "" || teusername.Text.Trim() == "" || tepassword.Text.Trim() == "")
                {
                    MessageShow("连接字符有误！");
                    return;
                }

                if (!PublicClass.Ping(teip.Text.Trim()))
                {
                    MessageShow("IP地址无法ping通，请确认，或直接修改配置文件！");
                    return;
                }
                string connectionstring = string.Format(connection, teip.Text.Trim(), teport.Text.Trim(), tecase.Text.Trim(), teusername.Text.Trim(), tepassword.Text.Trim());
                PublicClass.ConnectionStringsAdd("EMRDB", connectionstring, "System.Data.OracleClient");
                MessageShow("配置信息保存成功！");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageShow(ex.Message);
            }

        }
    }
}

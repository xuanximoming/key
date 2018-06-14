using System.Windows.Forms;

namespace DrectSoft.Core.BirthProcess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MainForm mainf = new MainForm();
            this.Controls.Add(mainf);
        }
    }
}

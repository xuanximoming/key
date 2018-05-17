using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;


namespace DrectSoft.SysTableEdit
{
    public partial class FormMain : XtraForm, IStartPlugIn
    {
        private IEmrHost m_app;

        public FormMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造树菜单中使用的数据源
        /// </summary>
        /// <returns></returns>
        private DataTable CreateTreeSource()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID", Type.GetType("System.String"));
            DataColumn dcName = new DataColumn("NAME", Type.GetType("System.String"));

            dt.Columns.Add(dcID);
            dt.Columns.Add(dcName);

            DataRow newrow = dt.NewRow();
            newrow["ID"] = "DiagnosisOfChinese";
            newrow["NAME"] = "中医诊断库";
            dt.Rows.Add(newrow);

            newrow = dt.NewRow();
            newrow["ID"] = "Diagnosis";
            newrow["NAME"] = "诊断库";
            dt.Rows.Add(newrow);

            newrow = dt.NewRow();
            newrow["ID"] = "DiseaseCFG";
            newrow["NAME"] = "病种设置库";
            dt.Rows.Add(newrow);

            newrow = dt.NewRow();
            newrow["ID"] = "Surgery";
            newrow["NAME"] = "手术代码库";
            dt.Rows.Add(newrow);

            newrow = dt.NewRow();
            newrow["ID"] = "Toxicosis";
            newrow["NAME"] = "损伤中毒库";
            dt.Rows.Add(newrow);

            newrow = dt.NewRow();
            newrow["ID"] = "Tumor";
            newrow["NAME"] = "肿瘤库";
            dt.Rows.Add(newrow);

            return dt;
        }

        /// <summary>
        /// 构造树
        /// </summary>
        private void MakeTree()
        {
            DataTable dt = CreateTreeSource();

            foreach (DataRow dr in dt.Rows)
            {
                TreeListNode node = treeListTableName.AppendNode(new object[] { dr["ID"], dr["NAME"] }, null);
                node.Tag = dr["ID"].ToString();
            }
        }



        public IPlugIn Run(IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;
            return plg;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            MakeTree();
            BingUCControl("DiagnosisOfChinese");
        }

        private void treeListTableName_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }
            if (e.Node.Focused)
            {
                if (e.Node.Tag == null)
                    return;
                if (e.Node.Tag.ToString() == null)
                    return;

                BingUCControl(e.Node.Tag.ToString());
            }
        }

        /// <summary>
        /// 根据表名加载对应维护模块
        /// </summary>
        /// <param name="tablename"></param>
        private void BingUCControl(string tablename)
        {
            switch (tablename)
            {
                case "Diagnosis":
                    UCEditDiagnosis ucdiagnosis = new UCEditDiagnosis(m_app);
                    this.Controlmain.Controls.Clear();
                    this.Controlmain.Controls.Add(ucdiagnosis);
                    ucdiagnosis.Dock = DockStyle.Fill;
                    break;

                case "DiagnosisOfChinese":
                    UCEditDiagnosisOfChinese ucdiagnosisofchinese = new UCEditDiagnosisOfChinese(m_app);
                    this.Controlmain.Controls.Clear();
                    this.Controlmain.Controls.Add(ucdiagnosisofchinese);
                    ucdiagnosisofchinese.Dock = DockStyle.Fill;
                    break;

                case "DiseaseCFG":
                    UCEditDiseaseCFG ucEditDiseaseCFG = new UCEditDiseaseCFG(m_app);
                    this.Controlmain.Controls.Clear();
                    this.Controlmain.Controls.Add(ucEditDiseaseCFG);
                    ucEditDiseaseCFG.Dock = DockStyle.Fill;
                    break;

                case "Surgery":
                    UCEditSurgery ucEditSurgery = new UCEditSurgery(m_app);
                    this.Controlmain.Controls.Clear();
                    this.Controlmain.Controls.Add(ucEditSurgery);
                    ucEditSurgery.Dock = DockStyle.Fill;
                    break;

                case "Toxicosis":
                    UCEditToxicosis ucEditToxicosis = new UCEditToxicosis(m_app);
                    this.Controlmain.Controls.Clear();
                    this.Controlmain.Controls.Add(ucEditToxicosis);
                    ucEditToxicosis.Dock = DockStyle.Fill;
                    break;

                case "Tumor":
                    UCEditTumor ucEditTumor = new UCEditTumor(m_app);
                    this.Controlmain.Controls.Clear();
                    this.Controlmain.Controls.Add(ucEditTumor);
                    ucEditTumor.Dock = DockStyle.Fill;
                    break;


            }
        }
    }
}
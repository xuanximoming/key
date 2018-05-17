using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.MainFrame
{
    public class FormItemFunction : XtraForm
    {
        private DeptWardInfo _selectDept;

        private IContainer components = null;

        private PanelControl panel1;

        private Label labelChangeWard;

        private SimpleButton buttonCancel;

        private SimpleButton buttonOK;

        private ComboBoxEdit comboBoxEdit_Dt;

        private LabelControl labelControl1;

        public DeptWardInfo SelectDept
        {
            get
            {
                return this._selectDept;
            }
        }

        public FormItemFunction()
        {
            this.InitializeComponent();
        }

        private void FormItemFunction_Load(object sender, EventArgs e)
        {
            this.InitWardInfo();
            this.SetChineseIEM();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.comboBoxEdit_Dt.SelectedItem != null && this.comboBoxEdit_Dt.SelectedItem is DeptWardInfo)
            {
                this._selectDept = (DeptWardInfo)this.comboBoxEdit_Dt.SelectedItem;
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                FormMain.Instance.MessageShow("请选择病区!", CustomMessageBoxKind.InformationOk);
            }
        }

        private void InitWardInfo()
        {
            try
            {
                string config = AppConfigReader.GetAppConfig("ChangeWardLookAllWard").Config;
                string[] array = config.Split(new char[]
				{
					','
				});
                string[] array2 = FormMain.Instance.User.GWCodes.Split(new char[]
				{
					','
				});
                bool flag = false;
                string commandText = "select id from jobs ";
                ArrayList arrayList = new ArrayList();
                DataTable dataTable = ((IEmrHost)FormMain.Instance).SqlHelper.ExecuteDataTable(commandText);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        arrayList.Add(dataTable.Rows[i]["ID"].ToString());
                    }
                }
                string[] array3 = array;
                for (int j = 0; j < array3.Length; j++)
                {
                    string text = array3[j];
                    string[] array4 = array2;
                    for (int k = 0; k < array4.Length; k++)
                    {
                        string b = array4[k];
                        if (text == b && text != "")
                        {
                            if (arrayList.Contains(text))
                            {
                                flag = true;
                            }
                        }
                    }
                }
                if (flag)
                {
                    string commandText2 = "SELECT a.id deptid, a.name deptname, b.id wardid, b.name wardname \r\n                                                 FROM department a, ward b, dept2ward c\r\n                                                WHERE a.id = c.deptid and b.id = c.wardid and a.valid = '1' and b.valid = '1'\r\n                                             ORDER BY a.name ";
                    DataTable dataTable2 = ((IEmrHost)FormMain.Instance).SqlHelper.ExecuteDataTable(commandText2);
                    foreach (DataRow dataRow in dataTable2.Rows)
                    {
                        DeptWardInfo current = new DeptWardInfo(dataRow["deptid"].ToString(), dataRow["deptname"].ToString(), dataRow["wardid"].ToString(), dataRow["wardname"].ToString());
                        this.comboBoxEdit_Dt.Properties.Items.Add(current);
                    }
                }
                else
                {
                    foreach (DeptWardInfo current in FormMain.Instance.User.RelateDeptWards)
                    {
                        this.comboBoxEdit_Dt.Properties.Items.Add(current);
                    }
                }
            }
            catch (SqlException message)
            {
                FormMain.Instance.Logger.Error(message);
            }
            catch (Exception message2)
            {
                FormMain.Instance.Logger.Error(message2);
            }
        }

        private void SetChineseIEM()
        {
            foreach (InputLanguage inputLanguage in InputLanguage.InstalledInputLanguages)
            {
                if (inputLanguage.LayoutName.IndexOf("拼音") >= 0)
                {
                    InputLanguage.CurrentInputLanguage = inputLanguage;
                    break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemFunction));
            this.panel1 = new DevExpress.XtraEditors.PanelControl();
            this.comboBoxEdit_Dt = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.buttonOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelChangeWard = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Dt.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel1.Controls.Add(this.comboBoxEdit_Dt);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.LookAndFeel.SkinName = "Blue";
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 138);
            this.panel1.TabIndex = 15;
            // 
            // comboBoxEdit_Dt
            // 
            this.comboBoxEdit_Dt.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.comboBoxEdit_Dt.Location = new System.Drawing.Point(79, 42);
            this.comboBoxEdit_Dt.Name = "comboBoxEdit_Dt";
            this.comboBoxEdit_Dt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_Dt.Properties.ImmediatePopup = true;
            this.comboBoxEdit_Dt.Size = new System.Drawing.Size(210, 20);
            this.comboBoxEdit_Dt.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 44);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 22;
            this.labelControl1.Text = "科室切换";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonCancel.Appearance.Options.UseFont = true;
            this.buttonCancel.Location = new System.Drawing.Point(186, 89);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(89, 25);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消(&C)";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOK.Appearance.Options.UseFont = true;
            this.buttonOK.Location = new System.Drawing.Point(65, 89);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(89, 25);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "确定(&O)";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelChangeWard
            // 
            this.labelChangeWard.AutoSize = true;
            this.labelChangeWard.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelChangeWard.Location = new System.Drawing.Point(39, 45);
            this.labelChangeWard.Name = "labelChangeWard";
            this.labelChangeWard.Size = new System.Drawing.Size(63, 14);
            this.labelChangeWard.TabIndex = 19;
            this.labelChangeWard.Text = "切换病区";
            this.labelChangeWard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormItemFunction
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 138);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "Blue";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormItemFunction";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统选项设置";
            this.Load += new System.EventHandler(this.FormItemFunction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Dt.Properties)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

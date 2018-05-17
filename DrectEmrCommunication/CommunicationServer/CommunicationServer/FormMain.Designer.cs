namespace YidanSoft.Core.Communication
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnBeginListener = new System.Windows.Forms.Button();
            this.listBoxMessage = new System.Windows.Forms.ListBox();
            this.dataGridViewAllCurrentUser = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonHide = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAllCurrentUser)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBeginListener
            // 
            this.btnBeginListener.Location = new System.Drawing.Point(12, 11);
            this.btnBeginListener.Name = "btnBeginListener";
            this.btnBeginListener.Size = new System.Drawing.Size(297, 27);
            this.btnBeginListener.TabIndex = 0;
            this.btnBeginListener.Text = "开始监听";
            this.btnBeginListener.UseVisualStyleBackColor = true;
            this.btnBeginListener.Click += new System.EventHandler(this.btnBeginListener_Click);
            // 
            // listBoxMessage
            // 
            this.listBoxMessage.FormattingEnabled = true;
            this.listBoxMessage.HorizontalScrollbar = true;
            this.listBoxMessage.ItemHeight = 12;
            this.listBoxMessage.Location = new System.Drawing.Point(12, 44);
            this.listBoxMessage.Name = "listBoxMessage";
            this.listBoxMessage.ScrollAlwaysVisible = true;
            this.listBoxMessage.Size = new System.Drawing.Size(297, 256);
            this.listBoxMessage.TabIndex = 1;
            // 
            // dataGridViewAllCurrentUser
            // 
            this.dataGridViewAllCurrentUser.AllowUserToAddRows = false;
            this.dataGridViewAllCurrentUser.AllowUserToDeleteRows = false;
            this.dataGridViewAllCurrentUser.AllowUserToResizeColumns = false;
            this.dataGridViewAllCurrentUser.AllowUserToResizeRows = false;
            this.dataGridViewAllCurrentUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAllCurrentUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridViewAllCurrentUser.Location = new System.Drawing.Point(315, 44);
            this.dataGridViewAllCurrentUser.Name = "dataGridViewAllCurrentUser";
            this.dataGridViewAllCurrentUser.RowHeadersVisible = false;
            this.dataGridViewAllCurrentUser.RowTemplate.Height = 23;
            this.dataGridViewAllCurrentUser.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewAllCurrentUser.Size = new System.Drawing.Size(104, 255);
            this.dataGridViewAllCurrentUser.TabIndex = 2;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "USERNAME";
            this.Column1.HeaderText = "用户列表";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "USERID";
            this.Column2.HeaderText = "用户ID";
            this.Column2.Name = "Column2";
            this.Column2.Visible = false;
            // 
            // buttonHide
            // 
            this.buttonHide.Location = new System.Drawing.Point(316, 11);
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.Size = new System.Drawing.Size(103, 27);
            this.buttonHide.TabIndex = 3;
            this.buttonHide.Text = "隐藏";
            this.buttonHide.UseVisualStyleBackColor = true;
            this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "双击弹出消息处理服务器端";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 314);
            this.Controls.Add(this.buttonHide);
            this.Controls.Add(this.dataGridViewAllCurrentUser);
            this.Controls.Add(this.listBoxMessage);
            this.Controls.Add(this.btnBeginListener);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "消息处理服务器端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAllCurrentUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBeginListener;
        private System.Windows.Forms.ListBox listBoxMessage;
        private System.Windows.Forms.DataGridView dataGridViewAllCurrentUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Button buttonHide;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}


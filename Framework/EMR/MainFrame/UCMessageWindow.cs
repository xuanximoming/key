using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.MainFrame
{
    public class UCMessageWindow : XtraUserControl
    {
        private MessageWindowState m_WindowState = MessageWindowState.Bottom;

        private readonly int m_WaitTime = 200;

        private int m_HaveWaiteTime = 0;

        private bool m_IsHover = false;

        private bool m_IsClickCloseButton = false;

        private int m_OffsetHeight = 20;

        private DataTable m_DataSource = new DataTable();

        private readonly string r_ID = "ID";

        private readonly string r_GCTypeID = "TYPEID";

        private readonly string r_GCTypeName = "TYPENAME";

        private readonly string r_GCContent = "CONTENT";

        private readonly string r_GCFlag = "FLAG";

        private readonly string r_OLD = "OLD";

        private readonly string r_NEW = "NEW";

        private StringFormat m_sf = new StringFormat();

        private bool m_IsShowMessageWindow = true;

        private IContainer components = null;

        private GroupControl groupControlMessageForm;

        private PictureBox pictureBoxExit;

        private Timer timerMoving;

        private GridControl gridControlMessage;

        private GridView gridViewMessage;

        private GridColumn gc_typeid;

        private GridColumn gc_typename;

        private GridColumn gc_content;

        public bool IsShowMessageWindow
        {
            get
            {
                return this.m_IsShowMessageWindow;
            }
        }

        public UCMessageWindow(string isShowMessageWindow, string offsetHeight, string waitTime)
        {
            this.InitializeComponent();
            this.m_IsShowMessageWindow = (isShowMessageWindow == "1");
            this.m_OffsetHeight = Convert.ToInt32(offsetHeight.Trim());
            this.m_WaitTime = Convert.ToInt32(waitTime.Trim());
            this.InitVariable();
            this.InitDataSource();
            if (this.m_IsShowMessageWindow)
            {
                this.timerMoving.Start();
            }
        }

        private void InitDataSource()
        {
            this.m_DataSource = new DataTable();
            this.m_DataSource.Columns.Add(this.r_ID);
            this.m_DataSource.Columns.Add(this.r_GCTypeID);
            this.m_DataSource.Columns.Add(this.r_GCTypeName);
            this.m_DataSource.Columns.Add(this.r_GCContent);
            this.m_DataSource.Columns.Add(this.r_GCFlag);
            this.gridControlMessage.DataSource = this.m_DataSource;
        }

        private void groupControlMessageForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString("欢迎进入电子病历系统!", this.Font, new SolidBrush(Color.Black), new RectangleF(0f, 20f, (float)base.Width, (float)base.Height - 20f), this.m_sf);
        }

        private void InitVariable()
        {
            this.m_sf.Alignment = StringAlignment.Center;
            this.m_sf.LineAlignment = StringAlignment.Center;
            this.m_IsHover = false;
            this.m_WindowState = MessageWindowState.MovingUp;
        }

        public void ShowMessageWindow(Form parentForm, DataTable dt, bool isClear)
        {
            if (isClear)
            {
                this.m_DataSource.Clear();
            }
            else
            {
                this.gridControlMessage.Visible = true;
            }
            if (dt != null && dt.Rows.Count != 0)
            {
                this.EnsureHasSpecifyCol(dt);
                foreach (DataRow dataRow in this.m_DataSource.Rows)
                {
                    dataRow[this.r_GCFlag] = this.r_OLD;
                }
                foreach (DataRow dataRow in dt.Rows)
                {
                    dataRow[this.r_GCFlag] = this.r_NEW;
                    DataRow dataRow2 = this.m_DataSource.NewRow();
                    dataRow2.ItemArray = dataRow.ItemArray;
                    this.m_DataSource.Rows.Add(dataRow2);
                }
                this.gridViewMessage.MoveLast();
                this.InitMessageWindow(parentForm);
            }
        }

        private void EnsureHasSpecifyCol(DataTable dt)
        {
            if (!dt.Columns.Contains(this.r_GCFlag))
            {
                dt.Columns.Add(this.r_GCFlag);
            }
        }

        private void InitMessageWindow(Form parentForm)
        {
            if (!parentForm.Controls.Contains(this))
            {
                parentForm.Controls.Add(this);
                Point location = new Point(1, Screen.PrimaryScreen.WorkingArea.Height + this.m_OffsetHeight);
                base.Location = location;
                base.BringToFront();
            }
            this.m_WindowState = MessageWindowState.MovingUp;
        }

        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            this.m_WindowState = MessageWindowState.MovingDown;
            this.m_IsClickCloseButton = true;
        }

        private void groupControlMessageForm_MouseEnter(object sender, EventArgs e)
        {
            this.m_IsHover = true;
            if (!this.m_IsClickCloseButton)
            {
                this.m_WindowState = MessageWindowState.MovingUp;
            }
        }

        private void groupControlMessageForm_MouseLeave(object sender, EventArgs e)
        {
            this.m_IsHover = false;
            this.m_HaveWaiteTime = 0;
        }

        private void timerMoving_Tick(object sender, EventArgs e)
        {
            base.BringToFront();
            if (this.m_WindowState == MessageWindowState.MovingUp)
            {
                this.m_HaveWaiteTime = 0;
                if (base.Location.Y > Screen.PrimaryScreen.WorkingArea.Height - base.Height)
                {
                    Point location = base.Location;
                    base.Location = new Point(location.X, location.Y - 3);
                }
                else
                {
                    this.m_WindowState = MessageWindowState.Top;
                }
            }
            else if (this.m_WindowState == MessageWindowState.MovingDown)
            {
                this.m_HaveWaiteTime = 0;
                if (base.Location.Y < Screen.PrimaryScreen.WorkingArea.Height + this.m_OffsetHeight)
                {
                    if (!this.m_IsHover || this.m_IsClickCloseButton)
                    {
                        Point location = base.Location;
                        base.Location = new Point(location.X, location.Y + 3);
                    }
                }
                else
                {
                    this.m_WindowState = MessageWindowState.Bottom;
                    this.m_IsClickCloseButton = false;
                }
            }
            else if (this.m_WindowState == MessageWindowState.Top)
            {
                if (this.m_HaveWaiteTime >= this.m_WaitTime)
                {
                    this.m_WindowState = MessageWindowState.MovingDown;
                }
                else if (!this.m_IsHover)
                {
                    this.m_HaveWaiteTime++;
                }
                else
                {
                    this.m_HaveWaiteTime = 0;
                }
            }
        }

        private void gridViewMessage_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            DataRowView dataRowView = this.gridViewMessage.GetRow(e.RowHandle) as DataRowView;
            if (dataRowView != null)
            {
                if (dataRowView[this.r_GCFlag].ToString() == this.r_OLD)
                {
                    e.Appearance.BackColor = Color.White;
                    e.Appearance.ForeColor = Color.Gray;
                }
                else if (dataRowView[this.r_GCFlag].ToString() == this.r_NEW)
                {
                    e.Appearance.BackColor = Color.Blue;
                    e.Appearance.ForeColor = Color.Yellow;
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
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(UCMessageWindow));
            this.groupControlMessageForm = new GroupControl();
            this.gridControlMessage = new GridControl();
            this.gridViewMessage = new GridView();
            this.gc_typeid = new GridColumn();
            this.gc_typename = new GridColumn();
            this.gc_content = new GridColumn();
            this.pictureBoxExit = new PictureBox();
            this.timerMoving = new Timer(this.components);
            ((ISupportInitialize)this.groupControlMessageForm).BeginInit();
            this.groupControlMessageForm.SuspendLayout();
            ((ISupportInitialize)this.gridControlMessage).BeginInit();
            ((ISupportInitialize)this.gridViewMessage).BeginInit();
            ((ISupportInitialize)this.pictureBoxExit).BeginInit();
            base.SuspendLayout();
            this.groupControlMessageForm.AppearanceCaption.Font = new Font("宋体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.groupControlMessageForm.AppearanceCaption.ForeColor = Color.FromArgb(192, 0, 0);
            this.groupControlMessageForm.AppearanceCaption.Options.UseFont = true;
            this.groupControlMessageForm.AppearanceCaption.Options.UseForeColor = true;
            this.groupControlMessageForm.Controls.Add(this.gridControlMessage);
            this.groupControlMessageForm.Controls.Add(this.pictureBoxExit);
            this.groupControlMessageForm.Dock = DockStyle.Fill;
            this.groupControlMessageForm.Location = new Point(0, 0);
            this.groupControlMessageForm.Name = "groupControlMessageForm";
            this.groupControlMessageForm.Size = new Size(313, 184);
            this.groupControlMessageForm.TabIndex = 1;
            this.groupControlMessageForm.Text = "系统提示";
            this.groupControlMessageForm.Paint += new PaintEventHandler(this.groupControlMessageForm_Paint);
            this.groupControlMessageForm.MouseEnter += new EventHandler(this.groupControlMessageForm_MouseEnter);
            this.groupControlMessageForm.MouseLeave += new EventHandler(this.groupControlMessageForm_MouseLeave);
            this.gridControlMessage.Dock = DockStyle.Fill;
            this.gridControlMessage.Location = new Point(2, 24);
            this.gridControlMessage.MainView = this.gridViewMessage;
            this.gridControlMessage.Name = "gridControlMessage";
            this.gridControlMessage.Size = new Size(309, 158);
            this.gridControlMessage.TabIndex = 1;
            this.gridControlMessage.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewMessage
			});
            this.gridViewMessage.Columns.AddRange(new GridColumn[]
			{
				this.gc_typeid,
				this.gc_typename,
				this.gc_content
			});
            this.gridViewMessage.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            this.gridViewMessage.GridControl = this.gridControlMessage;
            this.gridViewMessage.Name = "gridViewMessage";
            this.gridViewMessage.OptionsBehavior.Editable = false;
            this.gridViewMessage.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewMessage.OptionsFilter.AllowFilterEditor = false;
            this.gridViewMessage.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewMessage.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewMessage.OptionsView.ShowColumnHeaders = false;
            this.gridViewMessage.OptionsView.ShowGroupPanel = false;
            this.gridViewMessage.OptionsView.ShowIndicator = false;
            this.gridViewMessage.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewMessage_CustomDrawCell);
            this.gc_typeid.Caption = "类型ID";
            this.gc_typeid.FieldName = "TYPEID";
            this.gc_typeid.Name = "gc_typeid";
            this.gc_typename.Caption = "类型名称";
            this.gc_typename.FieldName = "TYPENAME";
            this.gc_typename.Name = "gc_typename";
            this.gc_typename.Visible = true;
            this.gc_typename.VisibleIndex = 0;
            this.gc_typename.Width = 60;
            this.gc_content.Caption = "内容";
            this.gc_content.FieldName = "CONTENT";
            this.gc_content.Name = "gc_content";
            this.gc_content.Visible = true;
            this.gc_content.VisibleIndex = 1;
            this.gc_content.Width = 249;
            this.pictureBoxExit.BackgroundImage = (Image)resources.GetObject("pictureBoxExit.BackgroundImage");
            this.pictureBoxExit.BackgroundImageLayout = ImageLayout.Stretch;
            this.pictureBoxExit.Cursor = Cursors.Hand;
            this.pictureBoxExit.Location = new Point(289, 3);
            this.pictureBoxExit.Name = "pictureBoxExit";
            this.pictureBoxExit.Size = new Size(18, 18);
            this.pictureBoxExit.TabIndex = 0;
            this.pictureBoxExit.TabStop = false;
            this.pictureBoxExit.Click += new EventHandler(this.pictureBoxExit_Click);
            this.timerMoving.Interval = 10;
            this.timerMoving.Tick += new EventHandler(this.timerMoving_Tick);
            base.AutoScaleDimensions = new SizeF(7f, 14f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(this.groupControlMessageForm);
            base.Name = "UCMessageWindow";
            base.Size = new Size(313, 184);
            ((ISupportInitialize)this.groupControlMessageForm).EndInit();
            this.groupControlMessageForm.ResumeLayout(false);
            ((ISupportInitialize)this.gridControlMessage).EndInit();
            ((ISupportInitialize)this.gridViewMessage).EndInit();
            ((ISupportInitialize)this.pictureBoxExit).EndInit();
            base.ResumeLayout(false);
        }
    }
}

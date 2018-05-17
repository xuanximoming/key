using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Security;
using System.Security.Permissions;

namespace DrectSoft.Core
{
    #region MyFormVisualStyle
    /// <summary>
    /// 窗口可视型枚举项
    /// </summary>
    public enum MyFormVisualStyle
    {
        /// <summary>
        /// 空,默认当前
        /// </summary>
        Null = 0,

        /// <summary>
        /// _3D
        /// </summary>
        ThreeD = 1,

        /// <summary>
        /// _2D
        /// </summary>
        TwoD = 2
    }
    #endregion

    #region FormStyleExtender
    /// <summary>
    /// 扩展窗口可视
    /// </summary>
    //[ProvideProperty("UseFormStyle", typeof(Control))]
    [ProvideProperty("Style", typeof(Form))]
    [ProvideProperty("StyleFile", typeof(Form))]
    public partial class FormStyleExtender : Component, IExtenderProvider
    {
        //保存属性表
        private Hashtable properties = new Hashtable();

        /// <summary>
        /// ctor
        /// </summary>
        public FormStyleExtender()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ctor2
        /// </summary>
        /// <param name="container"></param>
        public FormStyleExtender(IContainer container)
        {
            if (container != null) container.Add(this);
            InitializeComponent();
        }

        private Properties EnsurePropertiesExists(object key)
        {
            Properties p = (Properties)properties[key];

            if (p == null)
            {
                p = new Properties();

                properties[key] = p;
            }

            return p;
        }

        #region Properties
        private class Properties
        {

            public VisualStyle FormVisualStyle;
            public bool UseFormStyle;
            public string FormStyleFile;

            public Properties()
            {
                FormVisualStyle = new VisualStyle();
                UseFormStyle = true;
                FormStyleFile = string.Empty;
            }
        }

        #endregion

        #region IExtenderProvider Members

        /// <summary>
        /// 判断是否可扩展
        /// </summary>
        /// <param name="extendee"></param>
        /// <returns></returns>
        public bool CanExtend(object extendee)
        {
            if (extendee is Control)
                return true;
            else
                return false;
        }

        #endregion

        #region provide property "Style"

        /// <summary>
        /// getStyle
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Description("供选择的可视型")]
        [Category("VisualStyle")]
        public MyFormVisualStyle GetStyle(Form form)
        {
            //if (form == null) return FormVisualStyle.Null;
            //return EnsurePropertiesExists(form).FormVisualStyle.styleId;
            return EnsurePropertiesExists(form).FormVisualStyle.VisualStyleId;
        }

        /// <summary>
        /// setStyle
        /// </summary>
        /// <param name="form"></param>
        /// <param name="value"></param>
        public void SetStyle(Control form, MyFormVisualStyle value)
        {
            if (form == null) throw new ArgumentNullException("form");

            EnsurePropertiesExists(form).FormVisualStyle = new VisualStyle(value);
            if (DesignMode)
            {
                form.Paint -= new PaintEventHandler(form_Paint);
                form.Paint += new PaintEventHandler(form_Paint);
                form.Invalidate();
            }
        }

        void form_Paint(object sender, PaintEventArgs e)
        {
            Form form = sender as Form;
            Properties pro = properties[form] as Properties;
            foreach (Control control in form.Controls)
            {
                Panel panel = control as Panel;
                if (panel != null)
                {
                    panel.BackColor = pro.FormVisualStyle.BGColor;
                    panel.BorderStyle = pro.FormVisualStyle.ButtonStyle;
                }
                else
                {
                    Button button = control as Button;
                    if (button != null)
                    {
                        button.BackColor = pro.FormVisualStyle.BGColor;
                    }
                }
            }
        }

        #endregion

        #region provide property "UseFormStyle"

        /// <summary>
        /// getUseFormStyle
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        [Description("是否使用窗口可视型")]
        [Category("VisualStyle")]
        public bool GetUseFormStyle(Control control)
        {
            return EnsurePropertiesExists(control).UseFormStyle;
        }

        /// <summary>
        /// setUseFormStyle
        /// </summary>
        /// <param name="control"></param>
        /// <param name="useFormStyle"></param>
        public void SetUseFormStyle(Control control, bool useFormStyle)
        {
            EnsurePropertiesExists(control).UseFormStyle = useFormStyle;
        }
        #endregion

        #region provide property "StyleFile"

        /// <summary>
        /// getStyleFile
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Description("可视型设置文件")]
        [Category("VisualStyle")]
        [Editor("DrectSoft.Framework.XmlFileSelectEditor", typeof(UITypeEditor))]
        public string GetStyleFile(Form form)
        {
            return EnsurePropertiesExists(form).FormStyleFile;
        }

        /// <summary>
        /// setStyleFile
        /// </summary>
        /// <param name="form"></param>
        /// <param name="value"></param>
        public void SetStyleFile(Form form, string value)
        {
            EnsurePropertiesExists(form).FormStyleFile = value;
        }

        #endregion
    }

    #endregion

    #region class VisualStyle
    /// <summary>
    /// VisualStyle
    /// </summary>
    public class VisualStyle
    {
        private Color _bgColor = Color.FromKnownColor(KnownColor.Control);

        /// <summary>
        /// BackgroundColor
        /// </summary>
        public Color BGColor
        {
            get { return _bgColor; }
            set { _bgColor = value; }
        }

        private BorderStyle _buttonStyle = BorderStyle.Fixed3D;

        /// <summary>
        /// button style
        /// </summary>
        public BorderStyle ButtonStyle
        {
            get { return _buttonStyle; }
            set { _buttonStyle = value; }
        }

        private MyFormVisualStyle _visualStyleId = MyFormVisualStyle.Null;

        /// <summary>
        /// visual style id
        /// </summary>
        public MyFormVisualStyle VisualStyleId
        {
            get { return _visualStyleId; }
            set { _visualStyleId = value; }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public VisualStyle()
            : this(Color.FromKnownColor(KnownColor.Control), BorderStyle.Fixed3D)
        {
        }

        /// <summary>
        /// ctor2
        /// </summary>
        /// <param name="bgColor"></param>
        /// <param name="bStyle"></param>
        public VisualStyle(Color bgColor, BorderStyle bStyle)
        {
            SetFields(bgColor, bStyle);
        }

        private void SetFields(Color bgColor, BorderStyle bStyle)
        {
            _bgColor = bgColor;
            _buttonStyle = bStyle;
        }

        /// <summary>
        /// ctor3
        /// </summary>
        /// <param name="styleId"></param>
        public VisualStyle(MyFormVisualStyle styleId)
        {
            _visualStyleId = styleId;
            if (styleId == MyFormVisualStyle.Null || styleId == MyFormVisualStyle.ThreeD)
            {
                SetFields(Color.FromKnownColor(KnownColor.Control), BorderStyle.Fixed3D);
            }

            if (styleId == MyFormVisualStyle.TwoD)
            {
                SetFields(SystemColors.Window, BorderStyle.FixedSingle);
            }
        }
    }
    #endregion

    #region XmlFileSelectEditor

    /// <summary>
    /// XmlFileSelectEditor
    /// </summary>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust"), PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class XmlFileSelectEditor : UITypeEditor
    {
        private FileDialog fileDialog;

        /// <summary>
        /// ctor
        /// </summary>
        public XmlFileSelectEditor()
        { 
        }

        /// <summary>
        /// 文件后缀
        /// </summary>
        /// <returns></returns>
        //protected static string CreateFilterEntry(XmlFileSelectEditor e)
        protected static string CreateFilterEntry()
        {
            return "可视型文件(*.xml)|*.xml";
        }

        /// <summary>
        /// override EditValue
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                if (this.fileDialog == null)
                {
                    this.fileDialog = new OpenFileDialog();
                    string text1 = XmlFileSelectEditor.CreateFilterEntry();
                    this.fileDialog.Filter = text1;
                }
                IntPtr ptr1 = UnsafeNativeMethods.GetFocus();
                try
                {
                    if (this.fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        value = this.fileDialog.FileName; 
                    }
                }
                finally
                {
                    if (ptr1 != IntPtr.Zero)
                    {
                        UnsafeNativeMethods.SetFocus(new System.Runtime.InteropServices.HandleRef(null, ptr1));
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// override GetEditStyle
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    [System.Security.SuppressUnmanagedCodeSecurity]
    internal class UnsafeNativeMethods
    {
        // Methods
        private UnsafeNativeMethods()
        { }

        //[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        //public static extern int ClientToScreen(System.Runtime.InteropServices.HandleRef hWnd, [System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out] NativeMethodsPoint pt);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetFocus();
        //[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        //public static extern void NotifyWinEvent(int winEvent, System.Runtime.InteropServices.HandleRef hwnd, int objType, int objID);
        //[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        //public static extern int ScreenToClient(System.Runtime.InteropServices.HandleRef hWnd, [System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out] NativeMethodsPoint pt);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr SetFocus(System.Runtime.InteropServices.HandleRef hWnd);

        // Fields
        public const int OBJID_CLIENT = -4;
    }

    /// <summary>
    /// Nested Types
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public class NativeMethodsPoint
    {
        private int x;

        /// <summary>
        /// x
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        private int y;

        /// <summary>
        /// y
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public NativeMethodsPoint()
        { }

        /// <summary>
        /// ctor2
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public NativeMethodsPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    #endregion

}

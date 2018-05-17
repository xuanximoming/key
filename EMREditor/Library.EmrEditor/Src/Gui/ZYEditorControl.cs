using DrectSoft.Library.EmrEditor.Src.Actions;
using DrectSoft.Library.EmrEditor.Src.Common;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Library.EmrEditor.Src.Print;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ZYTextDocumentLib;



namespace DrectSoft.Library.EmrEditor.Src.Gui
{
    /// <summary>
    /// 电子病历文本文档编辑控件
    /// </summary>
    /// <remarks>本用户控件是从ZYViewControl继承过来的，是电子病历文本文档的查看和编辑界面
    /// 本控件内部有一个电子病历文本文档对象和一个电子病历知识库弹出式列表</remarks>
    /// <seealso>ZYCommon.ZYViewControl</seealso>
    /// <seealso>ZYTextDocumentLib.ZYTextDocument</seealso>
    /// 
    [Serializable]
    public class ZYEditorControl : ZYViewControl
    {
        /// <summary>
        /// 内部的电子病历文本文档对象
        /// </summary>
        protected ZYTextDocument myEMRDoc = null;
        /// <summary>
        /// 内置的电子病历文本文档对象
        /// </summary>
        public ZYTextDocument EMRDoc
        {
            get { return myEMRDoc; }
            set { myEMRDoc = value; }
        }

        #region wwj
        /// <summary>
        /// Add by wwj 2013-05-29
        /// 此配置针对平板，是否将单击作为双击处理
        /// 由于平板上双击操作不方便，所以这里通过单击的方式设置结构化元素的值
        /// true:  表示单击结构化元素的效果等同双击结构化元素
        /// </summary>
        public bool IsSingleClickAsDoubleClick
        {
            get { return m_IsSingleClickAsDoubleClick; }
            set { m_IsSingleClickAsDoubleClick = value; }
        }
        private bool m_IsSingleClickAsDoubleClick = false;
        /// <summary>
        /// 鼠标按下的位置
        /// </summary>
        private Point m_MouseDownPosition = new Point();
        #endregion

        /// <summary>
        /// 初始化对象,其内部实例化一个电子病历文本文档对象
        /// </summary>
        public ZYEditorControl()
        {
            SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true); //双缓冲
            this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true); //自行绘制
            this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true); //减少闪烁

            this.GraphicsUnit = System.Drawing.GraphicsUnit.Document; //定义度量单位
            this.BackColor = Color.FromArgb(105, 141, 189); //定义背景色

            //InitializeDocument();

            #region 注释内容（右键菜单）
            //base.lblInfo.Font = myEMRDoc.View.DefaultFont;

            //System.Collections.ArrayList myMenuList = new System.Collections.ArrayList();
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("undo"), "&U  撤销"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("redo"), "&R  重复"));
            //myMenuList.Add(new ZYActionMenuItem(null, null));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("cut"), "&T  剪切"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("copy"), "&C  复制"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("paste"), "&P  粘贴"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("delete"), "&D  删除"));
            //myMenuList.Add(new ZYActionMenuItem(null, null));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("insertimage"), "&I  插入图片"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("insertkb"), "&K  插入知识库"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("insertgongshi"), "&G  插入公式"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("insertdiv"), "插入文本块"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("delete"), "删除"));
            //myMenuList.Add(new ZYActionMenuItem(null, null));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("showall"), "&A  显示所有"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("logicdelete"), "&L  逻辑删除"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("showmark"), "&M  显示标记"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("enablesmarttag"), "&S  使用智能标签"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("converttexttoselect"), "转化为知识列表..."));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("setmultiselect"), "下拉列表复选"));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("divproperty"), "文本块属性..."));
            //myMenuList.Add(new ZYActionMenuItem(null, null));
            //myMenuList.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("aboutme"), "&B  关于..."));

            //myPopupMenu = new System.Windows.Forms.ContextMenu();
            //myPopupMenu.MenuItems.AddRange((System.Windows.Forms.MenuItem[])myMenuList.ToArray(typeof(System.Windows.Forms.MenuItem)));
            //this.ContextMenu = myPopupMenu;
            //myPopupMenu.Popup += new EventHandler(myPopupMenu_Popup);
            //this.InitSmartTagProvider();
            #endregion




            #region bwy :
            GenerateDefaultActions();
            GenerateKeys();
            SetChineseIEM();
            this.Cursor = System.Windows.Forms.Cursors.IBeam;

            #endregion bwy :
        }
        /// <summary>
        /// 必须在控件所在窗体或UserControl的构造函数中调用此方法，以实现控件文档的始化，和正常显示。为什么不把它直接写在控件的构造函数中呢？因为拖动控件到窗体上时会出错。
        /// </summary>
        public void InitializeDocument()
        {
            myEMRDoc = new ZYTextDocument(); //初始化文档对象并自动添加一个根元素
            myEMRDoc.OwnerControl = this;
            myEMRDoc.OnContentChanged += new EventHandler(OnContentChanged);
            //设置文档状态
            myEMRDoc.Info.DocumentModel = DocumentModel.Design;

            //读取注册表设置值
            Font font = new Font(ZYEditorControl.GetDefaultSettings("fontname"), FontCommon.GetFontSizeByName(ZYEditorControl.GetDefaultSettings("fontsize")));

            //设置默认字体
            myEMRDoc.View.DefaultFont = font;

            //设置默认背景色
            ZYEditorControl.ElementBackColor = ColorTranslator.FromHtml(ZYEditorControl.GetDefaultSettings("elecolor"));

            //设置元素样式
            ZYEditorControl.ElementStyle = ZYEditorControl.GetDefaultSettings("elestyle");

            //设置默认行距
            float f = float.Parse(ZYEditorControl.GetDefaultSettings("linespace"));
            int rowheight = (int)((float)myEMRDoc.View.DefaultRowPixelHeight * f);
            myEMRDoc.Info.LineSpacing = rowheight;
            //myEMRDoc.Info.ParagraphSpacing = rowheight;

            //默认页面设置
            myEMRDoc.Pages.PageSettings = GetDefaultPageSettings();

            myDocument = myEMRDoc;
            myEMRDoc.RefreshSize();
            myEMRDoc.RefreshLine();

        }

        #region 记录键盘快捷键与事件的对应关系

        #region  记录键盘快捷键

        /// <summary>
        /// 记录不需要屏蔽的键盘按键
        /// </summary>
        private Dictionary<Keys, Keys> myKeys = new Dictionary<Keys, Keys>();
        /// <summary>
        /// 初始化记录不需要屏蔽的键盘按键
        /// </summary>
        private void GenerateKeys()
        {
            myKeys[Keys.Left] = Keys.Left;
            myKeys[Keys.Right] = Keys.Right;
            myKeys[Keys.Up] = Keys.Up;
            myKeys[Keys.Down] = Keys.Down;
            myKeys[Keys.Home] = Keys.Home;
            myKeys[Keys.End] = Keys.End;
            myKeys[Keys.PageUp] = Keys.PageUp;
            myKeys[Keys.PageDown] = Keys.PageDown;
            myKeys[Keys.F | Keys.Control] = Keys.F | Keys.Control;
            myKeys[Keys.S | Keys.Control] = Keys.S | Keys.Control;
            myKeys[Keys.C | Keys.Control] = Keys.C | Keys.Control;
            myKeys[Keys.Enter] = Keys.Enter;
        }

        /// <summary>
        /// <para>创建一个字典，记录键盘快捷键与事件的对应关系，</para>
        ///但并不一定所有的事件都要对应快捷键
        /// </summary>
        public Dictionary<Keys, ZYEditorAction> editactions = new Dictionary<Keys, ZYEditorAction>();
        /// <summary>
        /// 初始化创建一个字典，记录键盘快捷键与事件的对应关系
        /// </summary>
        private void GenerateDefaultActions()
        {

            editactions[Keys.Left] = new A_MoveLeft();//Left	光标定位到前一个字符
            editactions[Keys.Right] = new A_MoveRight();//Right	光标定位到后一个字符
            editactions[Keys.Up] = new A_MoveUp();//Up	光标向上移动到上一行
            editactions[Keys.Down] = new A_MoveDown();//Down	光标向下移动到下一行
            editactions[Keys.Home] = new A_MoveHome();//Home	光标定位到当前行的最开始
            editactions[Keys.End] = new A_MoveEnd();//End	光标定位到当前行的最末尾

            editactions[Keys.Up | Keys.Control] = new A_CtlMoveUp();//Ctrl+ Up	垂直滚动条按行滚动
            editactions[Keys.Down | Keys.Control] = new A_CtlMoveDown();//Ctrl+ Down	垂直滚动条按行滚动
            editactions[Keys.Left | Keys.Shift] = new A_ShiftMoveLeft();//Shift + Left	向左选择一个字符
            editactions[Keys.Right | Keys.Shift] = new A_ShiftMoveRight();//Shift + Right	向右选择一个字符
            editactions[Keys.Up | Keys.Shift] = new A_ShiftMoveUp();//Shift + Up	向上一行选择光标位置到垂直相同位置之间的所有字符
            editactions[Keys.Down | Keys.Shift] = new A_ShiftMoveDown();//Shift + Down	向下一行选择光标位置到垂直相同位置之间的所有字符
            editactions[Keys.Home | Keys.Shift] = new A_ShiftMoveHome();//Shift +Home	选择光标处到当前行的首部的字符
            editactions[Keys.End | Keys.Shift] = new A_ShiftMoveEnd();//Shift +End	选择光标处到当前行的尾部的字符
            editactions[Keys.PageUp | Keys.Shift] = new A_ShiftPageUp();//Shift +PageUp	选择光标处到当前页面的开始处的所有内容
            editactions[Keys.PageDown | Keys.Shift] = new A_ShiftPageDown();//Shift +PageDown	选择光标处到当前页面的结束处的所有内容
            editactions[Keys.A | Keys.Control] = new A_SelectAll();//Ctrl + A	选择整个文档的所有内容
            editactions[Keys.Z | Keys.Control] = new A_Undo();//Ctrl+Z 撤销
            editactions[Keys.Y | Keys.Control] = new A_Redo();//Ctrl+Y 重做
            editactions[Keys.C | Keys.Control] = new A_Copy();//Ctrl+C 复制
            editactions[Keys.X | Keys.Control] = new A_Cut();//Ctrl+X	剪切
            editactions[Keys.V | Keys.Control] = new A_Paste();//Ctrl+V 粘贴
            editactions[Keys.Delete] = new A_Delete();//Del 删除选择内容，删除后一个字符

            ////保留
            editactions[Keys.PageUp] = new A_PageUp();
            editactions[Keys.PageDown] = new A_PageDown();
            editactions[Keys.Back] = new A_BackSpace();
            editactions[Keys.Control | Keys.P] = new A_PrintDocument();
            editactions[Keys.Escape] = new A_DoNothing();

            //Add By wwj 2011-09-23 增加快捷键 查找、保存
            editactions[Keys.F | Keys.Control] = new A_ShowFindForm();//Ctrl+F 查找
            editactions[Keys.S | Keys.Control] = new A_SaveEMR();//Ctrl+S 保存
        }

        #endregion

        #region 键盘事件
        /// <summary>
        /// 覆写键盘字符事件（输入一个新字符）
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (bolLockingUI && !IsInActiveEditArea(this.Document.Content.CurrentElement)) return;
            LastMessageTick = System.Environment.TickCount;
            //BeforeUserMessage();

            if (myDocument != null)
            {
                // edit by Ukey zhang 2017-11-10处理维文模式数据
                if (WeiWenProcess.weiwen)
                {
                    WeiWenProcess.myDocument = myDocument;
                    //光标左右两边是维文屏蔽所有按键操作
                    if (WeiWenProcess.LeftAndRightIsWeiWen() && !((int)e.KeyChar == 13))
                        return;
                    WeiWenProcess.KeyPress(e);
                }

                A_InsertChar a = new A_InsertChar();
                a.KeyCode = (System.Windows.Forms.Keys)e.KeyChar;
                a.OwnerDocument = this.EMRDoc;
                a.Execute();
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }


        /// <summary>
        /// keydown事件不能响应方向键,OnPreviewKeyDown不能给出返回值，还是用ProcessDialogKey吧
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (bolLockingUI && keyData != (Keys.C | Keys.Control) && !IsInActiveEditArea(this.Document.Content.CurrentElement))
            {
                return true;
            }
            if (myDocument != null)
            {
                WeiWenProcess.document = null;
                WeiWenProcess.document = myDocument;
                //光标左右两边是维文屏蔽所有按键操作
                if (WeiWenProcess.LeftAndRightIsWeiWen() && !myKeys.ContainsKey(keyData))
                    return true;
                // 自动执行插入-修改模式的切换
                if (bolEnableInsertMode && keyData == System.Windows.Forms.Keys.Insert)
                    this.InsertMode = !this.InsertMode;
                else
                {
                    //如果是tab键
                    if (keyData == Keys.Tab)
                    {
                        A_InsertChar a = new A_InsertChar();
                        a.KeyCode = keyData;
                        a.OwnerDocument = this.EMRDoc;
                        a.Execute();
                        return true;
                    }

                    if (editactions.ContainsKey(keyData))
                    {

                        ZYEditorAction action = editactions[keyData];
                        action.OwnerDocument = this.EMRDoc;
                        action.Execute();
                        return true;
                    }
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        #endregion

        #endregion
        /// <summary>
        /// 编辑器是否运行在设计器中
        /// </summary>
        public bool RunInDesigner = false;

        /// <summary>
        /// 文档内容发生改变时的事件
        /// </summary>
        public event System.EventHandler ContentChanged;

        private void OnContentChanged(object sender, System.EventArgs e)
        {
            if (ContentChanged != null)
                ContentChanged(sender, e);
        }

        #region LoadXML / ToXML

        /* mfb 两个LoadXML方法中,对于
         *  myEMRDoc.RefreshSize();
            myEMRDoc.ContentChanged();
            myEMRDoc.EndUpdate();
         * 这三个方法,其中有重复的RefreshSize() 和 RefreshLine()
         * 会导致多次的分行和重新计算大小.
         */

        /// <summary>
        /// 加载XML字符串并重新绘制内容
        /// </summary>
        /// <param name="strXML">XML字符串</param>
        /// <returns>加载是否成功</returns>
        public bool LoadXML(string strXML)
        {
            System.Xml.XmlDocument myXMLDoc = new System.Xml.XmlDocument();
            myXMLDoc.PreserveWhitespace = false;
            myEMRDoc.BeginUpdate();
            myXMLDoc.LoadXml(strXML);
            myEMRDoc.FromXML(myXMLDoc.DocumentElement);
            myEMRDoc.RefreshSize();
            myEMRDoc.ContentChanged();
            myEMRDoc.EndUpdate();
            myEMRDoc.Modified = false;
            this.Refresh();
            return true;
        }

        /// <summary>
        /// 加载XMLdoc并重新绘制内容
        /// </summary>
        /// <param name="strXML">XMLdoc</param>
        /// <returns>加载是否成功</returns>
        public bool LoadXML(XmlDocument xmldoc)
        {
            if (xmldoc == null)
                return false;

            myEMRDoc.FromXML(xmldoc.DocumentElement);
            myEMRDoc.RefreshSize();
            myEMRDoc.ContentChanged();
            myEMRDoc.EndUpdate();
            myEMRDoc.Modified = false;

            //myEMRDoc.SaveLogs.CurrentUserName = "speacil";
            this.Refresh();
            return true;
        }

        /// <summary>
        /// 加载加密的字节数组
        /// </summary>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public bool LoadBinary(byte[] data)
        {
            XmlDocument doc = BinaryToXml(data);
            return LoadXML(doc);
        }

        /// <summary>
        /// 加密文档并保存成字节数组
        /// </summary>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public byte[] SaveBinary()
        {
            //xmldoc
            XmlDocument doc = new XmlDocument();
            this.EMRDoc.ToXMLDocument(doc);

            //转成字节数组
            byte[] data = XmlToBinary(doc);

            //返回
            return data;
        }

        /// <summary>
        /// 加载XML文件并重新绘制内容
        /// </summary>
        /// <param name="strURL">XML文件URL</param>
        /// <returns>加载是否成功</returns>
        public bool LoadXMLFile(string strURL)
        {
            System.Xml.XmlDocument myXMLDoc = new System.Xml.XmlDocument();
            myXMLDoc.PreserveWhitespace = true;
            myEMRDoc.BeginUpdate();
            //myEMRDoc.ClearContent(); mfb注释
            myXMLDoc.Load(strURL);
            myEMRDoc.FromXML(myXMLDoc.DocumentElement);
            myEMRDoc.RefreshSize();
            myEMRDoc.ContentChanged();
            myEMRDoc.EndUpdate();
            myEMRDoc.Modified = false;

            this.Refresh();
            return true;
        }


        #region bwy:加密、解密、二进制方法

        //RSACryptoServiceProvider RSAc = new RSACryptoServiceProvider();
        /// <summary>
        /// 获取 MDE 加密字符串
        /// </summary>
        /// <param name="data">字符串：要加密的数据</param>
        /// <returns>字符串：密文数据</returns>
        #region 获取 MDE 加密字符串
        public static string GetDesEncryptString(string data)
        {
            string KEY_64 = "34671764";
            string IV_64 = "34671764";
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        #endregion

        /// <summary>
        /// 获取 MDE 解密字符串
        /// </summary>
        /// <param name="data">字符串：要解密的数据</param>
        /// <returns>字符串：解密后的数据</returns>
        #region 获取 MDE 解密字符串
        public static string GetDesDencryptString(string data)
        {
            string KEY_64 = "34671764";
            string IV_64 = "34671764";

            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
        #endregion

        /// <summary>
        /// 字节数组解密转换成xmldocument
        /// </summary>
        /// <param name="data">加密的字节数组</param>
        /// <returns>xmldocument</returns>
        public XmlDocument BinaryToXml(byte[] data)
        {
            try
            {
                //解密
                byte[] mydata = Decrypt(data);
                //转成字符串
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
                string xmlContent = enc.GetString(mydata);
                //转成xml
                XmlDocument doc = new XmlDocument();
                try
                {
                    //this.LoadXML(xmlContent);
                    doc.LoadXml(xmlContent);
                }
                catch
                {
                    //如果 不成功，说明是加密的，要解密
                    //然后将解压缩后的字符串转成正常的xml字符串
                    xmlContent = GetDesDencryptString(xmlContent);
                    doc.PreserveWhitespace = true;
                    doc.LoadXml(xmlContent);
                }

                return doc;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "错误", MyMessageBoxButtons.Ok, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// xmldocument转换成字节数组后加密
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <returns>字节数组</returns>
        public byte[] XmlToBinary(XmlDocument doc)
        {
            try
            {
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");

                string strData = GetDesEncryptString(doc.OuterXml);

                //转成字节数组
                byte[] data = enc.GetBytes(strData);
                //加密
                return Encrypt(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Encrypt(byte[] data)
        {
            ////Create a new instance of RSACryptoServiceProvider.
            //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            ////Import the RSA Key information. This only needs
            ////toinclude the public key information.
            //RSA.ImportParameters(RSAc.ExportParameters(false));

            ////Encrypt the passed byte array and specify OAEP padding.  
            ////OAEP padding is only available on Microsoft Windows XP or
            ////later.  
            //return RSA.Encrypt(data, false);
            return data;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Decrypt(byte[] data)
        {
            //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            ////Import the RSA Key information. This needs
            ////to include the private key information.
            //RSA.ImportParameters(RSAc.ExportParameters(true));

            ////Decrypt the passed byte array and specify OAEP padding.  
            ////OAEP padding is only available on Microsoft Windows XP or
            ////later.  
            //return RSA.Decrypt(data, false);
            return data;
        }

        #endregion bwy:

        public string ToXMLString()
        {
            System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
            myDoc.PreserveWhitespace = false;

            this.EMRDoc.ToXMLDocument(myDoc);
            return myDoc.DocumentElement.OuterXml;
        }
        #endregion


        public void CallOnResize()
        {
            OnResize(null);
        }

        protected override void OnViewPaint(System.Windows.Forms.PaintEventArgs e, SimpleRectangleTransform trans)
        {


            if (this.Updating)
                return;

            myEMRDoc.PageIndex = trans.PageIndex;
            if (trans.Flag2 == 0)
                myEMRDoc.DrawHead(e.Graphics, trans.DescRect);
            else if (trans.Flag2 == 1)
                myEMRDoc.DrawDocument(e.Graphics, e.ClipRectangle);
            else if (trans.Flag2 == 2)
                myEMRDoc.DrawFooter(e.Graphics, trans.DescRect);
        }

        public override void UpdatePages()
        {
            base.UpdatePages();
            foreach (SimpleRectangleTransform item in this.PagesTransform)
            {
                item.Enable = (item.Flag2 == 1);
            }
        }
        #region ********************** OnPaint OnResize **********************
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {

            if (this.DesignMode)
            {
                using (System.Drawing.StringFormat f = new System.Drawing.StringFormat())
                {
                    e.Graphics.Clear(System.Drawing.SystemColors.AppWorkspace);
                    f.Alignment = System.Drawing.StringAlignment.Center;
                    f.LineAlignment = System.Drawing.StringAlignment.Center;
                    e.Graphics.DrawString(
                        this.Name + System.Environment.NewLine + this.GetType().Name,
                        System.Windows.Forms.Control.DefaultFont,
                        System.Drawing.Brushes.Red,
                        new System.Drawing.RectangleF(0, 0, this.ClientSize.Width, this.ClientSize.Height),
                        f);
                }
                return;
            }
            base.OnPaint(e);
            if (myEMRDoc.EnableJumpPrint && myEMRDoc.JumpPrintPosition > 0)
            {
                base.DrawJumpPrintArea(
                    e.Graphics,
                    e.ClipRectangle,
                    myEMRDoc.JumpPrintPosition,
                    System.Drawing.Color.FromArgb(80, 0, 123, 100));
            }
            //绘制选中区域 Add By wwj 2012-04-17
            else if (myEMRDoc.EnableSelectAreaPrint && myEMRDoc.SelectAreaPrintLeftTopPoint != Point.Empty && myEMRDoc.SelectAreaPrintRightBottomPoint != Point.Empty)
            {
                base.DrawSelectAreaPrint(
                    e.Graphics,
                    e.ClipRectangle,
                    myEMRDoc.SelectAreaPrintLeftTopPoint,
                    myEMRDoc.SelectAreaPrintRightBottomPoint,
                    System.Drawing.Color.FromArgb(80, 0, 123, 100));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateTextCaret();
        }
        #endregion

        /// <summary>
        /// 获得文本框中最终的文本内容
        /// </summary>
        /// <returns>纯文本内容</returns>
        public string GetFinalText()
        {
            return myEMRDoc.GetFinalText();
        }

        protected override System.Windows.Forms.ImeMode DefaultImeMode
        {
            get
            {
                //if (myKBPopupList != null && myKBPopupList.Visible)
                //    return System.Windows.Forms.ImeMode.Off;
                //else
                return base.DefaultImeMode;
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                #region mfb
                //if (myTagMenu != null)
                //{
                //    myTagMenu.Dispose();
                //    myTagMenu = null;
                //}

                //if (myKBPopupList != null)
                //{
                //    myKBPopupList.Dispose();
                //    myKBPopupList = null;
                //}
                //if (myPopupMenu != null)
                //{
                //    myPopupMenu.Dispose();
                //    myPopupMenu = null;
                //}
                //if (myImageList != null)
                //{
                //    myImageList.Dispose();
                //    myImageList = null;
                //}

                #endregion
                base.Dispose(disposing);
                if (myEMRDoc != null)
                {
                    myEMRDoc.Dispose();
                    myEMRDoc = null;
                }
            }
            catch
            { }
        }

        public ZYTextElement GetElementByPos(int x, int y)
        {
            if (this.myTransform.ContainsSourcePoint(x, y))
            {
                System.Drawing.Point p = myTransform.TransformPoint(x, y);
                if (!p.IsEmpty)
                {
                    return myEMRDoc.GetElementByPos(p.X, p.Y);
                }
            }
            return null;
        }

        #region 控制光标的函数群 **********************

        protected override void OnGotFocus(EventArgs e)
        {
            if (this.Focused)
            {
                base.bolMoveCaretWithScroll = false;
                this.UpdateTextCaret();
                base.bolMoveCaretWithScroll = true;
                base.OnGotFocus(e);
            }
        }

        /// <summary>
        /// 根据指定的文档元素对象更新光标
        /// </summary>
        /// <param name="myElement">指定的文档元素对象</param>
        public void UpdateTextCaret(ZYTextElement myElement)
        {
            if (myElement != null)
            {
                if (myEMRDoc.Content.LineEndFlag)
                {
                    myElement = myEMRDoc.Content.PreElement;
                    if (myElement != null)
                        base.MoveTextCaretTo(myElement.Bounds.Right, myElement.OwnerLine.Bottom, myElement.OwnerLine.Height);
                }
                else if (WeiWenProcess.weiwen)
                {
                    if (myElement.OwnerLine != null)
                        base.MoveTextCaretTo(myElement.Bounds.Right - 2, myElement.OwnerLine.Bottom, myElement.OwnerLine.Height);
                }
                else
                    base.MoveTextCaretTo(myElement.Bounds.Left, myElement.OwnerLine.Bottom, myElement.OwnerLine.Height);
            }
        }

        /// <summary>
        /// 根据当前元素更新光标
        /// </summary>
        public void UpdateTextCaret()
        {
            try
            {
                ZYTextElement myElement = myEMRDoc.Content.CurrentElement;
                if (myElement != null)
                {
                    if (myEMRDoc.Content.LineEndFlag)
                    {
                        myElement = myEMRDoc.Content.PreElement;
                        if (myElement != null)
                            base.MoveTextCaretTo(myElement.RealLeft + myElement.Width, myElement.RealTop + myElement.Height, myElement.Height);
                    }
                    else if (WeiWenProcess.weiwen)
                    {
                        if (myElement.OwnerLine != null)
                            base.MoveTextCaretTo(myElement.RealLeft + myElement.Width - 2, myElement.OwnerLine.RealBottom, myElement.OwnerLine.Height);
                    }
                    else

                        base.MoveTextCaretTo(myElement.RealLeft, myElement.RealTop + myElement.Height, myElement.Height);
                }

            }
            catch (Exception ex)
            {
                //此处catch有必要，因为始化时，许多对象还没有建立，可能会出错
                //MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region ============ HandleCommand Method (已经注释)============
        ///// <summary>
        ///// 执行编辑命令
        ///// </summary>
        ///// <remarks>本函数遍历内部的命令对象列表,根据名称名称查找指定名称的命令对象,然后设置参数,最后执行该命令</remarks>
        ///// <param name="CommandName">命令名称</param>
        ///// <param name="Param1">命令参数1</param>
        ///// <param name="Param2">命令参数2</param>
        ///// <param name="Param3">命令参数3</param>
        ///// <returns>命令执行结果</returns>
        //public bool HandleCommandNoUI(string CommandName, string Param1, string Param2, string Param3)
        //{
        //    //ZYErrorReport.Instance.DebugPrint("执行HandleCommandNoUI[" + CommandName + "]参数1:" + Param1 + " 参数2:" + Param2 + " 参数3:" + Param3);
        //    foreach (ZYEditorAction a in myEMRDoc.Actions)
        //    {
        //        if (a.ActionName() == CommandName)
        //        {
        //            if (a.isEnable())
        //            {
        //                a.Param1 = Param1;
        //                a.Param2 = Param2;
        //                a.Param3 = Param3;
        //                return a.Execute();
        //            }
        //        }
        //    }
        //    //ZYErrorReport.Instance.DebugPrint("未找到方法[" + CommandName + "]");
        //    return false;
        //}

        ///// <summary>
        ///// 执行编辑命令
        ///// </summary>
        ///// <param name="CommandName">命令名称</param>
        ///// <param name="Param1">命令参数1</param>
        ///// <param name="Param2">命令参数2</param>
        ///// <param name="Param3">命令参数3</param>
        ///// <returns>命令执行结果</returns>
        //public bool HandleCommand(string CommandName, string Param1, string Param2, string Param3)
        //{
        //    //ZYErrorReport.Instance.DebugPrint("执行HandleCommand[" + CommandName + "]参数1:" + Param1 + " 参数2:" + Param2 + " 参数3:" + Param3);
        //    foreach (ZYEditorAction a in myEMRDoc.Actions)
        //    {
        //        if (a.ActionName() == CommandName)
        //        {
        //            if (a.isEnable())
        //            {
        //                a.Param1 = Param1;
        //                a.Param2 = Param2;
        //                a.Param3 = Param3;
        //                return a.UIExecute();
        //            }
        //        }
        //    }
        //    //ZYErrorReport.Instance.DebugPrint("未找到方法[" + CommandName + "]");
        //    return false;
        //}

        ///// <summary>
        ///// 初始化当前事件数据对象
        ///// </summary>
        ///// <param name="myEventObj">事件数据对象</param>
        //internal void InitEventObject(ZYVBEventObject myEventObj)
        //{
        //    if (myEventObj != null)
        //    {
        //        myEventObj.AltKey = CommonFunction.GetIntAttribute((int)System.Windows.Forms.Control.ModifierKeys, (int)System.Windows.Forms.Keys.Alt);
        //        myEventObj.CtlKey = CommonFunction.GetIntAttribute((int)System.Windows.Forms.Control.ModifierKeys, (int)System.Windows.Forms.Keys.ControlKey);
        //        myEventObj.ShiftKey = CommonFunction.GetIntAttribute((int)System.Windows.Forms.Control.ModifierKeys, (int)System.Windows.Forms.Keys.ShiftKey);
        //        myEventObj.ScreenX = System.Windows.Forms.Control.MousePosition.X;
        //        myEventObj.ScreenY = System.Windows.Forms.Control.MousePosition.Y;
        //        myEventObj.MouseButton = CommonFunction.GetIntAttribute((int)System.Windows.Forms.Control.MouseButtons, (int)System.Windows.Forms.MouseButtons.Left) ? 1 : 2;
        //        myEventObj.ReturnValue = null;
        //        myEventObj.Cancel = false;
        //        System.Drawing.Point p = this.PointToClient(System.Windows.Forms.Control.MousePosition);
        //        myEventObj.ClientX = p.X;
        //        myEventObj.ClientY = p.Y;
        //        p = this.ClientPointToView(p.X, p.Y);
        //        myEventObj.X = p.X;
        //        myEventObj.Y = p.Y;
        //        myEventObj.CancelBubble = true;
        //    }
        //}
        ///// <summary>
        ///// 返回内置的图象列表
        ///// </summary>
        //public System.Windows.Forms.ImageList ImageList
        //{
        //    get { return myImageList; }
        //}
        //private System.Windows.Forms.ImageList myImageList;
        //private System.Windows.Forms.ContextMenu myPopupMenu;
        ///// <summary>
        ///// 绑定的知识库列表
        ///// </summary>
        //public ZYKBTreeView KBTreeView = null;
        //private string strKBBufferFile = null;
        ///// <summary>
        ///// 加载BASE64格式的XML字符串
        ///// </summary>
        ///// <param name="strXML">XML字符串的BASE64编码</param>
        ///// <returns>加载是否成功</returns>
        //public bool LoadBase64XML(string strXML)
        //{
        //    bool bolResult = true;
        //    try
        //    {
        //        if (ZYCommon.StringCommon.isBlankString(strXML))
        //        {
        //            myEMRDoc.ClearContent();
        //        }
        //        else
        //        {
        //            string strData = StringCommon.FromBase64String(strXML);
        //            bolResult = LoadXML(strData);
        //        }
        //    }
        //    catch (Exception ext)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ext.ToString());
        //        bolResult = false;
        //    }
        //    if (bolResult)
        //    {
        //        myEMRDoc.Info.ID = strCurrentID;
        //        myEMRDoc.SaveLogs.CurrentUserName = strCurrentUserName;
        //        if (myEMRDoc.Marks.Count > 0)
        //        {
        //            myEMRDoc.Info.ShowAll = true;
        //            myEMRDoc.Info.ShowMark = true;
        //            myEMRDoc.Info.LogicDelete = true;
        //            myEMRDoc.RefreshElements();
        //            myEMRDoc.RefreshLine();
        //            myEMRDoc.UpdateView();
        //        }
        //    }
        //    return bolResult;
        //}
        //private string strCurrentID = null;
        //private string strCurrentUserName = null;
        //private int intUserLevel = 0;
        ///// <summary>
        ///// 用户等级 0:管床医生 1:主治医生 2:主任医生
        ///// </summary>
        //public int UserLevel
        //{
        //    get { return intUserLevel; }
        //    set { intUserLevel = value; }
        //}
        ///// <summary>
        ///// 内部的电子病历知识库弹出式列表
        ///// </summary>
        //protected ZYKBPopupList myKBPopupList = null;

        //public void BindKBTreeView(ZYKBTreeView tvw)
        //{
        //    if (tvw != null)
        //    {
        //        KBTreeView = tvw;
        //        KBTreeView.DesignKBMode = false;
        //        KBTreeView.EnableClickEvent = true;
        //        KBTreeView.DoubleClickMode = true;
        //        KBTreeView.KBItemClick += new KBItemClickHandler(KBTreeView_KBItemClick);
        //        KBTreeView.KBListClick += new KBListClickHandler(KBTreeView_KBListClick);
        //    }
        //}
        ///// <summary>
        ///// 内部的快捷菜单项目对象
        ///// </summary>
        //private class ZYActionMenuItem : System.Windows.Forms.MenuItem
        //{
        //    /// <summary>
        //    /// 动作对象
        //    /// </summary>
        //    private ZYEditorAction myAction = null;
        //    public ZYEditorAction Action
        //    {
        //        get { return myAction; }
        //        set
        //        {
        //            myAction = value;
        //            this.UpdateAction();
        //        }
        //    }
        //    public void UpdateAction()
        //    {
        //        if (myAction == null)
        //        {
        //            this.Text = "-";
        //        }
        //        else
        //        {
        //            this.Enabled = myAction.isEnable();
        //            if (myAction.CheckState() == 1)
        //                this.Checked = true;
        //            else
        //                this.Checked = false;
        //            //this.Text = myAction.ActionName();
        //        }
        //    }
        //    public ZYActionMenuItem(ZYEditorAction a, string strText)
        //    {
        //        this.Action = a;
        //        this.Text = strText;
        //    }
        //    protected override void OnClick(EventArgs e)
        //    {
        //        if (myAction != null && myAction.isEnable())
        //        {
        //            myAction.Param1 = (this.Checked ? "0" : "1");
        //            myAction.UIExecute();
        //        }
        //        else
        //            base.OnClick(e);
        //    }
        //}// private class ZYActionMenuItem
        //private void myPopupMenu_Popup(object sender, EventArgs e)
        //{
        //    foreach (ZYActionMenuItem myItem in myPopupMenu.MenuItems)
        //        myItem.UpdateAction();
        //}
        //private int OldImageIndex;
        //private string OldToolTip;
        //private int OldToolTipX;
        //private int OldToolTipY;
        ///// <summary>
        ///// 显示内部的提示文本信息
        ///// </summary>
        ///// <param name="img">图标</param>
        ///// <param name="strText">提示的文本信息</param>
        ///// <param name="x">显示点在视图区域中的X坐标</param>
        ///// <param name="y">显示点在视图区域中的Y坐标</param>
        ///// <param name="height">注视区域的高度</param>
        //public void ShowInnerToolTip(int imgIndex, string strText, int x, int y, int height)
        //{
        //    if (imgIndex != OldImageIndex || OldToolTip != strText || OldToolTipX != x || OldToolTipY != y || lblInfo.Visible == false)
        //    {
        //        OldImageIndex = imgIndex;
        //        OldToolTip = strText;
        //        OldToolTipX = x;
        //        OldToolTipY = y;
        //        base.ShowInnerToolTip((imgIndex >= 0 && imgIndex < myImageList.Images.Count ? myImageList.Images[imgIndex] : null), strText, x, y, height);
        //    }
        //}
        //public bool LoadKBFromBuffer(string strKBBufferFile)
        //{
        //    if (StringCommon.isBlankString(strKBBufferFile))
        //        strKBBufferFile = System.Windows.Forms.Application.StartupPath + "\\kbbuffer.xml";
        //    try
        //    {
        //        if (System.IO.File.Exists(strKBBufferFile))
        //        {
        //            System.DateTime dtmCreate = System.IO.File.GetLastAccessTime(strKBBufferFile);
        //            //System.TimeSpan mySpan = System.DateTime.Now.Subtract( dtmCreate );
        //            System.TimeSpan mySpan = ZYCommon.ZYTime.GetServerTime().Subtract(dtmCreate);
        //            if (mySpan.TotalHours < 12)
        //            {
        //                System.Xml.XmlDocument myXMLDoc = new System.Xml.XmlDocument();
        //                myXMLDoc.Load(strKBBufferFile);
        //                ZYKBBuffer.Instance.LoadAllKBList(myXMLDoc.DocumentElement, false);
        //                return true;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return false;
        //}
        ///// <summary>
        ///// 本控件内嵌在IE浏览器中,需要初始化对象数据
        ///// </summary>
        ///// <param name="strCookie">IE浏览器的cookie值(document.cookie)</param>
        ///// <param name="strPage">读取数据使用的服务器页面</param>
        ///// <param name="strID">文档的ID号</param>
        ///// <param name="strUserName">当前用户的名称</param>
        ///// <param name="bolLoadKB">是否加载知识库</param>
        //public bool InitForIE(string strCookie, string strPage, string strID, string strUserName)
        //{
        //    try
        //    {
        //        //System.Windows.Forms.Application.DoEvents();
        //        //System.Windows.Forms.MessageBox.Show(intUserLevel.ToString());
        //        ZYKBBuffer.Instance.Connection = myEMRDoc.DataSource.DBConn;
        //        bolRunInIE = true;
        //        strCurrentID = strID;
        //        strCurrentUserName = strUserName;

        //        XMLHttpConnection myConn = new XMLHttpConnection();
        //        myConn.IECookies = strCookie;
        //        myConn.ConnectionString = strPage;
        //        myConn.Open();

        //        //myEMRDoc.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("CUSTOM" , 100,100);
        //        myEMRDoc.DataSource.DBConn.Connection = myConn;
        //        myEMRDoc.DataSource.DBConn.AutoOpen = false;
        //        myEMRDoc.Info.ID = strID;
        //        myEMRDoc.Info.ShowPageLine = false;
        //        //myEMRDoc.View.Width = 500 ;
        //        myEMRDoc.SaveLogs.CurrentUserName = strUserName;
        //        //myEMRDoc.SaveLogs.CurrentSaveLog.Level = intUserLevel ;
        //        //myEMRDoc.Content.UserLevel = intUserLevel ;
        //        this.Refresh();
        //        return true;
        //    }
        //    catch (Exception ext)
        //    {
        //        //				ZYErrorReport.Instance.SourceException = ext ;
        //        //				ZYErrorReport.Instance.SourceObject =  this ;
        //        //				ZYErrorReport.Instance.UserMessage = "加载电子病历文本编辑器错误";
        //        //				ZYErrorReport.Instance.MemberName = "ZYEditorControl.InitForIE"; 
        //        //				ZYErrorReport.Instance.ReportError();
        //        //				//System.Windows.Forms.MessageBox.Show("系统错误\n\r" + ext.ToString() );
        //    }
        //    return false;
        //}// InitForIE
        //public bool LoadKBFromDataBase()
        //{
        //    strKBBufferFile = System.Windows.Forms.Application.StartupPath + "\\kbbuffer.xml";
        //    ZYKBBuffer.Instance.LoadAllKBList(false);
        //    System.Xml.XmlDocument myOutDoc = new System.Xml.XmlDocument();
        //    myOutDoc.LoadXml("<kb />");
        //    //ZYKBBuffer.Instance.AllKBListToXML( myOutDoc.DocumentElement);
        //    myOutDoc.Save(strKBBufferFile);
        //    return true;
        //}
        ///// <summary>
        ///// 保存文档数据到一个Base64编码的XML字符串中
        ///// </summary>
        ///// <returns>经过Base64编码的GB2312格式的XML字符串</returns>
        //public string ToBase64XML()
        //{
        //    System.Xml.XmlDocument myXMLDoc = new System.Xml.XmlDocument();
        //    myEMRDoc.Info.SaveMode = 0;
        //    myEMRDoc.ToXMLDocument(myXMLDoc);
        //    string strData = myXMLDoc.DocumentElement.OuterXml;
        //    strData = StringCommon.ToBase64String(strData);
        //    return strData;
        //}
        ///// <summary>
        ///// 保存结构化文档数据到一个Base64编码的XML字符串中
        ///// </summary>
        ///// <returns>经过Base64编码的GB2312格式的XML字符串</returns>
        //public string ToBase64XML2()
        //{
        //    System.Xml.XmlDocument myXMLDoc = new System.Xml.XmlDocument();
        //    myEMRDoc.Info.SaveMode = 2;
        //    myEMRDoc.ToXMLDocument(myXMLDoc);
        //    return StringCommon.ToBase64String(ZYCommon.XMLCommon.CreateChildElement(myXMLDoc.DocumentElement, ZYTextConst.c_Body, false).OuterXml);
        //}
        ///// <summary>
        ///// 保存结构化数据到一个和模板化电子病历XML文档相同格式的XML字符串中
        ///// </summary>
        ///// <returns>经过Base64编码的GB2312格式的XML字符串</returns>
        //public string ToBase64EMRXML()
        //{
        //    System.Xml.XmlDocument myXMLDoc = new System.Xml.XmlDocument();
        //    myEMRDoc.Info.SaveMode = 3;
        //    myEMRDoc.ToXMLDocument(myXMLDoc);
        //    return StringCommon.ToBase64String(myXMLDoc.DocumentElement.OuterXml);
        //    //
        //    //			System.Xml.XmlDocument OutDoc = new System.Xml.XmlDocument();
        //    //			OutDoc.LoadXml("<emrxml />");
        //    //			System.Xml.XmlElement RootElement = ZYCommon.XMLCommon.CreateChildElement( myXMLDoc.DocumentElement , ZYTextConst.c_Body , false) ; 
        //    //			System.Xml.XmlElement NewElement = null;
        //    //			foreach(System.Xml.XmlNode myXMLNode in RootElement.ChildNodes)
        //    //			{
        //    //				if(myXMLNode is System.Xml.XmlElement )
        //    //				{
        //    //					System.Xml.XmlElement myElement = myXMLNode as System.Xml.XmlElement ;
        //    //					NewElement = OutDoc.CreateElement(myElement.GetAttribute(ZYTextConst.c_ID));
        //    //					NewElement.SetAttribute("name", myElement.GetAttribute("name"));
        //    //					NewElement.InnerText = myElement.InnerText ;
        //    //					OutDoc.DocumentElement.AppendChild( NewElement );
        //    //				}
        //    //			}
        //    //			return StringCommon.ToBase64String( OutDoc.DocumentElement.OuterXml );
        //}// string ToBase64EMRXML()
        #endregion

        #region ============ 初始化/显示电子病历知识库弹出式列表 (已经注释)============
        //protected override bool InitPopupList()
        //{
        //    if (myPopupList == null)
        //    {
        //        myKBPopupList = new ZYKBPopupList();
        //        myKBPopupList.Clear();
        //        myKBPopupList.AutoClose = true;
        //        myKBPopupList.OwnerEditor = this;

        //        myPopupList = myKBPopupList;
        //    }
        //    return true;
        //}
        ///// <summary>
        ///// 获得内置的电子病历知识库弹出式列表对象
        ///// </summary>
        ///// <returns></returns>
        //public ZYKBPopupList KBPopupList
        //{
        //    get { return myKBPopupList; }
        //}

        ///// <summary>
        ///// 初始化知识库弹出列表
        ///// </summary>
        //public void InitKBPopupList()
        //{
        //    if (myKBPopupList == null)
        //    {
        //        myKBPopupList = new ZYKBPopupList();
        //        myKBPopupList.AutoClose = true;
        //        myKBPopupList.OwnerEditor = this;
        //        //myKBPopupList.OwnerElement  = myElement  ;
        //        myKBPopupList.OwnerDocument = myEMRDoc;
        //        myKBPopupList.ImageList = myImageList;// ZYKBTreeView.GetKBImageList();// myImageList2 ;
        //        base.myPopupList = myKBPopupList;
        //        myKBPopupList.AutoClose = false;
        //        //myKBPopupList.DBConn = myEMRDoc.DataSource.DBConn ;
        //        //myKBPopupList.RootKB = myEMRDoc.DataSource.RootKB ;
        //        //bolFirstShow = true;
        //    }
        //    myKBPopupList.Title = "";
        //    myKBPopupList.CompositionRect = System.Drawing.Rectangle.Empty;
        //}
        //public object ShowKBPopupList(ZYTextElement myElement, System.Collections.ArrayList myKBList, string strTitle, bool MultiSelect, bool MustKBItem)
        //{
        //    InitKBPopupList();
        //    this.SetPopupPos(myElement.RealLeft, myElement.RealTop, myElement.Height);
        //    //myKBPopupList.OwnerElement = myElement ;
        //    myKBPopupList.SetKBList(strTitle, myKBList);
        //    myKBPopupList.Show();
        //    this.Focus();
        //    myKBPopupList.SelectedIndex = 0;
        //    myKBPopupList.MultiSelect = false;
        //    this.ImeMode = System.Windows.Forms.ImeMode.Disable;
        //    object obj = null;
        //    //			if( myKBPopupList.WaitUserSelected())
        //    //			{
        //    //				obj = myKBPopupList.SelectedObject ; 
        //    //			}
        //    //			myKBPopupList.CloseList();
        //    if (MustKBItem)
        //        obj = myKBPopupList.WaitForUserSelectKBItem(MultiSelect);
        //    else
        //    {
        //        if (myKBPopupList.WaitUserSelected())
        //        {
        //            obj = myKBPopupList.SelectedObject;
        //        }
        //        myKBPopupList.CloseList();
        //    }
        //    this.ImeMode = System.Windows.Forms.ImeMode.On;
        //    return obj;
        //}

        ///// <summary>
        ///// 显示一个电子病历知识库弹出式列表,直到用户确定或取消某操作才返回本函数
        ///// </summary>
        ///// <param name="myElement">该列表所属的文档元素</param>
        ///// <returns>用户在列表中选中的知识点对象,若列表为单选则返回选中的KB_Item对象,
        ///// 若为多选则返回保存所有选择的KB_Item对象的列表,若对象没有选择则返回空引用</returns>
        //public object ShowKBPopupList(ZYTextElement myElement)
        //{
        //    base.bolCaptureMouse = false;
        //    //bool bolFirstShow = false ;
        //    bool bolMultiSelect = false;
        //    InitKBPopupList();
        //    this.SetPopupPos(myElement.RealLeft, myElement.RealTop, myElement.Height);
        //    //myKBPopupList.OwnerElement = myElement ;
        //    ZYTextSelect mySelect = myElement as ZYTextSelect;
        //    if (mySelect == null)
        //        myKBPopupList.SetKBList((string)null);
        //    else
        //    {
        //        myKBPopupList.FirstKBSEQ = mySelect.ListSource;
        //        bolMultiSelect = mySelect.Multiple;
        //        myKBPopupList.MultiSelect = mySelect.Multiple;
        //        if (mySelect.HasOptions())
        //        {
        //            myKBPopupList.SetKBList("", mySelect.Options);
        //        }
        //        else
        //        {
        //            if (mySelect.OwnerKBList != null)
        //                myKBPopupList.SetKBList(mySelect.OwnerKBList);
        //            else
        //                myKBPopupList.SetKBList(mySelect.ListSource);
        //        }
        //        myKBPopupList.SelectedText = mySelect.Text;
        //        if (StringCommon.isBlankString(mySelect.Name))
        //            myKBPopupList.Title = "[知识点]";
        //        else
        //            myKBPopupList.Title = "[" + mySelect.Name + "]";
        //    }
        //    myKBPopupList.AutoClose = false;
        //    myKBPopupList.Show();
        //    this.Focus();
        //    this.ImeMode = System.Windows.Forms.ImeMode.Disable;
        //    //this.ImeMode = System.Windows.Forms.ImeMode.Off ;
        //    if (mySelect != null)
        //    {
        //        myKBPopupList.SelectedText = mySelect.Text;
        //        myKBPopupList.SelectedStateChanged = mySelect.GetSelectedStateChangedHandler();
        //        myKBPopupList.PopupSelection();
        //    }
        //    //System.Windows.Forms.ImeMode imeBack = System.Windows.Forms.ImeMode.NoControl ;
        //    if (this.FindForm() != null)
        //    {
        //        this.FindForm().Activate();
        //        //imeBack = this.FindForm().ImeMode ;
        //        //this.FindForm().ImeMode = System.Windows.Forms.ImeMode.Disable  ;
        //    }
        //    object objReturn = null;
        //    while (myKBPopupList.WaitUserSelected())
        //    {
        //        if (myKBPopupList.MultiSelect)
        //        {
        //            objReturn = myKBPopupList.SelectedObjects;
        //            break;
        //        }
        //        else
        //        {
        //            ZYPopupList.ListItem myItem = myKBPopupList.SelectedItem;
        //            if (myItem != null)
        //            {
        //                if (myItem.obj is KB_List)
        //                {
        //                    myKBPopupList.MultiSelect = bolMultiSelect;
        //                    myKBPopupList.SetKBList((myItem.obj as KB_List).SEQ);
        //                }
        //                else
        //                {
        //                    objReturn = myItem.obj;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    if (myKBPopupList != null)
        //        myKBPopupList.CloseList();
        //    this.ImeMode = System.Windows.Forms.ImeMode.On;
        //    //if( this.FindForm() != null) this.FindForm().ImeMode = imeBack ;
        //    return objReturn;
        //}
        #endregion

        #region ============ 控制拖拽操作的函数群 (已经注释)============
        //protected override bool OnStartDrag(int x, int y)
        //{
        //    //			if( myEMRDoc.isSelected( myEMRDoc.Content.GetElementAt(x,y)) )
        //    //			{
        //    //				System.Collections.ArrayList myList = myEMRDoc.Content.GetSelectElements();
        //    //				if( myList.Count > 0 )
        //    //				{
        //    //					System.Windows.Forms.DataObject d = new System.Windows.Forms.DataObject( myList );
        //    //					base.DoDragDrop( d , System.Windows.Forms.DragDropEffects.All | System.Windows.Forms.DragDropEffects.Link );
        //    //					return true;
        //    //				}
        //    //			}
        //    return false;
        //}

        //private bool CanDrag(System.Windows.Forms.DragEventArgs drgevent)
        //{
        //    if ((drgevent.Data.GetData(typeof(string)) as string) != null)
        //        return true;

        //    if ((drgevent.Data.GetData(typeof(KB_List)) as KB_List) != null)
        //        return true;

        //    if ((drgevent.Data.GetData(typeof(KB_Item)) as KB_Item) != null)
        //        return true;


        //    if ((drgevent.Data.GetData(typeof(System.Collections.ArrayList)) as System.Collections.ArrayList) != null)
        //        return true;

        //    return false;
        //}
        //protected override void OnDragDrop(System.Windows.Forms.DragEventArgs drgevent)
        //{
        //    // 拖拉方式插入字符串
        //    string strData = drgevent.Data.GetData(typeof(string)) as string;
        //    if (StringCommon.isBlankString(strData) == false)
        //    {
        //        A_InsertString a = new A_InsertString();
        //        a.OwnerDocument = myEMRDoc;
        //        if (a.isEnable())
        //        {
        //            a.Param1 = strData;
        //            a.Execute();
        //            this.UpdateTextCaret();
        //        }
        //        return;
        //    }
        //    // 拖拉方式插入一个列表
        //    KB_List myList = drgevent.Data.GetData(typeof(KB_List)) as KB_List;
        //    if (myList != null)
        //    {
        //        A_InsertKB a = new A_InsertKB();
        //        a.OwnerDocument = myEMRDoc;
        //        if (a.isEnable())
        //        {
        //            myEMRDoc._InsertKB(myList);
        //            this.UpdateTextCaret();
        //        }
        //        return;
        //    }

        //    // 插入一个模板
        //    KB_Item myItem = drgevent.Data.GetData(typeof(KB_Item)) as KB_Item;
        //    if (myItem != null)
        //    {
        //        if (myItem.isTemplate())
        //        {
        //            if (RunInDesigner)
        //            {
        //                A_InsertKeyWord a = new A_InsertKeyWord();
        //                a.OwnerDocument = myEMRDoc;
        //                a.Param1 = myItem.ItemSEQ.ToString();
        //                if (a.isEnable())
        //                    a.Execute();
        //            }
        //            else
        //            {
        //                myEMRDoc._InsertDocument(myItem.ItemValue);
        //            }
        //        }
        //        else
        //        {
        //            if (myEMRDoc.CanModify())
        //            {
        //                ZYTextSelect mySelect = new ZYTextSelect();
        //                mySelect.OwnerKBList = myItem.OwnerList;
        //                mySelect.Text = myItem.ItemText;
        //                mySelect.Value = myItem.ItemValue;
        //                myEMRDoc._InsertElement(mySelect);
        //            }
        //        }
        //        this.UpdateTextCaret();
        //        return;
        //    }

        //    System.Collections.ArrayList myDataList = drgevent.Data.GetData(typeof(System.Collections.ArrayList)) as System.Collections.ArrayList;
        //    if (myDataList != null && myEMRDoc.CanModify())
        //    {
        //        for (int iCount = 0; iCount < myDataList.Count; iCount++)
        //        {
        //            KB_List myKBList = myDataList[iCount] as KB_List;
        //            if (myKBList != null)
        //            {
        //                A_InsertKB a = new A_InsertKB();
        //                a.OwnerDocument = myEMRDoc;
        //                if (a.isEnable())
        //                {
        //                    myEMRDoc._InsertKB(myKBList);
        //                }
        //                this.UpdateTextCaret();
        //                return;
        //            }
        //            KB_Item myKBItem = myDataList[iCount] as KB_Item;
        //            if (myKBItem != null)
        //            {
        //                if (myKBItem.isTemplate())
        //                {
        //                    if (RunInDesigner)
        //                    {
        //                        A_InsertKeyWord a = new A_InsertKeyWord();
        //                        a.OwnerDocument = myEMRDoc;
        //                        a.Param1 = myKBItem.ItemSEQ.ToString();
        //                        if (a.isEnable())
        //                            a.Execute();
        //                    }
        //                    else
        //                    {
        //                        myEMRDoc._InsertDocument(myKBItem.ItemValue);
        //                    }
        //                }
        //                else
        //                {
        //                    ZYTextSelect mySelect = new ZYTextSelect();
        //                    mySelect.OwnerKBList = myKBItem.OwnerList;
        //                    mySelect.Text = myKBItem.ItemText;
        //                    mySelect.Value = myKBItem.ItemValue;
        //                    myEMRDoc._InsertElement(mySelect);
        //                }
        //            }
        //        }// for
        //        this.UpdateTextCaret();
        //        return;
        //    }
        //    base.OnDragDrop(drgevent);
        //}// void OnDragDrop()

        //protected override void OnDragEnter(System.Windows.Forms.DragEventArgs drgevent)
        //{
        //    if (this.CanDrag(drgevent))
        //    {
        //        drgevent.Effect = System.Windows.Forms.DragDropEffects.Copy;
        //    }
        //    base.OnDragEnter(drgevent);
        //}


        //protected override void OnDragOver(System.Windows.Forms.DragEventArgs drgevent)
        //{
        //    if (this.CanDrag(drgevent))
        //    {
        //        System.Drawing.Point p = new System.Drawing.Point(drgevent.X, drgevent.Y);
        //        p = this.PointToClient(p);
        //        base.PagesTransform.UseAbsTransformPoint = true;
        //        p = this.ClientPointToView(p.X, p.Y);
        //        base.PagesTransform.UseAbsTransformPoint = false;
        //        myEMRDoc.Content.AutoClearSelection = true;
        //        this.ForceShowCaret = true;
        //        myEMRDoc.Content.MoveTo(p.X, p.Y);
        //        this.ForceShowCaret = false;
        //    }
        //    base.OnDragOver(drgevent);
        //}

        #endregion

        #region ============ 智能标签控制函数群(已经注释) ============

        ///// <summary>
        ///// 智能标签菜单项目
        ///// </summary>
        //internal class SmartTagMenuItem : System.Windows.Forms.MenuItem
        //{
        //    private SmartTagItem myItem = null;

        //    /// <summary>
        //    /// 设置该菜单项目使用的智能标签项目
        //    /// </summary>
        //    public SmartTagItem Item
        //    {
        //        set
        //        {
        //            if (value != null)
        //            {
        //                myItem = value;
        //                this.Text = value.Text;
        //                this.Checked = value.Checked;
        //            }
        //        }
        //    }

        //    protected override void OnClick(EventArgs e)
        //    {
        //        if (myItem != null && myItem.Provider != null)
        //        {
        //            myItem.Provider.HandleCommand(myItem);
        //        }
        //    }

        //    public SmartTagMenuItem(SmartTagItem vItem)
        //    {
        //        this.Item = vItem;
        //    }

        //}//internal class SmartTagMenuItem


        //private System.Collections.ArrayList mySmartTagProviderList;
        //private System.Windows.Forms.ContextMenu myTagMenu;

        ///// <summary>
        ///// 初始化智能标签支持者对象列表
        ///// </summary>
        //private void InitSmartTagProvider()
        //{
        //    myTagMenu = new System.Windows.Forms.ContextMenu();
        //    mySmartTagProviderList = new System.Collections.ArrayList();
        //    mySmartTagProviderList.Add(new ImageTagProvider());
        //    mySmartTagProviderList.Add(new KBTagProvider());
        //    mySmartTagProviderList.Add(new InputTagProvider());
        //    mySmartTagProviderList.Add(new DivTagProvider());
        //    mySmartTagProviderList.Add(new ParagraphTagProvider());
        //    mySmartTagProviderList.Add(new HRuleTagProvider());
        //    mySmartTagProviderList.Add(new TextFlagProvider());

        //    foreach (SmartTagProvider p in mySmartTagProviderList)
        //    {
        //        p.OwnerControl = this;
        //        p.OwnerDocument = myEMRDoc;
        //    }
        //}
        ///// <summary>
        ///// 返回智能支持标签支持者对象列表
        ///// </summary>
        //public System.Collections.ArrayList SmartTagProviderList
        //{
        //    get { return mySmartTagProviderList; }
        //}
        //internal SmartTagProvider GetSmartTagProvider(ZYTextElement myElement)
        //{
        //    if (myElement == null)
        //        return null;
        //    if (myEMRDoc.Loading == false)
        //    {
        //        if (myElement != null && myElement.Deleteted == false && myElement.Parent.Locked == false)
        //        {
        //            foreach (SmartTagProvider p in mySmartTagProviderList)
        //            {
        //                p.Element = myElement;
        //                if (p.isEnable())
        //                {
        //                    return p;
        //                }
        //            }
        //            if (myElement.Parent != null)
        //            {
        //                foreach (SmartTagProvider p in mySmartTagProviderList)
        //                {
        //                    p.Element = myElement.Parent;
        //                    if (p.isEnable())
        //                    {
        //                        return p;
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    return null;
        //}

        //private bool PopupSmartTag(int x, int y)
        //{
        //    if (this.ShowingPopupList)
        //        this.HidePopupList();
        //    ZYTextElement element = this.GetElementByPos(x, y);
        //    if (element == null)
        //        return false;
        //    if (myDocument.IsLock(element))
        //        return false;

        //    SmartTagProvider p = this.GetSmartTagProvider(element);
        //    if (p != null)
        //    {
        //        if (p is DivTagProvider)
        //        {
        //            System.Collections.ArrayList myTagItems = new System.Collections.ArrayList();
        //            p.GetCommands(myTagItems);
        //            if (myTagItems.Count > 0)
        //            {
        //                myTagMenu.MenuItems.Clear();
        //                System.Windows.Forms.MenuItem TitleMenu = new System.Windows.Forms.MenuItem("标题设置");
        //                foreach (SmartTagItem myItem in myTagItems)
        //                {
        //                    TitleMenu.MenuItems.Add(new SmartTagMenuItem(myItem));
        //                }
        //                myTagMenu.MenuItems.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("undo"), "&U  撤销"));
        //                myTagMenu.MenuItems.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("redo"), "&R  重复"));
        //                myTagMenu.MenuItems.Add("-");
        //                myTagMenu.MenuItems.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("cut"), "&T  剪切"));
        //                myTagMenu.MenuItems.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("copy"), "&C  复制"));
        //                myTagMenu.MenuItems.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("paste"), "&P  粘贴"));
        //                myTagMenu.MenuItems.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("delete"), "&D  删除"));
        //                myTagMenu.MenuItems.Add("-");
        //                myTagMenu.MenuItems.Add(TitleMenu);
        //                myTagMenu.MenuItems.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("insertimage"), "&I  插入图片"));
        //                myTagMenu.MenuItems.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("insertkb"), "&K  插入知识库"));
        //                myTagMenu.MenuItems.Add(new ZYActionMenuItem(myEMRDoc.GetActionByName("insertgongshi"), "&G  插入公式"));
        //                myTagMenu.Show(this, new System.Drawing.Point(x, y));
        //                //myTagMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("设置标题");
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            System.Collections.ArrayList myTagItems = new System.Collections.ArrayList();
        //            p.GetCommands(myTagItems);
        //            if (myTagItems.Count > 0)
        //            {
        //                myTagMenu.MenuItems.Clear();
        //                foreach (SmartTagItem myItem in myTagItems)
        //                    myTagMenu.MenuItems.Add(new SmartTagMenuItem(myItem));
        //                myTagMenu.Show(this, new System.Drawing.Point(x, y));
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //} 

        //private void KBTreeView_KBItemClick(System.Windows.Forms.TreeNode TreeNode, KB_Item SelectedItem)
        //{
        //    if (SelectedItem.isTemplate())
        //    {
        //        myEMRDoc._InsertDocument(SelectedItem.ItemValue);
        //    }
        //}

        //private void KBTreeView_KBListClick(System.Windows.Forms.TreeNode TreeNode, KB_List SelectedList)
        //{
        //    myEMRDoc._InsertKB(SelectedList);
        //}
        #endregion

        #region bwy :
        /// <summary>
        /// 已重载:处理鼠标单击事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            this.Focus();
            base.OnClick(e);
            System.Drawing.Point p = this.ViewMousePosition;

            //Modified by wwj 2013-05-29 在平板上操作时，将单击操作作为双击操作进行处理
            if ((IsSingleClickAsDoubleClick && m_MouseDownPosition == Cursor.Position))
            {
                OnViewDoubleClick(new System.Windows.Forms.MouseEventArgs(System.Windows.Forms.Control.MouseButtons, 1, p.X, p.Y, 0));
            }
            else
            {
                OnViewClick(new System.Windows.Forms.MouseEventArgs(System.Windows.Forms.Control.MouseButtons, 1, p.X, p.Y, 0));
            }
        }

        /// <summary>
        /// 已重载:处理鼠标双击事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            System.Drawing.Point p = this.ViewMousePosition;
            OnViewDoubleClick(new System.Windows.Forms.MouseEventArgs(System.Windows.Forms.Control.MouseButtons, 1, p.X, p.Y, 0));

        }

        /// <summary>
        /// 已重载：鼠标按键按下事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_MouseDownPosition = Cursor.Position;
            m_IsMouseUp = false;

            #region bwy :
            //激发当前元素的鼠标键事件

            ZYTextElement element = this.GetElementByPos(e.X, e.Y);
            //由于现在container中foreach每个元素，调用元素的这个方法，因此需要在每个元素的该方法中先判断鼠标是否在该位置内，如果是，才能执行具体方法，这种方式有待优化。
            //但现在为了图片 的右键能显示出菜单，还是在此调用了下面方法。
            if (element is ZYTextImage)
            {
                element.HandleMouseDown(e.X, e.Y, e.Button);
            }
            if (element != null && element.Parent != null && element.Parent is ZYReplace)
            {
                element.Parent.HandleMouseDown(e.X, e.Y, e.Button);
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //续打
                if (myEMRDoc.EnableJumpPrint)
                {
                    int pos = base.GetJumpPrintPosition(e.X, e.Y);
                    if (pos >= 0)
                    {
                        pos = myEMRDoc.FixPageLine(pos);
                        if (pos > 0)
                        {
                            if (myEMRDoc.JumpPrintPosition != pos)
                            {
                                myEMRDoc.JumpPrintPosition = pos;
                                this.Refresh();
                            }
                        }
                    }
                    return;
                }
                //选中区域打印 Add By wwj 2012-04-17
                else if (myEMRDoc.EnableSelectAreaPrint)
                {
                    Point p = base.GetSelectAreaPoint(e.X, e.Y);
                    if (p != Point.Empty)
                    {
                        myEMRDoc.SelectAreaPrintLeftTopPoint = p;
                        myEMRDoc.SelectAreaPrintRightBottomPoint = p;
                        this.Refresh();
                    }
                }
            }

            if (this.bolMouseDragScroll)
            {
                this.myMouseDragPoint = new Point(e.X, e.Y);
            }
            #endregion bwy :

            if (this.myTransform.ContainsSourcePoint(e.X, e.Y))
            {
                System.Drawing.Point p = this.ClientPointToView(e.X, e.Y);
                OnViewMouseDown(new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
            }
        }


        /// <summary>
        /// 已重载:鼠标移动事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.bolScrolling)
            {
                System.Console.WriteLine(this.bolScrolling);
                return;
            }
            if (this.bolMouseDragScroll)
            {
                if (System.Windows.Forms.Control.MouseButtons == System.Windows.Forms.MouseButtons.Left)
                {
                    if (this.myMouseDragPoint.IsEmpty == false)
                    {
                        int dx = e.X - this.myMouseDragPoint.X;
                        int dy = e.Y - this.myMouseDragPoint.Y;
                        this.myMouseDragPoint = new Point(e.X, e.Y);
                        this.SetAutoScrollPosition(new Point(
                            -dx - this.AutoScrollPosition.X,
                            -dy - this.AutoScrollPosition.Y));
                        this.OnScroll();
                        return;
                    }
                }
                else
                {
                    myMouseDragPoint = System.Drawing.Point.Empty;
                }
            }

            if (this.myTransform.ContainsSourcePoint(e.X, e.Y))
            {
                System.Drawing.Point p = this.ClientPointToView(e.X, e.Y);
                OnViewMouseMove(new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
            }
            else
                this.Cursor = this.myDefaultCursor;

            //如果当前元素是可选元素，设置光标为手形
            if (this.GetElementByPos(e.X, e.Y) != null && this.GetElementByPos(e.X, e.Y).Parent is ZYTextBlock || this.GetElementByPos(e.X, e.Y) is ZYElement)
            {
                this.Cursor = Cursors.Hand;
            }

            //选中区域打印 Add By wwj 2012-04-17
            if (myEMRDoc.EnableSelectAreaPrint && !m_IsMouseUp)
            {
                Point p = base.GetSelectAreaPoint(e.X, e.Y);
                if (p != Point.Empty)
                {
                    myEMRDoc.SelectAreaPrintRightBottomPoint = p;
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// 判断鼠标是否按下
        /// 【1】在myEMRDoc.EnableSelectAreaPrint == true，即处于打印选中区域的时候，用于在鼠标按下弹起后选中的区域不再变化
        /// </summary>
        bool m_IsMouseUp = false;

        /// <summary>
        /// 已重载:鼠标按键松开事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_IsMouseUp = true;

            this.myMouseDragPoint = System.Drawing.Point.Empty;
            if (this.myTransform.ContainsSourcePoint(e.X, e.Y))
            {
                System.Drawing.Point p = this.ClientPointToView(e.X, e.Y);
                OnViewMouseUp(new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            OnScroll();
            base.OnMouseWheel(e);
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            OnScroll();
            base.OnScroll(se);

        }
        #endregion bwy :

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ZYEditorControl
            // 
            this.ForceShowCaret = true;
            this.GraphicsUnit = System.Drawing.GraphicsUnit.Document;
            this.ImeModeChanged += new System.EventHandler(this.ZYEditorControl_ImeModeChanged);
            this.ResumeLayout(false);

        }

        private void ZYEditorControl_ImeModeChanged(object sender, EventArgs e)
        {
            MessageBox.Show(e.ToString());
        }


        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            Debug.WriteLine("OnDragEnter");
            bolMouseDragScroll = true;
            drgevent.Effect = DragDropEffects.Copy;
            this.Focus();
            base.OnDragEnter(drgevent);
        }

        /// <summary>
        /// 在编辑器中拖动元素，
        /// todo：
        /// 发现人：wwj 
        ///     时间：2011-11-24           发现在第二次拖拽进入编辑器中会导致定位不准的Bug , 解决方法：将自动判断的滚动条的逻辑移除
        /// </summary>
        /// <param name="drgevent"></param>
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            //计算光标位置
            //Debug.WriteLine("OnDragOver");
            //判断如果当前元素的parent如果是zytextblock则不允许拖入
            Point p = new Point(drgevent.X, drgevent.Y);
            p = PointToClient(p);
            ZYTextElement element = this.GetElementByPos(p.X, p.Y);

            if (element != null)
            {
                Point viewMousePosition = this.ViewMousePosition;
                this.EMRDoc.ViewMouseDown(viewMousePosition.X, viewMousePosition.Y, MouseButtons.Left);
            }

            base.OnDragOver(drgevent);
        }

        /// <summary>
        /// 判断光标当前所在位置是否可以进行编辑操作 add by wwj
        /// </summary>
        /// <returns></returns>
        public bool CanEdit()
        {
            if (this.EMRDoc.Info.DocumentModel == DocumentModel.Edit || this.EMRDoc.Info.DocumentModel == DocumentModel.Design)
            {
                return true;
            }

            if (this.ActiveEditArea != null)
            {
                ActiveEditArea activeEditArea = this.ActiveEditArea;
                int selectIndex = this.EMRDoc.Content.SelectStart;
                ZYTextElement ele = this.EMRDoc.Elements[selectIndex] as ZYTextElement;
                if (activeEditArea.Top + activeEditArea.TopElement.Height <= ele.RealTop && ele.RealTop + ele.Height <= activeEditArea.End)
                {
                    return true;
                }
            }

            return false;
        }

        public void InsertElementByBytes(DragEventArgs drgevent)
        {
            OnDragDrop(drgevent);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            Debug.WriteLine("OnDragDrop");

            //Add By wwj 2011-10-14 判断是否可以进行拖拽
            if (!CanEdit()) return;

            bolMouseDragScroll = false;
            //获取数据

            //以前是用OutlookBarItem 传值，但只有此处用到，却要多引用一个另外的项目，所以改用KeyValuePair格式传值，为了保证以前的代码较小改动，虚拟了一个简单的OutlookBarItem类。尝试用过object传值但攻取总为null,也可以用字符串传值
            KeyValuePair<string, object> data = (KeyValuePair<string, object>)drgevent.Data.GetData(typeof(KeyValuePair<string, object>));

            OutlookBarItem item = new OutlookBarItem();
            item.Text = data.Key;
            item.Tag = data.Value;

            Debug.WriteLine("在文档中插入内容 " + item.Text + " " + item.Tag);

            this.EMRDoc.BeginUpdate();
            this.EMRDoc.BeginContentChangeLog();
            if (this.EMRDoc.Content.HasSelected())
            {
                this.EMRDoc.Content.DeleteSeleciton();
            }
            else if (this.EMRDoc.OwnerControl.InsertMode == false)
            {
                this.EMRDoc.Content.DeleteCurrentElement();
            }


            ZYTextElement myele = null;
            if (item.Tag is ElementType)
            {
                switch ((ElementType)item.Tag)
                {
                    case ElementType.SingleElement:
                        myele = new ZYSelectableElement(ElementType.SingleElement);
                        break;
                    case ElementType.MultiElement:
                        myele = new ZYSelectableElement(ElementType.MultiElement);
                        break;
                    case ElementType.HaveNotElement:
                        myele = new ZYSelectableElement(ElementType.HaveNotElement);
                        break;
                    case ElementType.CheckBox:
                        myele = new ZYCheckBox();
                        break;
                    case ElementType.FixedText:
                        myele = new ZYFixedText();
                        break;
                    case ElementType.FormatNumber:
                        myele = new ZYFormatNumber();
                        break;
                    case ElementType.FormatString:
                        myele = new ZYFormatString();
                        break;
                    case ElementType.FormatDatetime:
                        myele = new ZYFormatDatetime();
                        break;
                    case ElementType.PromptText:
                        myele = new ZYPromptText();
                        break;
                    case ElementType.Button:
                        myele = new ZYButton();
                        break;
                    case ElementType.MensesFormula:
                        myele = new ZYMensesFormula();
                        break;
                    //新增的牙齿检查 add by ywk 2012年11月27日16:56:41
                    case ElementType.ToothCheck:
                        myele = new ZYToothCheck();
                        break;

                    case ElementType.HorizontalLine:
                        myele = new ZYHorizontalLine();
                        break;
                    case ElementType.PageEnd:
                        myele = new ZYPageEnd();
                        break;
                    case ElementType.Macro:
                        myele = new ZYMacro();
                        break;
                    case ElementType.Replace:
                        myele = new ZYReplace();
                        break;
                    case ElementType.Template:
                        myele = new ZYTemplate();
                        break;
                    case ElementType.Flag:
                        myele = new ZYFlag();
                        break;
                    case ElementType.Text:
                        myele = new ZYText();
                        break;

                    case ElementType.LookUpEditor:
                        myele = new ZYLookupEditor();
                        break;
                }
            }



            // 首先试图找到向前最近的一个字符类型的数据
            ZYTextChar defChar = this.EMRDoc.Content.GetPreChar();
            //设置元素的格式
            if (defChar == null)
            {
                defChar = new ZYTextChar();
                //defChar.Char = 'M';
            }
            if ((item.Text != "ZYTemplate") && (item.Text != "ZYTextImage"))
            {
                defChar.Attributes.CopyTo(myele.Attributes);
                //不继承上下标
                myele.Attributes.SetValue(ZYTextConst.c_Sup, false);
                myele.Attributes.SetValue(ZYTextConst.c_Sub, false);
                myele.UpdateAttrubute();
            }

            if ((myele is ZYText) && ((this.EMRDoc.Info.DocumentModel == DocumentModel.Edit) || (this.EMRDoc.Info.DocumentModel == DocumentModel.Read)))
            {
                this.EMRDoc._InserString(item.Text);
                this.EMRDoc.ContentChanged();
                this.EMRDoc.EndContentChangeLog();
                this.EMRDoc.EndUpdate();
                base.OnDragDrop(drgevent);
                return;
            }


            if (myele is ZYTextBlock)
            {
                ((ZYTextBlock)myele).Name = item.Text;
                ((ZYTextBlock)myele).Text = item.Text;
                ((ZYTextBlock)myele).OwnerDocument = this.EMRDoc;
                ((ZYTextBlock)myele).Parent = this.EMRDoc.Content.CurrentElement.Parent;
                this.EMRDoc._InsertBlock(((ZYTextBlock)myele));
            }

            if (myele is ZYElement)
            {
                ((ZYElement)myele).Name = item.Text;
                this.EMRDoc._InsertElement((ZYElement)myele);
                ((ZYElement)myele).OwnerDocument = this.EMRDoc;

            }

            //todo add by 
            if ((item.Text.Equals("ZYTemplate") && (item.Tag != null)))
            {
                this.EMRDoc.InsertTemplate((byte[])item.Tag);

            }
            if ((item.Text.Equals("ZYTextImage") && (item.Tag != null)))
            {
                this.EMRDoc._InsertImage((byte[])item.Tag);
            }

            this.EMRDoc.ContentChanged();
            this.EMRDoc.EndContentChangeLog();
            this.EMRDoc.EndUpdate();

            //this.EMRDoc.UpdateCaret();
            base.OnDragDrop(drgevent);
        }

        #region bwy : 根据excel功能对照表，添加没有的功能,由于zytextdocument被m在用，先写在这里
        /// <summary>
        /// 清空编辑缓冲区
        /// </summary>
        public void CleanUndoBuffer()
        {
            this.EMRDoc.UndoStack.ClearAll();
        }

        /// <summary>
        /// 删除未使用的元素,用于自动排版功能：去除元素的必点属性限制，去除没有展开的子模版
        /// </summary>
        public void DeleteBlankElement()
        {
            this.EMRDoc.BeginUpdate();
            this.EMRDoc.BeginContentChangeLog();
            ArrayList al = new ArrayList();
            this.EMRDoc.GetAllSpecElement(al, this.EMRDoc.RootDocumentElement, ElementType.All, null);
            foreach (object o in al)
            {
                if (o is ZYTemplate)
                {
                    (o as ZYTemplate).Parent.RemoveChild(o as ZYTextElement);
                }
                else if (o is ZYTextBlock)
                {
                    (o as ZYTextBlock).MustClick = false;
                }

                else if (o is ZYElement)
                {
                    (o as ZYElement).MustClick = false;
                }
            }

            this.EMRDoc.ContentChanged();
            this.EMRDoc.EndContentChangeLog();
            this.EMRDoc.EndUpdate();
        }

        /// <summary>
        /// 设置行间距
        /// </summary>
        /// <param name="n">倍数</param>
        public void SetLineSpace(float f)
        {
            int rowheight = (int)((float)this.EMRDoc.View.DefaultRowPixelHeight * f);
            this.EMRDoc.Info.LineSpacing = rowheight;
            this.EMRDoc.Info.ParagraphSpacing = rowheight;
            this.EMRDoc.ContentChanged();
            this.EMRDoc.EndUpdate();
        }

        /// <summary>
        /// 获取文档状态
        /// </summary>
        /// <returns></returns>
        public DocumentModel GetDocumentModel()
        {
            return this.EMRDoc.Info.DocumentModel;
        }

        /// <summary>
        /// 设置文档状态
        /// </summary>
        /// <returns></returns>
        public void SetDocumentModel(DocumentModel value)
        {
            this.EMRDoc.Info.DocumentModel = value;
        }

        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="title"></param>
        public void SetTitle(string title)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.EMRDoc.HeadString);
            XmlNodeList nodelist = doc.SelectNodes("header/p");
            nodelist[0].FirstChild.InnerText = title;
            this.EMRDoc.HeadString = doc.InnerXml;
            //this.EMRDoc.Refresh();
            this.Refresh();
        }

        /// <summary>
        /// 获取标题
        /// </summary>
        /// <param name="title"></param>
        public string GetTitle()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.EMRDoc.HeadString);
            XmlNodeList nodelist = doc.SelectNodes("header/p");
            return nodelist[0].FirstChild.InnerText;
        }

        /// <summary>
        /// 设置子标题
        /// </summary>
        /// <param name="title"></param>
        public void SetSubTitle(string title)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.EMRDoc.HeadString);
                XmlNodeList nodelist = doc.SelectNodes("header/p");
                nodelist[1].FirstChild.InnerText = title;
                this.EMRDoc.HeadString = doc.InnerXml;

                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("请停止一切操作，请赶快联系 软件公司的技术人员 .\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 获取子标题
        /// </summary>
        /// <param name="title"></param>
        public string GetSubTitle()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.EMRDoc.HeadString);
            XmlNodeList nodelist = doc.SelectNodes("header/p");
            return nodelist[1].FirstChild.InnerText;
        }

        /// <summary>
        /// 设置三级标题
        /// </summary>
        /// <param name="title"></param>
        public void SetSub3Title(string title)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.EMRDoc.HeadString);
            XmlNodeList nodelist = doc.SelectNodes("header/p");
            nodelist[2].InnerText = title;
            this.EMRDoc.HeadString = doc.InnerXml;

            this.Refresh();
        }

        /// <summary>
        /// 获取三级标题,用于替换内容时用
        /// </summary>
        /// <param name="title"></param>
        public string GetSub3Title()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.EMRDoc.HeadString);
            XmlNodeList nodelist = doc.SelectNodes("header/text");
            return nodelist[2].InnerText;
        }
        /// <summary>
        /// 检查没有被关注过的必填元素
        /// </summary>
        /// <returns>没有被关注过的必填元素列表</returns>
        public ArrayList CheckMustClickElement()
        {
            ArrayList al = new ArrayList();
            ArrayList alremove = new ArrayList();
            //查找所有MustClick=true && Clicked = false的元素
            this.EMRDoc.GetAllSpecElement(al, this.EMRDoc.RootDocumentElement, ElementType.All, null);
            foreach (object o in al)
            {
                if (o is ZYTextBlock)
                {
                    if ((o as ZYTextBlock).MustClick && !(o as ZYTextBlock).Clicked)
                    {
                        //保留元素在列表中
                    }
                    else
                    {
                        alremove.Add(o);
                    }
                }
                else if (o is ZYElement)
                {
                    if ((o as ZYElement).MustClick && !(o as ZYElement).Clicked)
                    {
                        //保留元素在列表中
                    }
                    else
                    {
                        alremove.Add(o);
                    }
                }
            }

            //删除已点元素，保留未点元素
            foreach (object o in alremove)
            {
                al.Remove(o);
            }

            if (al.Count > 0)
            {
                if (this.EMRDoc.Info.DocumentModel == DocumentModel.Test)
                {
                    string msg = "有 " + al.Count + " 个必填元素还没有填。";
                    MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                if (al[0] is ZYTextBlock)
                {
                    this.EMRDoc.Content.MoveSelectStart((al[0] as ZYTextBlock).FirstElement);
                }
                else if (al[0] is ZYElement)
                {
                    this.EMRDoc.Content.MoveSelectStart(al[0] as ZYTextBlock);
                }

            }
            else
            {
                if (this.EMRDoc.Info.DocumentModel == DocumentModel.Test)
                {
                    MessageBox.Show("所有必填元素都已填。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return al;
        }

        /// <summary>
        /// 插入类似于诊断结果的返回值，在插入之前已经进行了“清空”操作，text 数组为每一行中的内容,只能用于按钮事件
        /// </summary>
        /// <param name="name">提示名称</param>
        /// <param name="text">内容</param>
        public void InsertResultPar(string[] text)
        {
            string name = "";
            Point mousep = PointToClient(Control.MousePosition);

            ZYTextElement curele = this.EMRDoc.Content.CurrentElement;

            ZYButton bt = null; ;
            if (curele is ZYButton)
            {
                bt = curele as ZYButton;
            }
            else
            {
                return;
            }

            if (bt != null)
            {
                name = bt.Event;
            }

            this.EMRDoc.BeginUpdate();
            this.EMRDoc.BeginContentChangeLog();
            //先删除包含该名字的元素所在的行
            ArrayList alele = new ArrayList();
            this.EMRDoc.GetAllSpecElement(alele, this.EMRDoc.RootDocumentElement, ElementType.Text, name);
            ArrayList alpara = new ArrayList();
            foreach (ZYTextElement ele in alele)
            {
                if (!alpara.Contains(ele.Parent))
                {
                    alpara.Add(ele.Parent);
                }
            }

            ////删除这些段，包括在这些段之间的段
            //if (alpara.Count > 0)
            //{
            //    int startp = this.EMRDoc.RootDocumentElement.IndexOf(alpara[0] as ZYTextElement);
            //    int endp = this.EMRDoc.RootDocumentElement.IndexOf(alpara[alpara.Count - 1] as ZYTextElement);
            //    alpara.Clear();
            //    for (int i = endp; i >= startp; i--)
            //    {
            //        alpara.Add(this.EMRDoc.RootDocumentElement.ChildElements[i]);
            //    }
            //}

            //这里应该用可以撤销的操作
            if (alpara.Count > 0)
            {
                (alpara[0] as ZYTextParagraph).Parent.RemoveChildRange(alpara);
                //由于按钮 可能在表格中了，所以段落，可能不再是RootDocumentElement 的第一子级了
                //this.EMRDoc.RootDocumentElement.RemoveChildRange(alpara);
            }


            ZYTextElement p = this.EMRDoc.Content.CurrentElement.Parent;
            while (!(p is ZYTextParagraph))
            {
                p = p.Parent;
            }

            ZYTextParagraph curpara = p as ZYTextParagraph;
            //设置当前元素为当前段落的最后一个元素，以便插入新段时位置正确
            this.EMRDoc.Content.CurrentElement = curpara.LastElement;


            ZYTextParagraph para = new ZYTextParagraph();
            this.EMRDoc.Content.InsertParagraph(para);

            for (int i = 0; i < text.Length; i++)
            {
                ZYText prom = new ZYText();
                prom.Name = name;
                prom.Text = text[i];
                prom.Parent = para;
                prom.OwnerDocument = this.EMRDoc;
                this.EMRDoc._InsertBlock(prom);
                //插入软回车
                //最后一个不输入软回车
                if (i < text.Length - 1)
                {
                    ZYTextLineEnd lineEnd = new ZYTextLineEnd();
                    lineEnd.Parent = para;
                    lineEnd.OwnerDocument = this.EMRDoc;
                    this.EMRDoc._InsertElement(lineEnd);
                }
            }

            this.EMRDoc.EndContentChangeLog();
            this.EMRDoc.EndUpdate();
        }

        /// <summary>
        /// 插入类似于诊断结果的返回值，在插入之前“不”进行清空操作,只能用于按钮事件
        /// </summary>
        /// <param name="name">提示名称</param>
        /// <param name="text">内容</param>
        public void InsertResultPar(string text)
        {
            string name = "";
            ZYTextElement curele = this.EMRDoc.Content.CurrentElement;
            ZYButton bt = null;
            if (curele is ZYButton)
            {
                bt = curele as ZYButton;
            }
            else
            {
                return;
            }

            if (bt != null)
            {
                name = bt.Event;
            }


            this.EMRDoc.BeginUpdate();
            this.EMRDoc.BeginContentChangeLog();


            ZYTextElement p = this.EMRDoc.Content.CurrentElement.Parent;
            while (!(p is ZYTextParagraph))
            {
                p = p.Parent;
            }

            ZYTextParagraph curpara = p as ZYTextParagraph;
            //设置当前元素为当前段落的最后一个元素，以便插入新段时位置正确
            this.EMRDoc.Content.CurrentElement = curpara.LastElement;

            ZYTextParagraph para = new ZYTextParagraph();
            this.EMRDoc.Content.InsertParagraph(para);

            ZYText prom = new ZYText();
            prom.Name = name;

            prom.Parent = para;
            prom.OwnerDocument = this.EMRDoc;

            prom.Text = text;

            this.EMRDoc._InsertBlock(prom);

            this.EMRDoc.EndContentChangeLog();
            this.EMRDoc.EndUpdate();
        }


        /// <summary>
        /// 在当前按钮后面插入一个文本元素，不换行 
        /// </summary>
        /// <param name="str"></param>
        public void InsertTextElementAfterButton(string str)
        {
            ZYTextElement curele = this.EMRDoc.Content.CurrentElement;
            ZYButton bt = null; ;
            if (curele is ZYButton)
            {
                bt = curele as ZYButton;
            }
            else
            {
                return;
            }

            ZYText ele = new ZYText();
            ele.Name = bt.Event;
            ele.Text = str;

            this.BeginUpdate();
            this.EMRDoc.BeginContentChangeLog();

            bt.Parent.InsertAfter(ele, bt);

            this.EMRDoc.EndContentChangeLog();
            this.EndUpdate();

            this.EMRDoc.ContentChanged();
            this.Refresh();
        }

        /// <summary>
        /// 在当前按钮后面插入一个文本元素，不换行 
        /// </summary>
        /// <param name="str"></param>
        public void InsertStringAfterButton(string str)
        {
            ZYTextElement curele = this.EMRDoc.Content.CurrentElement;
            ZYButton bt = null; ;
            if (curele is ZYButton)
            {
                bt = curele as ZYButton;
            }
            else
            {
                return;
            }

            this.EMRDoc.Content.CurrentElement = this.EMRDoc.Content.GetNextElement(curele);

            this.EMRDoc._InserString(str);
        }

        /// <summary>
        /// 元素景色
        /// </summary>
        public static System.Drawing.Color elementBackColor = SystemColors.Control;
        /// <summary>
        /// 元素景色
        /// </summary>
        public static System.Drawing.Color ElementBackColor
        {
            get
            {
                //if (this.bolLockingUI)
                //{
                //    return Color.Transparent;
                //}
                //else
                //{
                return elementBackColor;
                //}
            }
            set { elementBackColor = value; }
        }

        /// <summary>
        /// 元素样式
        /// </summary>
        static string elementStyle = "背景色";
        /// <summary>
        /// 元素样式
        /// </summary>
        public static string ElementStyle
        {
            get
            {
                return elementStyle;
            }
            set { elementStyle = value; }
        }

        /// <summary>
        /// 删除指定的元素
        /// </summary>
        /// <param name="ele"></param>
        public void DeleteElement(ZYTextElement ele)
        {
            if (ele.Parent is ZYTextBlock)
            {
                ele = ele.Parent;
            }

            ////删除ele
            this.BeginUpdate();
            this.EMRDoc.BeginContentChangeLog();

            ele.Parent.RemoveChild(ele);
            this.EMRDoc.ContentChanged();
            this.EMRDoc.EndContentChangeLog();
            this.EndUpdate();
        }
        #endregion bwy :

        #region bwy : 控件初始化时外界需要传给控件的值，包括按钮事件类型等
        //字典列表
        public static ArrayList WordBookList = new ArrayList();
        public static ArrayList helpEventList = new ArrayList();


        public delegate string HelpButtonClick(string name);
        public event HelpButtonClick OnHelpButtonClick = null;

        public string FireHelpButtonClick(string name)
        {
            if (OnHelpButtonClick != null)
            {
                if (name.Length != 0)
                {
                    return OnHelpButtonClick(name);
                }
            }
            return "";
        }
        #endregion bwy :

        #region bwy : 从外界获取xml文档
        /// <summary>
        /// 从编辑器控件外部获得xml文档
        /// </summary>
        /// <param name="type">元素类型</param>
        /// <param name="name">元素名称</param>
        /// <returns></returns>
        public delegate XmlDocument GetOuterData(ElementType type, string name);
        public delegate byte[] GetOuterDataBinary(ElementType type, string name);
        /// <summary>
        /// 从编辑器控件外部获得xml文档的委托事件
        /// </summary>
        public event GetOuterData OnGetOuterData = null;
        public event GetOuterDataBinary OnGetOuterDataBinary = null;

        public XmlDocument FireGetOuterData(ElementType type, string name)
        {
            if (OnGetOuterDataBinary != null)
            {
                byte[] data = OnGetOuterDataBinary(type, name);
                return BinaryToXml(data);
            }

            if (OnGetOuterData != null)
            {
                return OnGetOuterData(type, name);
            }
            return null;
        }

        /// <summary>
        /// 默认字体大小 add by wwj 2012-05-29
        /// </summary>
        public static string DefaultFontSize = "小四";

        public float GetDefaultFontSize()
        {
            return FontCommon.GetFontSizeByName(DefaultFontSize);
        }


        public static string GetDefaultSettings(string name)
        {
            string result = "";
            switch (name)
            {
                case "fontname":
                    result = "宋体";
                    break;
                case "fontsize":
                    //result = "五号";
                    result = DefaultFontSize;
                    break;
                case "paperkind":
                    result = "A4";
                    break;
                case "linespace":
                    result = "1";
                    break;
                case "elestyle":
                    result = "背景色";
                    break;
                case "elecolor":
                    result = ColorTranslator.ToHtml(SystemColors.Control).ToString();
                    break;
                //纸张类型
                case "width":
                    result = "210";
                    break;
                case "height":
                    result = "297";
                    break;
                case "aspect":
                    result = false.ToString();
                    break;
                case "topmargin":
                    result = "20";
                    break;
                case "bottommargin":
                    result = "13";
                    break;
                case "leftmargin":
                    result = "20";
                    break;
                case "rightmargin":
                    result = "13";
                    break;
                case "headertype":
                    result = "1";
                    break;
                case "footertype":
                    result = "2";
                    break;
            }

            return result;

        }

        /// <summary>
        /// 页面默认的设置对象
        /// </summary>
        /// <returns></returns>
        public static XPageSettings GetDefaultPageSettings()
        {
            XPageSettings Xps = new XPageSettings();
            int vWidth = (int)(XDesignerCommon.MeasureConvert.MillimeterToHundredthsInch(Convert.ToDouble(GetDefaultSettings("width"))));
            int vHeight = (int)(XDesignerCommon.MeasureConvert.MillimeterToHundredthsInch(Convert.ToDouble(GetDefaultSettings("height"))));
            PaperKind pk = (PaperKind)Enum.Parse(typeof(PaperKind), GetDefaultSettings("paperkind"));
            Xps.PaperSize = new XPaperSize(pk, vWidth, vHeight);

            Xps.Margins.Left = (int)XDesignerCommon.MeasureConvert.MillimeterToHundredthsInch(Convert.ToDouble(GetDefaultSettings("topmargin")));
            Xps.Margins.Top = (int)XDesignerCommon.MeasureConvert.MillimeterToHundredthsInch(Convert.ToDouble(GetDefaultSettings("bottommargin")));
            Xps.Margins.Right = (int)XDesignerCommon.MeasureConvert.MillimeterToHundredthsInch(Convert.ToDouble(GetDefaultSettings("leftmargin")));
            Xps.Margins.Bottom = (int)XDesignerCommon.MeasureConvert.MillimeterToHundredthsInch(Convert.ToDouble(GetDefaultSettings("rightmargin")));
            //为了和以前 0，1 兼容
            try
            {
                Xps.Landscape = bool.Parse(GetDefaultSettings("aspect"));
            }
            catch
            {
                Xps.Landscape = false;
            }
            return Xps;
        }

        /// <summary>
        /// 获取对象对应的属性类对象
        /// </summary>
        /// <param name="ele"></param>
        /// <returns></returns>
        public static object CreatePropertyObj(ZYTextElement element)
        {
            object ele = null;

            if (element.Parent is ZYSelectableElement)
            {
                if ((element.Parent as ZYSelectableElement).Type == ElementType.HaveNotElement)
                {
                    ele = new PropertyHaveNotElement(element.Parent);
                }
                else if ((element.Parent as ZYSelectableElement).Type == ElementType.MultiElement)
                {
                    ele = new PropertyMultiSelectableElement(element.Parent);
                }
                else if ((element.Parent as ZYSelectableElement).Type == ElementType.SingleElement)
                {
                    ele = new PropertySelectableElement(element.Parent);
                }
            }
            else if (element.Parent is ZYLookupEditor)
            {
                ele = new PropertyLookUpeditorElement(element.Parent);
            }
            else if (element.Parent is ZYFixedText)
            {
                ele = new PropertyFixedText(element.Parent);
            }
            else if (element.Parent is ZYFormatNumber)
            {
                ele = new PropertyFormatNumber(element.Parent);
            }
            else if (element.Parent is ZYFormatDatetime)
            {
                ele = new PropertyFormatDatetime(element.Parent);
            }
            else if (element.Parent is ZYFormatString)
            {
                ele = new PropertyFormatString(element.Parent);
            }
            else if (element.Parent is ZYPromptText)
            {
                ele = new PropertyPromptText(element.Parent);
            }
            else if (element.Parent is ZYText)
            {
                ele = new PropertyText(element.Parent);
            }
            else if (element.Parent is ZYMacro || element.Parent is ZYReplace || element.Parent is ZYTemplate || element is ZYPageEnd)
            {
                ele = new PropertyBase(element.Parent);
            }
            else if (element is ZYPageEnd)
            {
                ele = new PropertyBase(element);
            }
            else if (element is ZYButton)
            {
                ele = new PropertyButton(element);
            }
            else if (element is ZYMensesFormula)
            {
                ele = new PropertyMensesFormula(element);
            }
            //新增的牙齿检查 add by ywk 2012年11月27日16:57:15
            else if (element is ZYToothCheck)
            {
                ele = new PropertyToothCheck(element);
            }

            else if (element is ZYHorizontalLine)
            {
                ele = new PropertyHorizontalLine(element);
            }
            else if (element is ZYCheckBox)
            {
                ele = new PropertyCheckBox(element);
            }
            else if (element is ZYFlag)
            {
                ele = new PropertyFlag(element);
            }


            return ele;
        }
        /// <summary>
        /// 滚动视图顶端到相应位置
        /// </summary>
        public void ScrollViewtopToCurrentElement()
        {
            Point p = this.EMRDoc.Content.CurrentElement.Bounds.Location;
            this.AutoScrollPosition = new Point(0 - this.AutoScrollPosition.X, this.ViewPointToClient(p).Y - this.AutoScrollPosition.Y);
        }
        #endregion bwy :

        /// <summary>
        /// 设置中文输入法 Add By wwj 2012-04-16
        /// </summary>
        private void SetChineseIEM()
        {
            if (m_IsHaveSetChineseIEM) return;
            //this.ImeMode = System.Windows.Forms.ImeMode.Off;
            //if (this.ImeMode != System.Windows.Forms.ImeMode.Off)
            {
                foreach (InputLanguage MyInput in InputLanguage.InstalledInputLanguages)
                {
                    if (MyInput.LayoutName.IndexOf("拼音") >= 0)
                    {
                        InputLanguage.CurrentInputLanguage = MyInput;
                        m_IsHaveSetChineseIEM = true;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 在第一次调用编辑器的时候设置输入法
        /// </summary>
        public static bool m_IsHaveSetChineseIEM = false;

        #region 获取复用项目的数据 Add By wwj 2013-08-09

        /// <summary>
        /// 获取需要替换的病历内容
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static string GetReplaceTextByName(string emrContent, string itemName)
        {
            try
            {
                #region 这样捞取标签后面的元素，会造成一些模板中如果存在写在一起(add by yangwk   2013年5月14日17:16:26) 的情况。会把复用项目中的内容全部拖过来
                //string resultstring = string.Empty;
                //resultstring = this.zyEditorControl1.EMRDoc.GetReplaceText(itemName);
                //return resultstring;

                #endregion
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(emrContent);
                XmlNode itemNode = doc.GetElementsByTagName("roelement").Cast<XmlNode>().Where(node =>
                {
                    if (((XmlElement)node).HasAttribute("type") && ((XmlElement)node).HasAttribute("name"))
                    {
                        if (node.Attributes["type"].Value == "fixedtext" && node.Attributes["name"].Value == itemName)
                        {
                            return true;
                        }
                    }
                    return false;
                }).FirstOrDefault();
                if (itemNode != null)
                {
                    string replaceTextFirst = string.Empty;
                    string replaceTextFirstTemp = string.Empty;

                    bool isEncounterRightZYFlag = false;//是否遇到元素ZYFlag:"["
                    bool isEncounterLeftZYFlag = false;//是否遇到元素ZYFlag:"]"
                    bool isEncounterZYFlag = false;//是否遇到元素ZYFlag

                    bool isContinue = GetReplaceTextInner(itemNode, ref replaceTextFirst, false, out isEncounterRightZYFlag, out isEncounterLeftZYFlag);
                    replaceTextFirstTemp = replaceTextFirst;
                    CheckIsEncounterZYFlag(ref isEncounterZYFlag, isEncounterRightZYFlag, isEncounterLeftZYFlag);

                    if (isContinue)
                    {
                        string replaceTextSecond = string.Empty;

                        while (itemNode.ParentNode != null && itemNode.ParentNode.NextSibling != null)
                        {
                            if (itemNode.ParentNode.NextSibling.ChildNodes.Count > 0)
                            {
                                itemNode = itemNode.ParentNode.NextSibling.ChildNodes[0];
                                isContinue = GetReplaceTextInner(itemNode, ref replaceTextSecond, true, out isEncounterRightZYFlag, out isEncounterLeftZYFlag);

                                CheckIsEncounterZYFlag(ref isEncounterZYFlag, isEncounterRightZYFlag, isEncounterLeftZYFlag);

                                if (isEncounterRightZYFlag)
                                {
                                    replaceTextFirst = replaceTextSecond;
                                }
                                else
                                {
                                    replaceTextFirst += replaceTextSecond;
                                }

                                if (!isContinue)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (!isEncounterZYFlag)
                    {
                        replaceTextFirst = replaceTextFirstTemp;
                    }

                    return replaceTextFirst;
                }
                return "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 判断是否遭遇到元素ZYFlag
        /// </summary>
        /// <param name="isEncounterZYFlag"></param>
        /// <param name="isEncounterRightZYFlag"></param>
        /// <param name="isEncounterLeftZYFlag"></param>
        private static void CheckIsEncounterZYFlag(ref bool isEncounterZYFlag, bool isEncounterRightZYFlag, bool isEncounterLeftZYFlag)
        {
            try
            {
                if (!isEncounterZYFlag)
                {
                    isEncounterZYFlag = isEncounterRightZYFlag || isEncounterLeftZYFlag;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取itemNode所在段落的可替换项的内容
        /// 当遇到元素ZYFlag:"]"、或元素RoleElement不需要再循环调用此方法GetReplaceTextInner
        /// 例如：
        /// 【1】 
        /// 主诉：123456789
        /// 123456789
        /// 返回：123456789
        /// 【2】
        /// 主诉：123456789
        /// 12345]6789
        /// 返回：12345678912345
        /// 【3】
        /// 主诉：123[456789
        /// 12345]6789
        /// 返回：45678912345
        /// 【4】
        /// 主诉：123[456789
        /// 123456789
        /// 返回：456789123456789
        /// </summary>
        /// <param name="itemNode"></param>
        /// <param name="text1">从当前节点itemNode开始查找的病历内容</param>
        /// <param name="text2">从元素ZYFlag："[" 开始查找的病历内容</param>
        /// <param name="isNewParagraph">是否为新段落</param>
        /// <returns>表示是否下次再调用此方法</returns>
        private static bool GetReplaceTextInner(XmlNode itemNode, ref string replaceText, bool isNewParagraph, out bool isEncounterRightZYFlag, out bool isEncounterLeftZYFlag)
        {
            try
            {
                replaceText = string.Empty;
                isEncounterRightZYFlag = false;
                isEncounterLeftZYFlag = false;

                StringBuilder replaceTextStringBuilder = new StringBuilder();

                bool isContinue = true;//表示是否下次再调用此方法

                if (!isNewParagraph)
                {
                    itemNode = itemNode.NextSibling;
                }

                while (itemNode != null)
                {
                    if (itemNode.Name == "btnelement" && ((XmlElement)itemNode).HasAttribute("print"))//排除掉不显示“按钮”的文字
                    {
                        if (itemNode.Attributes["print"].Value.ToLower() == "false")
                        {
                            itemNode = itemNode.NextSibling;
                            continue;
                        }
                    }
                    #region 处理单选元素selement，将选中的元素抓取出来
                    if (itemNode.Name.ToLower() == "selement")
                    {
                        if (itemNode.ChildNodes.Count > 0)
                        {
                            var selectedNodes = itemNode.ChildNodes.Cast<XmlNode>().Where(node =>
                            {
                                if (((XmlElement)node).HasAttribute("selected") && node.Attributes["selected"].Value.ToLower() == "true")
                                {
                                    return true;
                                }
                                return false;
                            });
                            if (selectedNodes.Count() > 0)
                            {
                                foreach (var selectedNode in selectedNodes)
                                {
                                    //text += selectedNode.InnerText;
                                    replaceTextStringBuilder.Append(selectedNode.InnerText);
                                }
                            }
                            else
                            {
                                //text += itemNode.Attributes["text"].Value;
                                replaceTextStringBuilder.Append(itemNode.Attributes["text"].Value);
                            }
                        }
                        else
                        {
                            //text += itemNode.Attributes["text"].Value;
                            replaceTextStringBuilder.Append(itemNode.Attributes["text"].Value);
                        }
                        itemNode = itemNode.NextSibling;
                        continue;
                    }
                    #endregion

                    if (itemNode.Name.ToLower() == "roelement")//如果是roelement则退出数据的抓取
                    {
                        isContinue = false;
                        break;
                    }
                    else if (itemNode.Name.ToLower() == "flag")
                    {
                        if (((XmlElement)itemNode).HasAttribute("direction")
                            && itemNode.Attributes["direction"].Value.ToUpper() == "LEFT")//如果是ZYFlag："]"则退出数据的抓取
                        {
                            isEncounterLeftZYFlag = true;
                            isContinue = false;
                            break;
                        }
                        else if (((XmlElement)itemNode).HasAttribute("direction")
                           && itemNode.Attributes["direction"].Value.ToUpper() == "RIGHT")
                        {
                            isEncounterRightZYFlag = true;
                            //text = string.Empty;
                            replaceTextStringBuilder = new StringBuilder();
                        }
                    }
                    else
                    {
                        //text += itemNode.InnerText;
                        replaceTextStringBuilder.Append(itemNode.InnerText);
                    }

                    itemNode = itemNode.NextSibling;
                }

                //text = text.TrimStart(':').TrimStart('：');
                replaceText = replaceTextStringBuilder.ToString().TrimStart(':').TrimStart('：');

                return isContinue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }// class ZYEditorControl 

    public class ActiveEditArea
    {
        ZYTextElement topElement;
        public ZYTextElement TopElement
        {
            get { return topElement; }
            set { topElement = value; }
        }
        ZYTextElement endElement;
        public ZYTextElement EndElement
        {
            get { return endElement; }
            set { endElement = value; }
        }
        public int Top
        {
            get
            {
                return TopElement.RealTop;
            }
        }
        public int End
        {
            get
            {
                //如果End 是文档最后一个元素,返回最后一个元素的底线

                if (EndElement == null)
                {
                    ZYTextElement end = TopElement.OwnerDocument.Content.Elements[TopElement.OwnerDocument.Content.Elements.Count - 1] as ZYTextElement;
                    Debug.WriteLine("文档的最后一个元素 " + end.Bounds);
                    return end.Bounds.Bottom;
                }
                else
                {
                    Debug.WriteLine("文档的最后一个元素 " + EndElement.Bounds);
                    return EndElement.RealTop;
                }
            }
        }

    }

    public class OutlookBarItem
    {
        public string Text;
        public object Tag;
    }



}

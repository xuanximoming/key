#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Wordbook;
using DevExpress.XtraNavBar;
using System.Collections.ObjectModel;

#endregion

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// 显示预定义字典类的UserControl
   /// </summary>
   [ToolboxBitmapAttribute(typeof(DrectSoft.Wordbook.UCtrlWordbookTree), "Images.UCtrlWordbookTree.ico")]
   public partial class UCtrlWordbookTree : UserControl
   {
      #region property
      /// <summary>
      /// 当前选中的字典的类信息
      /// </summary>
      public WordbookInfo CurrentBookInfo
      {
         get { return _wordbookInfo; }
      }
      private WordbookInfo _wordbookInfo;
      #endregion

      #region private variable
      private Collection<WordbookInfo> m_BookCollection;
      #endregion

      /// <summary>
      /// 创建字典类选择树控件
      /// </summary>
      public UCtrlWordbookTree()
      {
         InitializeComponent();

         if (!this.DesignMode)
         {
            m_BookCollection = new Collection<WordbookInfo>();
            if (navBarControl1.ActiveGroup != navGroupHospital)
               navBarControl1.ActiveGroup = navGroupHospital;
            else
               navBarControl1_ActiveGroupChanged(this, new NavBarGroupEventArgs(navGroupHospital));
            //// 初始化代码字典树
            //InitializeWordbookSorts();
         }
      }

      #region private method
      ///// <summary>
      ///// 初始化字典分类
      ///// </summary>
      //private void InitializeWordbookSorts()
      //{
      //   treeWordbook.Dock = DockStyle.Fill;
      //   navBarControl1.Groups.Clear();

      //   // 创建代码字典分类
      //   NavBarGroup newItem;
      //   NavBarGroupControlContainer ctrlContainer;
      //   foreach (KeyValuePair<string, string> catalog in WordbookStaticInfo.WordbookCatalogs)
      //   {
      //      newItem = new NavBarGroup();
      //      ctrlContainer = new NavBarGroupControlContainer();

      //      newItem.Caption = catalog.Value;
      //      newItem.LargeImage = imageListWordbookSort.Images[catalog.Key];
      //      newItem.SmallImage = imageListWordbookSort.Images[catalog.Key];
      //      newItem.ControlContainer = ctrlContainer;
      //      newItem.GroupClientHeight = -1;
      //      newItem.GroupStyle = NavBarGroupStyle.ControlContainer;
      //      newItem.Tag = catalog.Key;

      //      navBarControl1.Groups.Add(newItem);
      //   }

      //   if (navBarControl1.Groups.Count > 0)
      //   {
      //      navBarControl1.Groups[0].Expanded = true;
      //   }
      //}

      /// <summary>
      /// 显示当前字典类别所属的字典
      /// </summary>
      private void ShowCurrentSortDictionaries(string catalog)
      {
         m_BookCollection.Clear();
         listBoxWordbook.Items.Clear();
         
         foreach (WordbookInfo info in WordbookStaticHandle.WordbookList)
         {
            if (info.Catalog == catalog)
            {
               m_BookCollection.Add(info);
               listBoxWordbook.Items.Add(String.Format(CultureInfo.CurrentCulture
                  , "{0,-16}{1}", info.Name, info.TypeName));
            }
         }
      }
      #endregion

      #region events

      /// <summary>
      /// 字典树双击事件
      /// </summary>
      public event EventHandler<EventArgs> TreeDoubleClick;

      
      private void OnChanged(EventArgs e)
      {
         if (TreeDoubleClick != null)
            TreeDoubleClick(this, e);
      }

      private void navBarControl1_ActiveGroupChanged(object sender, NavBarGroupEventArgs e)
      {
         navBarControl1.ActiveGroup.ControlContainer.Controls.Add(listBoxWordbook);
         //treeWordbook.BringToFront();

         ShowCurrentSortDictionaries(navBarControl1.ActiveGroup.Caption);
      }

      private void listBoxWordbook_DoubleClick(object sender, EventArgs e)
      {
         int index = listBoxWordbook.SelectedIndex;
         if (index >= 0)
         {
            _wordbookInfo = m_BookCollection[index];
            OnChanged(e);
         }
      }
      #endregion

   }
}

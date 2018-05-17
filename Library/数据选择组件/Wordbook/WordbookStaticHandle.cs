/*****************************************************************************\
**             Yindansoft & DrectSoft Software Co. Ltd.                          **
**                                                                           **
**             和字典类有关的一些静态方法和定义                                **
**                                                                           **
**                                                                           **
\*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DrectSoft.Common.Eop;
using DrectSoft.Wordbook.Schema;
using System.Xml.Serialization;
using DrectSoft.Core;
using System.Linq;

namespace DrectSoft.Wordbook
{
    /// <summary>
    /// 字典类静态信息和处理字典配置文件的静态方法
    /// </summary>
    public static class WordbookStaticHandle
    {
        #region public properties
        /// <summary>
        /// 代码字典分类(key: 字典分类的标识 value: 字典分类的名称)
        /// </summary>
        public static Dictionary<string, string> WordbookCatalogs
        {
            get
            {
                if (_wordbookCatalogs == null)
                {
                    _wordbookCatalogs = new Dictionary<string, string>();

                    foreach (WordbookCatalog catalog in AllWordbook.Catalogs)
                        _wordbookCatalogs.Add(catalog.CatalogName, catalog.Caption);
                }
                return _wordbookCatalogs;
            }
        }
        private static Dictionary<string, string> _wordbookCatalogs;

        /// <summary>
        /// 所有固定的代码字典类信息列表
        /// </summary>
        public static Collection<WordbookInfo> WordbookList
        {
            get
            {
                if (_wordbookList == null)
                {
                    _wordbookList = new Collection<WordbookInfo>();

                    foreach (WordbookCatalog catalog in AllWordbook.Catalogs)
                        foreach (Wordbook.Schema.Wordbook book in catalog.Wordbooks)
                            _wordbookList.Add(new WordbookInfo(catalog.Caption, book.Caption, book.WordbookName));
                }

                return _wordbookList;
            }
        }
        private static Collection<WordbookInfo> _wordbookList;
        #endregion

        #region private variables & properties
        private static Assembly m_WordbookAssembly;

        private static Wordbooks AllWordbook
        {
            get
            {

                if (_allWordbook == null)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Wordbooks));
                    Stream wordbookConfig = BasicSettings.GetConfig(BasicSettings.WordbookSetting);
                    _allWordbook = serializer.Deserialize(wordbookConfig) as Wordbooks;
                }
                return _allWordbook;
            }
        }
        private static Wordbooks _allWordbook;
        #endregion

        #region private methods
        private static string ConvertBookNameToFullName(string name)
        {
            char[] separator = new char[] { '.' };
            string[] pieceOfName = name.Split(separator);
            if (pieceOfName.Length == 0)
                throw new ArgumentException("字典名称为空");
            // 手工对Function类名做转换，以后模板基础数据修改后可以去掉！！！
            if (pieceOfName[pieceOfName.Length - 1] == "Function")
                pieceOfName[pieceOfName.Length - 1] = "BiologyFunction";
            return "DrectSoft.Wordbook." + pieceOfName[pieceOfName.Length - 1];
        }
        #endregion

        #region public methods
        /// <summary>
        /// 通过字典名称获取对应的字典类实例
        /// </summary>
        /// <param name="name">字典类的类名（全称或分类名+字典名）</param>
        /// <returns>类名正确，返回相应实例，否则报异常</returns>
        public static BaseWordbook GetWordbook(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(MessageStringManager.GetString("NullParameterName"));
            // 用Assembly.LoadFrom()需要解决不同应用程序域的问题，
            // 现在用此方式得到字典类的程序集，并进行反射处理。
            // 目前还不清楚这样做对性能有何影响，如果不行的话，只能还采用硬编码的方式
            Type bookType = null;
            if (m_WordbookAssembly == null)
            {
                Assembly[] assemlys = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemlys)
                {
                    //MessageBox.Show(assembly.GetName().Name);
                    // 通过比较Assembly的名称得到字典类的程序集
                    if (assembly.GetName().Name == "DrectSoft.Basic.Wordbook")
                    {
                        m_WordbookAssembly = assembly;
                        break;
                    }
                }
            }

            bookType = m_WordbookAssembly.GetType(ConvertBookNameToFullName(name));

            if (bookType == null)
            {
                //MessageBox.Show("Could not found the wordbook");
                return null;
            }
            else
            {
                BaseWordbook result = (BaseWordbook)System.Activator.CreateInstance(bookType); ;
                result.CacheTime = 0;
                return result;
            }
        }

        /// <summary>
        /// 通过字典名称获取对应的字典类实例
        /// </summary>
        /// <param name="name">字典类的中文（全称或分类名+字典名）</param>
        /// <returns>类名正确，返回相应实例，否则报异常</returns>
        public static BaseWordbook GetWordbookByName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(MessageStringManager.GetString("NullParameterName"));


            var wordbooks = WordbookList.Where(wd => wd.Name.Equals(name));

            try
            {
                WordbookInfo info = wordbooks.First();
                return GetWordbook(info.TypeName);
            }
            catch
            {
                return null;
            }


        }

        /// <summary>
        /// 根据传入的Wordbook关键信息字符串生成字典实例
        /// </summary>
        /// <param name="keyInfo">字典的关键信息组成的字符串</param>
        /// <returns>字典类实例</returns>
        public static BaseWordbook GetWordbookByString(string keyInfo)
        {
            if (String.IsNullOrEmpty(keyInfo))
                return null;
            //throw new ArgumentNullException("字典类名为空");

            // 解析出字典类名字
            int p = keyInfo.IndexOf(SeparatorSign.OtherSeparator);
            if (p < 0)
                return null;
            //throw new ArgumentException("未定义字典类名");
            keyInfo = keyInfo.Substring(p + 3, keyInfo.Length - p - 3);
            p = keyInfo.IndexOf(SeparatorSign.OtherSeparator);
            if (p < 0)
                return null;

            BaseWordbook wordbook = GetWordbook(keyInfo.Substring(0, p));
            if (wordbook != null)
                wordbook.ParameterValueComb = keyInfo.Substring(p + 3, keyInfo.Length - p - 3);
            return wordbook;
        }

        /// <summary>
        /// 获取指定字典的查询语句定义
        /// </summary>
        /// <param name="bookName">指定的字典名称</param>
        /// <returns>查询语句</returns>
        public static string GetWordbookQuerySentence(string bookName)
        {
            Schema.Wordbook book = GetSourceWordbookByName(bookName);
            if (book != null)
                return PersistentObjectFactory.GetQuerySentenceByName(book.QuerySentence);
            else
                throw new ArgumentOutOfRangeException("缺少指定字典的查询语句定义");
        }
        #endregion

        #region internal methods
        /// <summary>
        /// 根据字典名称，找到对应的原始的字典类实例
        /// </summary>
        /// <param name="bookName">由分类名和字典名组成的名称</param>
        /// <returns></returns>
        internal static Schema.Wordbook GetSourceWordbookByName(string bookName)
        {
            string[] pieceOfBookName = bookName.Split('.');
            foreach (WordbookCatalog catalog in AllWordbook.Catalogs)
            {
                if (catalog.CatalogName != pieceOfBookName[0])
                    continue;

                foreach (Schema.Wordbook book in catalog.Wordbooks)
                    if (book.WordbookName == pieceOfBookName[1])
                        return book;
            }

            return null;
        }
        #endregion
    }
}

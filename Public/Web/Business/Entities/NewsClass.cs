using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrectSoft.Emr.Web.Business.Service;

namespace DrectSoft.Emr.Web.Business.Entities
{
    public class NewsClass
    {

        #region EntityProperty

        private Guid _id;
        private string _className;
        private string _summary;

        /// <summary>
        /// 类别ID
        /// </summary>
        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 类别名称 
        /// </summary>
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        /// <summary>
        /// 类别说明 
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }
       
        static IList<NewsClass> _allNewsClass;
        static NewsPub _dal = new NewsPub();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="classname"></param>
        /// <param name="summary"></param>
        public NewsClass(Guid id, string classname, string summary)
        {
            _id = id;
            _className = classname;
            _summary = summary;
        }
        public NewsClass()
        { 
        
        }

        /// <summary>
        /// 所有分类
        /// </summary>
        public static IList<NewsClass> AllClass
        {
            get
            {
                _allNewsClass = _dal.GetClassList();
                return _allNewsClass;
            }
        }
        /// <summary>
        /// 得到指定分类
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static NewsClass SelectClass(Guid Id)
        {
            foreach (NewsClass news in _allNewsClass)
            {
                if (news._id == Id) return news;
            }
            return null;
        }
        #endregion

       
    }
}

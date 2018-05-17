using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrectSoft.Emr.Web.Business.Service;

namespace DrectSoft.Emr.Web.Business.Entities
{
    public class NewsContent
    {
        public NewsContent()
        {

        }

          #region Model

        private string _id;
        private string _title;
        private string _author;
        private DateTime _addtime;
        private string _classid;
        private string _departmentid;
        private string _defaultimage;
        private string _content;
        private int _valid;
        
        /// <summary>
        /// 新闻ID
        /// </summary>
        public string ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            set { _author = value; }
            get { return _author; }
        }
        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 部门
        /// </summary>
        public string DepartMentID
        {
            set { _departmentid = value; }
            get { return _departmentid; }
        }
        /// <summary>
        /// 类别
        /// </summary>
        public string ClassID
        {
            set { _classid = value; }
            get { return _classid; }
        }
        /// <summary>
        /// 默认图片地址
        /// </summary>
        public string DefaultImage
        {
            set { _defaultimage = value; }
            get { return _defaultimage; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 是否作废
        /// </summary>
        public int Valid
        {
            set { _valid = value; }
            get { return _valid; }
        }
  
        public NewsContent(string id, string title, string author, DateTime addtime, string classid, string departmentid, string defaultimage,string content, int valid)
        {
            _id = id; _title = title; _author = author; _addtime = addtime; _classid = classid; _departmentid = departmentid; _defaultimage = defaultimage; _valid = valid; _content = content;
        }

        static IList<NewsContent> _allNews;
        static NewsContentPub _dal = new NewsContentPub();
        /// <summary>
        /// 所有
        /// </summary>
        public static IList<NewsContent> AllNews
        {
            get
            {
                _allNews = _dal.GetNewsList();
                return _allNews;
            }
        }
       
        /// <summary>
        /// 得到指定新闻
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static NewsContent SelectClass(string Id)
        {
            foreach (NewsContent news in _allNews)
            {
                if (news._id == Id) return news;
            }
            return null;
        }
         #endregion
    }
}


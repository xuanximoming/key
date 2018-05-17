using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.JobManager
{
    public class SearchParameter
    {
        private string _searchText;
        /// <summary>
        /// 搜索的文本
        /// </summary>
        [Description("日志条目消息必须包含此文本(不区分大小写)")]
        [Category("常规")]
        [Browsable(true)]
        [DisplayName("消息包含文本")]
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; }
        }

        private string _source;
        /// <summary>
        /// 源
        /// </summary>
        [Description("日志条目的源")]
        [Category("常规")]
        [Browsable(true)]
        [DisplayName("源")]
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        private string _user;
        /// <summary>
        /// 用户
        /// </summary>
        [DisplayName("用户")]
        [Description("与日志条目关联的用户")]
        [Category("连接")]
        [Browsable(true)]
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        private string _computer;
        /// <summary>
        /// 计算机
        /// </summary>
        [DisplayName("计算机")]
        [Description("与日志条目关联的计算机")]
        [Category("连接")]
        [Browsable(true)]
        public string Computer
        {
            get { return _computer; }
            set { _computer = value; }
        }

        private DateTime _startTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        [DisplayName("开始时间")]
        [Description("日志条目的创建时间不能早于此日期")]
        [Category("日期")]
        [Browsable(true)]
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private DateTime _endTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        [DisplayName("结束时间")]
        [Description("日志条目的创建时间不能晚于此日期")]
        [Category("日期")]
        [Browsable(true)]
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        private bool _used;
        /// <summary>
        /// 是否启用配置
        /// </summary>
        [Browsable(false)]
        public bool Used
        {
            get { return _used; }
            set { _used = value; }
        }

        /// <summary>
        /// 完整构造
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="source"></param>
        /// <param name="user"></param>
        /// <param name="computer"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public SearchParameter(string searchText, string source, string user, string computer
           , DateTime startTime, DateTime endTime, bool used)
        {
            _searchText = searchText;
            _source = source;
            _user = user;
            _computer = computer;
            _startTime = startTime;
            _endTime = endTime;
            _used = used;
        }

        public SearchParameter()
        { }

        /// <summary>
        /// 判断是否为空的参数，返回值true表示非空，false表示空参数
        /// </summary>
        /// <returns></returns>
        public bool IsNotEmpty()
        {
            return !string.IsNullOrEmpty(this.Computer)
               || !string.IsNullOrEmpty(this.SearchText)
               || !string.IsNullOrEmpty(this.Source)
               || !string.IsNullOrEmpty(this.User)
               || !(this.StartTime == DateTime.MinValue)
               || !(this.EndTime == DateTime.MinValue
               || !(this.Used == false));
        }

        public SearchParameter Clone()
        {
            return new SearchParameter(this.SearchText, this.Source, this.User, this.Computer, this.StartTime, this.EndTime, this.Used);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SearchParameter))
                return false;
            SearchParameter parameter = obj as SearchParameter;
            if (parameter.Computer == this.Computer
               && parameter.EndTime == this.EndTime
               && parameter.SearchText == this.SearchText
               && parameter.Source == this.Source
               && parameter.StartTime == this.StartTime
               && parameter.User == this.User
               && parameter.Used == this.Used)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string temp = string.Empty;
            bool flagHead = false;
            if (!string.IsNullOrEmpty(_searchText))
            {
                temp += string.Format("包含“{0}”", _searchText);
                flagHead = true;
            }
            if (StartTime != DateTime.MinValue && EndTime != DateTime.MinValue)
            {
                temp += flagHead ? "，" : string.Empty;
                temp += string.Format("发生在{0}和{1}之间", StartTime.ToString(), EndTime.ToString());
                if (!flagHead)
                    flagHead = true;
            }
            if (!string.IsNullOrEmpty(_source))
            {
                temp += flagHead ? "，" : string.Empty;
                temp += string.Format("具有源“{0}”", _source);
                if (!flagHead)
                    flagHead = true;
            }
            if (!string.IsNullOrEmpty(_user))
            {
                temp += flagHead ? "，" : string.Empty;
                temp += string.Format("用户是“{0}”", _user);
                if (!flagHead)
                    flagHead = true;
            }
            if (!string.IsNullOrEmpty(_computer))
            {
                temp += flagHead ? "，" : string.Empty;
                temp += string.Format("计算机是“{0}”", _computer);
                if (!flagHead)
                    flagHead = true;
            }
            if (flagHead)
                temp += "。";
            return temp;
        }
    }
}

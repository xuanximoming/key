using DrectSoft.Common.Eop;

namespace DrectSoft.Core.DoctorAdvice
{
    /// <summary>
    /// 表示医嘱视图，目的是展现医嘱单上要显示的各项内容（未区分长期和临时）
    /// </summary>
    public sealed class OrderView
    {
        #region public const fieldname of unbound column
        /// <summary>
        /// 开始日期字段名
        /// </summary>
        public const string UNStartDate = "UNStartDate";
        /// <summary>
        /// 开始时间字段名
        /// </summary>
        public const string UNStartTime = "UNStartTime";
        /// <summary>
        /// 医嘱内容字段名
        /// </summary>
        public const string UNContent = "UNContent";
        /// <summary>
        /// 停止日期字段名
        /// </summary>
        public const string UNCeaseDate = "UNCeaseDate";
        /// <summary>
        /// 停止时间字段名
        /// </summary>
        public const string UNCeaseTime = "UNCeaseTime";
        /// <summary>
        /// 创建者
        /// </summary>
        public const string ColCreator = "Creator";
        /// <summary>
        /// 停止者
        /// </summary>
        public const string ColCeasor = "Ceasor";
        #endregion

        #region properties
        /// <summary>
        /// 医嘱序号
        /// </summary>
        public decimal SerialNo
        {
            get { return _serialNo; }
            set { _serialNo = value; }
        }
        private decimal _serialNo;

        /// <summary>
        /// 分组序号(所在组的第一条医嘱的序号)
        /// </summary>
        public decimal GroupSerialNo
        {
            get { return _groupSerialNo; }
            set { _groupSerialNo = value; }
        }
        private decimal _groupSerialNo;

        /// <summary>
        /// 在组中的位置标记(唯一/头/中间/尾)
        /// </summary>
        public GroupPositionKind GroupPosFlag
        {
            get { return _groupPosFlag; }
            set { _groupPosFlag = value; }
        }
        private GroupPositionKind _groupPosFlag;

        /// <summary>
        /// 医嘱开始日期
        /// </summary>
        public string StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        private string _startDate;

        /// <summary>
        /// 医嘱开始时间
        /// </summary>
        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        private string _startTime;

        /// <summary>
        /// 医嘱内容
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        private string _content;

        ///// <summary>
        ///// 创建日期
        ///// </summary>
        //public DateTime CreateDate
        //{
        //   get { return _createDate; }
        //   set { _createDate = value; }
        //}
        //private DateTime _createDate;

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime
        //{
        //   get { return _createTime; }
        //   set { _createTime = value; }
        //}
        //private DateTime _createTime;

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        private string _creator;

        /// <summary>
        /// 审核日期
        /// </summary>
        public string AuditDate
        {
            get { return _auditDate; }
            set { _auditDate = value; }
        }
        private string _auditDate;

        /// <summary>
        /// 审核时间
        /// </summary>
        public string AuditTime
        {
            get { return _auditTime; }
            set { _auditTime = value; }
        }
        private string _auditTime;

        /// <summary>
        /// 审核者
        /// </summary>
        public string Auditor
        {
            get { return _auditor; }
            set { _auditor = value; }
        }
        private string _auditor;

        /// <summary>
        /// 执行日期
        /// </summary>
        public string ExecuteDate
        {
            get { return _executeDate; }
            set { _executeDate = value; }
        }
        private string _executeDate;

        /// <summary>
        /// 执行时间
        /// </summary>
        public string ExecuteTime
        {
            get { return _executeTime; }
            set { _executeTime = value; }
        }
        private string _executeTime;

        /// <summary>
        /// 执行者
        /// </summary>
        public string Executor
        {
            get { return _executor; }
            set { _executor = value; }
        }
        private string _executor;

        ///// <summary>
        ///// 取消日期
        ///// </summary>
        //public DateTime CancelDate
        //{
        //   get { return _cancelDate; }
        //   set { _cancelDate = value; }
        //}
        //private DateTime _cancelDate;

        ///// <summary>
        ///// 取消时间
        ///// </summary>
        //public DateTime CancelTime
        //{
        //   get { return _cancelTime; }
        //   set { _cancelTime = value; }
        //}
        //private DateTime _cancelTime;

        /// <summary>
        /// 取消者
        /// </summary>
        public string Cancellor
        {
            get { return _cancellor; }
            set { _cancellor = value; }
        }
        private string _cancellor;

        /// <summary>
        /// 停止日期
        /// </summary>
        public string CeaseDate
        {
            get { return _ceaseDate; }
            set { _ceaseDate = value; }
        }
        private string _ceaseDate;

        /// <summary>
        /// 停止时间
        /// </summary>
        public string CeaseTime
        {
            get { return _ceaseTime; }
            set { _ceaseTime = value; }
        }
        private string _ceaseTime;

        /// <summary>
        /// 停止者
        /// </summary>
        public string Ceasor
        {
            get { return _ceasor; }
            set { _ceasor = value; }
        }
        private string _ceasor;

        /// <summary>
        /// 停止审核日期
        /// </summary>
        public string CeaseAuditorDate
        {
            get { return _ceaseAuditorDate; }
            set { _ceaseAuditorDate = value; }
        }
        private string _ceaseAuditorDate;

        /// <summary>
        /// 停止审核时间
        /// </summary>
        public string CeaseAuditorTime
        {
            get { return _ceaseAuditorTime; }
            set { _ceaseAuditorTime = value; }
        }
        private string _ceaseAuditorTime;

        /// <summary>
        /// 停止审核者
        /// </summary>
        public string CeaseAuditor
        {
            get { return _ceaseAuditor; }
            set { _ceaseAuditor = value; }
        }
        private string _ceaseAuditor;

        /// <summary>
        /// 医嘱状态
        /// </summary>
        public OrderState State
        {
            get { return _state; }
        }
        private OrderState _state;

        /// <summary>
        /// 关联的申请单序号
        /// </summary>
        public decimal ApplySerialNo
        {
            get { return _applySerialNo; }
        }
        private decimal _applySerialNo;

        /// <summary>
        /// 是否已同步标志
        /// </summary>
        public bool HadSynch
        {
            get { return _hadSynch; }
        }
        private bool _hadSynch;

        /// <summary>
        /// 对应的医嘱实例
        /// </summary>
        public Order OrderCache
        {
            get { return _orderCache; }
        }
        private Order _orderCache;

        public object this[string name]
        {
            get
            {
                // 现在只返回Unbound列的数据
                switch (name)
                {
                    case OrderView.UNStartDate:
                        return _startDate;
                    case OrderView.UNStartTime:
                        return _startTime;
                    case OrderView.UNContent:
                        return _content;
                    case OrderView.UNCeaseDate:
                        return _ceaseDate;
                    case OrderView.UNCeaseTime:
                        return _ceaseTime;
                    default:
                        return null;
                }
            }
        }
        #endregion

        #region ctors
        /// <summary>
        /// 根据传入的医嘱对象创建医嘱视图
        /// </summary>
        /// <param name="order">医嘱对象</param>
        public OrderView(Order order)
        {
            ClearProperties();

            if (order != null)
            {
                _orderCache = order;
                ResetProperties();
            }
        }
        #endregion

        /// <summary>
        /// 清空属性
        /// </summary>
        private void ClearProperties()
        {
            _serialNo = -1;
            _startDate = "";
            _startTime = "";
            _creator = "";
            _content = "";
            _auditDate = "";
            _auditTime = "";
            _auditor = "";
            _cancellor = "";
            _executeDate = "";
            _executeTime = "";
            _executor = "";
            _ceaseDate = "";
            _ceaseTime = "";
            _ceasor = "";
            _ceaseAuditor = "";
            _groupSerialNo = -1;
            _groupPosFlag = GroupPositionKind.SingleOrder;
            _orderCache = null;
            _state = OrderState.None;
            _applySerialNo = -1;
        }

        /// <summary>
        /// 重置属性的值
        /// </summary>
        public void ResetProperties()
        {
            if (_orderCache == null)
                ClearProperties();
            else
            {
                _serialNo = _orderCache.SerialNo;
                _groupSerialNo = _orderCache.GroupSerialNo;
                _groupPosFlag = _orderCache.GroupPosFlag;
                _startDate = UILogic.ConvertToDateString(_orderCache.StartDateTime);
                _startTime = UILogic.ConvertToTimeString(_orderCache.StartDateTime.TimeOfDay);
                // 以下信息并不一定存在
                if ((_orderCache.CreateInfo != null) && (_orderCache.CreateInfo.HadInitialized))
                    _creator = _orderCache.CreateInfo.Executor.ToString();

                if (_orderCache.Content == null)
                    _content = "";
                else
                    _content = _orderCache.Content.ToString();

                if ((_orderCache.AuditInfo != null) && (_orderCache.AuditInfo.HadInitialized))
                {
                    _auditDate = UILogic.ConvertToDateString(_orderCache.AuditInfo.ExecuteTime);
                    _auditTime = UILogic.ConvertToTimeString(_orderCache.AuditInfo.ExecuteTime.TimeOfDay);
                    _auditor = _orderCache.AuditInfo.Executor.ToString();
                }

                if ((_orderCache.CancelInfo != null) && (_orderCache.CancelInfo.HadInitialized))
                {
                    _cancellor = _orderCache.CancelInfo.Executor.ToString();
                }

                if ((_orderCache.ExecuteInfo != null) && (_orderCache.ExecuteInfo.HadInitialized))
                {
                    _executeDate = UILogic.ConvertToDateString(_orderCache.ExecuteInfo.ExecuteTime);
                    _executeTime = UILogic.ConvertToTimeString(_orderCache.ExecuteInfo.ExecuteTime.TimeOfDay);
                    _executor = _orderCache.ExecuteInfo.Executor.ToString();
                }
                _state = _orderCache.State;
                _hadSynch = _orderCache.HadSync;

                TempOrder tempTemp = _orderCache as TempOrder;
                if (tempTemp != null)
                    _applySerialNo = tempTemp.ApplySerialNo;
                else
                    _applySerialNo = -1;

                LongOrder tempLong = _orderCache as LongOrder;
                if (tempLong != null)
                {
                    if ((tempLong.CeaseInfo != null) && (tempLong.CeaseInfo.HadInitialized))
                    {
                        _ceaseDate = UILogic.ConvertToDateString(tempLong.CeaseInfo.ExecuteTime);
                        _ceaseTime = UILogic.ConvertToTimeString(tempLong.CeaseInfo.ExecuteTime.TimeOfDay);
                        _ceasor = tempLong.CeaseInfo.Executor.ToString();
                    }

                    if ((tempLong.CeaseAuditInfo != null) && (tempLong.CeaseAuditInfo.HadInitialized))
                    {
                        _ceaseAuditorDate = UILogic.ConvertToDateString(tempLong.CeaseAuditInfo.ExecuteTime);
                        _ceaseAuditorTime = UILogic.ConvertToTimeString(tempLong.CeaseAuditInfo.ExecuteTime.TimeOfDay);
                        _ceaseAuditor = tempLong.CeaseAuditInfo.Executor.ToString();
                    }
                }
            }
        }
    }
}

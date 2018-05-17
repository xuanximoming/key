using DrectSoft.Common.Eop;
using System;

namespace DrectSoft.Core.DoctorAdvice
{
    /// <summary>
    /// 处理操作提示参数
    /// </summary>
    internal class ProcessHintArgs : EventArgs
    {
        /// <summary>
        /// 提示信息
        /// </summary>
        public string ProcessHint
        {
            get { return _processHint; }
            set { _processHint = value; }
        }
        private string _processHint;

        /// <summary>
        /// 处理操作提示参数
        /// </summary>
        /// <param name="processHint">提示信息</param>
        public ProcessHintArgs(string processHint)
        {
            _processHint = processHint;
        }
    }

    /// <summary>
    /// 数据提交事件的参数
    /// </summary>
    public class DataCommitArgs : EventArgs
    {
        /// <summary>
        /// 数据提交类型
        /// </summary>
        public DataCommitType CommitType
        {
            get { return _commitType; }
        }
        private DataCommitType _commitType;

        /// <summary>
        /// 标记是否已经处理过
        /// </summary>
        public bool Handled
        {
            get { return _handled; }
            set { _handled = value; }
        }
        private bool _handled;

        /// <summary>
        /// 数据提交事件的参数
        /// </summary>
        /// <param name="commitType">数据提交类型</param>
        public DataCommitArgs(DataCommitType commitType)
        {
            _commitType = commitType;
        }
    }

    /// <summary>
    /// 当前选中项目参数
    /// </summary>
    public class OrderItemArgs : EventArgs
    {
        /// <summary>
        /// 标记是否含有项目数据
        /// </summary>
        public bool HadData
        {
            get { return _hadData; }
        }
        private bool _hadData;

        /// <summary>
        /// 项目类型
        /// </summary>
        public ItemKind Kind
        {
            get { return _kind; }
        }
        private ItemKind _kind;

        /// <summary>
        /// 项目代码
        /// </summary>
        public string ItemCode
        {
            get { return _itemCode; }
        }
        private string _itemCode;

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
        }
        private string _itemName;

        /// <summary>
        /// 剂量单位
        /// </summary>
        public string DoseUnit
        {
            get { return _doseUnit; }
        }
        private string _doseUnit;

        /// <summary>
        /// 用法
        /// </summary>
        public string Usage
        {
            get { return _usage; }
        }
        private string _usage;

        /// <summary>
        /// 当前选中项目参数
        /// </summary>
        /// <param name="kind">项目类型</param>
        /// <param name="code">项目代码</param>
        /// <param name="name">项目名称</param>
        /// <param name="unit">剂量单位</param>
        /// <param name="usage">用法</param>
        public OrderItemArgs(bool hadData, ItemKind kind, string code, string name, string unit, string usage)
        {
            _hadData = hadData;
            _kind = kind;
            _itemCode = code;
            _itemName = name;
            _doseUnit = unit;
            _usage = usage;
        }
    }
}

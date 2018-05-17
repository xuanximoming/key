using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Core
{
    /// <summary>
    /// 自定义消息提示框
    /// </summary>
    public interface ICustomMessageBox
    {

        /// <summary>
        /// 自定义提示
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        DialogResult MessageShow(string message);

        /// <summary>
        /// Override Show
        /// </summary>
        /// <param name="message"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        DialogResult MessageShow(string message, CustomMessageBoxKind kind);
    }

    /// <summary>
    /// 自定义消息提示框的类型
    /// </summary>
    public enum CustomMessageBoxKind
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0,

        /// <summary>
        /// ! icon ok button
        /// </summary>
        WarningOk = 1,

        /// <summary>
        /// ok + cancel button
        /// </summary>
        WarningOkCancel = 2,

        /// <summary>
        /// ? icon yes + no button
        /// </summary>
        QuestionYesNo = 3,

        /// <summary>
        /// ? icon yes + no + cancel button
        /// </summary>
        QuestionYesNoCancel = 4,

        /// <summary>
        /// ? icon ok + cancel button
        /// </summary>
        QuestionOkCancel = 5,

        /// <summary>
        /// x icon ok button
        /// </summary>
        ErrorOk = 6,

        /// <summary>
        /// x icon ok + cancel button
        /// </summary>
        ErrorOkCancel = 7,

        /// <summary>
        /// x icon yes button
        /// </summary>
        ErrorYes = 8,

        /// <summary>
        /// x icon yes + no button
        /// </summary>
        ErrorYesNo = 9,

        /// <summary>
        /// x icon yes + no + cancel button
        /// </summary>
        ErrorYesNoCancel = 10,

        /// <summary>
        /// i icon ok button
        /// </summary>
        InformationOk = 11
    }
}

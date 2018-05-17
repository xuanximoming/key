using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace DrectSoft.Core
{
    /// <summary>
    /// 日志类实现
    /// </summary>
    public class DrectSoftLog : IDrectSoftLog
    {
        private ILog log;

        public DrectSoftLog() {
            log = LogManager.GetLogger(string.Empty);
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logname">logname</param>
        public DrectSoftLog(string logname)
        {
            log = LogManager.GetLogger(logname);
        }

        #region IDrectSoftLog 成员
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message">Message</param>
        public void Debug(object message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message">Message</param>
        public void Info(object message)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message">Message</param>
        public void Warn(object message)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message</param>
        public void Error(object message)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
        }

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message">Message</param>
        public void Fatal(object message)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
        }

        #endregion
    }
}

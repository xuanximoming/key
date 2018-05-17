#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using DrectSoft.Core.Properties;
#endregion

namespace DrectSoft.Core
{
    /// <summary>
    /// 自定义的非法用户异常
    /// </summary>
    [Serializable]
    public class InvalidUserIdException : Exception, ISerializable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="errMessage"></param>
        public InvalidUserIdException(string errMessage)
            : base(errMessage)
        {
        }

        /// <summary>
        /// Ctor2
        /// </summary>
        public InvalidUserIdException()
            : this(Resources.InvalidUser)
        {
        }

        /// <summary>
        /// Ctor3
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected InvalidUserIdException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// Ctor4
        /// </summary>
        /// <param name="errMessage"></param>
        /// <param name="innerException"></param>
        public InvalidUserIdException(string errMessage, Exception innerException)
            : base(errMessage, innerException)
        {
        }
    }

    /// <summary>
    /// 自定义的用户密码错误异常
    /// </summary>
    [Serializable]
    public class InvalidUserPasswordException : Exception, ISerializable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="errMessage"></param>
        public InvalidUserPasswordException(string errMessage)
            : base(errMessage)
        {
        }

        /// <summary>
        /// Ctor2
        /// </summary>
        public InvalidUserPasswordException()
            : this(Resources.InvalidPassword)
        {
        }

        /// <summary>
        /// Ctor3
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected InvalidUserPasswordException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// Ctor4
        /// </summary>
        /// <param name="errMessage"></param>
        /// <param name="innerException"></param>
        public InvalidUserPasswordException(string errMessage, Exception innerException)
            : base(errMessage, innerException)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace CLF.Service.Core
{
    public class ServiceResult<T>
    {
        public ServiceResult()
        {
            ResultType = ServiceResultType.Success;
            HttpStatusCode = System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        ///     初始化一个 业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        public ServiceResult(ServiceResultType resultType)
        {
            ResultType = resultType;
            Success = ResultType == ServiceResultType.Success;
            HttpStatusCode = System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        ///     初始化一个 定义返回消息的业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        /// <param name="message">业务返回消息</param>
        public ServiceResult(ServiceResultType resultType, string message)
            : this(resultType)
        {
            this.message = message;
        }

        /// <summary>
        ///     初始化一个 定义返回消息与附加数据的业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        /// <param name="message">业务返回消息</param>
        /// <param name="result">业务返回数据</param>
        public ServiceResult(ServiceResultType resultType, string message, T result)
            : this(resultType, message)
        {
            Result = result;
        }

        /// <summary>
        ///     初始化一个 定义返回消息与日志消息的业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        /// <param name="message">业务返回消息</param>
        /// <param name="logMessage">业务日志记录消息</param>
        public ServiceResult(ServiceResultType resultType, string message, string logMessage)
            : this(resultType, message)
        {
            LogMessage = logMessage;
        }

        /// <summary>
        ///     初始化一个 定义返回消息、日志消息与附加数据的业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        /// <param name="message">业务返回消息</param>
        /// <param name="logMessage">业务日志记录消息</param>
        /// <param name="result">业务返回数据</param>
        public ServiceResult(ServiceResultType resultType, string message, string logMessage, T result)
            : this(resultType, message, logMessage)
        {
            Result = result;
        }


        public System.Net.HttpStatusCode HttpStatusCode { get; set; }

        public bool Success { get; set; }

        /// <summary>
        ///     获取或设置 操作结果类型
        /// </summary>
        public ServiceResultType ResultType { get; set; }

        /// <summary>
        ///     获取或设置 操作返回信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 获取或设置 操作返回的日志消息，用于记录日志
        /// </summary>
        public string LogMessage { get; set; }

        /// <summary>
        ///     获取或设置 操作结果附加信息
        /// </summary>
        public T Result { get; set; }

        public bool success
        {
            get
            {
                return ResultType == ServiceResultType.Success;
            }
        }
    }

    /// <summary>
    ///     表示业务操作结果的枚举
    /// </summary>
    [Description("业务操作结果的枚举")]

    public enum ServiceResultType
    {
        /// <summary>
        ///     操作成功
        /// </summary>
        [Description("操作成功。")]
        [EnumMember]
        Success = 0,

        /// <summary>
        ///     操作取消或操作没引发任何变化
        /// </summary>
        [Description("操作没有引发任何变化，提交取消。")]
        [EnumMember]
        NoChanged = 1,

        /// <summary>
        ///     参数错误
        /// </summary>
        [Description("参数错误。")]
        [EnumMember]
        ParamError = 2,

        /// <summary>
        ///     指定参数的数据不存在
        /// </summary>
        [Description("指定参数的数据不存在。")]
        [EnumMember]
        QueryNull = 3,

        /// <summary>
        ///     权限不足
        /// </summary>
        [Description("当前用户权限不足，不能继续操作。")]
        [EnumMember]
        PurviewLack = 4,

        /// <summary>
        /// 非法操作
        /// </summary>
        [Description("非法操作。")]
        [EnumMember]
        IllegalOperation = 5,

        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        [EnumMember]
        Warning = 6,

        /// <summary>
        ///操作引发错误
        /// </summary>
        [Description("操作引发错误。")]
        [EnumMember]
        Error = 7
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Exceptions
{
    /// <summary>
    ///     公共辅助操作类
    /// </summary>
    public static class PublicHelper
    {
        #region 公共方法
        /// <summary>
        ///     向调用层抛出组件异常
        /// </summary>
        /// <param name="msg"> 自定义异常消息 </param>
        /// <param name="e"> 实际引发异常的异常实例 </param>
        public static ComponentException ThrowComponentException(string msg, System.Exception e = null)
        {
            if (string.IsNullOrEmpty(msg) && e != null)
            {
                msg = e.Message;
            }
            else if (string.IsNullOrEmpty(msg))
            {
                msg = "未知组件异常，详情请查看日志信息。";
            }
            return e == null ? new ComponentException(string.Format("组件异常：{0}", msg)) : new ComponentException(string.Format("组件异常：{0}", msg), e);
        }

        /// <summary>
        ///     向调用层抛出数据访问层异常
        /// </summary>
        /// <param name="msg"> 自定义异常消息 </param>
        /// <param name="e"> 实际引发异常的异常实例 </param>
        public static DataAccessException ThrowDataAccessException(string msg, System.Exception e = null)
        {
            if (string.IsNullOrEmpty(msg) && e != null)
            {
                msg = e.Message;
            }
            else if (string.IsNullOrEmpty(msg))
            {
                msg = "未知数据访问层异常，详情请查看日志信息。";
            }
            return e == null
                ? new DataAccessException(string.Format("数据访问层异常：{0}", msg))
                : new DataAccessException(string.Format("数据访问层异常：{0}", msg), e);
        }

        /// <summary>
        ///     向调用层抛出数据访问层异常
        /// </summary>
        /// <param name="msg"> 自定义异常消息 </param>
        /// <param name="e"> 实际引发异常的异常实例 </param>
        public static BusinessException ThrowBusinessException(string msg, System.Exception e = null)
        {
            if (string.IsNullOrEmpty(msg) && e != null)
            {
                msg = e.Message;
            }
            else if (string.IsNullOrEmpty(msg))
            {
                msg = "未知业务逻辑层异常，详情请查看日志信息。";
            }
            return e == null ? new BusinessException(string.Format("业务逻辑层异常：{0}", msg)) : new BusinessException(string.Format("业务逻辑层异常：{0}", msg), e);
        }

        #endregion
    }
}

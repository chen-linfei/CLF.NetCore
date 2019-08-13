using CLF.Common.Exceptions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CLF.Service.Core
{
    public static class ServiceWrapper
    {
        public static Task<ServiceResult<T>> InvokeAsync<T>(string serviceName, string methodName,
            Func<T> function)
        {
            T result;
            string message = "调用服务({0})的方法({1})操作{2}。";
            try
            {
                result = function();
                message = string.Format(message, serviceName, methodName, "成功");

                return Task.FromResult(new ServiceResult<T>(ServiceResultType.Success, "操作成功！", result));
            }
            catch (ArgumentNullException argnullex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + argnullex.Message);
                Log.Error(message);
                return Task.FromResult(new ServiceResult<T>(ServiceResultType.QueryNull, argnullex.Message));
            }
            catch (ArgumentException argnullex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + argnullex.Message);
                Log.Error(message);
                return Task.FromResult(new ServiceResult<T>(ServiceResultType.ParamError, argnullex.Message));
            }
            catch (ComponentException cex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + cex.Message);
                Log.Error(message);
                return Task.FromResult(new ServiceResult<T>(ServiceResultType.Error, cex.Message));
            }
            catch (DataAccessException daex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + daex.Message);
                Log.Error(message);
                return Task.FromResult(new ServiceResult<T>(ServiceResultType.Error, daex.Message));
            }
            catch (BusinessException bex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + bex.Message);
                Log.Error(message);
                return Task.FromResult(new ServiceResult<T>(ServiceResultType.Error, bex.Message));
            }
            catch (SqlException sqlex)
            {
                var sberrors = new StringBuilder();
                foreach (SqlError err in sqlex.Errors)
                {
                    sberrors.Append("SQL Error: " + err.Number + ", Message: " + err.Message + Environment.NewLine);
                }

                string throwMsg = sberrors.Length > 0 ? sberrors.ToString() : sqlex.Message;
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + throwMsg);

                Log.Error(message);
                return Task.FromResult(new ServiceResult<T>(ServiceResultType.Error, throwMsg));
            }
            catch (Exception ex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + ex.Message);
                Log.Error(message);
                return Task.FromResult(new ServiceResult<T>(ServiceResultType.Error, ex.Message));
            }
            finally
            {
            }
        }

        public static ServiceResult<T> Invoke<T>(string serviceName, string methodName,
            Func<T> function)
        {
            T result;
            string message = "调用服务({0})的方法({1})操作{2}。";
            try
            {
                result = function();
                message = string.Format(message, serviceName, methodName, "成功");
                return new ServiceResult<T>(ServiceResultType.Success, "操作成功！", result);
            }
            catch (ArgumentNullException argnullex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + argnullex.Message);
                Log.Error(message);
                return new ServiceResult<T>(ServiceResultType.QueryNull, argnullex.Message);
            }
            catch (ArgumentException argnullex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + argnullex.Message);
                Log.Error(message);
                return new ServiceResult<T>(ServiceResultType.ParamError, argnullex.Message);
            }
            catch (ComponentException cex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + cex.Message);
                Log.Error(message);
                return new ServiceResult<T>(ServiceResultType.Error, cex.Message);
            }
            catch (DataAccessException daex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + daex.Message);
                Log.Error(message);
                return new ServiceResult<T>(ServiceResultType.Error, daex.Message);
            }
            catch (BusinessException bex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + bex.Message);
                Log.Error(message);
                return new ServiceResult<T>(ServiceResultType.Error, bex.Message);
            }
            catch (BusinessPromptException bexp)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + bexp.Message);
                Log.Error(message);
                return new ServiceResult<T>(ServiceResultType.Error, bexp.Message);
            }
            catch (SqlException sqlex)
            {
                var sberrors = new StringBuilder();
                foreach (SqlError err in sqlex.Errors)
                {
                    sberrors.Append("SQL Error: " + err.Number + ", Message: " + err.Message + Environment.NewLine);
                }

                string throwMsg = sberrors.Length > 0 ? sberrors.ToString() : sqlex.Message;
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + throwMsg);

                Log.Error(message);
                return new ServiceResult<T>(ServiceResultType.Error, throwMsg);
            }
            catch (Exception ex)
            {
                message = string.Format(message, serviceName, methodName, "失败，错误消息为：" + ex.Message);
                Log.Error(message);
                return new ServiceResult<T>(ServiceResultType.Error, ex.Message);
            }
            finally
            {
            }
        }

    }
}

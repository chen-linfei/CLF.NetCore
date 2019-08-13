using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace CLF.Common.Extensions
{
 public  static   class ObjectExtensions
    {
        #region 类型转换
        /// <summary>
        ///     把对象类型转化为指定类型，转化失败时返回该类型默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败返回类型的默认值 </returns>
        public static T CastTo<T>(this object value)
        {
            object result;
            Type type = typeof(T);
            try
            {
                if (type.IsEnum)
                {
                    result = Enum.Parse(type, value.ToString());
                }
                else if (type == typeof(Guid))
                {
                    result = Guid.Parse(value.ToString());
                }
                else
                {
                    result = Convert.ChangeType(value, type);
                }
            }
            catch
            {
                result = default(T);
            }

            return (T)result;
        }

        /// <summary>
        ///     把对象类型转化为指定类型，转化失败时返回指定的默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <param name="defaultValue"> 转化失败返回的指定默认值 </param>
        /// <returns> 转化后的指定类型对象，转化失败时返回指定的默认值 </returns>
        public static T CastTo<T>(this object value, T defaultValue)
        {
            object result;
            Type type = typeof(T);
            try
            {
                result = type.IsEnum ? Enum.Parse(type, value.ToString()) : Convert.ChangeType(value, type);
            }
            catch
            {
                result = defaultValue;
            }
            return (T)result;
        }
        #endregion

        #region 数据类型判断
        /// <summary>
        /// 是否是日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDate(this object date)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(date))
            {
                return false;
            }
            string strdate = date.ToString();
            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                //如果日期字符串中存在非数字，则返回false
                //if (!IsInt(strdate))
                //{
                //    return false;
                //}

                #region 对纯数字进行解析
                //对8位纯数字进行解析
                if (strdate.Length == 8)
                {
                    //获取年月日
                    string year = strdate.Substring(0, 4);
                    string month = strdate.Substring(4, 2);
                    string day = strdate.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (strdate.Length == 6)
                {
                    //获取年月
                    string year = strdate.Substring(0, 4);
                    string month = strdate.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (strdate.Length == 5)
                {
                    //获取年月
                    string year = strdate.Substring(0, 4);
                    string month = strdate.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (strdate.Length == 4)
                {
                    //获取年
                    string year = strdate.Substring(0, 4);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion

                return false;
            }

        }
        public static bool IsDecimal(this object obj)
        {
            try
            {
                Convert.ToDecimal(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsInt(this object obj)
        {
            try
            {
                Convert.ToInt32(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsBoolean(this object obj)
        {
            try
            {
                Convert.ToBoolean(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 判断对象是否为空

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(this object data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (String.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            if (data is IEnumerable)
            {
                return ((IEnumerable)data).AsQueryable().Count() == 0;
            }
            //不为空
            return false;
        }
        #endregion

        #region 强制转化

        /// <summary>
        /// object转化为Bool类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ObjToBool(this object obj)
        {
            bool flag;
            if (obj == null)
            {
                return false;
            }
            if (obj.Equals(DBNull.Value))
            {
                return false;
            }
            return (Boolean.TryParse(obj.ToString(), out flag) && flag);
        }

        /// <summary>
        /// object强制转化为DateTime类型(吃掉异常)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ObjToDateNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            try
            {
                return new DateTime?(Convert.ToDateTime(obj));
            }
            catch (ArgumentNullException ex)
            {
                return null;
            }
        }

        public static DateTime ObjToDate(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj is null.");
            }
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// int强制转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ObjToInt(this object obj)
        {
            if (obj != null)
            {
                int num;
                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }
                if (Int32.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }
            return 0;
        }

        /// <summary>
        /// 强制转化为long
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ObjToLong(this object obj)
        {
            if (obj != null)
            {
                long num;
                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }
                if (Int64.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }
            return 0;
        }

        /// <summary>
        /// 强制转化可空int类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? ObjToIntNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            return new int?(ObjToInt(obj));
        }

        /// <summary>
        /// 强制转化为string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjToStr(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            if (obj.Equals(DBNull.Value))
            {
                return "";
            }
            return Convert.ToString(obj);
        }

        /// <summary>
        /// Decimal转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ObjToDecimal(this object obj)
        {
            if (obj == null)
            {
                return 0M;
            }
            if (obj.Equals(DBNull.Value))
            {
                return 0M;
            }
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return 0M;
            }
        }

        /// <summary>
        /// Decimal可空类型转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? ObjToDecimalNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            return new decimal?(ObjToDecimal(obj));
        }

        #endregion

        public static bool ObjIsInt(this object number)
        {
            //如果为空，认为验证不合格
            if (number.IsNullOrEmpty())
            {
                return false;
            }

            //清除要验证字符串中的空格
            string strNum = number.ToString().Trim();

            //模式字符串
            string pattern = @"^[0-9]+[0-9]*$";

            //验证
            return Regex.IsMatch(strNum, pattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证字符串是否有sql注入字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidInput(this object objInput)
        {
            try
            {
                if (objInput.IsNullOrEmpty())
                    return false;
                else
                {
                    string input = objInput.ToString();
                    //替换单引号
                    input = input.Replace("'", "''").Trim();

                    //检测攻击性危险字符串
                    string testString = "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
                    string[] testArray = testString.Split('|');
                    foreach (string testStr in testArray)
                    {
                        if (input.ToLower().IndexOf(testStr) != -1)
                        {
                            //检测到攻击字符串,清空传入的值
                            input = "";
                            return false;
                        }
                    }

                    //未检测到攻击字符串
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static T DeepClone<T>(T obj)
        {
            T objResult;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Position = 0;
                objResult = (T)bf.Deserialize(ms);
            }

            return objResult;
        }

        public static Dictionary<string, string> GetDisplayAttributes<T>()
        {
            var dict = new Dictionary<string, string>();

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    var displayAttr = attr as System.ComponentModel.DataAnnotations.DisplayAttribute;
                    if (displayAttr != null)
                    {
                        dict.Add(prop.Name, displayAttr.Name);
                    }
                }
            }
            return dict;
        }

        public static bool Contains(this List<string> list, string data, StringComparison stringComparison)
        {
            foreach (var item in list)
            {
                if (item.Equals(data, stringComparison))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 去掉字符串中的空白字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpace(this string str)
        {
            return Regex.Replace(str, @"\s", "");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CLF.Common.Extensions
{
   public static  class EnumExtensions
    {
        private static readonly Dictionary<string, List<string>> _descriptionListCache = new Dictionary<string, List<string>>();

        /// <summary>
        ///     获取枚举项的Description特性的描述文字
        /// </summary>
        /// <param name="enumeration"> </param>
        /// <returns> </returns>
        public static string ToDescription(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo[] members = type.GetMember(enumeration.CastTo<string>());
            if (members.Length > 0)
            {
                return members[0].ToDescription();
            }
            return enumeration.CastTo<string>();
        }

        public static List<string> GetEnumDescriptions(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            string typename = type.FullName;
            if (!_descriptionListCache.ContainsKey(typename))
            {
                var fields = type.GetFields().Where(field => field.IsLiteral);
                var values = new List<string>();
                foreach (var field in fields)
                {
                    var a = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (a != null && a.Length > 0)
                    {
                        values.Add(a[0].Description);
                    }
                    else
                    {
                        values.Add(field.Name);
                    }
                }

                _descriptionListCache[typename] = values;
            }

            return _descriptionListCache[typename];
        }

        public static List<string> GetEnumValues(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            var itemList = new List<string>();

            var listOfValues = Enum.GetValues(type);
            foreach (var value in listOfValues)
            {
                itemList.Add(value.ToString());
            }

            return itemList;
        }


        public static T GetEnumFromDescription<T>(string description)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException("description");
            }

            Type type = typeof(T);
            var fieldInfos = type.GetFields().Where(field => field.IsLiteral);
            foreach (var field in fieldInfos)
            {
                var a = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (a != null && a.Length > 0)
                {
                    if (a[0].Description.ToLower() == description.ToLower())
                    {
                        return (T)Enum.Parse(typeof(T), field.Name, true);
                    }
                }
                else if (field.Name.ToLower() == description.ToLower())
                {
                    return (T)Enum.Parse(typeof(T), field.Name, true);
                }
            }

            return (T)Enum.Parse(typeof(T), fieldInfos.First().Name, true);
        }

        public static IEnumerable<T> GetEnumList<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Enum类型</typeparam>
        /// <param name="exceptEnums">排除的Enum列表</param>
        /// <returns></returns>
        public static IEnumerable<T> GetEnumList<T>(List<T> exceptEnums)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var enumList = Enum.GetValues(typeof(T)).Cast<T>();
            return enumList.Except(exceptEnums);
        }

        public static Dictionary<int, string> GetEnumDictionary<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            Type type = typeof(T);

            var list = new Dictionary<int, string>();
            var enumList = Enum.GetValues(type).Cast<T>();
            foreach (var aEnum in enumList)
            {
                MemberInfo[] members = type.GetMember(aEnum.CastTo<string>());
                if (members.Length > 0)
                {
                    list.Add(aEnum.CastTo<int>(), members[0].ToDescription());
                }
                else
                {
                    list.Add(aEnum.CastTo<int>(), aEnum.CastTo<string>());
                }
            }

            return list;
        }

        public static Dictionary<int, string> GetEnumDictionary<T>(List<T> exceptEnums)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            Type type = typeof(T);

            var list = new Dictionary<int, string>();
            var enumList = Enum.GetValues(type).Cast<T>();
            foreach (var aEnum in enumList.Except(exceptEnums))
            {
                MemberInfo[] members = type.GetMember(aEnum.CastTo<string>());
                if (members.Length > 0)
                {
                    list.Add(aEnum.CastTo<int>(), members[0].ToDescription());
                }
                else
                {
                    list.Add(aEnum.CastTo<int>(), aEnum.CastTo<string>());
                }
            }
            return list;
        }
    }
}

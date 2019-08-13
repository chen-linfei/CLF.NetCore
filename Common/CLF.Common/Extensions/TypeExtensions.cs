using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CLF.Common.Extensions
{

    /// <summary>
    ///     类型扩展方法类
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///     判断指定类型是否为数值类型
        /// </summary>
        /// <param name="type">要检查的类型</param>
        /// <returns>是否是数值类型</returns>
        public static bool IsNumeric(this Type type)
        {
            return type == typeof(Byte)
                || type == typeof(Int16)
                || type == typeof(Int32)
                || type == typeof(Int64)
                || type == typeof(SByte)
                || type == typeof(UInt16)
                || type == typeof(UInt32)
                || type == typeof(UInt64)
                || type == typeof(Decimal)
                || type == typeof(Double)
                || type == typeof(Single);
        }

        /// <summary>
        ///  获取成员元数据的Description特性描述信息
        /// </summary>
        /// <param name="member">成员元数据对象</param>
        /// <param name="inherit">是否搜索成员的继承链以查找描述特性</param>
        /// <returns>返回Description特性描述信息，如不存在则返回成员的名称</returns>
        public static string ToDescription(this MemberInfo member, bool inherit = false)
        {
            DescriptionAttribute desc = member.GetAttribute<DescriptionAttribute>(inherit);
            return desc == null ? null : desc.Description;
        }

        /// <summary>
        /// 检查指定指定类型成员中是否存在指定的Attribute特性
        /// </summary>
        /// <typeparam name="T">要检查的Attribute特性类型</typeparam>
        /// <param name="memberInfo">要检查的类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>是否存在</returns>
        public static bool AttributeExists<T>(this MemberInfo memberInfo, bool inherit) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Any(m => (m as T) != null);
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).SingleOrDefault() as T;
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T[] GetAttributes<T>(this MemberInfo memberInfo, bool inherit) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
        }

        /// <summary>
        ///  public class GenericInterface<T> {}
        ///  public class GenericClass<T> : GenericInterface<T>{}
        ///  public class Test : GenericClass<SomeType>{}
        ///     
        ///  typeof(Test).IsSubclassOfRawGeneric(typeof(GenericInterface<>)) return true.
        ///  typeof(Test).IsSubclassOfRawGeneric(typeof(GenericClass<>)) return true.
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        /// <summary>
        ///  public class SomeType : BaseClass {}
        ///  public class GenericInterface<T> {}
        ///  public class GenericClass<T> : GenericInterface<T>{}
        ///  public class Test : GenericClass<SomeType>{}
        ///     
        ///  typeof(Test).IsSubclassOfRawClass(typeof(GenericInterface<BaseClass>)) return true.
        ///  typeof(Test).IsSubclassOfRawClass(typeof(GenericClass<BaseClass>)) return true.
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        public static bool IsSubclassOfRawClass(this Type toCheck, Type generic)
        {
            if (toCheck.IsSameOrSubclass(generic))
            {
                return true;
            }

            if (toCheck.IsSubclassOfRawGeneric(generic))
            {
                return true;
            }

            Type genRawClass = GetRawClassOfGenericType(generic);
            if (genRawClass == null)
            {
                return false;
            }

            var curGenericDef = generic.IsGenericType ? generic.GetGenericTypeDefinition() : generic;
            while (toCheck != null && toCheck != typeof(object))
            {
                var curDef = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                var chkRawClass = GetRawClassOfGenericType(toCheck);

                if (curDef.IsSubclassOfRawGeneric(curGenericDef)
                    && chkRawClass != null && chkRawClass.IsSameOrSubclass(genRawClass))
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }
            return false;
        }

        private static Type GetRawClassOfGenericType(Type generic)
        {
            if (!generic.IsGenericType)
                return null;

            if (generic.GetGenericArguments().Any())
                return generic.GetGenericArguments().FirstOrDefault();

            return null;
        }

        /// <summary>
        ///  public class Base { }
        ///  public class Derived : Base { }
        /// 
        ///  typeof(Derived).IsSameOrSubclass(typeof(Base)) return true.
        ///  typeof(Base).IsSameOrSubclass(typeof(Base)) return true.
        /// </summary>
        /// <param name="potentialBase"></param>
        /// <param name="potentialDescendant"></param>
        /// <returns></returns>
        public static bool IsSameOrSubclass(this Type potentialDescendant, Type potentialBase)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        /// <summary>
        ///  protected interface IFooInterface {}
        ///  protected interface IGenericFooInterface<T> {}
        ///  protected class FooBase {}
        /// 
        ///  protected class FooImplementor : FooBase, IFooInterface {}
        ///  protected class GenericFooBase : FooImplementor, IGenericFooInterface<object> {}
        ///  protected class GenericFooImplementor<T> : FooImplementor, IGenericFooInterface<T> {}
        /// 
        ///  typeof(FooImplementor).InheritsOrImplements(typeof(IFooInterface)) return true
        ///  typeof(FooImplementor).InheritsOrImplements(typeof(FooBase)) return true
        ///  typeof(GenericFooBase).InheritsOrImplements(typeof(IGenericFooInterface<>)) return true
        ///  typeof(GenericFooImplementor<>).InheritsOrImplements(typeof(FooBase)) return true
        ///  typeof(GenericFooImplementor).InheritsOrImplements(typeof(IGenericFooInterface<>)) return true
        ///  new GenericFooImplementor<string>().GetType().InheritsOrImplements(typeof(IGenericFooInterface<>)) return true
        ///  new GenericFooImplementor<string>().GetType().InheritsOrImplements(typeof(IGenericFooInterface<int>)) return false
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool InheritsOrImplements(this Type child, Type parent)
        {
            parent = ResolveGenericTypeDefinition(parent);

            var currentChild = child.IsGenericType
                                   ? child.GetGenericTypeDefinition()
                                   : child;

            while (currentChild != typeof(object))
            {
                if (parent == currentChild || HasAnyInterfaces(parent, currentChild))
                    return true;

                currentChild = currentChild.BaseType != null
                               && currentChild.BaseType.IsGenericType
                                   ? currentChild.BaseType.GetGenericTypeDefinition()
                                   : currentChild.BaseType;

                if (currentChild == null)
                    return false;
            }
            return false;
        }

        private static bool HasAnyInterfaces(Type parent, Type child)
        {
            return child.GetInterfaces()
                .Any(childInterface =>
                {
                    var currentInterface = childInterface.IsGenericType
                        ? childInterface.GetGenericTypeDefinition()
                        : childInterface;

                    return currentInterface == parent;
                });
        }

        private static Type ResolveGenericTypeDefinition(Type parent)
        {
            var shouldUseGenericType = true;
            if (parent.IsGenericType && parent.GetGenericTypeDefinition() != parent)
                shouldUseGenericType = false;

            if (parent.IsGenericType && shouldUseGenericType)
                parent = parent.GetGenericTypeDefinition();
            return parent;
        }
    }
}

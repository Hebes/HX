using System;

namespace Framework.Core
{
    /// <summary>
    /// 创建注解实例
    /// </summary>
    public class CreateCore : Attribute, IComparable<CreateCore>
    {
        /// <summary>
        /// 执行顺序
        /// </summary>
        public readonly int NumberValue;

        /// <summary>
        /// 需要实例化的类型
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// 创建注释
        /// </summary>
        /// <param name="type">需要创建的类型</param>
        /// <param name="numberValue">创建的顺序</param>
        public CreateCore(Type type, int numberValue)
        {
            Type = type;
            NumberValue = numberValue;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(CreateCore other)
        {
            return NumberValue.CompareTo(other.NumberValue);
        }
    }
}
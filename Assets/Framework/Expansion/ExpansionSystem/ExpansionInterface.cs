using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class ExpansionInterface
    {
        /// <summary>
        /// 检查接口是否存在
        /// </summary>
        public static K ChackInterFaceExist<T, K>(this T t)
        {
            if (t is K k)
                return k;
            Debug.Error($"{typeof(T).FullName}请继承{typeof(K).FullName}");
            return default;
        }
    }
}

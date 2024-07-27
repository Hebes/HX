using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/*--------脚本描述-----------

描述:
    核心初始化

-----------------------*/

namespace Framework.Core
{
    public class CoreRun
    {
        public IEnumerator FrameworkCoreRun()
        {
            var instanceList = new List<CreateCore>();
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            // 遍历所有类型
            foreach (var type in types)
            {
                if (!Attribute.IsDefined(type, typeof(CreateCore))) continue;
                var attribute = Attribute.GetCustomAttribute(type, typeof(CreateCore));
                instanceList.Add((CreateCore)attribute);
            }

            //排序
            instanceList.Sort();
            //执行
            foreach (var instanceValue in instanceList)
            {
                if (!typeof(ICore).IsAssignableFrom(instanceValue.Type))
                    throw new Exception($"{instanceValue.Type.Name}请继承ICore接口");
                var instance = Activator.CreateInstance(instanceValue.Type);
                Debug.Log($"{instanceValue.Type.Name}初始化,序列号为{instanceValue.NumberValue}");
                var type = instance.GetType();
                //普通方法
                var init = type.GetMethod("Init");
                init?.Invoke(instance, new object[] { });
                //协程方法
                var asyncInit = type.GetMethod("AsyncInit");
                yield return asyncInit?.Invoke(instance, new object[] { });
            }
        }
    }
}
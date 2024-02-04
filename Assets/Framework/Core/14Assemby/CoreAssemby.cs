//using Cysharp.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using UnityEditor;

//namespace Core
//{
//    public class CoreAssemby : ICore
//    {
//        public static CoreAssemby Instance { get; private set; }
//        private Dictionary<string, Type> _typeCaches = new Dictionary<string, Type>();

//        public IEnumerator ICoreInit()
//        {
//            Instance = this;
            
//            yield return null;
//        }

//        //controller = System.Activator.CreateInstance(type)
//        /// <summary>
//        /// 根据名称来获取类型
//        /// </summary>
//        /// <param name="classFullName">类名(含命名空间)</param>
//        /// <returns></returns>
//        public static Type GetType(string classFullName)
//        {
//            if (string.IsNullOrEmpty(classFullName)) return null;

//            if (Instance._typeCaches.ContainsKey(classFullName) && Instance._typeCaches[classFullName] != null) return Instance._typeCaches[classFullName];

//            Instance._typeCaches.Remove(classFullName);

//            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
//            foreach (var item in assemblies)
//            {
//                foreach (var type in item.GetTypes())
//                {
//                    if (!Instance._typeCaches.ContainsKey(type.FullName))
//                        Instance._typeCaches.Add(type.FullName, type);
//                }
//            }
//            if (Instance._typeCaches.ContainsKey(classFullName))
//                return Instance._typeCaches[classFullName];
//            return null;
//        }
//    }
//}

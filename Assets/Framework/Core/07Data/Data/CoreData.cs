//using Cysharp.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using System.Reflection;

///*--------脚本描述-----------

//描述:
//    数据加载管理

//-----------------------*/

//namespace Core
//{
//    public class CoreData : ICore
//    {
//        public static CoreData Instance;
//        private Dictionary<string, List<IData>> bytesDataDic;//数据

//        public IEnumerator ICoreInit()
//        {
//            Instance = this;
//            bytesDataDic = new Dictionary<string, List<IData>>();
//            Debug.Log("数据初始化完毕");
//            yield return null;
//        }

//        public static void InitData<T>(string filePath) where T : IData
//        {
//            byte[] fileData = CoreResource.LoadByteData(filePath);
//            List<IData> itemDetailsList = BinaryAnalysis.GetData<T>(fileData);
//            if (Instance.bytesDataDic.ContainsKey(typeof(T).FullName))
//                Instance.bytesDataDic[typeof(T).FullName] = itemDetailsList;
//            Instance.bytesDataDic.Add(typeof(T).FullName, itemDetailsList);
//        }

//        /// <summary>
//        /// 交换数据
//        /// </summary>
//        public static K ExchangeData<T, K>(T t) where T : IData where K : IData, new()
//        {
//            Type ttype = t.GetType();
//            FieldInfo[] fisttype = ttype.GetFields();

//            K k = new K();
//            Type ktype = k.GetType();
//            FieldInfo[] fisktype = ktype.GetFields();

//            for (int i = 0; i < fisttype.Length; i++)
//            {
//                fisktype[i].SetValue(k, fisttype[i].GetValue(t));
//            }
//            return k;
//        }

//        public static T GetData1<T>(int id) where T : class, IData
//        {
//            if (Instance.bytesDataDic.TryGetValue(typeof(T).FullName, out List<IData> datas))
//                return datas.Find(data => { return data.GetId() == id; }) as T;
//            Debug.Error($"未能找到数据请先初始化{typeof(T).FullName}");
//            return null;
//        }

//        public static IData GetData2<T>(int id) where T : class, IData
//        {
//            if (Instance.bytesDataDic.TryGetValue(typeof(T).FullName, out List<IData> datas))
//                return datas.Find(data => { return data.GetId() == id; });
//            Debug.Error($"未能找到数据请先初始化{typeof(T).FullName}");
//            return null;
//        }

//        public static List<IData> GetDataList<T>() where T : class, IData
//        {
//            if (Instance.bytesDataDic.TryGetValue(typeof(T).FullName, out List<IData> datas))
//                return datas;
//            Debug.Error($"未能找到数据请先初始化{typeof(T).FullName}");
//            return null;
//        }

//        public static List<T> GetDataListAsT<T>() where T : class, IData
//        {
//            if (Instance.bytesDataDic.TryGetValue(typeof(T).FullName, out List<IData> datas))
//                return datas as List<T>;
//            //foreach (IData item in Instance.bytesDataDic[typeof(T).FullName])
//            //    dataListTemp.Add(item as T);
//            Debug.Error($"未能找到数据请先初始化{typeof(T).FullName}");
//            return null;
//        }

//        public static void RemoveData(string dataKey)
//        {
//            if (Instance.bytesDataDic.ContainsKey(dataKey))
//                Instance.bytesDataDic.Remove(dataKey);
//        }

//        public static void RemoveData<T>()
//        {
//            if (Instance.bytesDataDic.ContainsKey(typeof(T).FullName))
//                Instance.bytesDataDic.Remove(typeof(T).FullName);
//        }
//    }
//}

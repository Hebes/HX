//using Cysharp.Threading.Tasks;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///*--------脚本描述-----------

//描述:
//    系统指令

//-----------------------*/

//namespace Core
//{
//    public class CoreSystemOrder : ICore
//    {
//        public static CoreSystemOrder Instance;
//        private bool _openSystemOrder = true;           //指令开启
//        private List<string> _command;                  //指令列表
//        private static int position = -1;               // 当前读取历史记录的位置
//        private static List<ISystemOder> consoleHistory;     // 控制台历史记录


//        public IEnumerator ICoreInit()
//        {
//            Instance = this;
//            consoleHistory = new List<ISystemOder>();
//            _command = new List<string>()
//            {
//                "help", //帮助
//                "cls",  //清除
//                "test"  //测试
//            };
//            GameObject gameObject = new GameObject("指令");
//            gameObject.AddComponent<UISystemOrder>();
//            GameObject.DontDestroyOnLoad(gameObject);
//            yield return null;
//        }

//        /// <summary>
//        /// 指令开启
//        /// </summary>
//        public static void OpenSystemOrder(bool isOpen)
//        {
//            Instance._openSystemOrder = isOpen;
//        }

//        public static bool ChackOpenSystemOrder() => Instance._openSystemOrder;

//        /// <summary>
//        /// 添加指令
//        /// </summary>
//        public static void AddOrder(ISystemOder systemOder)
//        {
//            if (consoleHistory.Contains(systemOder))
//            {
//                Debug.Error("指令已存在");
//                return;
//            }
//            consoleHistory.Add(systemOder);
//            systemOder.OrderInit();
//        }

//        /// <summary>
//        /// 指令输入
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public static string OrderInput(string input)
//        {
//            //分割字符串获取参数列表
//            List<string> args = new List<string>(input.Split(' '));
//            //指令类型
//            EOrderType orderType = EnumHelper.FromString<EOrderType>(args[0]);

//            // 控制与回调
//            string output = null;
//            ISystemOder systemOder = consoleHistory.Find((data) => { return data.OrderType == orderType; });
//            if (systemOder == null)// 错误指令
//                output = "No such command.";
//            else
//                output = systemOder.TriggerOder(args);
//            return output;
//        }



//        /// <summary>
//        /// 向控制台输入指令。
//        /// </summary>
//        /// <param name="input">指令字符串。</param>
//        /// <returns>回调信息。</returns>
//        public static string Input<T>(string input) where T : ISystemOder, new()
//        {
//            //分割字符串获取参数列表
//            List<string> args = new List<string>(input.Split(' '));
//            //创建新的指令
//            T t = new T();
//            t.OrderName = input;
//            //添加数据
//            consoleHistory.Add(t);
//            position = consoleHistory.Count;
//            // 控制与回调
//            string output = null;
//            switch (args[0])
//            {
//                // 帮助
//                case "help":
//                    output = Show();
//                    break;
//                // 清空控制台
//                case "cls":
//                    output = Clear();
//                    break;
//                // 测试
//                case "test":
//                    output = Test();
//                    break;
//                // 错误指令
//                default:
//                    output = "No such command.";
//                    break;
//            }
//            return output;
//        }

//        /// <summary>
//        /// 获取控制台上一条历史记录。
//        /// </summary>
//        /// <returns>上一条指令字段。</returns>
//        public static ISystemOder Last()
//        {
//            if (position == -1)
//                return null;
//            position -= 1;
//            if (position < 0)
//                position = 0;
//            return consoleHistory[position];
//        }

//        /// <summary>
//        /// 获取控制台下一条历史记录。
//        /// </summary>
//        /// <returns>下一条指令字段。</returns>
//        public static ISystemOder Next()
//        {
//            if (position == -1)
//                return null;
//            position += 1;
//            if (position >= consoleHistory.Count)
//                position = consoleHistory.Count - 1;
//            return consoleHistory[position];
//        }

//        /// <summary>
//        /// 显示全部控制台命令。
//        /// </summary>
//        /// <returns>回调信息。</returns>
//        private static string Show()
//        {
//            string output = null;
//            for (int i = 0; i < Instance._command.Count; i++)
//            {
//                output += Instance._command[i];
//                if (i != Instance._command.Count - 1)
//                    output += "\n";
//            }
//            return output;
//        }

//        /// <summary>
//        /// 清空控制台记录。
//        /// </summary>
//        /// <returns>回调信息。</returns>
//        private static string Clear()
//        {
//            position = -1;
//            consoleHistory.Clear();
//            return "cls";
//        }

//        /// <summary>
//        /// 测试方法。
//        /// </summary>
//        /// <returns>回调信息。</returns>
//        private static string Test()
//        {
//            GameObject gameObject = Resources.Load("Test") as GameObject;
//            if (gameObject)
//            {
//                GameObject.Instantiate(gameObject);
//                return "Object has been generated.";
//            }
//            return "There have no such object.";
//        }
//    }
//}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// <summary>
// @Author: zrh
// @Date: 2022,12,06,15:50
// @Description:
// </summary>

namespace Game.UI
{
    public static partial class Extension
    {
        /// <summary>
        /// Tpye 如果在common.dll 里是 找不到 game.dll的类型的
        /// </summary>
        /// <param name="me"></param>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public static Component AddSingleComponent(this GameObject me, System.Type componentType)
        {
            var comp = me.GetComponent(componentType);
            return comp ? comp : me.gameObject.AddComponent(componentType);
        }
        //只增加一个组件 不重复添加
        public static T AddSingleComponent<T>(this Component me) where T : Component
        {
            var comp = me.GetComponent<T>();
            return comp ? comp : me.gameObject.AddComponent<T>();
        }
        
        /// <summary>
        /// 获取子节点组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T AddChildComponent<T>(this Component me, string name) where T : Component
        {
            var child = me.transform.Find(name);
            var comp = child.AddSingleComponent<T>();
            return child ? comp : null;
        }
        public static T AddChildComponent<T>(this Component me, string name, bool isChild) where T : Component
        {
            if (isChild)
            {
                var child = me.transform.FindChildByName(name);
                var comp = child.AddSingleComponent<T>();
                return child ? comp : null;
            }
            else
            {
                return AddChildComponent<T>(me, name);
            }
        }
        
        /// <summary>
        /// 获取子节点组件  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"> 全路径 </param>
        /// <returns></returns>
        public static T GetChildComponent<T>(this Component me, string name) where T : Component
        {
            var child = me.transform.Find(name);
            return child ? child.GetComponent<T>() : null;
        }
        
        /// <summary>
        /// 获取子节点组件 (除非是list里面一个完整的cell，否则都不可以再用本宫)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetChildComponent<T>(this Component me, int index) where T : Component
        {
            var child = me.transform.GetChild(index);
            return child ? child.GetComponent<T>() : null;
        }
        
        /// <summary>
        /// 获取子节点组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"> 全路径 </param>
        /// <returns></returns>
        public static T GetChildComponent<T>(this Component me, string name, bool isChild) where T : Component
        {
            if (isChild)
            {
                var child = me.transform.FindChildByName(name);
                return child ? child.GetComponent<T>() : null; 
            }
            else
            {
                return GetChildComponent<T>(me, name);
            }
        }
        /// <summary>
        /// 递归查找 该子类成员下满足name条件的成员 避免需查找时需要大量层级地址
        /// </summary>
        /// <param name="currentTF"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform FindChildByName(this Transform currentTF, string childName)
        {
            //递归 方法内部又调用自身过程
            //1.在子物体中查找
            Transform childTF = currentTF.Find(childName);
            if (childTF != null) return childTF;
            for (int i = 0; i < currentTF.childCount; i++)
            {
                //将任务交给子物体
                childTF = FindChildByName(currentTF.GetChild(i), childName);
                if (childTF != null) return childTF;
            }
            return null;
        }
        /// <summary>
        /// 坐标标准化
        /// </summary>
        /// <param name="t"></param>
        public static void Normalize(this RectTransform t)
        {
#if UNITY_EDITOR
            if (!t)
            {
                Debug.LogError("Normalize RectTransform == null");
                return;
            }
#endif
            t.anchoredPosition = Vector2.zero;
            t.localScale = Vector3.one;
            t.localRotation = Quaternion.identity;
        }
        /// <summary>
        /// 坐标标准化
        /// </summary>
        /// <param name="t"></param>
        public static void Normalize(this Transform t)
        {
#if UNITY_EDITOR
            if (!t)
            {
                Debug.LogError("Normalize Transform == null");
                return;
            }
#endif
            t.localPosition = Vector2.zero;
            t.localScale = Vector3.one;
            t.localRotation = Quaternion.identity;
        }
        /// <summary>
        /// 重置描点
        /// </summary>
        /// <param name="t"></param>
        public static void ResetAnchor(this RectTransform t)
        {
#if UNITY_EDITOR
            if (!t)
            {
                Debug.LogError("Normalize RectTransform == null");
                return;
            }
#endif
            t.anchorMin = Vector2.zero;
            t.anchorMax = Vector2.one;
            t.offsetMin = Vector2.zero;
            t.offsetMax = Vector2.zero;
        }
        
        /// <summary>
        /// 按钮单机扩展
        /// </summary>
        /// <param name="button"></param>
        /// <param name="action"></param>
        public static void AppendClick(this Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }
        /// <summary>
        /// 按钮单机扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="button"></param>
        /// <param name="action"></param>
        public static void AppendClick<T>(this Button button, UnityAction<T> action) where T : Component
        {
            var t = button.GetComponent<T>();
            button.onClick.AddListener(() =>
            {
                action(t);
            });
        }
        public static void SetActiveEfficiently(this GameObject gameObject, bool value)
        {
            if (gameObject.activeSelf != value)
            {
                gameObject.SetActive(value);
            }
        }
        /// <summary>
        /// 获取指定节点的所有指定类型的组件列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<T> GetChildComponents<T>(this Component me, string name) where T : Component
        {
            var child = me.transform.Find(name);
            var founds = child.GetComponentsInChildren<T>();
            var list = new List<T>();
            for (var i = 0; i < founds.Length; i++)
                list.Add(founds[i]);
            return list;
        }
        public static void CutOffExtraInputText(this InputField me, string configKey, string text)
        {
            Debug.LogError("zrh 改");
            // // 最大长度限制，中文占2个字符，英文1个，采用GBK编码实现
            // var characterLimitConfig = TcommCommconf4ProtoArrayCsvHelper.GetList().Find(c => c.keyName == configKey);
            //
            // if (characterLimitConfig != null)
            // {
            //     var characterLimit = characterLimitConfig.valueStr.ToInteger();
            //     me.characterLimit = characterLimit;
            //     var gbkEncoding = Encoding.GetEncoding("GBK");
            //
            //     if (gbkEncoding != null)
            //     {
            //         // gbk编码
            //         var gbks = gbkEncoding.GetBytes(text);
            //         if (gbks.Length > characterLimit)
            //         {
            //             // gbk解码
            //             me.text = gbkEncoding.GetString(gbks, 0, characterLimit);
            //         }
            //     }
            // }
        }
        
        /// <summary>
        /// 设置中心点位置为目标中心点
        /// </summary>
        /// <param name="targetRectTransform"></param>
        /// <param name="sourceRectTransform"></param>
        /// <returns></returns>
        public static void SetCenterAtTargetCenter(this RectTransform sourceRectTransform, RectTransform targetRectTransform)
        {
            var anchoredCenterPosition = TransformRectPoint(targetRectTransform, targetRectTransform.rect.center, sourceRectTransform);

            sourceRectTransform.anchoredPosition = anchoredCenterPosition - sourceRectTransform.rect.center;
        }
        private static Vector2 TransformRectPoint(RectTransform sourceRectTransform, Vector2 sourcePoint, RectTransform targetRectTransform)
        {
            var sourceCamera = GetCamera(sourceRectTransform);
            var sourceWorldPoint = sourceRectTransform.TransformPoint(sourcePoint);
            var sourceScreenPoint = RectTransformUtility.WorldToScreenPoint(sourceCamera, sourceWorldPoint);

            var targetCamera = GetCamera(targetRectTransform);
            Vector2 targetLocalPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRectTransform, sourceScreenPoint, targetCamera, out targetLocalPoint);

            // 得到相对于anchor的点
            return targetRectTransform.anchoredPosition + targetLocalPoint;
        }
        private static Camera GetCamera(RectTransform rectTransform)
        {
            return rectTransform.GetComponentInParent<Canvas>().worldCamera ?? Camera.main;
        }
        /// <summary>
        /// 子节点销毁
        /// </summary>
        /// <param name="me"></param>
        public static void DestroyAllChildren(this Component me)
        {
            for (var i = me.transform.childCount - 1; i >= 0; --i)
                Object.Destroy(me.transform.GetChild(i).gameObject);
        }
        /// <summary>
        /// 设置模型层
        /// </summary>
        /// <param name="c"></param>
        /// <param name="layer"></param>
        public static void SetModelLayer(this Component c, int layer)
        {
            var renderers = c.GetComponentsInChildren<Renderer>();
            renderers.Foreach(rend => rend.gameObject.layer = layer);
        }
        /// <summary>
        /// 遍历数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void Foreach<T>(this T[] array, Action<T> action)
        {
            for (var i = 0; i < array.Length; ++i)
                action(array[i]);
        }
        /// <summary>
        /// 转换成万为单位的数据
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string ToTenThousandString(this long me)
        {
            return ToTenThousandWithADecimalString(me);
        }

        /// <summary>
        /// 转化成万为单位的数字,转换率起步10w,10w以下精确显示，10w以上保留一位小数显示成xxx.y万
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToTenThousandWithADecimalString(long num)
        {
            // if (num >= 100000000L)
            // {//显示xxx.y亿
            //     return Zrh_Localization.GetTextFormatEx("yi_149", (num / 100000000f).ToString("F1"));
            // }
            // else if (num >= 100000L)
            // {//显示xxx.y万
            //     return Zrh_Localization.GetTextFormatEx("wan", (num / 10000f).ToString("F1"));
            // }
            // else
            // {//显示具体数字从0到最大99999
                return num.ToString();
            //}
        }
        /// <summary>
        /// 判断一个Unity Object是否已经被销毁
        /// </summary>
        /// <returns><c>true</c> 如果对象==null，但是真实引用不是null,则说明已经被Unity引擎标记为销毁状态了 <c>false</c>.</returns>
        /// <param name="unityObject">Unity object.</param>
        public static bool IsDestroyed(this Object unityObject)
        {
            return unityObject == null && !ReferenceEquals(unityObject, null);
        }
        /// <summary>
        /// 转换浮点到百分比
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string ToPercent(this float me)
        {
            me *= 100;
            var str = me.ToString("####");
            str = str == string.Empty ? "0" : str;
            return str + '%';
        }
        /// <summary>
        /// 帧结束前，就立即标记子节点销毁
        /// </summary>
        /// <param name="me"></param>
        public static void DestroyAllChildrenImmediate(this Component me)
        {
            for (var i = me.transform.childCount - 1; i >= 0; --i)
                Object.DestroyImmediate(me.transform.GetChild(i).gameObject);
        }
        
        /// <summary>
        /// 将输入的prefab生成实例后，添加到调用对象的子节点中。
        /// 并返回一个期望的组件类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static T AddToChild<T>(this GameObject me, GameObject prefab)
        {
            var newObject = Object.Instantiate(prefab);
            newObject.transform.SetParent(me.transform);
            newObject.transform.localScale = Vector3.one;
            return newObject.GetComponent<T>();
        }
        /// <summary>
        /// 数组索引查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Exists<T>(this T[] array, Predicate<T> predicate)
        {
            return FindIndex(array, predicate) != -1;
        }
        /// <summary>
        /// 数组索引查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int FindIndex<T>(this T[] array, Predicate<T> predicate)
        {
            for (var i = 0; i < array.Length; ++i)
            {
                var item = array[i];
                if (predicate(item))
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 数组查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T Find<T>(this T[] array, Predicate<T> predicate)
        {
            for (var i = 0; i < array.Length; ++i)
            {
                var item = array[i];
                if (predicate(item))
                    return item;
            }
            return default(T);
        }
        /// <summary>
        /// 保留两位小数的百分比显示,传入的值必须是已经转化为百分比的数,万分比要再计算一次再传入
        /// </summary>
        public static string ToPercentWithDecimal(this float val)
        {
            var str = val.ToString("0.##");
            return str + '%';
        }
        /// <summary>
        /// 禁用启用父节点
        /// </summary>
        /// <param name="c"></param>
        /// <param name="arg"></param>
        public static void SetActiveParent(this Component c, bool arg)
        {
            c.transform.parent.gameObject.SetActive(arg);
        }
    }
}
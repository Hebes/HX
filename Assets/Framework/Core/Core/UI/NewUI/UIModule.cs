using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Framework.Core
{
    [CreateCore(typeof(UIModule), 7)]
    public class UIModule : ICore
    {
        public static UIModule Instance { get; private set; }
        public Camera UICamera; // UI摄像机
        public Camera MainCamera; // 主摄像机
        private Dictionary<string, WindowBase> mAllWindowDic = new(); //所有窗口的Dic
        private List<WindowBase> mAllWindowList = new(); //所有窗口的列表
        private List<WindowBase> mVisibleWindowList = new(); //所有可见窗口的列表 
        private Queue<WindowBase> mWindowStack = new(); //队列， 用来管理弹窗的循环弹出
        private bool mStartPopStackWndStatus = false; //开始弹出堆栈的标志，可以用来处理多种情况，比如：正在出栈中有其他界面弹出，可以直接放到栈内进行弹出 等
        public bool SINGMASK_SYSTEM;//是否启用单遮模式
        
        public void Init()
        {
            Instance = this;
        }

        public IEnumerator AsyncInit()
        {
            $"加载物体".Error();
            yield return CoreResource.LoadAsync<GameObject>(GameSetting.UICanvasPath, LoadOkOver);

            yield break;

            void LoadOkOver(GameObject gameObject)
            {
                var canvasGo = Object.Instantiate(gameObject);
                canvasGo.name = "UI界面";
                //实例化
                Object.DontDestroyOnLoad(canvasGo);
                //获取子节点
                UICamera = canvasGo.GetChildComponent<Camera>("UICamera"); //UI相机要添加到主相机的Stack中
                MainCamera = canvasGo.GetChildComponent<Camera>("MainCamera");
                var nodeParent = canvasGo.transform.Find("Canvas");
                //创建节点
                var uiTypeArray = Enum.GetValues(typeof(EUIType));
                foreach (var uiType in uiTypeArray)
                    CreatNode(uiType, nodeParent);
            }

            void LoadUI()
            {
            }
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="objValue"></param>
        /// <param name="parent"></param>
        private void CreatNode(object objValue, Transform parent)
        {
           var type =  objValue.GetType();
           var attribute = (UITypeAttribute)Attribute.GetCustomAttribute(type, typeof(UITypeAttribute));
            var nodeGo = new GameObject(objValue.ToString());
            nodeGo.transform.SetParent(parent, false);
            nodeGo.AddComponent<GraphicRaycaster>();
            Canvas canvasTemp =  nodeGo.AddComponent<Canvas>();
            canvasTemp.sortingOrder = attribute.OrderInLayer;
            canvasTemp.camera
            nodeGo.AddComponent<CanvasScaler>();
        }

        /// <summary>
        /// 只加载物体，不调用生命周期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void PreLoadWindow<T>(GameObject go) where T : WindowBase
        {
            //克隆界面，初始化界面信息
            var windowBase = go.AddComponent<T>();
            //1.生成对应的窗口预制体
            var nWnd = TempLoadWindow(windowBase.name);
            //2.初始出对应管理类
            if (!nWnd)
                throw new Exception($"没有{windowBase.name}窗口");
            windowBase.OnAwake();
            windowBase.SetVisible(false);
            var rectTrans = nWnd.GetComponent<RectTransform>();
            rectTrans.anchorMax = Vector2.one;
            rectTrans.offsetMin = rectTrans.offsetMax = Vector2.zero;
            mAllWindowDic.Add(windowBase.name, windowBase);
            mAllWindowList.Add(windowBase);
            Debug.Log($"预加载窗口 窗口名字：{windowBase.name}");
        }

        /// <summary>
        /// 弹出一个弹窗
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T PopUpWindow<T>() where T : WindowBase, new()
        {
            var wndName = typeof(T).Name;
            var wnd = GetWindow(wndName);
            return wnd ? ShowWindow(wndName) as T : InitializeWindow(wndName) as T;
        }

        /// <summary>
        /// 弹出界面
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        private WindowBase PopUpWindow(WindowBase window)
        {
            var wnd = GetWindow(window.name);
            return wnd ? ShowWindow(window.name) : InitializeWindow(window.name);
        }

        /// <summary>
        /// 创建窗口
        /// </summary>
        /// <param name="wndName"></param>
        /// <returns></returns>
        private WindowBase InitializeWindow(string wndName)
        {
            //1.生成对应的窗口预制体
            var nWnd = TempLoadWindow(wndName);
            //2.初始出对应管理类
            if (!nWnd)
                throw new Exception($"窗口{wndName}没有加载");
            nWnd.transform.SetAsLastSibling();
            nWnd.OnAwake();
            nWnd.SetVisible(true);
            nWnd.OnShow();
            var rectTrans = nWnd.GetComponent<RectTransform>();
            rectTrans.anchorMax = Vector2.one;
            rectTrans.offsetMin = rectTrans.offsetMax = Vector2.zero;
            mAllWindowDic.Add(nWnd.name, nWnd);
            mAllWindowList.Add(nWnd);
            mVisibleWindowList.Add(nWnd);
            SetWidnowMaskVisible();
            return nWnd;
        }

        /// <summary>
        /// 获取一个窗口
        /// </summary>
        /// <param name="winName"></param>
        /// <returns></returns>
        private WindowBase GetWindow(string winName)
        {
            return mAllWindowDic.GetValueOrDefault(winName);
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="winName"></param>
        /// <returns></returns>
        private WindowBase ShowWindow(string winName)
        {
            if (mAllWindowDic.TryGetValue(winName, out var window))
            {
                if (window.Visible) return window;
                mVisibleWindowList.Add(window);
                window.transform.SetAsLastSibling();
                window.SetVisible(true);
                SetWidnowMaskVisible();
                window.OnShow();
                return window;
            }

            Debug.LogError(winName + " 窗口不存在，请调用PopUpWindow 进行弹出");
            return null;
        }

        /// <summary>
        /// 获取已经弹出的弹窗
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetVisibleWindow<T>() where T : WindowBase
        {
            foreach (var item in mVisibleWindowList)
            {
                if (item is not T t) continue;
                return t;
            }

            throw new Exception($"该窗口没有获取到：{typeof(T).Name}");
        }


        public void HideWindow(string wndName)
        {
            var window = GetWindow(wndName);
            HideWindow(window);
        }

        private void HideWindow(WindowBase window)
        {
            if (window.Visible)
            {
                mVisibleWindowList.Remove(window);
                window.SetVisible(false); //隐藏弹窗物体
                SetWidnowMaskVisible();
                window.OnHide();
            }

            //在出栈的情况下，上一个界面隐藏时，自动打开栈中的下一个界面
            PopNextStackWindow(window);
        }


        public void DestroyWinodw<T>() where T : WindowBase
        {
            DestroyWindow(typeof(T).Name);
        }

        private void DestroyWindow(string wndName)
        {
            var window = GetWindow(wndName);
            DestoryWindow(window);
        }

        private void DestoryWindow(WindowBase window)
        {
            if (window == null) return;
            if (mAllWindowDic.ContainsKey(window.name))
            {
                mAllWindowDic.Remove(window.name);
                mAllWindowList.Remove(window);
                mVisibleWindowList.Remove(window);
            }

            window.SetVisible(false);
            SetWidnowMaskVisible();
            window.OnHide();
            window.OnDes();
            Object.Destroy(window.gameObject);
            //在出栈的情况下，上一个界面销毁时，自动打开栈种的下一个界面
            PopNextStackWindow(window);
        }

        /// <summary>
        /// 删除所有的窗口
        /// </summary>
        /// <param name="filterlist"></param>
        public void DestroyAllWindow(List<string> filterlist = null)
        {
            for (var i = mAllWindowList.Count - 1; i >= 0; i--)
            {
                var window = mAllWindowList[i];
                if (window == null || (filterlist != null && filterlist.Contains(window.name)))
                    continue;
                DestroyWindow(window.name);
            }

            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 设置遮罩
        /// </summary>
        private void SetWidnowMaskVisible()
        {
            if (!SINGMASK_SYSTEM)return;
            WindowBase maxOrderWndBase = null; //最大渲染层级的窗口
            int maxOrder = 0; //最大渲染层级
            int maxIndex = 0; //最大排序下标 在相同父节点下的位置下标
            //1.关闭所有窗口的Mask 设置为不可见
            //2.从所有可见窗口中找到一个层级最大的窗口，把Mask设置为可见
            for (var i = 0; i < mVisibleWindowList.Count; i++)
            {
                WindowBase window = mVisibleWindowList[i];
                if (window != null && window.gameObject != null)
                {
                    window.SetMaskVisible(false);
                    if (maxOrderWndBase == null)
                    {
                        maxOrderWndBase = window;
                        maxOrder = window.Canvas.sortingOrder;
                        maxIndex = window.transform.GetSiblingIndex();
                    }
                    else
                    {
                        //找到最大渲染层级的窗口，拿到它
                        if (maxOrder < window.Canvas.sortingOrder)
                        {
                            maxOrderWndBase = window;
                            maxOrder = window.Canvas.sortingOrder;
                        }
                        //如果两个窗口的渲染层级相同，就找到同节点下最靠下一个物体，优先渲染Mask
                        else if (maxOrder == window.Canvas.sortingOrder && maxIndex < window.transform.GetSiblingIndex())
                        {
                            maxOrderWndBase = window;
                            maxIndex = window.transform.GetSiblingIndex();
                        }
                    }
                }
            }

            if (maxOrderWndBase != null)
            {
                maxOrderWndBase.SetMaskVisible(true);
            }
        }


        private WindowBase TempLoadWindow(string wndName)
        {
            var window = mAllWindowDic[wndName];
            window.transform.localScale = Vector3.one;
            window.transform.localPosition = Vector3.zero;
            window.transform.rotation = Quaternion.identity;
            return window;
        }


        #region 堆栈系统

        /// <summary>
        /// 进栈一个界面
        /// </summary>
        /// <param name="windowBase"></param>
        /// <param name="popCallBack"></param>
        public void PushWindowToStack(WindowBase windowBase, Action<WindowBase> popCallBack = null)
        {
            windowBase.PopStackListener = popCallBack;
            mWindowStack.Enqueue(windowBase);
        }

        /// <summary>
        /// 弹出堆栈中第一个弹窗
        /// </summary>
        public void StartPopFirstStackWindow()
        {
            if (mStartPopStackWndStatus) return;
            mStartPopStackWndStatus = true; //已经开始进行堆栈弹出的流程，
            PopStackWindow();
        }

        /// <summary>
        /// 压入并且弹出堆栈弹窗
        /// </summary>
        /// <param name="windowBase"></param>
        /// <param name="popCallBack"></param>
        public void PushAndPopStackWindow(WindowBase windowBase, Action<WindowBase> popCallBack = null)
        {
            PushWindowToStack(windowBase, popCallBack);
            StartPopFirstStackWindow();
        }

        /// <summary>
        /// 弹出堆栈中的下一个窗口
        /// </summary>
        /// <param name="windowBase"></param>
        private void PopNextStackWindow(WindowBase windowBase)
        {
            if (!mStartPopStackWndStatus) return;
            if (!windowBase.PopStack) return;
            windowBase.PopStack = false;
            PopStackWindow();
        }

        /// <summary>
        /// 弹出堆栈弹窗
        /// </summary>
        /// <returns></returns>
        public bool PopStackWindow()
        {
            if (mWindowStack.Count > 0)
            {
                var window = mWindowStack.Dequeue();
                var popWindow = PopUpWindow(window);
                popWindow.PopStackListener = window.PopStackListener;
                popWindow.PopStack = true;
                popWindow.PopStackListener?.Invoke(popWindow);
                popWindow.PopStackListener = null;
                return true;
            }

            mStartPopStackWndStatus = false;
            return false;
        }

        /// <summary>
        /// 清空堆栈
        /// </summary>
        public void ClearStackWindows()
        {
            mWindowStack.Clear();
        }

        #endregion
    }
}
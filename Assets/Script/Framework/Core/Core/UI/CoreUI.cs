using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Core
{
    [CreateCore(typeof(CoreUI), 6)]
    public class CoreUI : ICore
    {
        public static CoreUI Instance;

        public void Init()
        {
            Instance = this;
            GameObject gameObjectMemory = GameSetting.UICanvasPath.Load<GameObject>();
            GameObject gameObjectTemp = Object.Instantiate(gameObjectMemory);
            gameObjectTemp.name = gameObjectMemory.name;
            UIComponent uiComponent = gameObjectTemp.GetComponent<UIComponent>();
            _uiCamera = uiComponent.GetComponent<Camera>("T_UICamera");
            _uiRoot = _uiCamera.transform;
            Object.DontDestroyOnLoad(gameObjectTemp);
        }

        public IEnumerator AsyncEnter()
        {
            yield break;
        }

        public IEnumerator Exit()
        {
            yield break;
        }

        /// <summary>
        /// 是否启用单遮模式 "窗口遮罩模式",
        /// "True：开启单遮罩模式(多个窗口叠加只有一个Mask遮罩，透明度唯一)
        /// False:开启叠着模式(一个窗口一个单独的Mask遮罩，透明度叠加)"
        /// </summary>
        public bool SINGMASK_SYSTEM = true;

        /// <summary>
        /// UI摄像机
        /// </summary>
        private Camera _uiCamera;

        /// <summary>
        /// UI挂载节点
        /// </summary>
        private Transform _uiRoot;

        /// <summary>
        /// 所有窗口的Dic
        /// </summary>
        private readonly Dictionary<string, WindowBase> _allWindowDic = new Dictionary<string, WindowBase>();

        /// <summary>
        /// 所有窗口的列表
        /// </summary>
        private readonly List<WindowBase> _allWindowList = new List<WindowBase>(); //

        /// <summary>
        /// 所有可见窗口的列表 
        /// </summary>
        private readonly List<WindowBase> _visibleWindowList = new List<WindowBase>();

        private readonly Queue<WindowBase> _windowStack = new Queue<WindowBase>(); // 队列， 用来管理弹窗的循环弹出
        private bool _startPopStackWndStatus = false; //开始弹出堆栈的表只，可以用来处理多种情况，比如：正在出栈种有其他界面弹出，可以直接放到栈内进行弹出 等

        #region 智能显隐

        private bool _smartShowHide = true; //智能显隐开关（可根据情况选择开启或关闭）
        //智能显隐：主要用来优化窗口叠加时被遮挡的窗口参与渲染计算，导致帧率降低的问题。
        //显隐规则：由程序设定某个窗口是否为全屏窗口。(全屏窗口设定方式：在窗口的OnAwake接口中设定该窗口是否为全屏窗口如 FullScreenWindow=true)
        //1.智能隐藏:当FullScreenWindow=true的全屏窗口打开时，框架会自动通过伪隐藏的方式隐藏所有被当前全屏窗口遮挡住的窗口，避免这些看不到的窗口参与渲染运算，
        //从而提高性能。
        //2.智能显示：当FullScreenWindow=true的全屏窗口关闭时，框架会自动找到上一个伪隐藏的窗口把其设置为可见状态，若上一个窗口为非全屏窗口，框架则会找上上个窗口进行显示，
        //以此类推进行循环，直到找到全屏窗口则停止智能显示流程。
        //注意：通过智能显隐进行伪隐藏的窗口在逻辑上仍属于显示中的窗口，可以通过GetWindow获取到该窗口。但是在表现上该窗口为不可见窗口，故称之为伪隐藏。
        //智能显隐逻辑与（打开当前窗口时隐藏其他所有窗口相似）但本质上有非常大的区别，
        //1.通过智能显隐设置为不可见的窗口属于伪隐藏窗口，在逻辑上属于显示中的窗口。
        //2.通过智能显隐设置为不可见的窗口可以通过关闭当前窗口，自动恢复当前窗口之前的窗口的显示。
        //3.通过智能显隐设置为不可见的窗口不会触发UGUI重绘、不会参与渲染计算、不会影响帧率。
        //4.程序只需要通过FullScreenWindow=true配置那些窗口为全屏窗口即可，智能显隐的所有逻辑均有框架自动维护处理。

        #endregion

        #region 窗口管理

        /// <summary>
        /// 只加载物体，不调用生命周期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void PreLoadWindow<T>() where T : WindowBase
        {
            string wndName = typeof(T).Name;
            //生成对应的窗口预制体
            GameObject nWnd = LoadWindow(wndName);
            T windowBase = nWnd.GetComponent<T>();
            //2.初始出对应管理类
            if (!windowBase)
            {
                windowBase = nWnd.AddComponent<T>();
                windowBase.gameObject = nWnd;
                windowBase.transform = nWnd.transform;
                windowBase.Canvas = nWnd.GetComponent<Canvas>();
                windowBase.Canvas.worldCamera = _uiCamera;
                windowBase.Name = nWnd.name;
                windowBase.OnAwake();
                windowBase.SetVisible(false);
                RectTransform rectTrans = nWnd.GetComponent<RectTransform>();
                rectTrans.anchorMax = Vector2.one;
                rectTrans.offsetMax = Vector2.zero;
                rectTrans.offsetMin = Vector2.zero;
                _allWindowDic.Add(wndName, windowBase);
                _allWindowList.Add(windowBase);
            }

            Debug.Log("预加载窗口 窗口名字：" + wndName);
        }

        /// <summary>
        /// 弹出一个弹窗
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T PopUpWindow<T>() where T : WindowBase => PopUpWindow<T>(typeof(T).Name);

        /// <summary>
        /// 弹出一个窗口
        /// </summary>
        /// <param name="wndName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T PopUpWindow<T>(string wndName) where T : WindowBase
        {
            var wnd = GetWindow(wndName);
            if (!wnd)
                ($"请先PreLoadWindow窗口{wndName}").Error();
            return wnd ? ShowWindow(wndName) as T : InitializeWindow(wnd, wndName) as T;
        }

        public void PopUpWindow(string wndName)
        {
            var wnd = GetWindow(wndName);
            if (wnd)
            {
                ShowWindow(wndName);
                return;
            }

            ($"请先PreLoadWindow窗口{wndName}").Error();
            InitializeWindow(wnd, wndName);
        }

        /// <summary>
        /// 获取已经弹出的弹窗
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetWindow<T>() where T : WindowBase
        {
            var wndName = typeof(T).Name;
            foreach (var item in _visibleWindowList)
            {
                if (wndName.Equals(item.Name))
                    return (T)item;
            }

            Debug.LogError("该窗口没有获取到：" + wndName);
            return null;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void HideWindow(string wndName)
        {
            WindowBase window = GetWindow(wndName);
            if (window != null && window.Visible)
            {
                _visibleWindowList.Remove(window);
                window.SetVisible(false); //隐藏弹窗物体
                SetWidnowMaskVisible();
                HideWindowAndModifyAllWindowCanvasGroup(window, 1);
                window.OnHide();
            }

            //在出栈的情况下，上一个界面隐藏时，自动打开栈种的下一个界面
            PopNextStackWindow(window);
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="winName"></param>
        /// <returns></returns>
        private WindowBase ShowWindow(string winName)
        {
            if (_allWindowDic.TryGetValue(winName, out var window))
            {
                if (window.gameObject != null && window.Visible == false)
                {
                    _visibleWindowList.Add(window);
                    window.transform.SetAsLastSibling();
                    window.SetVisible(true);
                    SetWidnowMaskVisible();
                    ShowWindowAndModifyAllWindowCanvasGroup(window, 0);
                    window.OnShow();
                }
                else if (window.gameObject != null && window.Visible) //窗口若已经弹出，调用Onshow生命周期接口刷新界面数据
                {
                    window.OnShow();
                }

                return window;
            }

            Debug.LogError(winName + " 窗口不存在，请调用PopUpWindow 进行弹出");

            return null;
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="winName"></param>
        /// <returns></returns>
        private WindowBase GetWindow(string winName)
        {
            return _allWindowDic.GetValueOrDefault(winName);
        }

        /// <summary>
        /// 销毁窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void DestroyWinodw<T>() where T : WindowBase
        {
            WindowBase window = GetWindow(typeof(T).Name);
            if (!window) return;
            if (_allWindowDic.ContainsKey(window.Name))
            {
                _allWindowDic.Remove(window.Name);
                _allWindowList.Remove(window);
                _visibleWindowList.Remove(window);
            }

            window.SetVisible(false);
            SetWidnowMaskVisible();
            window.OnHide();
            window.OnDestroy();
            //ZMAsset.Release(window.gameObject, true);
            //在出栈的情况下，上一个界面销毁时，自动打开栈种的下一个界面
            PopNextStackWindow(window);
            window = null;

            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 设置窗口遮罩开启
        /// </summary>
        private void SetWidnowMaskVisible()
        {
            if (!CoreUI.Instance.SINGMASK_SYSTEM) return;

            WindowBase maxOrderWndBase = null; //最大渲染层级的窗口
            int maxOrder = 0; //最大渲染层级
            int maxIndex = 0; //最大排序下标 在相同父节点下的位置下标
            //1.关闭所有窗口的Mask 设置为不可见
            //2.从所有可见窗口中找到一个层级最大的窗口，把Mask设置为可见
            for (int i = 0; i < _visibleWindowList.Count; i++)
            {
                WindowBase window = _visibleWindowList[i];
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
                        else if (maxOrder == window.Canvas.sortingOrder &&
                                 maxIndex < window.transform.GetSiblingIndex())
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

        /// <summary>
        /// 加载窗口
        /// </summary>
        /// <param name="wndNamePath"></param>
        /// <returns></returns>
        private GameObject LoadWindow(string wndNamePath)
        {
            string path = $"AssetsPackage/Prefab/UI/{wndNamePath}";
            GameObject windowMemory = Resources.Load<GameObject>(path);
            GameObject window = Object.Instantiate<GameObject>(windowMemory, _uiRoot);
            window.transform.SetParent(_uiRoot);
            window.transform.localScale = Vector3.one;
            window.transform.localPosition = Vector3.zero;
            window.transform.rotation = Quaternion.identity;
            window.name = wndNamePath;
            return window;
        }

        /// <summary>
        /// 初始化窗口
        /// </summary>
        /// <param name="windowBase"></param>
        /// <param name="wndName"></param>
        /// <returns></returns>
        private WindowBase InitializeWindow(WindowBase windowBase, string wndName)
        {
            //1.生成对应的窗口预制体
            GameObject nWnd = LoadWindow(wndName);
            //2.初始出对应管理类
            if (nWnd != null)
            {
                windowBase.gameObject = nWnd;
                windowBase.transform = nWnd.transform;
                windowBase.Canvas = nWnd.GetComponent<Canvas>();
                windowBase.Canvas.worldCamera = _uiCamera;
                windowBase.transform.SetAsLastSibling();
                windowBase.Name = nWnd.name;
                windowBase.OnAwake();
                windowBase.SetVisible(true);
                windowBase.OnShow();
                RectTransform rectTrans = nWnd.GetComponent<RectTransform>();
                rectTrans.anchorMax = Vector2.one;
                rectTrans.offsetMax = Vector2.zero;
                rectTrans.offsetMin = Vector2.zero;
                //增强代码鲁棒性 增加处理异常的健壮性
                if (_allWindowDic.ContainsKey(wndName))
                {
                    if (_allWindowDic[wndName] != null && _allWindowDic[wndName].gameObject != null)
                    {
                        //ZMAsset.Release(mAllWindowDic[wndName].gameObject, true);释放 回收到对象池 啥的
                        _allWindowDic.Remove(wndName);
                    }
                    else
                        _allWindowDic.Remove(wndName);

                    if (_allWindowList.Contains(windowBase))
                        _allWindowList.Remove(windowBase);
                    if (_visibleWindowList.Contains(windowBase))
                        _visibleWindowList.Remove(windowBase);
                    Debug.LogError("mAllWindow Dic Alread Contains key:" + wndName);
                }

                _allWindowDic.Add(wndName, windowBase);
                _allWindowList.Add(windowBase);
                _visibleWindowList.Add(windowBase);
                SetWidnowMaskVisible();
                ShowWindowAndModifyAllWindowCanvasGroup(windowBase, 0);
                return windowBase;
            }

            Debug.LogError("没有加载到对应的窗口 窗口名字：" + wndName);
            return null;
        }

        #endregion

        #region 智能显隐

        private void ShowWindowAndModifyAllWindowCanvasGroup(WindowBase window, int value)
        {
            if (!_smartShowHide)
            {
                return;
            }

            //if (WorldManager.IsHallWorld && window.FullScreenWindow) 可以以此种方式决定智能显隐开启场景
            if (window.FullScreenWindow)
            {
                try
                {
                    //当显示的弹窗是大厅是，不对其他弹窗进行伪隐藏，
                    if ("HallWindow".Equals(window.Name)) return;
                    if (_visibleWindowList.Count > 1)
                    {
                        //处理先弹弹窗 后关弹窗的情况
                        WindowBase curShowBase = _visibleWindowList[_visibleWindowList.Count - 2];
                        if (!curShowBase.FullScreenWindow &&
                            window.Canvas.sortingOrder < curShowBase.Canvas.sortingOrder)
                        {
                            return;
                        }
                    }

                    for (int i = _visibleWindowList.Count - 1; i >= 0; i--)
                    {
                        WindowBase item = _visibleWindowList[i];
                        if (item.Name != window.Name)
                        {
                            item.PseudoHidden(value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error:" + ex);
                }
            }
        }

        private void HideWindowAndModifyAllWindowCanvasGroup(WindowBase window, int value)
        {
            if (!_smartShowHide) return;
            //if (WorldManager.IsHallWorld && window.FullScreenWindow) 可以以此种方式决定智能显隐开启场景
            if (window.FullScreenWindow)
            {
                for (int i = _visibleWindowList.Count - 1; i >= 0; i--)
                {
                    if (_visibleWindowList[i] != window)
                    {
                        _visibleWindowList[i].PseudoHidden(1);
                        //找到上一个窗口，如果是全屏窗口，将其设置可见，终止循转。否则循环至最终
                        if (_visibleWindowList[i].FullScreenWindow)
                            break;
                    }
                }
            }
        }

        #endregion

        #region 堆栈系统

        /// <summary>
        /// 进栈一个界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="popCallBack"></param>
        public void PushWindowToStack<T>(Action<WindowBase> popCallBack = null) where T : WindowBase, new()
        {
            T wndBase = new T();
            wndBase.PopStackListener = popCallBack;
            _windowStack.Enqueue(wndBase);
        }

        /// <summary>
        /// 弹出堆栈中第一个弹窗
        /// </summary>
        public void StartPopFirstStackWindow()
        {
            if (_startPopStackWndStatus) return;
            _startPopStackWndStatus = true; //已经开始进行堆栈弹出的流程，
            PopStackWindow();
        }

        /// <summary>
        /// 压入并且弹出堆栈弹窗，若已弹出则只压入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="popCallBack"></param>
        public void PushAndPopStackWindow<T>(Action<WindowBase> popCallBack = null) where T : WindowBase, new()
        {
            PushWindowToStack<T>(popCallBack);
            StartPopFirstStackWindow();
        }

        /// <summary>
        /// 弹出堆栈中的下一个窗口
        /// </summary>
        /// <param name="windowBase"></param>
        private void PopNextStackWindow(WindowBase windowBase)
        {
            if (windowBase == null) return;
            if (!_startPopStackWndStatus) return;
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
            if (_windowStack.Count > 0)
            {
                WindowBase window = _windowStack.Dequeue();
                WindowBase popWindow = PopUpWindow<WindowBase>();
                popWindow.PopStackListener = window.PopStackListener;
                popWindow.PopStack = true;
                popWindow.PopStackListener?.Invoke(popWindow);
                popWindow.PopStackListener = null;
                return true;
            }

            _startPopStackWndStatus = false;
            return false;
        }

        /// <summary>
        /// 清空栈数据
        /// </summary>
        public void ClearStackWindows()
        {
            _windowStack.Clear();
        }

        #endregion
    }

    /// <summary>
    /// UI拓展
    /// </summary>
    public static class UIExpand
    {
        public static T PopUpWindow<T>(this string wndName) where T : WindowBase =>
            CoreUI.Instance.PopUpWindow<T>(wndName);

        public static void PopUpWindow(this string wndName) =>
            CoreUI.Instance.PopUpWindow(wndName);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

描述:
    UI管理类

-----------------------*/

namespace Core
{
    /// <summary>
    /// https://blog.csdn.net/jiafuyong/article/details/131519393
    /// </summary>
    public class CoreUI : ICore, IUpdata
    {
        public static CoreUI Instance;

        /// <summary>
        /// UI根节点
        /// </summary>
        public Transform CanvasTransfrom = null;
        /// <summary>
        /// UI摄像机
        /// </summary>
        public Camera UICamera = null;
        /// <summary>
        /// 主摄像机
        /// </summary>
        public Camera MainCamera = null;
        /// <summary>
        /// 缓存所有UI窗体
        /// </summary>
        private Dictionary<string, IUI> _DicALLUIForms;
        /// <summary>
        /// 当前显示的UI窗体
        /// </summary>
        private Dictionary<string, IUI> _DicCurrentShowUIForms;
        /// <summary>
        /// 定义“栈”集合,存储显示当前所有[反向切换]的窗体类型
        /// </summary>
        private Stack<IUI> _StaCurrentUIForms;

        /// <summary>
        /// 全屏幕显示的节点
        /// </summary>
        private Transform Normal = null;
        /// <summary>
        /// 固定显示的节点
        /// </summary>
        private Transform Fixed = null;
        /// <summary>
        /// 弹出节点
        /// </summary>
        public Transform PopUp = null;
        /// <summary>
        /// 独立的窗口可移动的
        /// </summary>
        private Transform Mobile = null;
        /// <summary>
        /// 渐变过度窗体
        /// </summary>
        private Transform Fade = null;


        public void CoreBehaviourUpdata()
        {
            //当前显示的窗口
            foreach (IUI item in _DicCurrentShowUIForms.Values)
            {
                if (item is IUIUpdata updata)
                    updata.UIUpdata();
            }

            //反向切换的窗口
            if (_StaCurrentUIForms.Count >= 1)
            {
                if (_StaCurrentUIForms.Peek() is IUIUpdata uiUpdata)
                    uiUpdata.UIUpdata();
            }
        }
        public void Init()
        {
            Instance = this;
            _DicALLUIForms = new Dictionary<string, IUI>();
            _DicCurrentShowUIForms = new Dictionary<string, IUI>();
            _StaCurrentUIForms = new Stack<IUI>();
        }
        public IEnumerator AsyncInit()
        {
            yield return CoreResource.LoadAsync<GameObject>(SettingCore.uiCanvasPath, LoadOkOver);

            void LoadOkOver(GameObject gameObject)
            {
                GameObject CanvasGoInstantiate = GameObject.Instantiate(gameObject);
                CanvasGoInstantiate.name = "UI界面";
                //实例化
                CanvasTransfrom = CanvasGoInstantiate.transform;
                GameObject.DontDestroyOnLoad(CanvasTransfrom);
                //获取子节点
                Normal = CanvasTransfrom.GetChildComponent<Transform>(EUIType.Normal.ToString());
                Fixed = CanvasTransfrom.GetChildComponent<Transform>(EUIType.Fixed.ToString());
                PopUp = CanvasTransfrom.GetChildComponent<Transform>(EUIType.PopUp.ToString());
                Mobile = CanvasTransfrom.GetChildComponent<Transform>(EUIType.Mobile.ToString());
                Fade = CanvasTransfrom.GetChildComponent<Transform>(EUIType.Fade.ToString());
                UICamera = CanvasTransfrom.GetChildComponent<Camera>("UICamera");//UI相机要添加到主相机的Stack中
                MainCamera = CanvasTransfrom.GetChildComponent<Camera>("MainCamera");
                UnityEngine.Debug.Log("UI管理初始化完毕");
            }
            yield break;
        }


        public static T ShwoUIPanel<T>(string uiFormName) where T : Component, IUI
        {
            Instance._DicALLUIForms.TryGetValue(uiFormName, out IUI ui);
            T t = ui as T ?? Instance.LoadUIPanel<T>(uiFormName);

            //是否清空“栈集合”中得数据
            //if (t.IsClearStack)
            //    Instance.ClearStackArray();

            //根据不同的UI窗体的显示模式，分别作不同的加载处理
            switch (t.UIMode)
            {
                case EUIMode.Normal: Instance.LoadUIToCurrentCache(uiFormName); break; //“普通显示”窗口模式
                case EUIMode.ReverseChange: Instance.PushUIFormToStack(uiFormName); break; //需要“反向切换”窗口模式
                case EUIMode.HideOther: Instance.EnterUIFormsAndHideOther(uiFormName); break;//“隐藏其他”窗口模式
            }
            return t;
        }
        public static T GetUIPanl<T>(string uiFormName) where T : Component, IUI
        {
            Instance._DicALLUIForms.TryGetValue(uiFormName, out IUI ui);
            return ui as T;
        }
        public static void CloseUIForms(string uiFormName)
        {
            //“所有UI窗体”集合中，如果没有记录，则直接返回
            if (!Instance._DicALLUIForms.TryGetValue(uiFormName, out IUI baseUiForm)) return;

            //DLog.Log($"当前的UI窗体的显示类型是:{baseUiForm.mode}");
            //DLog.Log(LogCoLor.Blue, $"当前的UI窗体的显示类型是:{baseUiForm.mode}");
            //根据窗体不同的显示类型，分别作不同的关闭处理
            //CoreBehaviour.RemoveMonoEvent(EMonoType.Updata, baseUiForm.UIUpdate);
            switch (baseUiForm.UIMode)
            {
                case EUIMode.Normal: Instance.ExitUIForms(uiFormName); break;//普通窗体的关闭
                case EUIMode.ReverseChange: Instance.PopUIFroms(); break;//反向切换窗体的关闭
                case EUIMode.HideOther: Instance.ExitUIFormsAndDisplayOther(uiFormName); break;//隐藏其他窗体关闭
            }
        }
        public static void RemoveUIFroms(string uiFormName)
        {
            //“所有UI窗体”集合中，如果没有记录，则直接返回
            if (!Instance._DicALLUIForms.TryGetValue(uiFormName, out IUI ui)) return;
            if (ui is IUIOnDestroy uiOnDestroy)
                uiOnDestroy.UIOnDestroy();
        }

        /// <summary>
        /// 加载UI面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uiFormName"></param>
        /// <returns></returns>
        private T LoadUIPanel<T>(string uiFormName) where T : Component, IUI
        {
            GameObject uiPanel = CoreResource.Load<GameObject>(uiFormName);
            GameObject uiPanelInstantiate = Object.Instantiate(uiPanel);
            T t = uiPanelInstantiate.GetComponent<T>() ?? uiPanelInstantiate.AddComponent<T>();

            if (t is IUI ui)
            {
                ui.GameObject = t.gameObject;
                ui.UIName = uiFormName;
            }

            //调用初始化接口
            if (t is IUIAwake uIAwake)
                uIAwake.UIAwake();

            //设置父物体
            if (t is IUI ui1)
            {
                switch (ui1.UIType)
                {
                    case EUIType.Normal: uiPanelInstantiate.transform.SetParent(Normal, false); break;//普通窗体节点
                    case EUIType.Fixed: uiPanelInstantiate.transform.SetParent(Fixed, false); break;//固定窗体节点
                    case EUIType.Mobile: uiPanelInstantiate.transform.SetParent(Mobile, false); break;//独立的窗口可移动的
                    case EUIType.PopUp: uiPanelInstantiate.transform.SetParent(PopUp, false); break;//弹出窗体节点
                    case EUIType.Fade: uiPanelInstantiate.transform.SetParent(Fade, false); break;//渐变过度窗体
                }
            }

            //把克隆体，加入到“所有UI窗体”（缓存）集合中。
            _DicALLUIForms.Add(uiFormName, t);
            return t;
        }

        /// <summary>
        /// 把当前UI加载到“当前UI”集合中
        /// </summary>
        /// <param name="uiFormName">窗体预设的名称</param>
	    private void LoadUIToCurrentCache(string uiFormName)
        {
            //如果“正在显示”的集合中，存在整个UI窗体，则直接返回
            if (_DicCurrentShowUIForms.ContainsKey(uiFormName))
                return;
            //把当前窗体，加载到“正在显示”集合中
            if (_DicALLUIForms.TryGetValue(uiFormName, out IUI ui))
            {
                _DicCurrentShowUIForms.Add(uiFormName, ui);
                ui.GameObject.SetActive(true);
                if (ui is IUIOnEnable uiOnEnable)
                {
                    //显示当前窗体的UIOnEnable函数
                    uiOnEnable.UIOnEnable();
                    //设置模态窗体调用(必须是弹出窗体)
                    if (ui.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.SetMaskWindow(ui.GameObject, ui.UILucenyType);
                }
            }
        }

        /// <summary>
        /// UI窗体入栈
        /// </summary>
        /// <param name="uiFormName">窗体的名称</param>
        private void PushUIFormToStack(string uiFormName)
        {
            if (_StaCurrentUIForms.Count > 0)//判断“栈”集合中，是否有其他的窗体，有则“冻结”处理。
            {
                //_StaCurrentUIForms.Peek().Freeze();//返回位于 Stack 顶部的对象但不将其移除。栈顶元素作冻结处理 冻结状态（即：窗体显示在其他窗体下面）
                _StaCurrentUIForms.Peek().GameObject.SetActive(true);
            }

            if (_DicALLUIForms.TryGetValue(uiFormName, out IUI ui))//判断“UI所有窗体”集合是否有指定的UI窗体，有则处理。
            {
                ui.GameObject.SetActive(true);
                if (ui is IUIOnEnable uiOnEnable)
                {
                    //显示当前窗体的UIOnEnable函数
                    uiOnEnable.UIOnEnable();
                    //设置模态窗体调用(必须是弹出窗体)
                    if (ui.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.SetMaskWindow(ui.GameObject, ui.UILucenyType);
                }
                _StaCurrentUIForms.Push(ui);//把指定的UI窗体，入栈操作。
                return;
            }
            ExtensionDebug.Error($"{uiFormName} 是空的！");
        }

        /// <summary>
        /// 打开UI，且隐藏其他UI
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void EnterUIFormsAndHideOther(string strUIName)
        {
            if (string.IsNullOrEmpty(strUIName)) return;
            if (_DicCurrentShowUIForms.ContainsKey(strUIName)) return;

            //把“正在显示集合”与“栈集合”中所有窗体都隐藏。
            foreach (IUI baseUI in _DicCurrentShowUIForms.Values)
            {
                baseUI.GameObject.SetActive(false);
                if (baseUI is IUIOnDisable uiOnDisable)
                {
                    uiOnDisable.UIOnDisable();
                    if (baseUI.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.CancelMaskWindow();
                }
            }
            foreach (IUI staUI in _StaCurrentUIForms)
            {
                staUI.GameObject.SetActive(false);
                if (staUI is IUIOnDisable uiOnDisable)
                {
                    uiOnDisable.UIOnDisable();
                    if (staUI.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.CancelMaskWindow();
                }
            }

            //把当前窗体加入到“正在显示窗体”集合中，且做显示处理。
            if (_DicALLUIForms.TryGetValue(strUIName, out IUI ui))
            {
                _DicCurrentShowUIForms.Add(strUIName, ui);
                ui.GameObject.SetActive(true);
                if (ui is IUIOnEnable uiOnEnable)
                {
                    uiOnEnable.UIOnEnable();
                    if (ui.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.SetMaskWindow(ui.GameObject, ui.UILucenyType);
                }
            }
        }



        //界面关闭
        /// <summary>
        /// 退出指定UI窗体
        /// </summary>
        /// <param name="strUIFormName"></param>
        private void ExitUIForms(string strUIFormName)
        {
            //"正在显示集合"中如果没有记录，则直接返回。
            if (!_DicCurrentShowUIForms.TryGetValue(strUIFormName, out IUI ui)) return;
            //指定窗体，标记为“隐藏状态”，且从"正在显示集合"中移除。
            ui.GameObject.SetActive(false);
            if (ui is IUIOnDisable uiOnDisable)
            {
                uiOnDisable.UIOnDisable();
                //取消模态窗体调用
                if (ui.UIType == EUIType.PopUp)
                    UIMaskMgr.Instance.CancelMaskWindow();
                _DicCurrentShowUIForms.Remove(strUIFormName);
            }
        }

        /// <summary>
        /// （“反向切换”属性）窗体的出栈逻辑
        /// </summary>
        private void PopUIFroms()
        {
            if (_StaCurrentUIForms.Count >= 2)
            {
                IUI topUIForms = _StaCurrentUIForms.Pop();//出栈处理
                topUIForms.GameObject.SetActive(false);
                if (topUIForms is IUIOnDisable uiOnDisabl)//做隐藏处理
                {
                    uiOnDisabl.UIOnDisable();
                    if (topUIForms.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.CancelMaskWindow();
                }
                IUI nextUIForms = _StaCurrentUIForms.Peek();//出栈后，下一个窗体做“重新显示”处理。
                nextUIForms.GameObject.SetActive(true);
                if (nextUIForms is IUIOnEnable uiOnEnable)
                {
                    uiOnEnable.UIOnEnable();
                    if (nextUIForms.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.SetMaskWindow(nextUIForms.GameObject, nextUIForms.UILucenyType);
                }
            }
            else if (_StaCurrentUIForms.Count == 1)
            {
                IUI topUIForms = _StaCurrentUIForms.Pop();//出栈处理
                topUIForms.GameObject.SetActive(false);
                if (topUIForms is IUIOnDisable uiOnDisabl)//做隐藏处理
                {
                    uiOnDisabl.UIOnDisable();
                    if (topUIForms.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.CancelMaskWindow();
                }
            }
        }

        /// <summary>
        /// (“隐藏其他”属性)关闭窗体，且显示其他窗体
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void ExitUIFormsAndDisplayOther(string strUIName)
        {
            if (!_DicCurrentShowUIForms.TryGetValue(strUIName, out IUI ui)) return;
            //当前窗体隐藏状态
            ui.GameObject.SetActive(false);
            if (ui is IUIOnDisable uiOnDisabl)
            {
                uiOnDisabl.UIOnDisable();
                if (ui.UIType == EUIType.PopUp)
                    UIMaskMgr.Instance.CancelMaskWindow();
            }
            //“正在显示”集合中，移除本窗体
            _DicCurrentShowUIForms.Remove(strUIName);

            //把“正在显示集合”与“栈集合”中所有窗体都定义重新显示状态。
            foreach (IUI currentShowUI in _DicCurrentShowUIForms.Values)
            {
                ui.GameObject.SetActive(true);
                if (currentShowUI is IUIOnEnable uiOnEnable)
                {
                    uiOnEnable.UIOnEnable();
                    if (currentShowUI.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.SetMaskWindow(currentShowUI.GameObject, currentShowUI.UILucenyType);
                }
            }

            foreach (IUI staUI in _StaCurrentUIForms)
            {
                ui.GameObject.SetActive(true);
                if (staUI is IUIOnEnable uiOnEnable)
                {
                    uiOnEnable.UIOnEnable();
                    if (staUI.UIType == EUIType.PopUp)
                        UIMaskMgr.Instance.SetMaskWindow(staUI.GameObject, staUI.UILucenyType);
                }
            }
        }


        //其他
        /// <summary>
        /// 是否清空“栈集合”中得数据
        /// </summary>
        /// <returns></returns>
        private bool ClearStackArray()
        {
            if (_StaCurrentUIForms != null && _StaCurrentUIForms.Count >= 1)
            {
                _StaCurrentUIForms.Clear();//清空栈集合
                return true;
            }
            return false;
        }
    }
}

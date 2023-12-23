using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*--------脚本描述-----------

描述:
	脱离框架版本

-----------------------*/

namespace Core
{
    /// <summary> 摇杆类型 </summary>
    public enum E_JoystickType
    {
        /// <summary> 固定摇杆 </summary>
        Normal,
        /// <summary> 可变位置摇杆 </summary>
        CanChangePos,
        /// <summary> 可移动摇杆 </summary>
        CanMove,
    }

    public class JpyStickPanelView : MonoBehaviour
    {
        /// <summary>
        /// 鼠标按下抬起拖曳3个事件的监听这它主要用于控制范围
        /// </summary>
        private Image imageTouchRect;
        /// <summary>
        /// 摇杆背景图片
        /// </summary>
        private Image imgBk;
        /// <summary>
        /// 摇杆圆圈
        /// </summary>
        private Image imgControl;

        public E_JoystickType e_JoystickType = E_JoystickType.Normal;
        public float maxL = 150;

        public Action<Vector2> action;

        private void Awake()
        {
            imageTouchRect = transform.Find("JpyStickPanel/ImgTouchRect").GetComponent<Image>();
            imgBk = transform.Find("JpyStickPanel/ImgTouchRect/ImageBK").GetComponent<Image>();
            imgControl = transform.Find("JpyStickPanel/ImgTouchRect/ImageBK/ImgControl").GetComponent<Image>();

            switch (e_JoystickType)
            {
                default:
                case E_JoystickType.Normal: imgBk.gameObject.SetActive(true); break;
                case E_JoystickType.CanChangePos:
                case E_JoystickType.CanMove: imgBk.gameObject.SetActive(false); break;//可变位置摇杆 - 开始隐藏
            }

            //监听
            AddCustomEventListener(imageTouchRect, EventTriggerType.PointerDown, PointerDown);
            AddCustomEventListener(imageTouchRect, EventTriggerType.PointerUp, PointerUp);
            AddCustomEventListener(imageTouchRect, EventTriggerType.Drag, Drag);

            //测试代码
            GameObject gameObject = new GameObject("移动物体");
            CubeMove cubeMove= gameObject.AddComponent<CubeMove>();
            action = cubeMove.CheckDirChange;
        }

        /// <summary>
        /// 拖拽
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Drag(BaseEventData data)
        {
            Debug.Log("Drag");
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imgBk.rectTransform,//你想要改变位置的对象的父对象
                (data as PointerEventData).position,//得到当前屏幕鼠标位置
                (data as PointerEventData).pressEventCamera,// UI用的摄像机
                 out Vector2 localPos);//可以得到一个转换来的相对坐标

            //更新位置
            imgControl.transform.localPosition = localPos;


            //范围判断
            if (localPos.magnitude > maxL)//159代表ImageBK的Wight一半
            {
                switch (e_JoystickType)
                {
                    default:
                    case E_JoystickType.Normal:
                    case E_JoystickType.CanChangePos: break;
                    case E_JoystickType.CanMove:
                        imgBk.transform.localPosition += (Vector3)(localPos.normalized * (localPos.magnitude - maxL)); break;//超出多少就让背景图动多少
                }
                //超出范围 等于这个范围
                imgControl.transform.localPosition = localPos.normalized * maxL;
            }

            //分发我的摇杆方向
            action?.Invoke(localPos.normalized);
            //EventCenter.GetInstance().EventTrigger<Vector2>("Joystick", localPos.normalized);
        }

        /// <summary>
        /// 抬起
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PointerUp(BaseEventData data)
        {
            Debug.Log("Up");
            imgControl.transform.localPosition = Vector2.zero;
            //分发我的摇杆方向
            //EventCenter.GetInstance().EventTrigger<Vector2>("Joystick", Vector2.zero);
            action?.Invoke(Vector2.zero);

            switch (e_JoystickType)
            {
                default:
                case E_JoystickType.Normal: imgBk.gameObject.SetActive(true); break;
                case E_JoystickType.CanChangePos:
                case E_JoystickType.CanMove: imgBk.gameObject.SetActive(false); break;//可变位置摇杆 - 开始隐藏
            }
        }

        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="arg0"></param>
        private void PointerDown(BaseEventData data)
        {
            Debug.Log("Down");
            //可变位置摇杆 - 按下显示
            imgBk.gameObject.SetActive(true);

            switch (e_JoystickType)
            {
                default:
                case E_JoystickType.Normal: break;
                case E_JoystickType.CanChangePos:
                case E_JoystickType.CanMove:
                    //可变位置摇杆 - 点击屏幕位置显示 
                    Vector2 localPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                       imageTouchRect.rectTransform,//你想要改变位置的对象的父对象
                       (data as PointerEventData).position,//得到当前屏幕鼠标位置
                       (data as PointerEventData).pressEventCamera,// UI用的摄像机
                        out localPos);//可以得到一个转换来的相对坐标

                    imgBk.transform.localPosition = localPos;
                    break;//可变位置摇杆 - 开始隐藏
            }
        }

        /// <summary>
        /// 给控件添加自定义事件监听
        /// </summary>
        /// <param name="control">控件对象</param>
        /// <param name="type">事件类型</param>
        /// <param name="callBack">事件的响应函数</param>
        private void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
        {
            EventTrigger trigger = control.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = control.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener(callBack);

            trigger.triggers.Add(entry);
        }
    }

    #region 测试代码
    public class CubeMove : MonoBehaviour
    {
        private Vector3 dir;

        void Start()
        {
            //EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", CheckDirChange);
        }

        void Update()
        {
            this.transform.Translate(dir * Time.deltaTime, Space.World);
        }

        public void CheckDirChange(Vector2 dir)
        {
            this.dir.x = dir.x;
            this.dir.z = dir.y;
        }

    }
    #endregion
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class ShowUIObj : MonoBehaviour
    {
        public GameObject MouseDownUi
        {
            get
            {
                var raycastList = new List<RaycastResult>();
                //场景中的EventSystem
                var eventData = new PointerEventData(EventSystem.current);
                //鼠标位置
                eventData.position = Input.mousePosition;
                //调用所有GraphicsRacaster里面的Raycast，然后内部会进行排序，
                //直接拿出来，取第一个就可以用了
                EventSystem.current.RaycastAll(eventData, raycastList);
                //取第一个值
                RaycastResult raycast = default;
                foreach (var raycastResult in raycastList)
                {
                    if (!raycastResult.gameObject) continue;
                    raycast = raycastResult;
                }

                //获取父类中事件注册接口
                //如Button，Toggle之类的，毕竟我们想知道哪个Button被点击了，而不是哪张Image被点击了
                //当然可以细分为IPointerClickHandler, IBeginDragHandler之类细节一点的，各位可以自己取尝试
                var go = ExecuteEvents.GetEventHandler<IEventSystemHandler>(raycast.gameObject);
                //既然没拿到button之类的，说明只有Image挡住了，取点中结果即可
                if (!go)
                    go = raycast.gameObject;
                //编辑器中高亮显示
#if UNITY_EDITOR
                if (go)
                {
                    EditorGUIUtility.PingObject(go);
                    Selection.activeObject = go;
                }
#endif
                return go;
            }
        }

    }
}

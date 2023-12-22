//using System.Collections;
//using System.Collections.Generic;
//using System.Configuration;
//using UnityEngine;
//using UnityEngine.UI;
//using XFGameFramework;

///// <summary>
///// 背景适配 ( 原理是让当前的游戏物体 与 屏幕大小保持一致 )
///// </summary>
//public class BackgroundAdaptation : MonoBehaviour
//{

//    private float currentScreenWidth;
//    private float currentScreenHeight;

//    private float width;
//    private float height;

//    private Canvas canvas;
//    private RectTransform canvasRectTransform;

//    private void Awake()
//    {
//        RectTransform rect = GetComponent<RectTransform>();
//        if (rect == null) return;
//        width = rect.rect.width;
//        height = rect.rect.height;

//        canvas = GetComponentInParent<Canvas>();
//        canvasRectTransform = canvas.GetComponent<RectTransform>();

//        EventManager.AddEvent(XFGameFrameworkEvents.ON_SCREEN_RESOLUTION_CHANGE, OnScreenResolutionChange);

//    }

//    private void OnDestroy()
//    {
//        EventManager.RemoveEvent(XFGameFrameworkEvents.ON_SCREEN_RESOLUTION_CHANGE, OnScreenResolutionChange);
//    }


//    private void OnEnable()
//    {
//        Refresh();
//    }

//    private void OnScreenResolutionChange(object[] p)
//    {
//        Refresh();
//    }

//    // 如果能够动态调整分辨率 这里需要监听对应事件刷新( 暂时不考虑 )
//    private void Refresh()
//    {
//        if (currentScreenWidth == canvasRectTransform.rect.width && currentScreenHeight == canvasRectTransform.rect.height) return;
//        currentScreenWidth = canvasRectTransform.rect.width;
//        currentScreenHeight = canvasRectTransform.rect.height;

//        float scale_x = currentScreenWidth / width;
//        float scale_y = currentScreenHeight / height;
//        float s = scale_x > scale_y ? scale_x : scale_y;
//        transform.localScale = new Vector3(s, s, s);
//    }
//}

using UnityEngine;
using UnityEngine.UI;

namespace Core
{

    [RequireComponent(typeof(CanvasScaler))]
    public class AutomaticAdaptationCanvasScaler : MonoBehaviour
    {

        private CanvasScaler scaler;
        private int currentWidth;
        private int currentHeight;

        private void Awake()
        {
            scaler = GetComponent<CanvasScaler>();
            //CoreEvent.EventAdd<object[]>((int)ECoreEvent.ON_SCREEN_RESOLUTION_CHANGE, UpdateScreenAspect);
        }

        private void OnDestroy()
        {
            //CoreEvent.EventRemove<object[]>((int)ECoreEvent.ON_SCREEN_RESOLUTION_CHANGE, UpdateScreenAspect);
        }

        private void OnEnable()
        {
            UpdateScreenAspect(null);
        }


        // 如果能够动态调整分辨率 这里需要监听对应事件刷新( 暂时不考虑 )
        private void UpdateScreenAspect(object[] param)
        {
            if (Screen.width == currentWidth || Screen.height == currentHeight)
                return;

            // 计算出比例
            float aspect = (float)Screen.width / Screen.height;
            float inverse_lerp = 0;
            if (IsLandscape())
                inverse_lerp = Mathf.InverseLerp(1.33f, 1.77f, aspect); // 12:9 ~ 16:9  
            else
                inverse_lerp = Mathf.InverseLerp(9.0f / 16, 9.0f / 12, aspect); // 

            scaler.matchWidthOrHeight = inverse_lerp;

            currentWidth = Screen.width;
            currentHeight = Screen.height;
        }



        // 判断当前是不是横屏
        private bool IsLandscape()
        {
            return Screen.width > Screen.height;
        }

    }
}

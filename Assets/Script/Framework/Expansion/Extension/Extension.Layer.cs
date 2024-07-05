using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,06,15:50
// @Description:
// </summary>

namespace zhaorh.UI
{
    public static partial class Extension
    {
        /// <summary>
        /// 设置层级 扩展方法
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="layerName"></param>
        public static void SetLayerInChildren(this GameObject gameObject, string layerName)
        {
            gameObject.SetLayerInChildren(LayerMask.NameToLayer(layerName));
        }
        public static void SetLayerInChildren(this GameObject gameObject, int layer)
        {
            var children = gameObject.GetComponentsInChildren<Transform>(true);
            for (var i = 0; i < children.Length; i++)
            {
                children[i].gameObject.layer = layer;
            }
        }
        public static void ReplaceLayerInChildren(this GameObject gameObject, int layer, int replacedLayer = 0)
        {
            var children = gameObject.GetComponentsInChildren<Transform>(true);
            for (var i = 0; i < children.Length; i++)
            {
                if (children[i].gameObject.layer == replacedLayer)
                {
                    children[i].gameObject.layer = layer;
                }
            }
        }
    }
    
}
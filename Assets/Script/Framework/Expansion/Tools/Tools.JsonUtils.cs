using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,06,16:52
// @Description:
// </summary>

namespace zhaorh
{
    public partial class Tools
    {
        /// <summary>
        /// 将Json解析为Vector3 坐标字段为x,y,z
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Vector3 JsonToVector3(string json)
        {
            var pos = Vector3.zero;
            try
            {
                JsonObject jo = SimpleJson.SimpleJson.DeserializeObject(json) as JsonObject;
                pos = new Vector3(toFloat(jo["x"].ToString()), toFloat(jo["y"].ToString()), toFloat(jo["z"].ToString()));
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            return pos;
        }
        public static Vector2 JsonToVector2(string json)
        {
            JsonObject jo = SimpleJson.SimpleJson.DeserializeObject(json) as JsonObject;
            Vector2 pos = new Vector2(Tools.toFloat(jo["x"].ToString()), Tools.toFloat(jo["y"].ToString()));
            return pos;
        }
        /// <summary>
        /// 将Json解析为Color 坐标字段为r,g,b,a
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Color JsonToColor(string json)
        {
            JsonObject jo = SimpleJson.SimpleJson.DeserializeObject(json) as JsonObject;
            Color col = new Color(Tools.toFloat(jo["r"].ToString()) / 255f,
                Tools.toFloat(jo["g"].ToString()) / 255f,
                Tools.toFloat(jo["b"].ToString()) / 255f,
                Tools.toFloat(jo["a"].ToString()) / 255f
            );
            return col;
        }
        
    }
}
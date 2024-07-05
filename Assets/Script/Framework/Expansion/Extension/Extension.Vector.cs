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
        /// 返回-180~180的欧拉角
        /// </summary>
        /// <param name="from">自身</param>
        /// <param name="to">目标向量</param>
        /// <returns></returns>
        public static float AnglePN180(this Vector3 from ,Vector3 to)
        {
            float angle = Vector3.Angle(from, to);
            Vector3 cross = Vector3.Cross(from, to);
            if (cross.y < 0)
            {
                angle = -angle;
            }
            return angle;
        }
        public static Vector3 ZeroY(this Vector3 target)
        {
            target.y = 0;
            return target;
        }
        public static Vector3 ZeroYNormal(this Vector3 target)
        {
            target.y = 0;;
            return target.normalized;
        }
        public static Vector2 ToVector2XZ(this Vector3 target)
        {
            Vector2 v2;
            v2.x = target.x;
            v2.y = target.z;
            return v2;
        }
    }
}
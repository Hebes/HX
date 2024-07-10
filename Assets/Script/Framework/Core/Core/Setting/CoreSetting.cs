using System.Collections;
using Framework.Core;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// 设置
    /// </summary>
    [CreateCore(typeof(CoreSetting), 2)]
    public class CoreSetting : ICore
    {
        public IEnumerator AsyncEnter()
        {
            yield return null;
        }

        public IEnumerator Exit()
        {
            yield break;
        }

        public void Init()
        {
            //限制帧数
            Application.targetFrameRate = 60;
        }
    }
}

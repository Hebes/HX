using System;
using System.Collections;
using Framework.Core;

namespace Framework.Core
{

    /// <summary>
    /// 存档接口
    /// </summary>
    public interface ISave
    {
        void Save();
    }

    /// <summary>
    /// 存读档数据
    /// </summary>
    [CreateCore(typeof(CoreSaveLoadData), 2)]
    public class CoreSaveLoadData : ICore
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
        }
    }
}

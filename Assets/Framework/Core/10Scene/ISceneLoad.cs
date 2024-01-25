using System.Collections;
using UnityEngine.SceneManagement;

/*--------脚本描述-----------
				
描述:
    场景加载接口

-----------------------*/

namespace Core
{
    public interface ISceneLoad
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void CoreSceneInit();

        /// <summary>
        /// 加载场景
        /// </summary>
        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode);

        /// <summary>
        /// 卸载场景
        /// </summary>
        public IEnumerator UnloadSceneAsync(string sceneName);
    }
}

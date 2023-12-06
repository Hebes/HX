using Cysharp.Threading.Tasks;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
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
        public void LoadScene(string sceneName, ELoadSceneModel loadSceneModel);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <returns></returns>
        public UniTask LoadSceneAsync(string sceneName, ELoadSceneModel loadSceneModel);

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="oldSceneName"></param>
        /// <param name="newSceneName"></param>
        /// <param name="loadSceneModel"></param>
        /// <returns></returns>
        public UniTask ChangeSceneAsync(string oldSceneName, string newSceneName, ELoadSceneModel loadSceneModel);

        /// <summary>
        /// 卸载场景
        /// </summary>
        public UniTask UnloadSceneAsync(string sceneName);

        /// <summary>
        /// 获取场景数据(暂时无用)
        /// </summary>
        /// <returns></returns>
        public object GetManagerDic();
    }
}

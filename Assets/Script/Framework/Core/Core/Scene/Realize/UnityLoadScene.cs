using Cysharp.Threading.Tasks;
using System.Collections;
using Framework.Core;
using UnityEngine;
using UnityEngine.SceneManagement;


/*--------脚本描述-----------

描述:
     场景接口加载
https://blog.csdn.net/weixin_44350205/article/details/99684904
https://zhuanlan.zhihu.com/p/47779154

-----------------------*/

namespace Framework.Core
{
    public class UnityLoadScene : ISceneLoad
    {
        public const int key = 1;

        public bool IsLoadOver { get; set; }

        public void CoreSceneInit()
        {
        }
        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
        }

        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            //用Slider 展示的数值
            IsLoadOver = false;
            int disableProgress = 0;
            int toProgress = 0;
            //异步场景切换
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            //不允许有场景切换功能
            op.allowSceneActivation = false;
            //op.progress 只能获取到90%，最后10%获取不到，需要自己处理
            while (op.progress < 0.9f)
            {
                //获取真实的加载进度
                toProgress = (int)(op.progress * 100);
                while (disableProgress < toProgress)
                {
                    ++disableProgress;
                    //UnityEngine.Debug.Log($"{sceneName}加载进度是:{disableProgress}");
                    CoreEvent.I.Trigger(EEvent.LoadSceneEvent, disableProgress);
                    //_progress.value = disableProgress / 100.0f;//0.01开始
                    //yield return new WaitForEndOfFrame();
                    yield return null;
                }
            }

            //因为op.progress 只能获取到90%，所以后面的值不是实际的场景加载值了
            toProgress = 100;
            while (disableProgress < toProgress)
            {
                ++disableProgress;
                //UnityEngine.Debug.Log($"{sceneName}加载进度是:{disableProgress}");
                CoreEvent.I.Trigger(EEvent.LoadSceneEvent, disableProgress);
                //_progress.value = disableProgress / 100.0f;
                //yield return new WaitForEndOfFrame();
                yield return null;
            }
            op.allowSceneActivation = true;
            //if (op.isDone == false)
            //    Debug.Log($"{sceneName}场景加载失败");
            IsLoadOver = true;
        }

        public IEnumerator UnloadSceneAsync(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
            yield return asyncOperation;
            //yield return asyncOperation.ToIEnumerator();
            //while (asyncOperation.isDone)
            //    yield return null;
        }
    }
}

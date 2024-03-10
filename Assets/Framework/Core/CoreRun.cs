using System.Collections;

/*--------脚本描述-----------

描述:
    核心初始化

-----------------------*/


namespace Core
{
    public class CoreRun
    {
        public IEnumerator CoreInit()
        {
            //可以首先加载的
            yield return Init<CoreDebug>();          //日志
            yield return Init<CoreLanguage>();       //多语言
            yield return Init<CoreEvent>();          //事件
            yield return Init<CoreBehaviour>();      //生命周期
            yield return Init<CoreResource>();       //资源加载
            yield return Init<CoreAduio>();          //音乐
            yield return Init<CoreUI>();             //UI
            //yield return Init<CoreData>();           //数据
            yield return Init<CoreScene>();          //场景
            yield return Init<CoreInput>();          //输入
            yield return Init<CoreDataSystem>();     //数据
            //yield return Init<CoreSystemOrder>();    //指令
        }

        public IEnumerator Init<T>() where T : ICore, new()
        {
            yield return new T().ICoreInit();
        }
    }
}

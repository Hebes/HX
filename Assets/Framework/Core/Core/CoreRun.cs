/*--------脚本描述-----------

描述:
    核心初始化

-----------------------*/

using Farm2D;

namespace Core
{
    public class CoreRun
    {
        public CoreRun()
        {
            Init<CoreDebug>();          //日志
            Init<CoreEvent>();          //事件
            Init<CoreBehaviour>();      //生命周期
            Init<CoreResource>();       //资源加载
            Init<CoreAduio>();          //音乐
            Init<CoreUI>();             //UI
            Init<CoreData>();           //数据
            Init<CoreScene>();          //场景
            Init<CoreInput>();          //输入
            Init<CoreDataSystem>();     //数据
            Init<CoreSystemOrder>();    //指令 
        }

        public T Init<T>() where T : ICore, new()
        {
            T t = new T();
            t.ICoreInit();
            return t;
        }
    }
}

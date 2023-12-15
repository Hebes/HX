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
            Init<CoreDebug>();
            Init<CoreEvent>();
            Init<CoreBehaviour>();
            Init<CoreResource>();
            Init<CoreAduio>();
            Init<CoreUI>();
            Init<CoreData>();
            Init<CoreScene>();
            Init<CoreInput>();
            Init<CoreDataSystem>();
            Init<CoreSystemOrder>();
        }

        public T Init<T>() where T : ICore, new()
        {
            T t = new T();
            t.ICoreInit();
            return t;
        }
    }
}

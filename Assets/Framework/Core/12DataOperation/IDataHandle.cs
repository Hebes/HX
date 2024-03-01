/*--------脚本描述-----------
				
描述:
    数据处理

-----------------------*/

namespace Core
{
    public interface IDataHandle
    {
        public abstract void Save(object obj, string fileName);

        public abstract T Load<T>(string fileName) where T : class;
    }
}

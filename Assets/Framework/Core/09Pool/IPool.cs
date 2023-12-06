/*--------脚本描述-----------

描述:
    对象池接口

-----------------------*/

namespace Core
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// 从对象池出来之后需要做的事
        /// </summary>
        public void GetAfter();

        /// <summary>
        /// 进对象池之前需要做的事
        /// </summary>
        public void PushBefore();
    }
}

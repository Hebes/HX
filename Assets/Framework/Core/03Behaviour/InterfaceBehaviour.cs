using System.Collections;

namespace Core
{
    public interface IBehaviour
    {

    }

    /// <summary>
    /// 生命周期接口
    /// </summary>
    public interface IUpdata : IBehaviour
    {
        /// <summary>
        /// Updata接口
        /// </summary>
        void CoreBehaviourUpdata();
    }

    /// <summary>
    /// 等待帧更新
    /// </summary>
    public interface IWaitFrameUpdata : IBehaviour
    {
        IEnumerator WaitFrameUpdata();
    }

    /// <summary>
    /// 固定帧更新
    /// </summary>
    public interface IFixedUpdate : IBehaviour
    {
        public void OnFixedUpdate();
    }
}

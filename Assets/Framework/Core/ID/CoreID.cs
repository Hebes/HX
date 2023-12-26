namespace Core
{
    /// <summary>
    /// 生成唯一ID
    /// </summary>
    public class CoreID : ICore
    {
        public static CoreID Instance;
        private int m_id = 0;

        public void ICoreInit()
        {
            Instance = this;
        }

        /// <summary>
        /// 生成ID,唯一
        /// </summary>
        public static int GenerateID()
        {
            return Instance.m_id++;
        }
    }
}

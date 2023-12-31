namespace Core
{
    /// <summary>
    /// 生成唯一ID
    /// </summary>
    public class CoreID
    {
        private static int m_id = 0;

        /// <summary>
        /// 生成ID,唯一
        /// </summary>
        public static int GenerateID()
        {
            return m_id++;
        }
    }
}

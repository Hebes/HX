using UnityEditor;

namespace Tool
{
    public static class ToolExpansion_AssetDatabase
    {
        /// <summary>
        /// 重命名API
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="newName"></param>
        public static void RenameAsset(this string pathName, string newName)
        {
            AssetDatabase.RenameAsset(pathName, newName);//改名API
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public static void Refresh()
        {
            AssetDatabase.Refresh();
        }
    }
}

using UnityEditor;
using UnityEditor.SceneManagement;

namespace ToolEditor
{
    public class SwitchScene: EditorWindow
    {
        //public static 
        
        private static void SwitchToScene(string scenePathValue)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(scenePathValue);
        }
    }
}
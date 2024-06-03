using UnityEngine;


/*--------脚本描述-----------

描述:
    指令器

-----------------------*/

namespace Core
{
    public class UISystemOrder : MonoBehaviour
    {
        /// <summary>
        /// 是否允许调试
        /// </summary>
        public bool AllowDebugging = true;
        private Rect _windowRect = new Rect(0, 60, 100, 60);
        private bool _expansion = false;//扩大或者缩小


        private Vector2 _scrollLogView = Vector2.zero;

        private int _fps = 0;
        private Color _fpsColor = Color.white;
        private int _frameNumber = 0;
        private float _lastShowFPSTime = 0f;

        private string singleText = "测试文本..";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _expansion = !_expansion;
                if (_expansion)
                {
                    _windowRect.width = 640;//(Screen.width / 640) * 
                    _windowRect.height = 360;//(Screen.height / 360) *
                }
                else
                {
                    _windowRect.width = 100;
                    _windowRect.height = 60;
                }
            }

            if (AllowDebugging)
            {
                _frameNumber += 1;
                float time = Time.realtimeSinceStartup - _lastShowFPSTime;
                if (time >= 1)
                {
                    _fps = (int)(_frameNumber / time);
                    _frameNumber = 0;
                    _lastShowFPSTime = Time.realtimeSinceStartup;
                }
            }
        }


        private void OnGUI()
        {
            if (AllowDebugging)
            {
                if (_expansion)
                    _windowRect = GUI.Window(0, _windowRect, ExpansionGUIWindow, "指令器");
                else
                    _windowRect = GUI.Window(0, _windowRect, ShrinkGUIWindow, "指令器");
            }
        }
        private void ExpansionGUIWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));//窗口拖动

            #region title
            GUILayout.BeginHorizontal();
            GUI.contentColor = _fpsColor;
            if (GUILayout.Button("FPS:" + _fps, GUILayout.Height(30)))
            {
                _expansion = false;
                _windowRect.width = 100;
                _windowRect.height = 60;
            }
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            #endregion

            #region console
            _scrollLogView = GUILayout.BeginScrollView(_scrollLogView, "Box", GUILayout.Height(240));
            GUILayout.Label("111");
            GUILayout.EndScrollView();
            #endregion

            singleText = GUILayout.TextField(singleText);
            if (GUILayout.Button("发送指令", GUILayout.Height(30)))
            {
                UnityEngine.Debug.Log(singleText);
            }
        }
        private void ShrinkGUIWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            GUI.contentColor = _fpsColor;
            if (GUILayout.Button("FPS:" + _fps, GUILayout.Width(80), GUILayout.Height(30)))
            {
                _expansion = true;
                _windowRect.width = 640;//(Screen.width / 640) * 
                _windowRect.height = 360;//(Screen.height / 360) *
            }
            GUI.contentColor = Color.white;
        }
    }

}

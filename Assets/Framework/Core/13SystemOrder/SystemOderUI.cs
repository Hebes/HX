using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class SystemOderUI : MonoBehaviour
    {
        private Rect _windowRect = new Rect(0, 0, (Screen.width / 640) * 100, (Screen.height / 360) * 60);

        private void OnGUI()
        {
            //if (CoreSystemOrder.ChackOpenSystemOrder() == false) return;
            //_windowRect = GUI.Window(0, _windowRect, ExpansionGUIWindow, "指令器");
        }


        /// <summary>
        /// 扩大GUI窗口
        /// </summary>
        /// <param name="windowId"></param>
        private void ExpansionGUIWindow(int windowId)
        {

        }
    }
}

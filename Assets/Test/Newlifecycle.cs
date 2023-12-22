using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Newlifecycle : MonoBehaviour
{
    public List<int> ints = new List<int>();
    private void Awake()
    {
        CoreDebug coreDebug = new CoreDebug();
        coreDebug.ICoreInit();

        CoreBehaviour coreBehaviour = new CoreBehaviour();
        coreBehaviour.ICoreInit();

        CoreResource coreResource = new CoreResource();
        coreResource.ICoreInit();


        InputAndroid inputAndroid = new InputAndroid();
        inputAndroid.Init();

        CoreInput coreInput = new CoreInput();
        coreInput.ICoreInit();
        InputMouse inputMouse = new InputMouse();
        inputMouse.Init();

        ints.Add(1);
        ints.Add(2);
        ints.Add(3);
    }

}

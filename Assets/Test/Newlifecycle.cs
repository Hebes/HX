using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Newlifecycle : MonoBehaviour
{
    private void Awake()
    {
        CoreResource coreResource =new CoreResource();
        coreResource.ICoreInit();

        CoreDebug coreDebug =  new CoreDebug();
        coreDebug.ICoreInit();

        InputAndroid inputAndroid =new InputAndroid();
        inputAndroid.Init();
    }
}

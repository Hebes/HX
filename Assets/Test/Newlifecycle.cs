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
        CoreDebug coreDebug = new CoreDebug();
        coreDebug.ICoreInit();
        CoreBehaviour coreBehaviour = new CoreBehaviour();
        coreBehaviour.ICoreInit();

        Test1 test1 = new Test1();
        CoreBehaviour.Add(test1);

        GameObject gameObject = new GameObject("ttt");
        Test2 test2 = gameObject.AddComponent<Test2>();
        CoreBehaviour.Add(test2);


        //CoreResource coreResource = new CoreResource();
        //coreResource.ICoreInit();
        //CorePool corePool = new CorePool();
        //corePool.ICoreInit();
        //UnityEngine.Debug.Log($"corePool初始化完毕");

        //Test1 test1 = CorePool.Get<Test1>();
        //CorePool.Push(test1);


        //Test2 test2 = CorePool.GetMono<Test2>("111");
        //test2.gameObject.name = "Test2";
        ////corePool.PushMono(test2);

        //Test2 test3= CorePool.GetMono<Test2>("111");
        //test3.gameObject.name = "Test3";
        ////corePool.PushMono(test3);

        //Test2 test4 = CorePool.GetMono<Test2>("111");
        //test4.gameObject.name = "Test4";
        //CorePool.PushMono(test4);

    }
}

﻿using Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.PlayerLoop.EarlyUpdate;
using Debug = UnityEngine.Debug;

public class Newlifecycle : MonoBehaviour
{
    //public List<int> ints = new List<int>();
    //private TickTimer timer;

    //public int id;
    public long id;
    //public string id;

    private void Awake()
    {
        CoreDebug coreDebug = new CoreDebug();
        coreDebug.ICoreInit();


        //CoreBehaviour coreBehaviour = new CoreBehaviour();
        //coreBehaviour.ICoreInit();

        //CoreResource coreResource = new CoreResource();
        //coreResource.ICoreInit();


        //InputAndroid inputAndroid = new InputAndroid();
        //inputAndroid.Init();

        //CoreInput coreInput = new CoreInput();
        //coreInput.ICoreInit();
        //InputMouse inputMouse = new InputMouse();
        //inputMouse.Init();

        //ints.Add(1);
        //ints.Add(2);
        //ints.Add(3);

        //Debug.Log(1111);

        //UnityEngine.Debug.Log("测试");
        //timer = new TickTimer(10,true) 
        //{
        //    LogFunc = UnityEngine.Debug.Log,
        //    WarnFunc = UnityEngine.Debug.LogWarning,
        //    ErrorFunc = UnityEngine.Debug.LogError
        //};
        //timer.AddTask(10, (id) =>
        //{
        //    UnityEngine.Debug.Log($"执行了{id}次");
        //},
        //(id) =>
        //{
        //    UnityEngine.Debug.Log("取消任务");
        //},
        //10);
    }

    private void Update()
    {
        //id = CoreID.CreateId();
        DateTime now = DateTime.Now;
        Int64 ID = HelperID.GetTimestamp(now);
        HelperID.NewDate(ID);
        Debug.Log(ID);
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    id = CoreID.CreateTimeId();
        //}
    }

    //private void Update()
    //{
    //    timer.UpdateTask();
    //    timer.HandleTask();
    //}

}


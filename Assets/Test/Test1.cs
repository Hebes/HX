using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Debug = UnityEngine.Debug;
using Assets.Test;

public class Test1 : IUpdata,IBuffCarrier
{
    public int ID => throw new System.NotImplementedException();

    public List<IBuffData> BuffList { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    //public void GetAfter()
    //{
    //    UnityEngine.Debug.Log($"Test1出了对象池");
    //    CoreBehaviour.Add(this);
    //}

    //public void PushBefore()
    //{
    //    UnityEngine.Debug.Log($"Test1进了对象池");
    //    CoreBehaviour.Remove(this);
    //}

    public void OnUpdata()
    {
        BuffData1 buffData1 = new BuffData1();
        UnityEngine.Debug.Log($"Test1   Updata");
        //IBuffCarrier.Remove(buffData1);
    }
}

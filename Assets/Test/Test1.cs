using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Test1 //: IUpdata,IBuffCarrier
{
    public int ID => throw new System.NotImplementedException();

    public List<IBuffData> BuffList { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    //public void GetAfter()
    //{
    //    UnityEngine.Debug.Log($"Test1���˶����");
    //    CoreBehaviour.Add(this);
    //}

    //public void PushBefore()
    //{
    //    UnityEngine.Debug.Log($"Test1���˶����");
    //    CoreBehaviour.Remove(this);
    //}

    public void OnUpdata()
    {
        BuffData1 buffData1 = new BuffData1();
        UnityEngine.Debug.Log($"Test1   Updata");
        //IBuffCarrier.Remove(buffData1);
    }
}

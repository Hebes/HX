using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Debug = UnityEngine.Debug;

public class Test1 : IUpdata
{

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
        UnityEngine.Debug.Log($"Test1   Updata");
    }
}

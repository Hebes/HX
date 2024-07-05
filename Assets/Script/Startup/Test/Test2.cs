using Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Test2 : MonoBehaviour, IUpdata
{
    //public const string pth = "11";

    //public string Name => pth;

    //public void GetAfter()
    //{
    //    UnityEngine.Debug.Log($"Test2出了对象池");
    //    gameObject.SetActive(true);
    //    CoreBehaviour.Add(this);
    //}

    //public void PushBefore()
    //{
    //    UnityEngine.Debug.Log($"Test2进了对象池");
    //    gameObject.SetActive(false);
    //    CoreBehaviour.Remove(this);
    //}

    private void Awake()
    {
        new CoreResource().Init();
        var pool = new CorePool();
        pool.Init();

        CorePool.Instance.GetClass<Test1>();
    }

    public void CoreBehaviourUpdata()
    {
        UnityEngine.Debug.Log($"{gameObject.name}   Updata");

    }
}

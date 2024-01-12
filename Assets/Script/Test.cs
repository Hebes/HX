using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        ManagerSerializableData managerSerializableData = new ManagerSerializableData();
        managerSerializableData.Init();
        Data data = new Data();
        data.init();
    }
}

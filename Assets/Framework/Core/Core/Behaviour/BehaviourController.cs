using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

描述:
    生命周期控制
    https://blog.csdn.net/beihuanlihe130/article/details/76098844
    https://blog.csdn.net/qq_40544338/article/details/115414085
-----------------------*/

namespace Framework.Core
{
    public enum EMonoType
    {
        Updata,
        FixedUpdate,
    }

    public class BehaviourController : MonoBehaviour
    {
        public static BehaviourController Instance;
        private List<IUpdata> updatasList;
        private List<IFixedUpdate> fixedUpdatesList;

        private void Awake()
        {
            Instance = this;
            updatasList = new List<IUpdata>();
            fixedUpdatesList = new List<IFixedUpdate>();
        }
        private void Update()
        {
            for (int i = 0; i < updatasList.Count; i++)
                updatasList[i].CoreBehaviourUpdata();
        }
        private void FixedUpdate()
        {
            for (int i = 0; i < fixedUpdatesList.Count; i++)
                fixedUpdatesList[i].OnFixedUpdate();
        }


        public void Remove<T>(T t, EMonoType monoType = EMonoType.Updata) where T : IBehaviour
        {
            switch (monoType)
            {
                case EMonoType.Updata:
                    updatasList.Remove(t as IUpdata);
                    break;
                case EMonoType.FixedUpdate:
                    fixedUpdatesList.Remove(t as IFixedUpdate);
                    break;
                default: break;
            }
        }
        internal void Add<T>(T t, EMonoType monoType = EMonoType.Updata) where T : IBehaviour
        {
            switch (monoType)
            {
                case EMonoType.Updata:
                    updatasList.Add(t as IUpdata);
                    break;
                case EMonoType.FixedUpdate:
                    fixedUpdatesList.Add(t as IFixedUpdate);
                    break;
                default:
                    break;
            }
        }
    }
}

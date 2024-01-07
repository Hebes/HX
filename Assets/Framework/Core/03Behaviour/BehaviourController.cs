using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

描述:
    生命周期控制

-----------------------*/

namespace Core
{
    public enum EMonoType
    {
        Updata,
        FixedUpdate,
        WaitFrameUpdata,
    }

    public class BehaviourController : SingletonNewMono<BehaviourController>
    {
        private List<IUpdata> updatasList;
        private List<IFixedUpdate> fixedUpdatesList;
        private List<IWaitFrameUpdata> waitFrameUpdatasList;
        private Coroutine waitFrameUpdata;
        private bool ttt = true;

        private void Awake()
        {
            updatasList = new List<IUpdata>();
            fixedUpdatesList = new List<IFixedUpdate>();
            waitFrameUpdatasList = new List<IWaitFrameUpdata>();

        }
        private void Update()
        {
            for (int i = 0; i < updatasList.Count; i++)
                updatasList[i].CoreBehaviourUpdata();
            if (ttt)
            {
                StartCoroutine(WaitFrameUpdata());
            }
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
                case EMonoType.WaitFrameUpdata:
                    waitFrameUpdatasList.Remove(t as IWaitFrameUpdata);
                    break;
                default:
                    break;
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
                case EMonoType.WaitFrameUpdata:
                    waitFrameUpdatasList.Add(t as IWaitFrameUpdata);
                    break;
                default:
                    break;
            }


        }
        IEnumerator WaitFrameUpdata()
        {
            ttt = false;
            for (int i = 0; i < waitFrameUpdatasList.Count; i++)
            {
                //yield return null;
                yield return new WaitForSeconds(0.02f);
                waitFrameUpdatasList[i].WaitFrameUpdata();
            }
            ttt = true;
        }
    }
}

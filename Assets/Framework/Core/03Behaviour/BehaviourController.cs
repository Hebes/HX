﻿using System.Collections.Generic;

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
    }

    public class BehaviourController : SingletonNewMono<BehaviourController>
    {
        private List<IUpdata> updatasList;
        private List<IFixedUpdate> fixedUpdatesList;

        private void Awake()
        {
            updatasList = new List<IUpdata>();
            fixedUpdatesList = new List<IFixedUpdate>();
        }

        private void Update()
        {
            for (int i = 0; i < updatasList.Count; i++)
                updatasList[i].OnUpdata();
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
                default:
                    break;
            }
        }
    }
}

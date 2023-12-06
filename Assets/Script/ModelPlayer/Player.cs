using Core;
using UnityEngine;

namespace FieldEdge
{
    /// <summary>
    /// 玩家脚本
    /// </summary>
    [RequireComponent(typeof(ItemPickUp))]                  //物品拾取
    [RequireComponent(typeof(TriggerObscuringItemFader))]   //触发物品模糊
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Player : MonoBehaviour,IFixedUpdate,IUpdata
    {
        private void Awake()
        {
            CoreBehaviour.Add(this);
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdata()
        {
        }

        
    }
}

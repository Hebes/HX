//using UnityEngine;

///// <summary>
///// 触发物品模糊状态
///// </summary>
//public class TriggerObscuringItemFader : MonoBehaviour
//{
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        //获取我们碰撞的游戏对象，然后获取它及其子对象上的所有模糊项目Fader组件-然后触发淡出
//        ObscuringItemFader[] obscuringItemFader = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();
//        if (obscuringItemFader.Length <= 0) return;
//        for (int i = 0; i < obscuringItemFader.Length; i++)
//            obscuringItemFader[i].FadeOut();
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        //获取我们碰撞的游戏对象，然后获取它及其子对象上的所有模糊项目Fader组件-然后触发淡入
//        ObscuringItemFader[] obscuringItemFader = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();
//        if (obscuringItemFader.Length <= 0) return;
//        for (int i = 0; i < obscuringItemFader.Length; i++)
//            obscuringItemFader[i].FadeIn();
//    }
//}

// using System.Collections;
// using System.Collections.Generic;
// using System.Resources;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.UI.Extensions;
//
// // <summary>
// // @Author: zrh
// // @Date: 2023,02,10,16:30
// // @Description:
// // </summary>
//
// namespace Game.UI
// {
//     /// <summary>
//     /// 浮动列表：当小于可显示数目的时候，居中并禁用ScrollRect，否则，启用ScrollRect
//     /// 可以控制参数是否启用居中功能
//     /// </summary>
//     public class UIFlowLayoutList : MonoBehaviour
//     {
//         private Transform tran;
//         private HorizontalOrVerticalLayoutGroup layoutGroup;
//         private FlowLayoutGroup flowLayoutGroup;
//         private RectMask2D mask2D;
//         private ScrollRect scrollRect;
//         private UICell templateCell;
//         public string cellPrefabName;
//
//         public bool alwaysPaddingLeft = false;
//
//         private void Awake()
//         {
//             this.tran = this.transform;
//
//             scrollRect = tran.GetComponent<ScrollRect>();
//             layoutGroup = tran.GetChildComponent<HorizontalOrVerticalLayoutGroup>("Mask/Root");
//             mask2D = tran.GetChildComponent<RectMask2D>("Mask");
//             flowLayoutGroup = tran.GetChildComponent<FlowLayoutGroup>("Mask");
//             templateCell = tran.GetChildComponent<UICell>("Cell");
//
//             if (templateCell == null)
//             {
//                 Debug2.LogError("Zrh 改");
//                 // templateCell = ResourceManager.Instance.Load<UICell>(cellPrefabName);
//             }
//
//             templateCell.gameObject.SetActive(false);
//         }
//
//         /// <summary>
//         /// 生成对应的列表内容
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <param name="dataList"></param>
//         public void SetContent<T>(List<T> dataList)
//         {
//             Clear();
//             for (var i = 0; i < dataList.Count; i++)
//             {
//                 var newCell = Instantiate(templateCell);
//                 newCell.gameObject.SetActive(true);
//                 newCell.transform.SetParent(layoutGroup.transform);
//                 newCell.transform.Normalize();
//                 newCell.SetContent(dataList[i]);
//             }
//             if (alwaysPaddingLeft == true)
//             {
//                 flowLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
//             }
//             else
//             {
//                 flowLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
//             }
//             DisableScrollRectIfDisplayCompletely(dataList.Count);
//             //小于可显示最大值时用来更新计算，这个写法好奇怪，有空找找新的写法
//             layoutGroup.enabled = false;
//             layoutGroup.enabled = true;
//         }
//
//         private void Clear()
//         {
//             for (var i = layoutGroup.transform.childCount - 1; i >= 0; i--)
//                 Destroy(layoutGroup.transform.GetChild(i).gameObject);
//         }
//
//         /// <summary>
//         /// 能完整显示的时候需要禁用，否会直接走scroll的默认布局
//         /// </summary>
//         /// <param name="count"></param>
//         private void DisableScrollRectIfDisplayCompletely(int count)
//         {
//             if (count <= 0)
//                 return;
//             var maskSize = mask2D.GetComponent<RectTransform>().sizeDelta;
//             var maxFullCount = maskSize.x / (templateCell.GetChildComponent<RectTransform>(0).sizeDelta.x + layoutGroup.spacing);
//
//             scrollRect.enabled = count >= maxFullCount;
//         }
//     }
// }
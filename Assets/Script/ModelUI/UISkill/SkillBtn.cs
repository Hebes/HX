using Core;
using ExpansionUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/*--------脚本描述-----------

描述:
    技能按钮点选

-----------------------*/

public class SkillBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPool
{
    private Text skilName;
    private Transform select;

    private void Awake()
    {
        skilName = transform.Find("SkilName").GetText();
        select = transform.Find("Select");
    }

    public void SetSkillBtnData(string btnName)
    {
        skilName.text = btnName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        select.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        select.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        select.gameObject.SetActive(false);
    }

    public void Get()
    {
        gameObject.SetActive(true);
        select.gameObject.SetActive(false);
    }

    public void Push()
    {
        gameObject.SetActive(false);
        select.gameObject.SetActive(false);
    }
}

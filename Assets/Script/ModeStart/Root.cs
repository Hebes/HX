using Core;
using System.Collections;
using UnityEngine;

/// <summary>
/// ��ڽű�
/// </summary>
public class Root : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        //��ʼ������
        yield return new CoreRun().CoreInit();
        //��ʾ������
        CoreUI.ShwoUIPanel<MainMenuView>(ConfigPrefab.prefabUIMianMenu);
        //������ģ��
        yield return new ModelRun().ModelInit();
    }
}

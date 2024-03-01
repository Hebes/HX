using Core;
using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

/// <summary>
/// ��ڽű�
/// </summary>
public class InitGame : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Init());
    }

    public  IEnumerator Init()
    {
        //��ʼ������
        CoreRun coreRun = new CoreRun();
        yield return coreRun.CoreInit();
        //��ʾ������
        CoreUI.ShwoUIPanel<MainMenuView>(ConfigPrefab.prefabUIMianMenu);
        //������ģ��
        ModelRun modelRun = new ModelRun();
        yield return modelRun.ModelInit();
    }
}

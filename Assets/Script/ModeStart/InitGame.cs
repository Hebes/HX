using Core;
using UnityEngine;

/// <summary>
/// ��ڽű�
/// </summary>
public class InitGame : MonoBehaviour
{
    private void Awake()
    {
        //��ʼ������
        new CoreRun();
        //������ģ��
        new ModelRun();
        //��ʾ������
        CoreUI.ShwoUIPanel<MainMenuView>(ConfigPrefab.prefabMianMenu);
    }
}

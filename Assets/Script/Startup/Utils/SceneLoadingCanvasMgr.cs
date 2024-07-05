using System;
using UnityEngine;

public class SceneLoadingCanvasMgr : MonoBehaviour
{
    public GameObject canvasGo = null;

    public SceneLoadingWindow sceneLoading = null;

    private void Awake()
    {
        InitCanvas();
    }

    /// <summary>
    /// 获取场景加载canvas实体
    /// </summary>
    /// <returns></returns>
    public SceneLoadingWindow GetLoadingCanvas()
    {
        if (canvasGo == null)
        {
            InitCanvas();
        }

        if (!canvasGo.activeInHierarchy)
        {
            canvasGo.SetActive(true);
        }

        return sceneLoading;
    }

    /// <summary>
    /// 关闭场景加载
    /// </summary>
    public void CloseSceneLoading()
    {
        if (canvasGo.gameObject.activeInHierarchy)
        {
            // SceneLoadingWindow loadingWindow = GetLoadingCanvas();
            // loadingWindow.progressValue = 0;
            sceneLoading.Close();
        }
    }

    /// <summary>
    /// 获取loading界面是否开启的标示
    /// </summary>
    /// <returns></returns>
    public bool isLoadingCanvasOpen()
    {
        if (canvasGo != null)
        {
            return canvasGo.activeInHierarchy;
        }

        return false;
    }

    /// <summary>
    /// 初始化canvas
    /// </summary>
    public void InitCanvas()
    {
        GameObject go = Resources.Load<GameObject>("UI/DontDestroyOnLoad/LoadingCanvas");

        if (go != null)
        {
            canvasGo = Instantiate(go);
            //这个作为不销毁的Ui canvas节点
            DontDestroyOnLoad(canvasGo);

            sceneLoading = canvasGo.GetComponent<SceneLoadingWindow>();

            sceneLoading.InitUI();
        }
        else
        {
            Debug.LogError("Resouces Load 找不到UI/DontDestroyOnLoad/LoadingCanvas");
        }
    }

    /// <summary>
    /// 加载窗口进度条设置
    /// </summary>
    /// <param name="value"></param>
    public void SetLoadingPercentage(int value)
    {
        try
        {
            SceneLoadingWindow loadingWindow = GetLoadingCanvas();
            if (!loadingWindow.gameObject.activeInHierarchy)
            {
                Debug.LogError("场景加载窗口再次开启");
                loadingWindow.gameObject.SetActive(true);
            }

            if (loadingWindow != null)
            {
                if (value != 0 && value < loadingWindow.progressValue)
                {
                    Exception e = new Exception("try set value to lower precent!");
                    Debug.LogError("进度设置问题：value=" + value + "|loadingWindow.ProgressValue=" + loadingWindow.progressValue + e.StackTrace + "|" + e.ToString());
                }
                else
                {
                    if (value == 0 || value > loadingWindow.progressValue)
                    {
                        loadingWindow.progressValue = value;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("设置进度条出错 ： " + e.ToString() + "|stacktrace : " + e.StackTrace);
        }
    }
}
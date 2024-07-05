using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    // Time.realtimeSinceStartup: 指的是我们当前从启动开始到现在运行的时间，单位(s)
    private readonly float _timeDelta = 0.5f; // 固定的一个时间间隔
    private float _prevTime = 0.0f; // 上一次统计FPS的时间;
    private float _fps = 0.0f; // 计算出来的FPS的值;
    private int _iFrames = 0; // 累计我们刷新的帧数;
    private GUIStyle _style; // GUI显示;

    void Awake()
    {
        // 假设CPU 100% 工作的状态下FPS 300，
        // 当你设置了这个以后，他就维持在60FPS左右，不会继续冲高;
        // -1, 游戏引擎就会不段的刷新我们的画面，有多高，刷多高; 60FPS左右;
        //Application.targetFrameRate = 60;
    }

    void Start()
    {
        this._prevTime = Time.realtimeSinceStartup;
        this._style = new GUIStyle();
        this._style.fontSize = 15;
        this._style.normal.textColor = new Color(255, 255, 255);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height - 20, 200, 200), "FPS:" + this._fps.ToString("f2"), this._style);
    }

    void Update()
    {
        this._iFrames++;

        if (Time.realtimeSinceStartup >= this._prevTime + _timeDelta)
        {
            this._fps = ((float)this._iFrames) / (Time.realtimeSinceStartup - this._prevTime);
            this._prevTime = Time.realtimeSinceStartup;
            this._iFrames = 0; // 重新累积我们的FPS
        }
    }
}
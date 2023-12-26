using System;
using UnityEngine;

/// <summary>
/// 序列帧动画
/// https://blog.csdn.net/qq_36374904/article/details/103064687
/// </summary>
public class FrameAnimatorPlayScript : MonoBehaviour
{
    /// <summary>
    /// 序列帧帧数
    /// </summary>
    private int _framesCount;
    /// <summary>
    /// 序列帧名字前半部分  例如： image_1.jpg中的   image_为前半部分
    /// </summary>
    public string _framesName;
    /// <summary>
    /// resource文件夹下的文件夹路径 例如：Resources/Test/Sprites  则直接赋值  Test/Sprites   序列帧放在Sprites文件夹下就好
    /// </summary>
    public string _folderPath;
    /// <summary>
    /// 序列帧精灵体数组
    /// </summary>
    private Sprite[] frames = null;
    /// <summary>
    /// 帧率
    /// </summary>
    public float Framerate { get { return framerate; } set { framerate = value; } }
    [Range(20, 60)]
    [SerializeField] private float framerate = 20.0f;
    /// <summary>
    /// 是否忽略timeScale
    /// </summary>
    public bool IgnoreTimeScale { get { return ignoreTimeScale; } set { ignoreTimeScale = value; } }
    [SerializeField] private bool ignoreTimeScale = true;
    /// <summary>
    /// 是否循环
    /// </summary>
    [SerializeField] private bool loop = true;
    public bool Loop { get { return loop; } set { loop = value; } }
    //目标Image组件
    private Image image;
    //目标SpriteRenderer组件
    private SpriteRenderer spriteRenderer;
    //当前帧索引
    private int FrameIndex = 0;
    //下一次更新时间
    private float timer = 0.0f;
    /// <summary>
    /// 是否倒放
    /// </summary>
    public bool IsRewind { get; private set; }
    /// <summary>
    /// 播放状态
    /// </summary>
    public bool IsPlay { get; private set; }


    /// <summary>
    /// 倒放
    /// </summary>
    public void PlayBackward()
    {
        //循环状态下先把循环关掉，倒放不能循环
        if (loop) loop = false;
        IsRewind = true;
        IsPlay = true;
    }

    /// <summary>
    /// 重设动画
    /// </summary>
    public void Reset()
    {
        FrameIndex = IsRewind ? _framesCount - 1 : 0;
    }

    /// <summary>
    /// 从停止的位置播放动画
    /// </summary>
    public void PlayForward()
    {
        IsRewind = false;
        IsPlay = true;
    }

    /// <summary>
    /// 暂停动画
    /// </summary>
    public void Pause() { IsPlay = false; }

    /// <summary>
    /// 停止动画，将位置设为初始位置
    /// </summary>
    public void Stop()
    {
        Pause();
        Reset();
    }

    //获取序列帧
    void Start()
    {
        image = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (image == null && spriteRenderer == null)
        {
            Debug.LogError("没找到组件", gameObject);
        }
        //读序列帧
        ReadFrames();
    }


    /// <summary>
    /// 动态读取序列帧
    /// </summary>
    private void ReadFrames()
    {
        //文件夹中的图片数
        _framesCount = GetCountInFolder(Application.dataPath + "/Resources/" + _folderPath);
        //初始化精灵体数组
        frames = new Sprite[_framesCount];
        for (int i = 0; i < frames.Length; i++)
            frames[i] = Resources.Load<Sprite>(_folderPath + "/" + _framesName + i) as Sprite;
    }


    /// <summary>
    /// 获取文件夹内文件数（不包括meta文件）
    /// </summary>
    /// <param name="_path">目标路径</param>
    /// <returns>文件数</returns>
    private int GetCountInFolder(string _path)
    {
        int _folderNumberSum = 0;
        string[] fileList = System.IO.Directory.GetFileSystemEntries(_path);
        foreach (string item in fileList)
        {
            if (!item.Contains(".meta"))
                _folderNumberSum++;
        }
        return _folderNumberSum;
    }


    void Update()
    {
        //帧数据无效，禁用脚本
        if (frames == null || frames.Length == 0 || !IsPlay) return;
        //帧率有效     控制帧率
        if (framerate != 0)
        {
            //获取当前时间
            float time = ignoreTimeScale ? Time.unscaledTime : Time.time;
            //计算帧间隔时间
            float interval = Mathf.Abs(1.0f / framerate);
            //满足更新条件，执行更新操作
            if (time - timer > interval)
            {
                //执行更新操作
                DoUpdate();
            }
        }
        else Debug.LogError("帧率必须大于0");
    }

    //具体更新操作
    private void DoUpdate()
    {
        //是否打开循环
        if (!loop)
        {
            FrameIndex = Mathf.Clamp(FrameIndex, 0, frames.Length - 1);
            //正向播放判断||倒放判断
            if ((!IsRewind && FrameIndex == (frames.Length - 1)) || (IsRewind && FrameIndex == 0))
            {
                IsPlay = false;
                return;
            }
        }
        //帧数赋值，更新图片
        if (image != null) image.sprite = frames[FrameIndex];
        if (spriteRenderer != null) spriteRenderer.sprite = frames[FrameIndex];
        if (!IsRewind) FrameIndex++;//正向播放索引自增
        else
        {
            FrameIndex--;//倒放，索引自减
            if (FrameIndex < 0) FrameIndex = 0;
        }
        FrameIndex = FrameIndex % frames.Length;
        //设置计时器为当前时间
        timer = ignoreTimeScale ? Time.unscaledTime : Time.time;
    }
}

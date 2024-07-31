using System;
using UnityEngine;

/// <summary>
/// 自动删除
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    [HideInInspector]
    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event Action OnDestroy;

    private void Start()
    {
        this.startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - this.startTime > this.destroyTime)
        {
            this.DestroySelf();
        }
    }

    private void DestroySelf()
    {
        if (!this.destroyed)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            if (this.OnDestroy != null)
            {
                this.OnDestroy();
            }
            this.destroyed = true;
        }
    }

    [SerializeField]
    public float destroyTime = 2f;

    private float startTime;

    private bool destroyed;
}
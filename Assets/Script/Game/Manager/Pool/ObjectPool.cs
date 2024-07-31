using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
public class ObjectPool
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <param name="size"></param>
    /// <param name="maxSize"></param>
    /// <returns></returns>
    public IEnumerator Init(GameObject prefab, GameObject parent, int size, int maxSize)
    {
        this.queue = new Queue<GameObject>();
        this.prefab = prefab;
        this.parent = parent;
        this.maxSize = maxSize;
        if (prefab != null)
        {
            for (int i = 0; i < size; i++)
            {
                GameObject temp = this.InsertObject();
                temp.SetActive(true);
                temp.transform.position = new Vector3(-1000f, 0f, 0f);
                yield return null;
                temp.SetActive(false);
            }
        }

        yield break;
    }

    /// <summary>
    /// 获取物体
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        if (this.queue.Count == 0)
        {
            return this.InsertObject();
        }

        if (this.queue.Peek() == null)
        {
            this.queue.Dequeue();
            return this.InsertObject();
        }

        if (this.queue.Peek().activeSelf && this.queue.Count < this.maxSize)
        {
            return this.InsertObject();
        }

        GameObject gameObject = this.queue.Dequeue();
        this.queue.Enqueue(gameObject);
        if (this.queue.Count >= this.maxSize)
        {
            gameObject.SetActive(false);
        }

        return gameObject;
    }

    /// <summary>
    /// 插入对象
    /// </summary>
    /// <returns></returns>
    public GameObject InsertObject()
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab, Vector3.zero, Quaternion.identity);
        gameObject.name = this.prefab.name;
        if (this.parent != null)
        {
            gameObject.transform.parent = this.parent.transform;
        }

        gameObject.SetActive(false);
        this.queue.Enqueue(gameObject);
        return gameObject;
    }

    public Queue<GameObject> queue;

    public GameObject prefab;

    public GameObject parent;

    public int maxSize = 10;
}
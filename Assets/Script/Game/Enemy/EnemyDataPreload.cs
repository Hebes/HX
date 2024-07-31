using System;
using System.Collections.Generic;
using Framework.Core;
using LitJson;
using UnityEngine;

/// <summary>
/// 敌方数据预加载
/// </summary>
public class EnemyDataPreload : SingletonMono<EnemyDataPreload>
{
    private void Awake()
    {
        this.DataInit(this.attackData, this.attack);
        "攻击数据已加载".Log();
        this.DataInit(this.hurtData, this.hurt);
        "损伤数据已加载".Log();
        this.VibrationData = this.ParseJson<float[][]>("VibrationConfig");
    }

    private void DataInit(EnemyDataPreload.EnemyData[] data, IDictionary<EnemyType, JsonData1> dict)
    {
        for (var i = 0; i < data.Length; i++)
        {
            dict.Add(data[i].type, new JsonData1());//JsonMapper.ToObject(data[i].text.text)
        }
    }

    private T ParseJson<T>(string name)
    {
        return Asset.DeserializeFromFile<T>("Conf/", name);
    }

    [ContextMenu("load")]
    private void Load()
    {
        string[] names = Enum.GetNames(typeof(EnemyType));
        this.attackData = new EnemyDataPreload.EnemyData[names.Length];
        this.hurtData = new EnemyDataPreload.EnemyData[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            this.attackData[i] = new EnemyDataPreload.EnemyData();
            this.hurtData[i] = new EnemyDataPreload.EnemyData();
            this.attackData[i].type = names[i].ToEnum<EnemyType>(false);
            this.hurtData[i].type = names[i].ToEnum<EnemyType>(false);
        }
    }

    [SerializeField]
    private EnemyDataPreload.EnemyData[] attackData;

    [SerializeField]
    private EnemyDataPreload.EnemyData[] hurtData;

    public Dictionary<EnemyType, JsonData1> attack = new Dictionary<EnemyType, JsonData1>();

    public Dictionary<EnemyType, JsonData1> hurt = new Dictionary<EnemyType, JsonData1>();

    public float[][] VibrationData;

    [Serializable]
    public class EnemyData
    {
        public TextAsset text;

        public EnemyType type;
    }
}
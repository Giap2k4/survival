using System;
using UnityEngine;
using Newtonsoft.Json;

public abstract class DataPlayer<T> : DataPlayerBase
{
    [JsonProperty("database")]
    public T database;

    public void InitData()
    {
        T data = Activator.CreateInstance<T>();
        database = data;
        SetDataDefault();
        Save();
    }

    public override void Load()
    {
        string type = GetType().Name;

        if (PlayerPrefs.HasKey(type)) Save();

        string dataJson = PlayerPrefs.GetString(type);
        database = JsonConvert.DeserializeObject<T>(dataJson);

        if (database == null) InitData();
    }

    public override void Save()
    {
        string type = GetType().Name;
        string dataJson = JsonConvert.SerializeObject(database);
        PlayerPrefs.SetString(type, dataJson);
        PlayerPrefs.Save();
    }

    public virtual void SetDataDefault() { }

}

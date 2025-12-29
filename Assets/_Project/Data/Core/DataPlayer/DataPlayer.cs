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
        //if (database == null)
        //{
        //    Debug.Log("Data null: " + GetType().Name);
        //    SetDataDefault();
        //    Save();
        //}
    }

    public override void Load()
    {
        string type = GetType().Name;

        if (!PlayerPrefs.HasKey(type))
        {
            InitData();
            //Save();
            return;
        }

        string dataJson = PlayerPrefs.GetString(type);
        //Debug.Log("Manager: " + dataJson);
        database = JsonConvert.DeserializeObject<T>(dataJson);

        if (database == null)
        {
            Debug.Log("Data null: " + GetType().Name);
            InitData();
        }
    }

    public override void Save()
    {
        string type = GetType().Name;
        string dataJson = JsonConvert.SerializeObject(database);
        PlayerPrefs.SetString(type, dataJson);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Set data mặc định (k cần gọi hàm lưu)
    /// </summary>
    public virtual void SetDataDefault() { }

}

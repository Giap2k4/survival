using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public static Dictionary<string, Object> cacheDataConfig = new Dictionary<string, Object>();

    public static T Get<T> () where T : Object
    {
        var key = typeof(T).Name;
        if (cacheDataConfig.TryGetValue(key, out var value))
        {
            return (T)value;
        }

        var data = Resources.Load<T>(key);
        cacheDataConfig.Add(key, data);
        return data;
    }

    public static HeroCollection Hero => Get<HeroCollection>();
    public static ResourceCollection Resource => Get<ResourceCollection>();
}

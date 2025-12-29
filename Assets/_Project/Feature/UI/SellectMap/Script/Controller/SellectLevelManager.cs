using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellectLevelManager : DataPlayer<SellectLevelData>
{
    public static int GetLevelMaxUser() => PlayerDataManager.SellectLevel.database.levelMaxUser;
    public static int GetLevelCurrent() => PlayerDataManager.SellectLevel.database.levelCurrent;
    public static int GetIdMap() => PlayerDataManager.SellectLevel.database.idMap;
    public static void SetLevelCurrent(int level)
    {
        PlayerDataManager.SellectLevel.database.levelCurrent = level;
        PlayerDataManager.SellectLevel.Save();

        var key = "SellectLevelManager"; // ví dụ
        var json = PlayerPrefs.GetString(key, "(missing)");
        Debug.Log(json);
    }

    public static void SetLevelMaxUser(int level)
    {
        PlayerDataManager.SellectLevel.database.levelMaxUser = level;
        PlayerDataManager.SellectLevel.Save();
    }

    public static void SetIdMap(int id)
    {
        PlayerDataManager.SellectLevel.database.idMap = id;
        PlayerDataManager.SellectLevel.Save();

        var key = "SellectLevelManager"; // ví dụ
        var json = PlayerPrefs.GetString(key, "(missing)");
        Debug.Log(json);
    }

    public override void SetDataDefault()
    {
        PlayerDataManager.SellectLevel.database.levelMaxUser = 1;
        PlayerDataManager.SellectLevel.database.levelCurrent = 1;
        PlayerDataManager.SellectLevel.database.idMap = 1;
        PlayerDataManager.SellectLevel.Save();
    }

    public static string GetNameMap(int idMap)
    {
        return DataManager.SellectMap.GetById(idMap).nameMap;
    }
}

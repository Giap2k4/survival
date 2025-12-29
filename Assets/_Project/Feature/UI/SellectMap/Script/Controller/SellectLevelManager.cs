using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellectLevelManager : DataPlayer<SellectLevelData>
{
    public static int GetLevelMaxUser() => PlayerDataManager.SellectLevel.database.levelMaxUser;
    public static int GetLevelCurrent() => PlayerDataManager.SellectLevel.database.levelCurrent;
    public static int GetIdMap() => PlayerDataManager.SellectLevel.database.idMap;
    public static int GetLevelTotal() => PlayerDataManager.SellectLevel.database.levelTotal;
    public static void SetLevelCurrent(int level)
    {
        PlayerDataManager.SellectLevel.database.levelCurrent = level;
        PlayerDataManager.SellectLevel.Save();
    }

    /// <summary>
    /// Set level mà user đã đạt đến
    /// </summary>
    /// <param name="level"></param>
    public static void SetLevelMaxUser(int level)
    {
        PlayerDataManager.SellectLevel.database.levelMaxUser = level;
        PlayerDataManager.SellectLevel.Save();
    }

    public static void SetIdMap(int id)
    {
        PlayerDataManager.SellectLevel.database.idMap = id;
        PlayerDataManager.SellectLevel.Save();
    }

    public override void SetDataDefault()
    {
        PlayerDataManager.SellectLevel.database.levelMaxUser = 1;
        PlayerDataManager.SellectLevel.database.levelCurrent = 1;
        PlayerDataManager.SellectLevel.database.idMap = 1;
        PlayerDataManager.SellectLevel.database.levelTotal = 20;
        PlayerDataManager.SellectLevel.Save();
    }

    public static string GetNameMap(int idMap)
    {
        return DataManager.SellectMap.GetById(idMap).nameMap;
    }
}

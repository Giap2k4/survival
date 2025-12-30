using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenDayLoginManager : DataPlayer<SevenDayLoginData>
{
    public static int GetSevenDayProgress() => PlayerDataManager.SevenDayLogin.database.sevenDayProgress;
    public static long GetTimeLast() => PlayerDataManager.SevenDayLogin.database.timeLastCollected;

    public static void SetDayProgress(int progress)
    {
        PlayerDataManager.SevenDayLogin.database.sevenDayProgress = progress;
        PlayerDataManager.SevenDayLogin.Save();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager
{
    private static HeroManager _hero;

    public static HeroManager Hero 
    {
        get
        {
            if (_hero == null) _hero = DataPlayerBase.GetModule<HeroManager>();
            return _hero;
        }

        set => _hero = value;
    }

    private static ResourceManager _resource;

    public static ResourceManager Resource
    {
        get
        {
            if (_resource == null) _resource = DataPlayerBase.GetModule<ResourceManager>();
            return _resource;
        }

        set => _resource = value;
    }

    private static SellectLevelManager _sellectLevel;
    public static SellectLevelManager SellectLevel
    {
        get
        {
            if (_sellectLevel == null) _sellectLevel = DataPlayerBase.GetModule<SellectLevelManager>();
            return _sellectLevel;
        }

        set => _sellectLevel = value;
    }

    private static GrowthFundManager _growthFund;
    public static GrowthFundManager GrowthFund
    {
        get
        {
            if (_growthFund == null) _growthFund = DataPlayerBase.GetModule<GrowthFundManager>();
            return _growthFund;
        }

        set => _growthFund = value;
    }

    private static SevenDayLoginManager _sevenDayLogin;
    public static SevenDayLoginManager SevenDayLogin
    {
        get
        {
            if (_sevenDayLogin == null) _sevenDayLogin = DataPlayerBase.GetModule<SevenDayLoginManager>();
            return _sevenDayLogin;
        }

        set => _sevenDayLogin = value;
    }
}

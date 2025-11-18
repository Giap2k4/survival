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
}

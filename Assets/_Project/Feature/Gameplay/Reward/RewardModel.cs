using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardModel
{
    public EnumBase.BattleMode battleMode;
    public RewardBattleModeDetails[] details;
}

[Serializable]
public class RewardBattleModeDetails
{
    public int idMap;
    public RewardTimeGame[] details;
}

[Serializable]
public class RewardTimeGame
{
    public float timeSurvival;
    public PackageReward[] rewards;
}

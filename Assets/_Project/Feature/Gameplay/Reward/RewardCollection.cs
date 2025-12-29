using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardCollection : ScriptableObject
{
    public RewardModel[] dataGroups;

    public RewardTimeGame[] GetRewardByIdMap(int id)
    {
        var obj = dataGroups.First(x => x.battleMode == BattleManager.BattleMode);
        var reward = obj.details.First(x => x.idMap == id);

        return reward.details;
    }

    public List<RewardTimeGame> Clone(List<RewardTimeGame> list)
    {
        if (list == null) return null;

        var result = new List<RewardTimeGame>(list.Count);

        for (int i = 0; i < list.Count; i++)
        {
            var src = list[i];

            var clone = new RewardTimeGame
            {
                timeSurvival = src.timeSurvival,
                rewards = CloneRewards(src.rewards)
            };

            result.Add(clone);
        }

        return result;
    }

    private PackageReward[] CloneRewards(PackageReward[] rewards)
    {
        if (rewards == null) return null;

        var arr = new PackageReward[rewards.Length];

        for (int i = 0; i < rewards.Length; i++)
        {
            var r = rewards[i];
            arr[i] = new PackageReward
            {
                resType = r.resType,
                resId = r.resId,
                resQuantity = r.resQuantity
            };
        }

        return arr;
    }


}

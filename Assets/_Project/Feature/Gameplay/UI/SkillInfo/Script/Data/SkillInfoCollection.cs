using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillInfoCollection : ScriptableObject
{
    public SkillInfoModel[] dataGroups;

    public SkillInfoModel GetSkillById(int id) => dataGroups.FirstOrDefault(x => x.id == id);

    /// <summary>
    /// list các id skill dùng chung
    /// </summary>
    /// <returns></returns>
    public int[] GetSharedSkill()
    {
        var value = dataGroups.Where(x => x.sharedSkill).Select(x => x.id).ToArray();
        //var hero = BattleController.instance.GetPlayer().GetComponent<PlayerController>();
        //var skillInfo = GetSkillById(hero.GetIdDefaultSkill());
        //array.Add(skillInfo);

        return value;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillInfoCollection : ScriptableObject
{
    public SkillInfoModel[] dataGroups;

    public SkillInfoModel GetSkillById(int id) => dataGroups.FirstOrDefault(x => x.id == id);

    public List<SkillInfoModel> GetSharedSkill()
    {
        var array = dataGroups.Where(x => x.sharedSkill).ToList();
        var hero = BattleController.instance.GetPlayer().GetComponent<PlayerController>();
        var skillInfo = GetSkillById(hero.GetIdDefaultSkill());
        array.Add(skillInfo);

        return array;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SkillCollection : ScriptableObject
{
    public SkillModel dataGroups;

    public SkillModel GetSkillByName(string nameSkill)
    {
        return Resources.Load<SkillCollection>(nameSkill).dataGroups;
    }
}

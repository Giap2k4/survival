using System;
using Unity.VisualScripting;

[Serializable]
public class SkillModel
{
    public int id;
    public string nameSkill;
    public SkillDetails[] details;
}

[Serializable]
public class SkillDetails
{
    public int level;
    public MechanicTypes[] mechanicType;
}

[Serializable]
public class MechanicTypes
{
    public EnumBase.MechanicTypes mechanicTypes;
    public string values;
}
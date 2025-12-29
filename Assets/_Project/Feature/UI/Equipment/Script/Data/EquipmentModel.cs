using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentModel
{
    public int id;
    public string name;
    public int quantityUpgrade;
    public int quantityLevelUp;
    public EquipmentDetail[] details;
}

[Serializable]
public class EquipmentDetail
{
    public EnumBase.RPGStatType statType;
    public string value;
    public string description;
}

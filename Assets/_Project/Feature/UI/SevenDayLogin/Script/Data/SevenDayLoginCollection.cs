using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenDayLoginCollection : ScriptableObject
{
    public SevenDayLoginModel[] dataGroups;

    public SevenDayLoginModel[] GetAll() => dataGroups;
}

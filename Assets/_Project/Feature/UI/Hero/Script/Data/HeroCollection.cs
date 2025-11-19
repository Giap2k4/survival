using System;
using UnityEngine;

[Serializable]
public class HeroCollection : ScriptableObject
{
    public HeroModel[] dataGroups;

    public HeroModel[] GetAll()
    {
        return dataGroups;
    }
}

using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class HeroCollection : ScriptableObject
{
    public HeroModel[] dataGroups;

    public HeroModel[] GetAll()
    {
        return dataGroups;
    }

    public HeroModel GetHeroById(int id) => dataGroups.First(x => x.id == id);
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrowthFundCollection : ScriptableObject
{
    public GrowthFundModel[] dataGroups;

    public GrowthFundModel GetById(int id) => dataGroups.FirstOrDefault(x => x.id == id);
    public GrowthFundModel[] GetAll() => dataGroups;
}

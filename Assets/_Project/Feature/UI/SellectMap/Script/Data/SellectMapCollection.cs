using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SellectMapCollection : ScriptableObject
{
    public SellectMapModel[] dataGroups;

    public SellectMapModel GetById(int id) => dataGroups.FirstOrDefault(x => x.id == id);
    public SellectMapModel[] GetAll() => dataGroups;
}

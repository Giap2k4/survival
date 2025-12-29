using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentCollection : ScriptableObject
{
    public EquipmentModel[] dataGroups;

    public EquipmentModel GetById(int id) => dataGroups.FirstOrDefault(x => x.id == id);
}

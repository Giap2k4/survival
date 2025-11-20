using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceCollection : ScriptableObject
{
    public ResourceModel[] dataGroups;

    /// <summary>
    /// Default Data
    /// </summary>
    /// <returns></returns>
    public ResourceModel[] GetAll() => dataGroups;
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PoolingManager
{
    public static List<GameObject> listDamageText = new List<GameObject>();
    public static List<GameObject> listProjectile = new List<GameObject>();
    public static List<GameObject> listEnemy = new List<GameObject>();

    public static GameObject GetProjectilePooling(string nameProjectile)
    {
        if (listProjectile.Count > 0)
        {
            var obj = listProjectile.FirstOrDefault(x => x.name == nameProjectile);
            listProjectile.Remove(obj);
            return obj;
        }
        return null;
    }

    public static void AddProjectilePooling(GameObject obj)
    {
        if (!listProjectile.Contains(obj))
        {
            listProjectile.Add(obj);
        }
    }
}

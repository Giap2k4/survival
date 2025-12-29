using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PoolingManager
{
    private static List<GameObject> _listDamageText = new List<GameObject>();
    private static List<GameObject> _listProjectile = new List<GameObject>();
    private static List<GameObject> _enemyActive = new List<GameObject>();
    private static List<GameObject> _enemyDisable = new List<GameObject>();
    private static List<GameObject> _exp = new List<GameObject>();

    public static GameObject GetProjectile(string nameProjectile)
    {
        if (_listProjectile.Count > 0)
        {
            var obj = _listProjectile.FirstOrDefault(x => x.name == nameProjectile);
            _listProjectile.Remove(obj);
            return obj;
        }
        return null;
    }

    public static void AddProjectile(GameObject obj)
    {
        if (!_listProjectile.Contains(obj))
        {
            _listProjectile.Add(obj);
        }
    }

    public static void AddEnemyActive(GameObject obj)
    {
        if (!_enemyActive.Contains(obj)) _enemyActive.Add(obj);
    }

    /// <summary>
    /// Số lượng enemy trên màn hình
    /// </summary>
    /// <returns></returns>
    public static int QuantityEnemyActive() => _enemyActive.Count;

    /// <summary>
    /// Add enemy vào pooling
    /// </summary>
    /// <param name="obj"></param>
    public static void AddEnemyDisable(GameObject obj)
    {
        if (!_enemyDisable.Contains(obj)) _enemyDisable.Add(obj);
        if (_enemyActive.Contains(obj)) _enemyActive.Remove(obj);
    }

    /// <summary>
    /// Lấy 1 enemy trong pooling (lấy ra thì chắc chắn dùng nên sẽ add vào Active luôn)
    /// </summary>
    /// <param name="nameProjectile"></param>
    /// <returns></returns>
    public static GameObject GetEnemyDisable(string nameProjectile)
    {
        if (_enemyDisable.Count > 0)
        {
            var obj = _enemyDisable.FirstOrDefault(x => x.name == nameProjectile);
            _enemyDisable.Remove(obj);
            _enemyActive.Add(obj);
            return obj;
        }
        return null;
    }

    public static void AddExpPooling(GameObject obj)
    {
        if (!_exp.Contains(obj)) _exp.Add(obj);
    }

    public static GameObject GetExp(string nameExp)
    {
        if (_exp.Count > 0)
        {
            var obj = _exp.FirstOrDefault(x => x.name == nameExp);
            _exp.Remove(obj);
            return obj;
        }
        return null;
    }

    public static void Clear()
    {
        _listDamageText.Clear();
        _listProjectile.Clear();
        _enemyActive.Clear();
        _enemyDisable.Clear();
        _exp.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : SingletonTemporary<UpdateManager>
{
    private HashSet<IUpdateManager> _table = new HashSet<IUpdateManager>();
    private List<IUpdateManager> _toRemove = new List<IUpdateManager>();

    public void Register(IUpdateManager obj)
    {
        _table.Add(obj);
    }
    public void UnRegister(IUpdateManager obj)
    {
        _toRemove.Add(obj);
    }

    private void Update()
    {
        foreach (var item in _toRemove)
        {
            _table.Remove(item);
        }
        _toRemove.Clear();

        foreach (IUpdateManager obj in _table)
        {
            obj.UpdateMe(); 
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : SingletonTemporary<UpdateManager>
{
    private HashSet<IUpdateManager> _table = new HashSet<IUpdateManager>();

    public void Register(IUpdateManager obj) => _table.Add(obj); 
    public void UnRegister(IUpdateManager obj) => _table.Remove(obj);

    private void Update()
    {
        foreach (IUpdateManager obj in _table)
        {
            obj.UpdateMe();
        }
    }
}

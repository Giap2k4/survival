using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppStartup : Singleton<AppStartup>
{
    protected override void Awake()
    {
        base.Awake();
        DataPlayerBase.Init();
    }
}  

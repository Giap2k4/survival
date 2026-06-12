using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DurationObject : MonoBehaviour, IUpdateManager
{
    [SerializeField]
    protected float duration;

    protected float timeCount;

    public void UpdateMe ()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= duration)
        {

        }
    }
}

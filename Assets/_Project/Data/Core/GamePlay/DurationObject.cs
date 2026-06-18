using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DurationObject : MonoBehaviour
{
    [SerializeField]
    protected float duration;

    protected float timeCount;

    protected void OnEnable()
    {
        timeCount = 0;
    }

    public void Update ()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= duration)
        {
            gameObject.SetActive(false);
        }
    }
}

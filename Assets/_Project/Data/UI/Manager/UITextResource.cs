using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextResource : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI txt;

    [SerializeField]
    protected int resIdObj;

    [SerializeField]
    protected EnumBase.ResourcesType resType;

    protected void OnEnable()
    {
        ResourceManager.eventChangeRes += UpdateUI;
    }

    protected void OnDisable()
    {
        ResourceManager.eventChangeRes -= UpdateUI;
    }

    protected void UpdateUI(EnumBase.ResourcesType type ,int resId)
    {
        if (resType == type && resId == resIdObj)
        {
            txt.text = ResourceManager.GetQuantityRes(type, resId).ToString();
        }
    }
}

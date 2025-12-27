using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemResourcesController : MonoBehaviour
{
    [SerializeField]
    protected Image image;

    [SerializeField]
    protected TextMeshProUGUI txtQuantity;

    public void SetData(PackageReward res)
    {
        switch (res.resType)
        {
            case EnumBase.ResourcesType.Money:
                image.sprite = Resources.Load<Sprite>("Money/" + res.resId);
                txtQuantity.text = res.resQuantity.ToString();
                break;
            case EnumBase.ResourcesType.Hero:
                image.sprite = Resources.Load<Sprite>("Hero/" + res.resId);
                txtQuantity.text = res.resQuantity.ToString();
                break;
        }
        
    }
}

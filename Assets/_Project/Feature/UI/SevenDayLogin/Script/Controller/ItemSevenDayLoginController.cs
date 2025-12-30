using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSevenDayLoginController : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI txtDay;

    [SerializeField]
    protected TextMeshProUGUI txtQuantityRes;

    [SerializeField]
    protected Image icon;

    [SerializeField]
    protected GameObject glow;

    [SerializeField]
    protected Button btnCollect;

    [SerializeField]
    protected GameObject objlock;

    [SerializeField]
    protected GameObject txtCollected;

    protected PackageReward res;

    protected void Start()
    {
        btnCollect.onClick.AddListener(ClaimReward);
    }

    public void SetData(SevenDayLoginModel model)
    {
        res = model.resource;
        txtDay.text = "Ngày " + model.day.ToString();
        txtQuantityRes.text = model.resource.resQuantity.ToString();
        
        switch(model.resource.resType)
        {
            case EnumBase.ResourcesType.Hero:
                icon.sprite = Resources.Load<Sprite>("Hero/" + model.resource.resId);
                break;

            case EnumBase.ResourcesType.Money:
                icon.sprite = Resources.Load<Sprite>("Money/" + model.resource.resId);
                break;
        }

        var dayProgress = SevenDayLoginManager.GetSevenDayProgress();
        CheckCanClaim(dayProgress, model.day);
    }

    protected void CheckCanClaim(int dayProgress, int dayCurrent)
    {
        // đã claim
        if (dayCurrent <= dayProgress) Claimed();
        // check xem qua ngày hay chưa
        else if (dayCurrent == dayProgress + 1) CheckNextDay();
        else LockReward();
    }

    protected void Claimed()
    {
        txtCollected.SetActive(true);
        glow.SetActive(false);
        objlock.SetActive(true);
    }

    protected void CheckNextDay()
    {
        // nếu qua ngày rồi thì cho claim, chưa thì giống lock
    }

    protected void LockReward()
    {
        glow.SetActive(false);
        objlock.SetActive(true);
        txtCollected.SetActive(false);
    }

    public void ClaimReward()
    {
        ResourceManager.ResourcesCollect(res);
        var day = SevenDayLoginManager.GetSevenDayProgress();
        SevenDayLoginManager.SetDayProgress(day + 1);
        Claimed();
    }
}

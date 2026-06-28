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

    [SerializeField]
    protected LayoutElement element;

    [SerializeField]
    protected RectTransform rect;

    [SerializeField]
    protected Sprite spritePurple;

    [SerializeField]
    protected Image img;

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
        if (model.day == 7) CheckSevenDay(model.day);
    }

    protected void CheckCanClaim(int dayProgress, int dayCurrent)
    {
        // đã claim
        if (dayCurrent <= dayProgress) Claimed();
        // check xem qua ngày hay chưa
        else if (dayCurrent == dayProgress + 1) CheckNextDay();
        else if (dayProgress == 0 && dayCurrent == 1) { }
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

    protected void CheckSevenDay (int day)
    {
        element.ignoreLayout = true;
        // Anchor
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);

        // Pivot
        rect.pivot = new Vector2(0.5f, 0.5f);

        // Width = 810 (giữ nguyên Height)
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 810f);

        // Pos Y = 490 (giữ nguyên X)
        Vector2 pos = rect.anchoredPosition;
        pos.y = 490f;
        rect.anchoredPosition = pos;

        img.sprite = spritePurple;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGrowthFundController : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI txtLevel;

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
    protected int level;

    public void SetData(GrowthFundModel model)
    {
        level = model.levelReward;
        res = model.resource;
        txtLevel.text = model.levelReward.ToString();
        txtQuantityRes.text = model.resource.resQuantity.ToString();
        icon.sprite = Resources.Load<Sprite>("Money/" + model.resource.resId);

        var levelMaxUser = SellectLevelManager.GetLevelMaxUser();

        // check xem đã nhận thưởng hay chưa nữa
        if (model.levelReward <= levelMaxUser) CollectReward();
        else LockReward();
    }

    protected void CollectReward()
    {
        objlock.SetActive(false);

        if (GrowthFundManager.CheckClaimed(level)) 
        {
            txtCollected.SetActive(true); 
            glow.SetActive(false);
            objlock.SetActive(true);
        }
        else
        {
            txtCollected.SetActive(false);
            glow.SetActive(true);
        }
    }

    protected void LockReward()
    {
        glow.SetActive(false);
        objlock.SetActive(true);
        txtCollected.SetActive(false);
    }
}

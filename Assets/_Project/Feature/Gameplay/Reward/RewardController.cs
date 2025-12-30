using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : FeatureBaseController
{
    [Header("Cấu hình riêng")]
    [SerializeField]
    protected Button btnContinue;

    [SerializeField]
    protected GameObject itemRes;

    [SerializeField]
    protected Transform parentItem;

    [SerializeField]
    protected TextMeshProUGUI txtNoReward;

    [SerializeField]
    protected Canvas canvas;

    protected override void Start()
    {
        base.Start();
        btnContinue.onClick.AddListener(BtnContinue);
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 201;
    }

    public void SetData(float timeSurvival)
    {
        var idMap = SellectLevelManager.GetLevelCurrent();
        var reward = DataManager.Reward.GetRewardByIdMap(idMap).ToList();
        var rewardClone = DataManager.Reward.Clone(reward);

        var rewardCollect = rewardClone.Where(x => x.timeSurvival <= timeSurvival).Select(x => x.rewards).ToArray();

        if (rewardCollect == null || rewardCollect.Length == 0)
        {
            txtNoReward.text = "Không có phần thưởng!";
            txtNoReward.gameObject.SetActive(true);
            return;
        }
        List<PackageReward> rewardList = new List<PackageReward>();
        foreach (var array in rewardCollect)
        {
            foreach (var item in array)
            {
                rewardList.Add(item);
            }
        }
        // lưu phần thưởng xuống db
        ResourceManager.ResourcesCollect(rewardList);

        foreach (var item in HandleListReward(rewardList))
        {
            GameObject obj = Instantiate(itemRes, parentItem);
            obj.GetComponent<ItemResourcesController>().SetData(item);
        }

        CheckLevelUp(rewardClone, timeSurvival);
    }

    protected void CheckLevelUp(List<RewardTimeGame> reward, float timeSurvival)
    {
        var levelCurrent = SellectLevelManager.GetLevelCurrent();
        var checkLevelUp = reward[reward.Count - 1];
        if (timeSurvival >= checkLevelUp.timeSurvival && levelCurrent > SellectLevelManager.GetLevelMaxUser())
        {
            SellectLevelManager.SetLevelMaxUser(levelCurrent);
            SellectLevelManager.SetLevelCurrent(levelCurrent + 1);
        }
    }

    public List<PackageReward> HandleListReward(List<PackageReward> list)
    {
        List<PackageReward> listReward = new List<PackageReward>();
        foreach (var item in list)
        {
            var obj = listReward.FirstOrDefault(x => x.resType == item.resType && x.resId == item.resId);
            if (obj == null)
            {
                listReward.Add(item);
                continue;
            }

            obj.resQuantity += item.resQuantity;
        }

        return listReward;
    }

    public void BtnContinue()
    {
        PoolingManager.Clear();
        SceneController.instance.ChangeScene(EnumBase.Scenes.HomeScene);
    }
}

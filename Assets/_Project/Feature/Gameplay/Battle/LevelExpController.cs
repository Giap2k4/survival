using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelExpController : SingletonTemporary<LevelExpController>
{
    [SerializeField]
    protected float levelExp; // level exp hiện tại

    [SerializeField]
    protected float expCurrent; // số lượng exp
    protected float levelExpStart; // level exp trước khi tăng level
    protected float numberSidebar;
    protected FormulaExpBattleModel model;

    [SerializeField]
    protected TextMeshProUGUI txtLevel;

    [SerializeField]
    protected Image fillImage;

    [SerializeField]
    protected Button btnTimeScale;

    [SerializeField]
    protected TextMeshProUGUI txtSpeedBattle;
    [SerializeField]
    protected bool checkTimeScale;
    [SerializeField]
    protected int tempTimeScale;
    [SerializeField]
    protected Button btnPause;

    protected float timeGame;
    protected float timeSurvival;

    [SerializeField]
    protected TextMeshProUGUI txtTime;

    [SerializeField]
    protected CanvasGroup canvasGroup;

    protected void Start()
    {
        StartCoroutine(Cooldown());

        tempTimeScale = 1;
        levelExp = 1;
        txtLevel.text = levelExp.ToString();
        expCurrent = 0;
        model = DataManager.FormulaExpBattle.GetByBattleMode(BattleManager.BattleMode);
        fillImage.fillAmount = 0f;
        btnTimeScale.onClick.AddListener(OnClick);
        btnPause.onClick.AddListener(OnClickPause);
        txtSpeedBattle.text = "x1";
    }

    IEnumerator Cooldown()
    {
        var item = SellectLevelManager.GetLevelCurrent();
        var valueTimeEndCsv = DataManager.SpawnEnemy.GetSpawnEnemyById(item).totalTime;
        var timeEnd = Time.time + valueTimeEndCsv;

    Start:
        timeGame = TimeManager.Cooldown(timeEnd);
        long seconds = Mathf.CeilToInt(timeGame);
        txtTime.text = TimeManager.FormatTime(seconds);

        if (timeGame <= 0)
        {
            EndGame();
            yield break;
        }

        yield return new WaitForSeconds(1f);
        timeSurvival++;
        goto Start;
    }

    /// <summary>
    /// Xử lý khi xong màn chơi
    /// </summary>
    protected virtual void EndGame()
    {
        // giết hết quái mới end game đó
        Debug.Log("EndGame");
    }

    public void AddExp(int value)
    {
        expCurrent += value;
        levelExpStart = levelExp;
        CaculaterExp();
    }

    protected void CaculaterExp()
    {
        var levelTemp = FormulaEvaluator.EvaluateLevel(expCurrent, model.valueBase, model.bonus);

        levelExp = levelTemp;
        var levelUp = levelExp - levelExpStart;
        HandleLevelUp(levelUp);
        levelExpStart = levelExp;
        txtLevel.text = levelExp.ToString();

        if (levelExp <= 0)
        {
            levelExp = 1;
            numberSidebar = expCurrent;
            float target1 = numberSidebar / 30;
            fillImage.fillAmount = target1;
            //fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, target1, Time.unscaledDeltaTime * 40);
            return;
        }
        numberSidebar = expCurrent - FormulaEvaluator.Evaluate(model.formula, model.valueBase, model.bonus, levelExp - 1);
        float target = numberSidebar / 30;
        fillImage.fillAmount = target;
        //fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, target, Time.unscaledDeltaTime * 40);
    }

    protected void HandleLevelUp(float numberLevel)
    {
        // numberLevel: tăng mấy level
        if (numberLevel <= 0) return;

        // xử lý tăng level
        Time.timeScale = 0;
        if (UIManager.instance.listFeatureOpen.TryGetValue(EnumBase.Feature.SkillInfo, out var obj))
        {
            obj.GetComponent<SkillInfoController>().SetQuantityLevel(numberLevel);
        }
        else
        {
            var item = UIManager.instance.OpenFeature(EnumBase.Feature.SkillInfo).GetComponent<SkillInfoController>();
            item.SetQuantityLevel(numberLevel);
        }
    }

    public void OnClick()
    {
        if (!checkTimeScale)
        {
            tempTimeScale = 2;
            Time.timeScale = tempTimeScale;
            checkTimeScale = true;
            txtSpeedBattle.text = "x2";
        }
        else
        {
            tempTimeScale = 1;
            Time.timeScale = tempTimeScale;
            checkTimeScale = false;
            txtSpeedBattle.text = "x1";
        }
    }

    public void TimeScale() => Time.timeScale = tempTimeScale;

    public void OnClickPause()
    {
        UIManager.instance.OpenFeature(EnumBase.Feature.PauseGame);
        Time.timeScale = 0;
    }

    public float GetTimeSurvival() => timeSurvival;
    public void SetCanvasGroup(bool block) => canvasGroup.blocksRaycasts = block;
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : SingletonTemporary<SkillManager>
{
    protected List<SkillBaseController> skill = new(); // các skill đã sở hữu trong lần chơi này
    protected Queue<SkillBaseController> queue = new(); // các skill được lên cấp
    protected List<int> skillRandom = new(); // key: id skill, value: level skill
    protected const int MAX_SKILL_OWNED = 5;

    protected void Start()
    {
        foreach (var item in DataManager.SkillInfo.GetSharedSkill())
        {
            skillRandom.Add(item);
        }
    }

    public List<SkillBaseController> GetAllSkill() => skill;
    public SkillBaseController AddSkillDefault(SkillBaseController skill)
    {
        this.skill.Add(skill);
        skillRandom.Add(skill.GetIdSkill());
        return skill;
    }

    /// <summary>
    /// Xử lý các skill lên level
    /// </summary>
    public void HanldeSkillLevelUP()
    {
        while (queue.Count > 0)
        {
            var skill = queue.Dequeue();
            skill.HandleLevelUP();

        }
    }

    public void SetOrAddSkill(int idSkill)
    {
        var item = skill.FirstOrDefault(x => x.GetIdSkill() == idSkill);
        if (item == null)
        {
            var prefab = Resources.Load<GameObject>("SkillController_" + idSkill);
            GameObject obj = Instantiate(prefab);
            obj.transform.position = Vector3.zero;
            skill.Add(obj.GetComponent<SkillBaseController>());
            obj.GetComponent<SkillBaseController>().HandleLevelUP();

            if (skill.Count >= MAX_SKILL_OWNED)
            {
                skillRandom.Clear();
                foreach (var s in skill)
                {
                    if (s.GetLevelCurrent() < 5)
                    {
                        skillRandom.Add(s.GetIdSkill());
                    }
                }
            }

            return;
        }

        item.SetLevelSkill();
        if (item.GetLevelCurrent() >= 5) skillRandom.Remove(item.GetIdSkill());
        if (!queue.Contains(item)) queue.Enqueue(item);
    }

    public List<int> GetSkillRandom() => skillRandom;

    public bool IsAllSkillMaxLevel()
    {
        return skill.Count >= MAX_SKILL_OWNED && skillRandom.Count == 0;
    }
}

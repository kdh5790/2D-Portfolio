using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSkillData
{
    public SkillData skill;
    public int skillLevel;

    public PlayerSkillData(SkillData _skill, int _level = 0)
    {
        skill = _skill;
        skillLevel = _level;
    }
}


public class SkillDataManager : MonoBehaviour
{
    public List<SkillData> archerSkillData;
    public List<SkillData> warriorSkillData;

    public List<PlayerSkillData> playerSkillList;

    private void Start()
    { 
        switch (Character.instance.playerClass)
        {
            case Character.Warrior:
                if (warriorSkillData != null)
                    for (int i = 0; i < warriorSkillData.Count; i++)
                        playerSkillList.Add(new PlayerSkillData(warriorSkillData[i]));
                break;

            case Character.Archer:
                if (archerSkillData != null)
                    for (int i = 0; i < archerSkillData.Count; i++)
                        playerSkillList.Add(new PlayerSkillData(archerSkillData[i]));
                break;
        }
    }
}

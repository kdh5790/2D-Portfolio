using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data", order = int.MaxValue)]
public class SkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public float skillCoolTime;
    public float skillDamage;
    public float mp;
    public Sprite skillSprite;
    public SkillType skillType;
    public Class skillClass;

    public enum SkillType
    {
        ActiveSkill,
        PassiveSkill
    }

    public enum Class
    {
        Warrior,
        Archer
    }
}

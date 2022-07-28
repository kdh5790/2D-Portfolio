using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillList : MonoBehaviour, IPointerClickHandler
{
    public PlayerSkillData skill;
    public Image skillIcon;
    public Text skillName;
    bool isSkill;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSkill)
        {
            FindObjectOfType<SkillUI>().SetSkill(skill);
        }
    }

    public void SetSkill(PlayerSkillData _skill)
    {
        isSkill = true;
        skill = _skill;
        skillIcon.sprite = skill.skill.skillSprite;
        skillName.text = skill.skill.skillName;
    }

    private void OnEnable()
    {
        if (isSkill)
            skillIcon.color = new Color(1, 1, 1, 1);
        else
            skillIcon.color = new Color(1, 1, 1, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    SkillDataManager skillData;

    PlayerSkillData selectSkill;

    Character player;

    public GameObject skillPanel;
    public SkillList[] skillList;
    public QuickSlot[] quickSlot;

    public Image skillImage;
    public Text levelText;
    public Text desText;
    public Text nextLevelText;
    public Text pointText;
    public Image fadeImage;
    public AudioClip clip;

    bool isOpen;

    private void Start()
    {
        player = FindObjectOfType<Character>();
        skillData = FindObjectOfType<SkillDataManager>();

        StartCoroutine(SetSkillList());
    }

    public void OpenSkillUI()
    {
        AudioManager.instance.PlayClip(clip);
        isOpen = true;
        PointUpdate();
        skillPanel.SetActive(true);
    }

    public void CloseSkillUI()
    {
        AudioManager.instance.PlayButtonClip();
        isOpen = false;
        skillPanel.SetActive(false);
    }

    IEnumerator SetSkillList()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < skillData.playerSkillList.Count; i++)
        {
            skillList[i].SetSkill(skillData.playerSkillList[i]);
        }
        PointUpdate();
        SetSkill(skillList[0].skill);
    }

    public void PointUpdate() => pointText.text = $"보유한 스킬포인트 : {player.playerSkillPoint}";

    public void SetSkill(PlayerSkillData skill)
    {
        selectSkill = skill;
        skillImage.sprite = selectSkill.skill.skillSprite;
        levelText.text = $"스킬명 : {selectSkill.skill.skillName}\n스킬 레벨 : {selectSkill.skillLevel}";

        if (skill.skill.skillName == "구르기" || skill.skill.skillName == "Guard")
        {
            skill.skillLevel = 1;
            desText.text = $"{selectSkill.skill.skillDescription}\n";
            nextLevelText.text = $"{selectSkill.skill.skillDescription}\n 스킬 레벨업 불가.";
        }

        else
        {
            desText.text = $"{selectSkill.skill.skillDescription}\n데미지 : {player.playerDamage} * {selectSkill.skill.skillDamage + ((float)selectSkill.skillLevel / 10.0f)}";
            nextLevelText.text = $"{selectSkill.skill.skillDescription}\n데미지 : {player.playerDamage} * {selectSkill.skill.skillDamage + ((float)(selectSkill.skillLevel + 1) / 10.0f)}";
        }
    }

    public void LevelUp()
    {
        Debug.Log(selectSkill.skill.skillName);
        if (selectSkill.skill.skillName == "구르기" || selectSkill.skill.skillName == "Guard")
        {
            StartCoroutine(GameManager.instance.ChangeInfoText("레벨업이 불가능한 스킬입니다."));
        }
        else if(selectSkill.skill.skillName != "구르기" || selectSkill.skill.skillName != "Guard")
        {
            if (player.playerSkillPoint > 0)
            {
                AudioManager.instance.PlayButtonClip();
                player.playerSkillPoint -= 1;
                selectSkill.skillLevel += 1;
                PointUpdate();
                levelText.text = $"스킬명 : {selectSkill.skill.skillName}\n스킬 레벨 : {selectSkill.skillLevel}";
                desText.text = $"{selectSkill.skill.skillDescription}\n데미지 : {player.playerDamage} * {selectSkill.skill.skillDamage + ((float)selectSkill.skillLevel / 10.0f)}";
                nextLevelText.text = $"{selectSkill.skill.skillDescription}\n데미지 : {player.playerDamage} * {selectSkill.skill.skillDamage + ((float)(selectSkill.skillLevel + 1) / 10.0f)}";
            }
            else
                StartCoroutine(GameManager.instance.ChangeInfoText("스킬 포인트가 부족합니다."));
        }
    }

    public void QuickSlot()
    {
        if (selectSkill.skillLevel > 0)
        {
            AudioManager.instance.PlayButtonClip();
            for (int i = 0; i < quickSlot.Length; i++)
            {
                fadeImage.gameObject.SetActive(true);
                quickSlot[i].cursor.SetActive(true);
                quickSlot[i].btn.interactable = true;
            }
        }
        else
                StartCoroutine(GameManager.instance.ChangeInfoText("아직 스킬을 배우지 못했습니다."));
    }

    public void SetQuickSlot(int selectNum)
    {
        AudioManager.instance.PlayButtonClip();
        quickSlot[selectNum].SetSkill(selectSkill);
        for (int i = 0; i < quickSlot.Length; i++)
        {
            quickSlot[i].btn.interactable = false;
            quickSlot[i].cursor.SetActive(false);
        }
    }
}

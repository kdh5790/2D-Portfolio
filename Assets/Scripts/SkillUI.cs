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

    public void PointUpdate() => pointText.text = $"������ ��ų����Ʈ : {player.playerSkillPoint}";

    public void SetSkill(PlayerSkillData skill)
    {
        selectSkill = skill;
        skillImage.sprite = selectSkill.skill.skillSprite;
        levelText.text = $"��ų�� : {selectSkill.skill.skillName}\n��ų ���� : {selectSkill.skillLevel}";

        if (skill.skill.skillName == "������" || skill.skill.skillName == "Guard")
        {
            skill.skillLevel = 1;
            desText.text = $"{selectSkill.skill.skillDescription}\n";
            nextLevelText.text = $"{selectSkill.skill.skillDescription}\n ��ų ������ �Ұ�.";
        }

        else
        {
            desText.text = $"{selectSkill.skill.skillDescription}\n������ : {player.playerDamage} * {selectSkill.skill.skillDamage + ((float)selectSkill.skillLevel / 10.0f)}";
            nextLevelText.text = $"{selectSkill.skill.skillDescription}\n������ : {player.playerDamage} * {selectSkill.skill.skillDamage + ((float)(selectSkill.skillLevel + 1) / 10.0f)}";
        }
    }

    public void LevelUp()
    {
        Debug.Log(selectSkill.skill.skillName);
        if (selectSkill.skill.skillName == "������" || selectSkill.skill.skillName == "Guard")
        {
            StartCoroutine(GameManager.instance.ChangeInfoText("�������� �Ұ����� ��ų�Դϴ�."));
        }
        else if(selectSkill.skill.skillName != "������" || selectSkill.skill.skillName != "Guard")
        {
            if (player.playerSkillPoint > 0)
            {
                AudioManager.instance.PlayButtonClip();
                player.playerSkillPoint -= 1;
                selectSkill.skillLevel += 1;
                PointUpdate();
                levelText.text = $"��ų�� : {selectSkill.skill.skillName}\n��ų ���� : {selectSkill.skillLevel}";
                desText.text = $"{selectSkill.skill.skillDescription}\n������ : {player.playerDamage} * {selectSkill.skill.skillDamage + ((float)selectSkill.skillLevel / 10.0f)}";
                nextLevelText.text = $"{selectSkill.skill.skillDescription}\n������ : {player.playerDamage} * {selectSkill.skill.skillDamage + ((float)(selectSkill.skillLevel + 1) / 10.0f)}";
            }
            else
                StartCoroutine(GameManager.instance.ChangeInfoText("��ų ����Ʈ�� �����մϴ�."));
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
                StartCoroutine(GameManager.instance.ChangeInfoText("���� ��ų�� ����� ���߽��ϴ�."));
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

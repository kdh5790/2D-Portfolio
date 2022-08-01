using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPQuickSlot : MonoBehaviour
{
    PVPCharacter player;

    bool isSkill;
    bool isCool;

    public PlayerSkillData skill;
    public Image image;
    public Image bgImage;
    public Button useBtn;

    public void SetSkill(PlayerSkillData _skill)
    {
        isSkill = true;
        skill = _skill;
        image.sprite = skill.skill.skillSprite;
        image.color = new Color(1, 1, 1, 1);
        bgImage.sprite = skill.skill.skillSprite;
        bgImage.color = new Color(1, 1, 1, 0.3f);
        useBtn.interactable = true;
        useBtn.gameObject.SetActive(true);
    }

    public void UseSkill()
    {
        if (isSkill && !isCool)
        {
            var players = FindObjectsOfType<PVPCharacter>();
            for (int i = 0; i < players.Length; i++)
                if (players[i].ptView.IsMine)
                    player = players[i];
            if (!player.isJump && !player.isRoll && !player.isGuard)
            {
                player.UsingSkill(skill);
                StartCoroutine(CoolTime(skill.skill.skillCoolTime));
            }
        }
    }

    IEnumerator CoolTime(float time)
    {
        image.fillAmount = 0;
        isCool = true;
        float lefttime = 0;
        while (lefttime < time)
        {
            lefttime += Time.deltaTime;
            image.fillAmount = lefttime / time;
            yield return new WaitForFixedUpdate();
        }
        image.fillAmount = 1f;

        isCool = false;
    }
}

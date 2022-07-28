//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class SkillQuickSlot : MonoBehaviour
//{
//    bool isSkill;
//    bool isCool;

//    public GameObject cursor;
//    public PlayerSkillData skill;
//    public Image image;
//    public Image bgImage;
//    public Button useBtn;
//    public Button btn;

//    public void SetSkill(PlayerSkillData _skill)
//    {
//        FindObjectOfType<SkillUI>().fadeImage.gameObject.SetActive(false);
//        isSkill = true;
//        skill = _skill;
//        image.sprite = skill.skill.skillSprite;
//        image.color = new Color(1, 1, 1, 1);
//        bgImage.sprite = skill.skill.skillSprite;
//        bgImage.color = new Color(1, 1, 1, 0.3f);
//        useBtn.interactable = true;
//        useBtn.gameObject.SetActive(true);
//    }

//    public void UseSkill()
//    {
//        if (isSkill && !isCool && Character.instance.currentMp >= skill.skill.mp && Character.instance.isGround)
//        {
//            Character.instance.UsingSkill(skill);
//            StartCoroutine(CoolTime(skill.skill.skillCoolTime));
//        }
//        else if(isSkill && !isCool && Character.instance.currentMp < skill.skill.mp)
//        {
//            StartCoroutine(GameManager.instance.ChangeInfoText("MP가 부족합니다."));
//        }
//        else if(isSkill && isCool)
//        {
//            StartCoroutine(GameManager.instance.ChangeInfoText("아직 스킬을 사용할 수 없습니다."));
//        }
//    }

//    IEnumerator CoolTime(float time)
//    {
//        image.fillAmount = 0;
//        isCool = true;
//        float lefttime = 0;
//        while(lefttime < time)
//        {
//            lefttime += Time.deltaTime;
//            image.fillAmount = lefttime / time;
//            yield return new WaitForFixedUpdate();
//        }
//        image.fillAmount = 1f;

//        isCool = false;
//    }
//}

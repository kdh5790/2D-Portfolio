using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Character
{
    public GameObject rightWeaponCol;
    public GameObject leftWeaponCol;
    public GameObject skill;

    float comboAttackTime;
    int currentAttack;


    void Awake()
    {
        playerClass = Warrior;
        this.enabled = true;
        base.AwakeInit();
    }

    void Start()
    {
        base.StartInit();
    }

    void Update()
    {
        comboAttackTime += Time.deltaTime;

        base.UpdateInit();
    }
    public override void Attack(int num = 0)
    {
        if (num != 0 && comboAttackTime > 0.25f && !isRoll && !isTalk && !isGuard)
        {

            isAttack = true;
            walkSpeed = 0;
            currentAttack++;

            //AudioManager.instance.PlayClip(clip);

            // 콤보 공격 1번(처음)으로 변경
            if (currentAttack > 3)
                currentAttack = 1;

            // 일정시간 지나면 콤보공격 초기화
            if (comboAttackTime > 1.0f)
                currentAttack = 1;

            // 공격 애니메이션 재생
            anim.SetTrigger("Attack" + currentAttack);

            // 콤보 공격 타이머 초기화
            comboAttackTime = 0.0f;
        }
    }

    protected override void GroundCheck()
    {
        // 착지 확인용 ray
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2, LayerMask.GetMask("Ground"));
        if (rayHit.collider != null)
        {
            if (rayHit.distance < 0.2f)
            {
                anim.SetBool("isGround", true);
                isJump = false;
                isGround = true;
            }
            else
            {
                anim.SetBool("isGround", false);
                isGround = false;
            }
        }
    }

    void ActiveWeapon()
    {
        AudioManager.instance.PlayClip(clip);
        isAttack = true;
        walkSpeed = 0;
        rightWeaponCol.SetActive(true);
        leftWeaponCol.SetActive(false);
    }

    void InactiveWeapon()
    {
        rightWeaponCol.SetActive(false);
        leftWeaponCol.SetActive(false);
        isAttack = false;
        walkSpeed = 5;
    }

    public override void UsingSkill(PlayerSkillData data)
    {
        if (!isDead)
        {
            if (data.skill.skillName == "Guard")
            {
                StartCoroutine(Guard());
            }
            else
            {
                isAttack = true;
                walkSpeed = 0;
                currentMp -= data.skill.mp;
                if (data.skill.skillName == "Skill2")
                {
                    skill.SetActive(true);
                    AudioManager.instance.PlayClip(clip);
                }

                anim.SetTrigger(data.skill.skillName);
            }
        }
    }

    IEnumerator Guard()
    {
        float time = 2f;
        isGuard = true;
        walkSpeed = 0;
        while (time >= 0f)
        {
            time -= Time.deltaTime;
            anim.SetBool("isGuard", true);
            yield return new WaitForFixedUpdate();
        }

        walkSpeed = 5;
        isGuard = false;
        anim.SetBool("isGuard", false);
    }
}

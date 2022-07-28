using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character
{
    public List<SkillData> skill;

    public GameObject rightWeaponCol;
    public GameObject leftWeaponCol;
    public GameObject shootPos;

    public int tempDamage;



    void Awake()
    {
        AwakeInit();
    }

    protected override void AwakeInit()
    {
        playerClass = Archer;
        this.enabled = true;
        base.AwakeInit();
    }

    void Start()
    {
        StartInit();
    }

    protected override void StartInit()
    {
        base.StartInit();
    }

    void Update()
    {
        UpdateInit();
    }

    protected override void UpdateInit()
    {
        base.UpdateInit();
    }

    public override void Attack(int num = 0)
    {
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, new Vector2(currentDirection * 2, 0.5f), 1f, LayerMask.GetMask("Enemy"));

        if (Input.GetKeyDown(KeyCode.Z) || num != 0 && !isRoll && !isTalk && !isAttack && isGround)
        {
            tempDamage = playerDamage;
            isAttack = true;
            walkSpeed = 0;
            if (rayHit.collider != null)
                anim.SetTrigger("MeleeAttack");
            else
                anim.SetTrigger("RangeAttack");

            Invoke("InactiveWeapon", 1f);
        }
    }
    public override void UsingSkill(PlayerSkillData data)
    {
        if (!isRoll && !isTalk && !isAttack && isGround)
        {
            if (data.skill.skillName == "구르기")
                Rolling(1);
            else
            {
                currentMp -= data.skill.mp;
                isAttack = true;
                tempDamage = playerDamage;
                playerDamage = (int)((float)playerDamage * data.skill.skillDamage);
                walkSpeed = 0;
                anim.SetTrigger(data.skill.skillName);
                Invoke("InactiveWeapon", 1f);
            }
        }
    }

    protected override void GroundCheck()
    {
        // 착지 확인용 ray
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 2, LayerMask.GetMask("Ground"));
        if (rayHit.collider != null)
        {
            Debug.Log( rayHit.distance);
            if (rayHit.distance < 0.72f)
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
        isAttack = true;
        rightWeaponCol.SetActive(true);
    }

    void InactiveWeapon()
    {
        rightWeaponCol.SetActive(false);
        isAttack = false;
        playerDamage = tempDamage;
        walkSpeed = 5;
    }

    void Shoot()
    {
        AudioManager.instance.PlayClip(clip);
        ObjectPoolManager.Instance.pool.Pop().transform.position = shootPos.transform.position;
    }
}

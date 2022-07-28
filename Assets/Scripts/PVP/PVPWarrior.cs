using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PVPWarrior : PVPCharacter
{
    public GameObject skill;
    public GameObject rightWeaponCol;
    public GameObject leftWeaponCol;

    private float comboAttackTime;
    private int currentAttack;

    private int tempDamage;

    protected override void Start()
    {
        groundDistance = 0.2f;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (ptView.IsMine) //  && isStart
        {
            Guard();
        }
        comboAttackTime += Time.deltaTime;
    }


    public override void Attack()
    {
        if (comboAttackTime > 0.25f && !isRoll && !isGuard && ptView.IsMine && !isDead) // && isStart
        {
            isAttack = true;
            AudioManager.instance.PlayClip(clip);
            walkSpeed = 0;
            currentAttack++;

            if (currentAttack > 3)
                currentAttack = 1;

            if (comboAttackTime > 1.0f)
                currentAttack = 1;

            ptView.RPC("RPCTriggerAnimation", RpcTarget.AllBuffered, "Attack" + currentAttack);

            comboAttackTime = 0.0f;
        }
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

                if (data.skill.skillName == "Skill2")
                    ptView.RPC("Skill2", RpcTarget.AllBuffered);

                ptView.RPC("RPCTriggerAnimation", RpcTarget.AllBuffered, data.skill.skillName);
            }
        }
    }

    [PunRPC]
    void Skill2() => skill.gameObject.SetActive(true);

    void ActiveWeapon() =>  ptView.RPC("ActiveWeaponRPC", RpcTarget.AllBuffered);

    [PunRPC]
    void ActiveWeaponRPC()
    {
        isAttack = true;
        walkSpeed = 0;
        switch (currentDirection)
        {
            case 1:
                rightWeaponCol.SetActive(true);
                leftWeaponCol.SetActive(false);
                break;

            case -1:
                rightWeaponCol.SetActive(false);
                leftWeaponCol.SetActive(true);
                break;
        }
    }

    public override void Hit(int _damage)
    {
        if (isGuard)
            base.Hit(_damage / 3);
        else
            base.Hit(_damage);
    }

    IEnumerator Guard()
    {
        float time = 2f;
        isGuard = true;
        walkSpeed = 0;
        while (time >= 0f)
        {
            time -= Time.deltaTime;
            ptView.RPC("RPCBoolAnimation", RpcTarget.AllBuffered, "isGuard", true);
            yield return new WaitForFixedUpdate();
        }

        walkSpeed = 5;
        isGuard = false;
        ptView.RPC("RPCBoolAnimation", RpcTarget.AllBuffered, "isGuard", false);
    }


    void InactiveWeapon() => ptView.RPC("InactiveWeaponRPC", RpcTarget.AllBuffered); 

    [PunRPC]
    void InactiveWeaponRPC(){ rightWeaponCol.SetActive(false); leftWeaponCol.SetActive(false); isAttack = false; walkSpeed = 5; }

}

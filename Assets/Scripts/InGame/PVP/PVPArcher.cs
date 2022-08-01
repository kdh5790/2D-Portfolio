using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PVPArcher : PVPCharacter
{
    public PVPArrow[] arrow;

    public List<PVPArrow> arrowList;

    public Transform shootPos;
    public GameObject rightWeaponCol;
    public GameObject leftWeaponCol;

    protected override void Start()
    {
        base.Start();
        groundDistance = 0.72f;
        arrowList = new List<PVPArrow>();

        for (int i = 0; i < arrow.Length; i++)
        {
            arrow[i].gameObject.SetActive(false);
            arrowList.Add(arrow[i]);
        }
    }


    protected override void Update()
    {
        base.Update();
        if(ptView.IsMine)
        Rolling();
    }


    public override void Attack()
    {
        if (!isRoll && !isGuard && ptView.IsMine && !isDead && !isAttack) // && isStart
        {
            isAttack = true;
            ptView.RPC("RPCTriggerAnimation", RpcTarget.AllBuffered, "RangeAttack");
        }
    }

    public override void UsingSkill(PlayerSkillData data)
    {
        if (data.skill.skillName == "±¸¸£±â")
        {
            isRoll = true;
            ptView.RPC("RPCTriggerAnimation", RpcTarget.AllBuffered, "Roll");
        }

        else
        {
            isAttack = true;
            walkSpeed = 5;


            ptView.RPC("RPCTriggerAnimation", RpcTarget.AllBuffered, data.skill.skillName);
        }
    }

    public void Rolling()
    {
            if (isRoll && isGround && !isAttack)
            {
                rigid.velocity = new Vector2(currentDirection * 8, rigid.velocity.y);
                col.enabled = false;
                rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
    }

    void StopRoll()
    {
        isRoll = false; col.enabled = true;
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    void Shoot() => ptView.RPC("ShootRPC", RpcTarget.Others);


    void ActiveWeapon() => ptView.RPC("ActiveWeaponRPC", RpcTarget.AllBuffered);

    [PunRPC]
    void ActiveWeaponRPC()
    {
        rightWeaponCol.gameObject.SetActive(true);
    }

    [PunRPC]
    void ShootRPC()
    {
        Debug.Log("ArcherShoot");
        isAttack = true;
        walkSpeed = 5;
        for (int i = 0; i < arrowList.Count; i++)
        {
            if (!arrowList[i].gameObject.activeSelf)
            {
                arrowList[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    void InactiveWeapon() => ptView.RPC("InactiveWeaponRPC", RpcTarget.AllBuffered);

    [PunRPC]
    void InactiveWeaponRPC() { isAttack = false; walkSpeed = 5; rightWeaponCol.gameObject.SetActive(false); }
}

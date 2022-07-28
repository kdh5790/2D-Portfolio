using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public abstract class PVPCharacter : MonoBehaviourPunCallbacks, IPunObservable
{
    public Image hpImage;
    public PhotonView ptView;
    public Transform damageTextTf;
    public AudioClip clip;
    public AudioSource source;
    public DamageText _text;

    protected Joystick joystick;
    protected CapsuleCollider2D col;
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected SpriteRenderer sprite;

    public float playerDef;
    public int playerDamage;

    public float currentHp;
    public float maxHp;

    public float currentMp;
    public float maxMp;


    protected int jumpCount;
    protected float groundDistance;
    protected float walkSpeed = 5f;
    protected float jumpPower = 5f;
    protected float inputX;
    protected float axis;
    public float currentDirection;

    protected bool isGround = true;
    protected bool isMove = false;
    public bool isRoll = false;
    protected bool isAttack = false;
    public bool isJump = false;
    public bool isGuard = false;

    public bool isKill;
    public bool isDead;
    public bool isMaster;
    public bool isPVP;
    public bool isStart;

    public Player player;

    Vector3 currentPos;
    RaidBoss boss;

    protected virtual void Start()
    {
        ptView = GetComponent<PhotonView>();
        col = GetComponent<CapsuleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        joystick = FindObjectOfType<Joystick>();

        if (PVPManager.instance != null)
            PVPManager.instance.players.Add(this);

        if (RaidManager.instance != null)
        {
            RaidManager.instance.players.Add(this);
            if (RaidManager.instance.players.Count >= 2)
            {
                boss = PhotonNetwork.Instantiate("RaidBoss", Vector3.zero, Quaternion.identity).GetComponent<RaidBoss>();
            }
        }

        if (ptView.IsMine)
        {
            var cinemachine = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            cinemachine.Follow = transform;
            cinemachine.LookAt = transform;
            player = PhotonNetwork.LocalPlayer;
            isMaster = PhotonNetwork.IsMasterClient;
        }

    }

    protected virtual void Update()
    {
        if (ptView.IsMine && !isDead)
        {
            anim.SetFloat("AirSpeedY", rigid.velocity.y);

            GroundCheck();
            Move();
            MoveCheck();
        }

    }

    protected void Move()
    {
        axis = joystick.GetInputX();
        rigid.velocity = new Vector2(axis * walkSpeed, rigid.velocity.y);
        if (axis != 0)
            ptView.RPC("FlipSprite", RpcTarget.AllBuffered, axis);
    }

    public void Jump()
    {
        if ((jumpCount < 1 || isGround) && !isRoll && !isGuard && !isDead)
        {
            if (!isJump)
                isJump = true;
            jumpCount++;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            ptView.RPC("RPCTriggerAnimation", RpcTarget.AllBuffered, "Jump");
        }
    }

    [PunRPC]
    protected void JumpRPC()
    {
        if (ptView.IsMine)
        {
            anim.SetTrigger("Jump");
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }


    protected void GroundCheck()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2, LayerMask.GetMask("Ground"));
        if (rayHit.collider != null)
        {
            if (rayHit.distance < groundDistance)
            {
                anim.SetBool("isGround", true);
                isJump = false;
                jumpCount = 0;
                isGround = true;
            }
            else
            {
                anim.SetBool("isGround", false);
                isGround = false;
            }
        }
    }

    protected void MoveCheck()
    {
        if (Mathf.Abs(axis) > Mathf.Epsilon)
        {
            isMove = true;
            anim.SetBool("isRun", true);
            if (!source.isPlaying)
                source.Play();
        }
        else
        {
            isMove = false;
            anim.SetBool("isRun", false);
        }
    }

    [PunRPC]
    protected void FlipSprite(float axis)
    {
        if (axis > 0)
        {
            sprite.flipX = false;
            currentDirection = 1;
        }

        else if (axis < 0)
        {
            sprite.flipX = true;
            currentDirection = -1;
        }
    }

    public abstract void Attack();

    public abstract void UsingSkill(PlayerSkillData data);
    public virtual void Hit(int _damage)
    {
        currentHp -= _damage;
        SetFillAmount();

        PhotonNetwork.Instantiate("DamageText", damageTextTf.position, Quaternion.identity).GetComponent<DamageText>().damage = _damage;

        if (currentHp > 0)
            ptView.RPC("RPCTriggerAnimation", RpcTarget.AllBuffered, "Hurt");

        else if (currentHp <= 0)
        {
            isDead = true;
            currentHp = 0;
            SetFillAmount();
            ptView.RPC("RPCBoolAnimation", RpcTarget.AllBuffered, "isDead", true);
            ptView.RPC("RPCStopTriggerAnimation", RpcTarget.AllBuffered, "Hurt");


            if (!isPVP)
            {
                var cinemachine = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
                var players = FindObjectsOfType<PVPCharacter>();
                ptView.RPC("Die", RpcTarget.AllBuffered);
                if (players.Length > 1)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (!players[i].ptView.IsMine)
                        {
                            cinemachine.Follow = players[i].transform;
                            cinemachine.LookAt = players[i].transform;
                            break;
                        }
                    }
                }
                else
                    RaidManager.instance.RaidFail();

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public void SetFillAmount() => hpImage.fillAmount = currentHp / maxHp;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerDamage);
            stream.SendNext(transform.position);
            stream.SendNext(hpImage.fillAmount);
            if (FindObjectOfType<PVPManager>() != null)
            {
                stream.SendNext(PVPManager.instance.leftText.text);
                stream.SendNext(PVPManager.instance.rightText.text);
            }
            stream.SendNext(currentHp);
            stream.SendNext(isMaster);
        }
        else
        {
            playerDamage = (int)stream.ReceiveNext();
            currentPos = (Vector3)stream.ReceiveNext();
            hpImage.fillAmount = (float)stream.ReceiveNext();
            if (FindObjectOfType<PVPManager>() != null)
            {
                PVPManager.instance.leftText.text = (string)stream.ReceiveNext();
                PVPManager.instance.rightText.text = (string)stream.ReceiveNext();
            }
            currentHp = (float)stream.ReceiveNext();
            isMaster = (bool)stream.ReceiveNext();

        }
    }

    [PunRPC]
    protected void Die()
    {
        sprite.enabled = false;
    }

    [PunRPC]
    protected void Spawn()
    {
        sprite.enabled = true;
    }

    [PunRPC]
    public void ResetCollider()
    {
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.freezeRotation = true;
        rigid.isKinematic = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
    }

    [PunRPC]
    protected void RPCTriggerAnimation(string aniParam)
    {
        anim.SetTrigger(aniParam);
    }

    [PunRPC]
    protected void RPCBoolAnimation(string aniParam, bool isBool)
    {
        anim.SetBool(aniParam, isBool);
    }

    [PunRPC]
    protected void RPCStopTriggerAnimation(string aniParam)
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.ResetTrigger(aniParam);
    }
}

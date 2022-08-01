using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RaidBoss : MonoBehaviourPunCallbacks, IPunObservable
{
    public static RaidBoss instance;

    public GameObject rightShootPos;
    public GameObject leftShootPos;

    public Transform damageTextTf;
    public BoxCollider2D weaponCol;
    public PhotonView ptView;
    public Image hpImage;

    public float currentHp;
    public float MaxHp;
    public float damage;

    Rigidbody2D rigid;
    Animator anim;
    PVPCharacter[] players;
    PVPCharacter target;
    SpriteRenderer sprite;

    Vector2 targetPos;
    Vector2 direction;

    bool isDead;
    bool isCooling;
    float timer;
    float intTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
            instance = this;
        }
    }

    void Start()
    {
        timer = 5f;
        intTimer = timer;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(FindTargetCoroutine());
        StartCoroutine(FireBallCoroutine());
    }

    void Update()
    {
        if (ptView.IsMine && RaidBoss_AnimState.isInit && !isDead)
        {
            if (isCooling)
                intTimer -= Time.deltaTime;
            if (intTimer <= 0)
                isCooling = false;

            if (target != null)
            {
                targetPos = new Vector2(target.transform.position.x, rigid.position.y);
                direction = (Vector2)target.transform.position - rigid.position;
                if (direction.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }

                else if (direction.x < 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }

                Debug.Log($"Distance : {Vector2.Distance(targetPos, rigid.position)}");

                if (Vector2.Distance(targetPos, rigid.position) > 3f)
                {
                    ptView.RPC("RPCBoolAnimation", RpcTarget.AllBuffered, "isMove", true);
                    targetPos = new Vector2(target.transform.position.x, transform.position.y);
                    rigid.position = Vector2.MoveTowards(transform.position, targetPos, 12f * Time.deltaTime);
                }

                else
                {
                    ptView.RPC("RPCBoolAnimation", RpcTarget.AllBuffered, "isMove", false);
                    if (!isCooling)
                    {
                        ptView.RPC("RPCTriggerAnimation", RpcTarget.AllBuffered, "Attack");
                        isCooling = true;
                        intTimer = timer;
                    }
                }
            }
        }

    }

    IEnumerator FireBallCoroutine()
    {
        float timer = 3f;
        PhotonNetwork.Instantiate("RaidSkill", new Vector3(Random.Range(-10f, 26f), -5f, 1f), Quaternion.identity);
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(FireBallCoroutine());
    }

    [PunRPC]
    void FlipSprite(bool isFlip) => sprite.flipX = isFlip;

    public void FindTarget() => StartCoroutine(FindTargetCoroutine());

    IEnumerator FindTargetCoroutine()
    {
        while (currentHp > 0)
        {
            players = FindObjectsOfType<PVPCharacter>();
            target = players[Random.Range(0, players.Length)];
            yield return new WaitForSeconds(7.0f);
        }
    }

    public void Damaged(int _damage)
    {
        currentHp -= _damage;
        SetFillAmount();
        if (ptView.IsMine)
            PhotonNetwork.Instantiate("DamageText", damageTextTf.position, Quaternion.identity).GetComponent<DamageText>().damage = _damage;

        if (currentHp <= 0)
        {
            isDead = true;
            ptView.RPC("RPCTriggerAnimation", RpcTarget.AllBuffered, "Dead");
        }
    }

    void SetFillAmount() => hpImage.fillAmount = currentHp / MaxHp;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hpImage.fillAmount);
            stream.SendNext(currentHp);
            stream.SendNext(transform.position);
        }
        else
        {
            hpImage.fillAmount = (float)stream.ReceiveNext();
            currentHp = (float)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
        }
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

    void ActiveWeapon()
    {
        weaponCol.gameObject.SetActive(true);
    }
    void InactiveWeapon()
    {
        weaponCol.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PVPWarriorSkill : MonoBehaviourPunCallbacks
{
    public int damage;
    public PhotonView ptView;
    private SpriteRenderer sprite;
    Vector3 originPos;
    float dir;

    private void Awake()
    {
        originPos = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        damage = GetComponentInParent<PVPWarrior>().playerDamage;
    }

    public override void OnEnable()
    {
        damage = GetComponentInParent<PVPWarrior>().playerDamage;
        dir = GetComponentInParent<PVPWarrior>().currentDirection;
        ptView.RPC("FlipRPC", RpcTarget.AllBuffered);
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        float time = 1.5f;
        while (time >= 0)
        {
            time -= Time.deltaTime;
            transform.Translate(Vector3.right * 7 * Time.deltaTime * dir);
            yield return new WaitForFixedUpdate();
        }
        Disable();
    }

    public override void OnDisable()
    {
        transform.localPosition = new Vector3(0, 0.7f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PVPCharacter>().ptView.IsMine && collision.GetComponent<PVPCharacter>().isPVP)
        {
            collision.GetComponent<PVPCharacter>().Hit(damage * 3);
            ptView.RPC("DisableRPC", RpcTarget.AllBuffered);
        }

        if (collision.tag == "Enemy")
        {
            collision.GetComponent<RaidBoss>().Damaged(damage * 3);
            ptView.RPC("DisableRPC", RpcTarget.AllBuffered);
        }
    }

    public void Enable() => gameObject.SetActive(true);
    public void Disable() => ptView.RPC("DisableRPC", RpcTarget.AllBuffered);

    [PunRPC]
    void FlipRPC()
    {
        var player = GetComponentInParent<PVPWarrior>();
        if (player.currentDirection == 1)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    [PunRPC]
    void DisableRPC() => gameObject.SetActive(false);

}

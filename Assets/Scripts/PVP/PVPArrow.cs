using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PVPArrow : MonoBehaviourPunCallbacks
{
    public PVPArcher player;
    public PhotonView ptView;
    private SpriteRenderer sprite;
    Vector3 originPos;
    float dir;

    void Start()
    {
    }

    public override void OnEnable()
    {
        dir = player.currentDirection;
        sprite = GetComponent<SpriteRenderer>();
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
        ptView.RPC("InactiveArrow", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void InactiveArrow()
    {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }

    [PunRPC]
    void FlipRPC()
    {
        if (player.currentDirection == 1)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PVPCharacter>().ptView.IsMine && collision.GetComponent<PVPCharacter>().isPVP && collision.name != player.name)
        {
            collision.GetComponent<PVPCharacter>().Hit(player.playerDamage);
            ptView.RPC("InactiveArrow", RpcTarget.AllBuffered);
        }

        if (collision.tag == "Enemy")
        {
            collision.GetComponent<RaidBoss>().Damaged(player.playerDamage);
            ptView.RPC("InactiveArrow", RpcTarget.AllBuffered);
        }
    }
}

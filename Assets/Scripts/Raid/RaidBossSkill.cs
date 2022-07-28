using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RaidBossSkill : MonoBehaviourPunCallbacks
{
    public SpriteRenderer sprite;
    public GameObject go;
    public PhotonView ptView;

    void Start()
    {
        StartCoroutine(MoveToDown());
    }

    IEnumerator MoveToDown()
    {
        yield return new WaitForSeconds(1.0f);
        sprite.enabled = false;

        while(go.transform.position.y > -5f)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, new Vector2(go.transform.position.x, -5f), 5 * Time.deltaTime);

            yield return new WaitForFixedUpdate();
        }
        ptView.RPC("DestroyRPC", RpcTarget.AllBuffered);
    }

    public void Destroy() => ptView.RPC("DestroyRPC", RpcTarget.AllBuffered);

    [PunRPC]
    public void DestroyRPC() => Destroy(gameObject);
}

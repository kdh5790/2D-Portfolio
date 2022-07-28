using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public Transform goalTf;
    bool isPlayer;
    public GameObject player;
    public int goalNum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject;
            isPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = null;
            isPlayer = false;
        }
    }

    public void MapChange()
    {
        if (goalTf != null)
            player.transform.position = goalTf.position;
        else if (goalTf == null)
        {
            player.transform.position = new Vector3(-5.5f, -1f);
            Destroy(gameObject, 2f);
        }
        FindObjectOfType<CinemachineRange>().ChangeCollider(goalNum);
        StartCoroutine(UIManager.instance.FadeCoroutine());
        player.GetComponent<Character>().isTalk = false;
    }
}

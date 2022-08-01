using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPWeapon : MonoBehaviour
{
    public PVPCharacter character;

    private void Awake()
    {
        character = GetComponentInParent<PVPCharacter>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            col.GetComponent<RaidBoss>().Damaged(character.playerDamage);
            return;
        }

        if (col.tag == "Player" && col.GetComponent<PVPCharacter>().ptView.IsMine && col.GetComponent<PVPCharacter>().isPVP && character.ptView.ViewID != col.GetComponent<Photon.Pun.PhotonView>().ViewID)
        {
            Debug.Log("hit");

            col.GetComponent<PVPCharacter>().Hit(character.playerDamage);
        }

        if(col.tag == "Player" && GetComponentInParent<RaidBoss>() != null)
        {
            col.GetComponent<PVPCharacter>().Hit((int)FindObjectOfType<RaidBoss>().damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidBossSkill_Collider : MonoBehaviour
{
    public RaidBossSkill parent;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            col.GetComponent<PVPCharacter>().Hit(70);
            parent.Destroy();
        }
    }
}

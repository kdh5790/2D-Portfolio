using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    PVPCharacter player;


    public GameObject attack;
    public GameObject check;

    public Button jumpButton;

    bool isAttack;

    public void TalkToNPC()
    {
        FindObjectOfType<Character>().TalkToNPC();
    }

    public void Attack()
    {
        var players = FindObjectsOfType<PVPCharacter>();
        for (int i = 0; i < players.Length; i++)
            if (players[i].ptView.IsMine)
                player = players[i];

        player.Attack();
            
    }

    public void Jump()
    {
        var players = FindObjectsOfType<PVPCharacter>();
        for (int i = 0; i < players.Length; i++)
            if (players[i].ptView.IsMine)
                player = players[i];

        player.Jump();
    }
}

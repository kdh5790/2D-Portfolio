using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Character player;

    void Update()
    {
        if (player == null)
            player = FindObjectOfType<Character>();

        if(player != null)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2.5f, transform.position.z);


    }
}

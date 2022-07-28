using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Character character;
    Enemy enemy;

    private void Awake()
    {
        character = FindObjectOfType<Character>();
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" && enemy == null)
        {
            col.GetComponent<Enemy>().Damaged(Character.instance.playerDamage + Character.instance.equipDamage);
        }

        else if (col.tag == "Player" && enemy != null)
            col.GetComponent<Character>().Damaged(enemy.damage);
    }
}

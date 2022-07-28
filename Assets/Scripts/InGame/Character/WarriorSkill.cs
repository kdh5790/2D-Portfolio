using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : MonoBehaviour
{
    private SpriteRenderer sprite;
    float damage;
    float dir;

    public void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();
        damage = GetComponentInParent<Warrior>().playerDamage + GetComponentInParent<Warrior>().equipDamage;
        dir = GetComponentInParent<Warrior>().currentDirection;
        sprite.flipX = true;

        StartCoroutine(MoveObject());
    }

    public void OnDisable()
    {
        transform.localPosition = new Vector3(0, 0.7f, 0);
    }

    IEnumerator MoveObject()
    {
        float time = 1.5f;
        while (time >= 0)
        {
            time -= Time.deltaTime;
            transform.Translate(Vector3.right * 7 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            col.GetComponent<Enemy>().Damaged((int)(damage * 2.5f));
            gameObject.SetActive(false);
        }
    }
}

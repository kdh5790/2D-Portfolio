using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    Character target;
    Rigidbody2D rigid;
    public AudioClip clip;

    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(Random.Range(-50, 50), Random.Range(200, 300)), ForceMode2D.Force);
        target = FindObjectOfType<Character>();
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(1f);
        while(true)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rigid.velocity = new Vector2(direction.x, direction.y) * 8f;
            yield return null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindObjectOfType<Character>().playerGold += 30;
            AudioManager.instance.PlayClip(clip);
            Instantiate(Resources.Load("GoldEffect"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

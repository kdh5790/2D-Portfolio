using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Poolable
{
    float speed = 350f;
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        float direction = Archer.instance.currentDirection;
        if (direction == -1)
            rigid.transform.eulerAngles = new Vector3(0, 180, 0);
        else if (direction == 1)
            rigid.transform.eulerAngles = new Vector3(0, 0, 0);

        StartCoroutine(MoveObject());

        StartCoroutine(PushArrow(1.5f));
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
    }

    IEnumerator PushArrow(float time)
    {
        yield return new WaitForSeconds(time);
        Push();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            Push();
    }
}

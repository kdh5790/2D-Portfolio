using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public float intTimer;
    public bool isCooling;

    protected float distance;

    public GameObject weaponCol;
    public GameObject damageText;

    public Transform damageTextTransform;

    protected Character player;
    protected Rigidbody2D rigid;
    protected Animator anim;

    public float exp = 30f;
    public int gold = 4;

    public int currentHp;
    public int damage;

    public bool isDead;

    protected int currentDirection = -1;

    protected virtual void Awake()
    {
        intTimer = timer;
        player = FindObjectOfType<Character>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (isCooling)
            intTimer -= Time.deltaTime;
        if (intTimer <= 0)
            isCooling = false;
    }

    void ActiveWeapon()
    {
        moveSpeed = 0;
        weaponCol.SetActive(true);
        rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    void InactiveWeapon() { weaponCol.SetActive(false); moveSpeed = 1f; rigid.constraints = RigidbodyConstraints2D.FreezeRotation; }

    public virtual void Damaged(int _damage)
    {
        GameObject _damageText = Instantiate(damageText);
        _damageText.transform.position = damageTextTransform.position;
        _damageText.GetComponent<DamageText>().damage = _damage;

        currentHp -= _damage;

        if (currentHp <= 0)
            Die();

        else
        {
            if (transform.name != "Boss")
                anim.SetTrigger("Hurt");
        }
    }

    public virtual void Die()
    {
        anim.SetTrigger("Die");
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        moveSpeed = 0;
        isDead = true;
        for (int i = 0; i < gold; i++)
            Instantiate(Resources.Load("Gold"), transform.position, Quaternion.identity);
        player.currentExp += exp;
        CancelInvoke();
        Destroy(gameObject, 0.8f);
    }
}

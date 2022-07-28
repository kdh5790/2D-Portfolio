using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;

    private RaycastHit2D hit;
    private GameObject target;
    private bool isInRange;

    public int randomX;

    protected override void Awake()
    {
        base.Awake();
        Move();
    }

    protected override void Update()
    {
        base.Update();


        RaycastDebugger();


        hit = Physics2D.Raycast(rayCast.position, Vector2.right * currentDirection, rayCastLength, raycastMask);

        if (hit.collider != null)
        {
            isInRange = true;
            rigid.velocity = Vector3.zero;
            target = hit.collider.gameObject;
            EnemyLogic();
        }

        if (hit.collider == null)
            isInRange = false;

        if (!isInRange)
        {
            rigid.velocity = new Vector2(randomX * moveSpeed, rigid.velocity.y);
            FlipSprite();
            MoveCheck();
            RaycastHit2D rayHit = Physics2D.Raycast(new Vector3(transform.position.x + randomX * 0.7f, transform.position.y + 0.8f), Vector3.down, 1f, LayerMask.GetMask("Wall", "Ground"));
            if (rayHit.collider != null && rayHit.collider.CompareTag("Wall"))
                Turn();
            else if (rayHit.collider == null)
                Turn();
        }
    }


    protected void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        Vector2 direction = player.transform.position - transform.position;
        if (direction.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            currentDirection = 1;
        }
        else if (direction.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            currentDirection = -1;
        }

        if (distance > attackDistance)
        {
            FollowPlayer();
        }
        else if (attackDistance >= Mathf.Abs(distance) && !isCooling)
        {
            anim.SetBool("isRun", false);
            if (!isDead)
            {
                anim.SetTrigger("Attack");
                isCooling = true;
                intTimer = timer;
            }
        }
    }

    void FollowPlayer()
    {
        anim.SetBool("isRun", true);
        Vector2 targetPos = new Vector2(player.transform.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void Move()
    {
        randomX = Random.Range(-1, 2);

        float nextMoveTime = Random.Range(3f, 6f);
        Invoke("Move", nextMoveTime);
    }

    void FlipSprite()
    {
        if (randomX > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            currentDirection = 1;
        }

        else if (randomX < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            currentDirection = -1;
        }
    }

    void MoveCheck()
    {
        if (Mathf.Abs(randomX) > Mathf.Epsilon)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }

    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.right * rayCastLength * currentDirection, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.right * rayCastLength * currentDirection, Color.green);
        }
    }

    void Turn()
    {
        randomX *= -1;

        CancelInvoke();
        Invoke("Move", 2);
    }

    private void OnDestroy()
    {
        if (gameObject.name.Contains("도적"))
        {
            foreach (var key in player.progressQuest.Keys)
            {
                if (player.progressQuest[key].needTargetName == "도적")
                {
                    player.progressQuest[key].progressPercent++;
                    if (player.progressQuest[key].progressPercent >= player.progressQuest[key].targetNum)
                    {
                        player.progressQuest[key].progressPercent = player.progressQuest[key].targetNum;
                        player.progressQuest[key].isComplete = true;
                    }
                    if (player.progressQuest[key].isComplete != true)
                        GameManager.instance.StartCoroutine(GameManager.instance.ChangeInfoText($"{player.progressQuest[key].needTargetName}  {player.progressQuest[key].progressPercent} / {player.progressQuest[key].targetNum}"));
                }
            }
        }
    }
}

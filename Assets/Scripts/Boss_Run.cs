using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rigid;
    Boss boss;

    // 새로운 상태로 변할 때 실행
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<Character>().transform;
        rigid = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // 처음과 마지막 프레임을 제외한 각 프레임 단위로 실행
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(player.position.x, rigid.position.y);
        rigid.position = Vector2.MoveTowards(rigid.position, target, 2f * Time.deltaTime);

        Vector2 direction = (Vector2)player.position - rigid.position;
        if (direction.x > 0)
        {
            rigid.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            rigid.transform.eulerAngles = new Vector3(0, 180, 0);
        }


        if (Vector2.Distance(player.position, rigid.position) <= 2f)
        {
            if (!boss.isCooling)
            {
                int temp = Random.Range(0, 6);
                if (temp != 0)
                {
                    animator.SetTrigger("Attack1");
                    boss.isCooling = true;
                    boss.intTimer = boss.timer;
                }
                else
                {
                    animator.SetTrigger("RangeAttack");
                    boss.isCooling = true;
                    boss.intTimer = boss.timer;
                }

            }
            else if (boss.isCooling)
                animator.SetTrigger("Idle");

        }
        else if(Vector2.Distance(player.position, rigid.position) > 3f)
        {
            if (Random.Range(0, 100) == 5 && !boss.isCooling)
            {
                animator.SetTrigger("RangeAttack");
                boss.isCooling = true;
                boss.intTimer = boss.timer;
            }
        }
    }

    // 상태가 다음 상태로 바뀌기 직전에 실행
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    //// MonoBehaviour.OnAnimatorMove 직후에 실행
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //// MonoBehaviour.OnAnimatorIK 직후에 실행
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //// 스크립트가 부착된 상태 기계로 전환이 왔을때 실행
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //}

    //// 스크립트가 부착된 상태 기계에서 빠져나올때 실행
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //}

}

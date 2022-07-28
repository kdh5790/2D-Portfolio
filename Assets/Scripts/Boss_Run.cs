using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rigid;
    Boss boss;

    // ���ο� ���·� ���� �� ����
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<Character>().transform;
        rigid = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // ó���� ������ �������� ������ �� ������ ������ ����
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

    // ���°� ���� ���·� �ٲ�� ������ ����
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    //// MonoBehaviour.OnAnimatorMove ���Ŀ� ����
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //// MonoBehaviour.OnAnimatorIK ���Ŀ� ����
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    //// ��ũ��Ʈ�� ������ ���� ���� ��ȯ�� ������ ����
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //}

    //// ��ũ��Ʈ�� ������ ���� ��迡�� �������ö� ����
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //}

}

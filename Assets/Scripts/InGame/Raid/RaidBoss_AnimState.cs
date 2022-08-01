using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidBoss_AnimState : StateMachineBehaviour
{
    public static bool isInit = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isInit == false)
        {
            isInit = true;
        }
    }
}

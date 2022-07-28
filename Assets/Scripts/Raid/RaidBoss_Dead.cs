using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class RaidBoss_Dead : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FindObjectOfType<RaidManager>().RaidClear();
        Photon.Pun.PhotonNetwork.Destroy(animator.gameObject);
        RaidBoss_AnimState.isInit = false;
    }
}

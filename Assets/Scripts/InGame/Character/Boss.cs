using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    public string bossName;
    public Text nameText;
    public Image hpFillamount;
    public float maxHp;
    public GameObject portal;

    Vector3 temp;

    protected override void Awake()
    {
        base.Awake();
        nameText.text = bossName;
    }

    void MoveToTarget()
    {
        temp = transform.position;
        transform.position = player.transform.position;
    }

    void MoveToOriginPos()
    {
        transform.position = temp;
    }

    public override void Damaged(int _damage)
    {
        base.Damaged(_damage);
        hpFillamount.fillAmount = currentHp / maxHp;
    }

    public override void Die()
    {
        base.Die();
        Instantiate(portal, transform.position, Quaternion.identity).GetComponent<Portal>();
    }
    private void OnDestroy()
    {
        if (gameObject.name.Contains("档利 滴格"))
        {
            foreach (var key in player.progressQuest.Keys)
            {
                if (player.progressQuest[key].needTargetName == "档利 滴格")
                {
                    player.progressQuest[key].progressPercent++;
                    if (player.progressQuest[key].progressPercent >= player.progressQuest[key].targetNum)
                    {
                        player.progressQuest[key].progressPercent = player.progressQuest[key].targetNum;
                        player.progressQuest[key].isComplete = true;
                    }
                }
            }
        }
    }
}


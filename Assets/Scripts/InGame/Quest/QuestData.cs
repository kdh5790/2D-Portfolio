using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Reward
{
    public int gold;
    public float exp;
    public int itemID;
    public Reward(int _gold, float _exp, int _itemID = 0)
    {
        gold = _gold;
        exp = _exp;
        itemID = _itemID;
    }
}

public class Target
{
    public const int npc = 0, enemy = 1, item = 2;

    public int category;

    public int needTargetID; // npc = npcID, enemy = enemyName, item = itemID
    public string needTargetName;
    public int progressPercent; // 진행도
    public int targetNum; // 얼마나 필요한지 // npc = null

    public bool isComplete;

    public Target(int _category, string _enemyName = "", int _targetNum = 0) // enemy
    {
        category = _category;
        needTargetName = _enemyName;
        progressPercent = 0;
        targetNum = _targetNum;

        isComplete = false;
    }

    public Target(int _category, int _targetID, int _targetNum) // item
    {
        category = _category;
        needTargetID = _targetID;
        progressPercent = 0;
        targetNum = _targetNum;

        isComplete = false;
    }

    public Target(int _category, int _targetID) // npc
    {
        category = _category;
        needTargetID = _targetID;
    }
}

public class QuestData
{
    public int questID;
    public string questName;
    public string questDiscription;
    public int npcID;
    public int requiredLevel;
    public int precedenceQuestID;
    public Reward reward;
    public Target target;


    public QuestData(int _questID, string _questName, string _questDiscription, int _npcID, int _requiredLevel, Reward _reward, Target _target, int _precedenceQuestID = 0)
    {
        questID = _questID;
        questName = _questName;
        questDiscription = _questDiscription;
        npcID = _npcID;
        requiredLevel = _requiredLevel;
        reward = _reward;
        target = _target;
        precedenceQuestID = _precedenceQuestID;
    }
}

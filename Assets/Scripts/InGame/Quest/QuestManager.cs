using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    Character player;

    public Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        InitQuest();
    }

    void Start()
    {
        player = FindObjectOfType<Character>();
    }

    void InitQuest()
    {
        questList.Add(10, new QuestData(10, "도적 소탕하기", "전투 경험을 쌓기 위해 도적을 5명 처치 해주세요.", 10000, 1, new Reward(300, 500f, 5021), new Target(Target.enemy, "도적", 5)));
        questList.Add(11, new QuestData(11, "도적 두목과 전투", "도적 두목에게 가는 길을 열어드렸습니다. 그를 처치 해주세요.", 10000, 1, new Reward(2500, 1500f, 5003), new Target(Target.enemy, "도적 두목", 1),10));
    }

    public void FindQuest()
    {
        var quest = new Dictionary<int, QuestData>();

        foreach (int key in questList.Keys)
        {
            if (questList[key].precedenceQuestID == 0)
            {
                if (player.completedQuest.Count != 0)
                {
                    for (int i = 0; i < player.completedQuest.Count; i++)
                    {
                        if (key != player.completedQuest[i]) // 완료한 퀘스트 목록에 없다면
                        {
                            Debug.Log(questList[key].questName);
                            player.acceptableQuest.Add(key);
                            break;
                        }
                    }
                }

                else
                    player.acceptableQuest.Add(key);
            }

            else
            {
                for (int j = 0; j < player.completedQuest.Count; j++)
                {
                    if (questList[key].precedenceQuestID == player.completedQuest[j])
                    {
                        Debug.Log(questList[key].questName);
                        player.acceptableQuest.Add(key);
                        break;
                    }
                }
            }
        }
    }

    public Dictionary<int, QuestData> GetNPCQuest(int npcID)
    {
        Dictionary<int, QuestData> returnData = new Dictionary<int, QuestData>();

        foreach (int key in questList.Keys)
        {
            if (questList[key].npcID == npcID)
            {
                returnData.Add(key, questList[key]);
            }
        }

        return returnData;
    }

    public QuestData GetUIIndexQuest(int questID)
    {
        QuestData temp;
        foreach (int key in questList.Keys)
        {
            if (key == questID)
            {
                temp = questList[key];
                return temp;
            }
        }
        return null;
    }
}

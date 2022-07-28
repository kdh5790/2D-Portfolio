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
        questList.Add(10, new QuestData(10, "���� �����ϱ�", "���� ������ �ױ� ���� ������ 5�� óġ ���ּ���.", 10000, 1, new Reward(300, 500f, 5021), new Target(Target.enemy, "����", 5)));
        questList.Add(11, new QuestData(11, "���� �θ�� ����", "���� �θ񿡰� ���� ���� �����Ƚ��ϴ�. �׸� óġ ���ּ���.", 10000, 1, new Reward(2500, 1500f, 5003), new Target(Target.enemy, "���� �θ�", 1),10));
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
                        if (key != player.completedQuest[i]) // �Ϸ��� ����Ʈ ��Ͽ� ���ٸ�
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

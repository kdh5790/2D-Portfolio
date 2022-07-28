using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    public int npcID;
    public bool isNpc = true;
    public bool isQuest;
    public Dictionary<int, QuestData> questData;

    QuestManager questManager;

    private void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();
        questData = new Dictionary<int, QuestData>();
    }

    private void Start()
    {
        StartCoroutine(GetData());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach (var key in questData.Keys)
            {
                Debug.Log(questData[key].questName);
            }
        }
    }

    IEnumerator GetData()
    {
        yield return new WaitForSeconds(0.5f);

        questData = questManager.GetNPCQuest(npcID);

        if (questData != null)
        {
            foreach (var key in questData.Keys)
            {
                Debug.Log(questData[key].questName);
            }
            isQuest = true;
        }
        else if (questData == null)
            isQuest = false;
    }

    public void GetQuestData()
    {
        questData = questManager.GetNPCQuest(npcID);
        if (questData != null)
            isQuest = true;
        else if (questData == null)
            isQuest = false;
    }

    public Dictionary<int, QuestData> IsAcceptable(List<int> acceptableQuest)
    {
        foreach (int key in questData.Keys)
        {
            for (int i = 0; i < acceptableQuest.Count; i++)
            {
                if (key == acceptableQuest[i])
                {
                    var temp = new Dictionary<int, QuestData>();
                    temp.Add(key, questData[key]);
                    return temp;
                }
            }
        }
        return null;
    }
}

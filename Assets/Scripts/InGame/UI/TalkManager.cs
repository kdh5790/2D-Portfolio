using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        AddTalkData();
    }

    void AddTalkData()
    {
        talkData.Add(10000, new string[] { $"어서오세요."});
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;

        else
            return talkData[id][talkIndex];
    }
}

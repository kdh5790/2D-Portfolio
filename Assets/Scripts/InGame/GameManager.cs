using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Character player;
    NPCData npcData;
    QuestUI questUI;

    Dictionary<int, QuestData> questData;

    public static GameManager instance;

    public Text infoText;
    public Text talkText;
    public TalkManager talkManager;
    public GameObject scanObject;
    public GameObject revivePos;

    public int talkIndex;

    public bool isTalk;

    public bool isQuest;

    private void Awake()
    {
        if (FindObjectOfType<Character>() == null)
        {
            if (FirebaseManager.instance.playerClass == 1)
            {
                GameObject obj = Instantiate(Resources.Load("Warrior"), Vector3.zero, Quaternion.identity) as GameObject;
                obj.GetComponent<Warrior>().enabled = true;
            }
            else
            {
                GameObject obj = Instantiate(Resources.Load("Archer"), Vector3.zero, Quaternion.identity) as GameObject;
                obj.GetComponent<Archer>().enabled = true;
            }
        }
    }

    private void Start()
    {
        instance = this;
        player = FindObjectOfType<Character>();
        questUI = FindObjectOfType<QuestUI>();
        questData = new Dictionary<int, QuestData>();
    }

    private void Update()
    {
        if (isTalk && Input.GetKeyDown(KeyCode.Space))
        {
            OnTalk(npcData.npcID, npcData.isNpc);
        }
    }

    public void ShowText(GameObject scanObj)
    {
        isTalk = true;
        scanObject = scanObj;
        npcData = scanObject.GetComponent<NPCData>();
        OnTalk(npcData.npcID, npcData.isNpc);
    }

    void OnTalk(int id, bool isNpc)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if (talkData == null)
        {

            if (npcData.isQuest)
            {
                foreach (var key in player.progressQuest.Keys)
                {
                    if (npcData.questData[key] != null && key == npcData.questData[key].questID && player.progressQuest[key].isComplete)
                    {
                        talkText.text = "현재 완료 가능한 퀘스트가 있습니다.";
                        StartCoroutine(questUI.Request(questData, key, true));
                        npcData.questData.Remove(key);
                        npcData.GetQuestData();

                        return;
                    }
                    else
                        break;
                }

                questData = npcData.IsAcceptable(player.acceptableQuest);
                if (questData != null)
                {
                    foreach (var key in questData.Keys)
                    {
                        talkText.text = "현재 수락 가능한 퀘스트가 있습니다.";
                        StartCoroutine(questUI.Request(questData, key, false));
                        return;
                    }
                }

                else
                {
                    UIManager.instance.talkPanel.SetActive(false);
                    Invoke("EndTalk", 0.1f);
                    return;
                }
            }
            else
            {
                UIManager.instance.talkPanel.SetActive(false);
                Invoke("EndTalk", 0.1f);
                return;
            }
        }

        UIManager.instance.talkPanel.SetActive(true);
        talkText.text = talkData;

        talkIndex++;
    }

    public void EndTalk()
    {
        scanObject = null;
        talkIndex = 0;
        Character.instance.isTalk = false;
        isTalk = false;
    }

    public void ContinueTalk() // Button
    {
        if (isTalk)
        {
            OnTalk(npcData.npcID, npcData.isNpc);
        }
    }

    public void Revive()
    {
        player.anim.SetBool("isDead", false);
        player.isDead = false;  
        player.transform.position = revivePos.transform.position;
        player.currentHp = player.maxHp;
        player.currentMp = player.maxMp;
        UIManager.instance.revivePanel.SetActive(false);
    }

    public void AdRevive()
    {
        player.walkSpeed = 5f;
        player.anim.SetBool("isDead", false);
        player.isDead = false;
        player.currentHp = player.maxHp;
        player.currentMp = player.maxMp;
        UIManager.instance.revivePanel.SetActive(false);
    }

    public  IEnumerator ChangeInfoText(string _text)
    {
        infoText.gameObject.SetActive(true);
        infoText.text = _text;
        yield return new WaitForSeconds(3.0f);
        infoText.text = "";
        infoText.gameObject.SetActive(false);
    }

    public IEnumerator Ending()
    {
        yield return new WaitForSeconds(2.0f);
        FindObjectOfType<Character>().SaveData();
        Destroy(FindObjectOfType<Character>());
        FirebaseManager.instance.auth.SignOut();
        Destroy(FirebaseManager.instance.gameObject);
        SceneManager.LoadScene("FirstScene");
    }

    public void Logout()
    {
        FirebaseManager.instance.auth.SignOut();
        Application.Quit();
    }


    public void Gold()
    {
        FindObjectOfType<Character>().playerGold += 10000;
    }

    public void Exp()
    {
        FindObjectOfType<Character>().currentExp += 3000;   
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    Character player;
    QuestManager questManager;
    ItemData itemData;
    [SerializeField] QuestUIIndex[] questList;
    GameManager gameManager;

    Dictionary<int, QuestData> questData;

    [Header("ProgressQuestPanel")]
    public Text nameText;
    public Text discriptionText;
    public Text progressText;
    public Text rewardText;
    public Image itemImage;

    [Header("RequestQuestPanel")]
    public Text rqNameText;
    public Text rqDiscriptionText;
    public Text rqRewardText;
    public Image rqItemImage;
    public GameObject acceptButton;
    public GameObject cancelButton;
    public GameObject completeButton;

    int questID;

    void Start()
    {
        player = FindObjectOfType<Character>();
        questManager = FindObjectOfType<QuestManager>();
        gameManager = FindObjectOfType<GameManager>();
        itemData = FindObjectOfType<ItemData>();
        questData = new Dictionary<int, QuestData>();
    }

    public void ClearUI()
    {
        nameText.text = "";
        discriptionText.text = "";
        rewardText.text = "";
        progressText.text = "";
        itemImage.gameObject.SetActive(false);
    }

    public void Progress(QuestData data)
    {
        nameText.text = data.questName;
        discriptionText.text = data.questDiscription;
        if (data.reward.itemID == 0)
        {
            rewardText.text = $"골드 : {data.reward.gold}  경험치 : {data.reward.exp}EXP";
            itemImage.gameObject.SetActive(false);
        }
        else if (data.reward.itemID != 0)
        {
            rewardText.text = $"골드 : {data.reward.gold}  경험치 : {data.reward.exp}EXP\n아이템 : ";
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = Resources.Load<Sprite>("Texture/" + data.reward.itemID.ToString());
        }

        switch(data.target.category)
        {
            case Target.npc:
                progressText.text = "";
                break;
            case Target.enemy:
                progressText.text = $"{data.target.needTargetName} 처치하기 {player.progressQuest[data.questID].progressPercent} / {data.target.targetNum}";
                break;
            case Target.item:
                progressText.text = $"{itemData.GetItemName(data.target.needTargetID)} 획득하기 {player.progressQuest[data.questID].progressPercent} / {data.target.targetNum}";
                break;
        }
        if (player.progressQuest[data.questID].isComplete)
            progressText.text += "\n퀘스트 완료.";
    }

    public IEnumerator Request(Dictionary<int, QuestData> quest, int key, bool isComplete)
    {
        yield return new WaitForSeconds(1.0f);

        questID = key;
        questData = quest;
        
        if (!isComplete)
        {
            acceptButton.SetActive(true);
            cancelButton.SetActive(true);
            completeButton.SetActive(false);
        }

        else if(isComplete)
        {
            acceptButton.SetActive(false);
            cancelButton.SetActive(false);
            completeButton.SetActive(true);
        }

        UIManager.instance.requestPanel.SetActive(true);
        rqNameText.text = quest[key].questName;
        rqDiscriptionText.text = quest[key].questDiscription;
        if (quest[key].reward.itemID == 0)
        {
            rqRewardText.text = $"골드 : {quest[key].reward.gold}  경험치 : {quest[key].reward.exp}EXP";
            rqItemImage.gameObject.SetActive(false);
        }
        else if (quest[key].reward.itemID != 0)
        {
            rqItemImage.gameObject.SetActive(true);
            rqItemImage.sprite = Resources.Load<Sprite>("Texture/" + quest[key].reward.itemID.ToString());
            rqRewardText.text = $"골드 : {quest[key].reward.gold}  경험치 : {quest[key].reward.exp}EXP\n아이템 : ";
        }
    }

    void AddList()
    {
        if(player.progressQuest == null)
            return;

        // 진행중인 퀘스트 퀘스트 UI 인덱스에 추가하기
        else if (player.progressQuest != null)
        {
            for (int i = 0; i < player.progressQuest.Count; i++)
            {
                questList[i].questData = questManager.GetUIIndexQuest(questID);
                questList[i].GetComponentInChildren<Text>().text = questList[i].questData.questName;
            }
        }
    }

    void RemoveList()
    {

    }

    public void OKButton()
    {
        int temp = player.acceptableQuest.FindIndex(x => x == questID);
        player.acceptableQuest.RemoveAt(temp);

        player.progressQuest.Add(questID, questData[questID].target);

        rqNameText.text = "";
        rqDiscriptionText.text = "";
        rqRewardText.text = "";
        rqItemImage.sprite = null;

        UIManager.instance.requestPanel.SetActive(false);
        UIManager.instance.talkPanel.SetActive(false);
        AddList();
        gameManager.EndTalk();
    }

    public void CancelButton()
    {
        rqNameText.text = "";
        rqDiscriptionText.text = "";
        rqRewardText.text = "";
        rqItemImage.sprite = null;

        UIManager.instance.requestPanel.SetActive(false);
        UIManager.instance.talkPanel.SetActive(false);
        gameManager.EndTalk();
    }

    public void CompleteButton()
    {
        player.currentExp += questData[questID].reward.exp;
        player.playerGold += questData[questID].reward.gold;
        FindObjectOfType<Inventory>().GetItem(questData[questID].reward.itemID);

        player.progressQuest.Remove(questID);
        player.completedQuest.Add(questID);

        rqNameText.text = "";
        rqDiscriptionText.text = "";
        rqRewardText.text = "";
        rqItemImage.sprite = null;

        questManager.FindQuest();

        UIManager.instance.requestPanel.SetActive(false);
        UIManager.instance.talkPanel.SetActive(false);
        gameManager.EndTalk();

        switch(questID)
        {
            case 10:
                StartCoroutine(FindObjectOfType<Lerp>().WallOpenEvent());
                break;
            case 11:
                UIManager.instance.endPanel.SetActive(true);
                StartCoroutine(gameManager.Ending());
                break;
        }
      
    }
}

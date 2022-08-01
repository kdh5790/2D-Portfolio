using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Button pvpButton;
    public Button raidButton;

    public GameObject endPanel;
    public GameObject settingPanel;
    public GameObject revivePanel;
    public GameObject talkPanel;
    public GameObject infoPanel;
    public GameObject inventoryUI;
    public GameObject scanObject;
    public GameObject questPanel;
    public GameObject requestPanel;
    public GameObject loadingPanel;
    public Text talkText;
    public Text infoText;
    public AudioClip openUiSound;

    public Image fadeoutPanel;

    QuestUI questUI;

    public string npcName;

    [HideInInspector] public bool isInventory = false; // 인벤토리 활성화 여부

    [HideInInspector] public bool isAction = false;

    bool isPanel = false;

    private void Awake()
    {
        instance = this;
        questUI = FindObjectOfType<QuestUI>();
    }

    public void OpenQuestPanel()
    {
        AudioManager.instance.PlayClip(openUiSound);
        isPanel = true;
        questPanel.SetActive(true);
    }

    public void CloseQuestPanel()
    {
        AudioManager.instance.PlayClip(openUiSound);
        isPanel = false;
        questPanel.SetActive(false);
        questUI.ClearUI();
    }

    public void OpenSettingUI()
    {
        AudioManager.instance.PlayButtonClip();
        settingPanel.SetActive(true);
    }

    public void CloseSettingUI()
    {
        AudioManager.instance.PlayButtonClip();
        settingPanel.SetActive(false);
    }

    public void PVPButton()
    {
                AudioManager.instance.PlayButtonClip();
                FindObjectOfType<Character>().SaveData();
                FindObjectOfType<Character>().isDead = true;
                SceneManager.LoadScene(2);
    }

    public void RaidButton()
    {
                AudioManager.instance.PlayButtonClip();
                FindObjectOfType<Character>().SaveData();
                FindObjectOfType<Character>().isDead = true;
                SceneManager.LoadScene(4);
    }

    public void CancelButton()
    {
        AudioManager.instance.PlayButtonClip();
        Character.instance.isTalk = false;
        infoPanel.SetActive(false);
    }

    public IEnumerator FadeCoroutine()
    {
        fadeoutPanel.gameObject.SetActive(true);
        float fadeCount = 1;
        fadeoutPanel.color = new Color(0, 0, 0, fadeCount);
        while (fadeCount >=  0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeoutPanel.color = new Color(0, 0, 0, fadeCount);
        }
        fadeoutPanel.gameObject.SetActive(false);
    }

    public void SaveButton()
    {
        AudioManager.instance.PlayButtonClip();
        FindObjectOfType<Character>().SaveData();
    }


    public void ExitButton()
    {
        AudioManager.instance.PlayButtonClip();
        FindObjectOfType<Character>().SaveData();
        Application.Quit();
  
    }

    public void MultiPlay()
    {
        var player = FindObjectOfType<Character>();
        infoText.text = "플레이 할 컨텐츠를 선택해주세요.\n(인터넷 연결이 필요합니다.)";
        Debug.Log(player.playerClass);
        infoPanel.SetActive(true);   
        
    }
}

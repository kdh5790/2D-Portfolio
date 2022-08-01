using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIManager : MonoBehaviour
{
    public static StatusUIManager instance;

    public Text playerNameTxt;
    public Text playerLevelTxt;
    public Text playerExpTxt;
    public Text playerGoldTxt;

    public Image expBar;
    public Image hpBar;
    public Image mpBar;

    public Character player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Update()
    {
        PlayerStatusUpdate();
    }

    public void PlayerStatusUpdate()
    {
        player = FindObjectOfType<Character>();
        if (player != null)
        {
            playerNameTxt.text = player.playerName;
            playerGoldTxt.text = player.playerGold.ToString();
            playerLevelTxt.text = "Level " + player.playerLevel.ToString();
            playerExpTxt.text = "EXP : " + player.currentExp / (player.maxExp / 100f) + "%";
            expBar.fillAmount = (player.currentExp / (player.maxExp / 100f)) / 100f;
            hpBar.fillAmount = player.currentHp / (player.maxHp + player.equipHp);
            mpBar.fillAmount = player.currentMp / (player.maxMp + player.equipMp);
        }
    }
}

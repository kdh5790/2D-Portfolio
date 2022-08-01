using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PVPManager : MonoBehaviourPunCallbacks
{
    public static PVPManager instance;

    public Transform rightSpawn;
    public Transform leftSpawn;

    public GameObject[] leftWinImage;
    public GameObject[] leftDefeatImage;

    public GameObject[] rightWinImage;
    public GameObject[] rightDefeatImage;

    private int round = 0;
    private float timer;

    public Button spawnButton;
    //public PhotonView ptView;

    public Text leftText;
    public Text rightText;
    public Text roundText;
    public Text timerText;
    public Text scoreText;

    private IEnumerator gameTimerCoroutine;
    private IEnumerator startTimerCoroutine;

    public bool isStart = false;
    public bool isDraw = false;

    public int leftWin;
    public int rightWin;

    public List<PVPCharacter> players = new List<PVPCharacter>();

    private PVPSkillManager skillManager;
    private PVPQuickSlot[] quickSlots;

    void Awake()
    {
        instance = this;
        skillManager = FindObjectOfType<PVPSkillManager>();
        quickSlots = FindObjectsOfType<PVPQuickSlot>();
        roundText.text = $"Round {round + 1}";
    }

    public void Spawn()
    {
        if (FirebaseManager.instance.playerClass == 1)
        {
            PhotonNetwork.Instantiate("PVPWarrior", PhotonNetwork.IsMasterClient ? leftSpawn.position : rightSpawn.position, Quaternion.identity).GetComponent<PVPCharacter>().isPVP = true; // -5, -1f    30f, -1f
            for (int i = 0; i < quickSlots.Length; i++)
            {
                quickSlots[i].SetSkill(skillManager.warriorSkills[i]);
            }
            spawnButton.gameObject.SetActive(false);
        }
        else if (FirebaseManager.instance.playerClass == 2)
        {
            PhotonNetwork.Instantiate("PVPArcher", PhotonNetwork.IsMasterClient ? leftSpawn.position : rightSpawn.position, Quaternion.identity).GetComponent<PVPCharacter>().isPVP = true;
            for (int i = 0; i < quickSlots.Length; i++)
            {
                quickSlots[i].SetSkill(skillManager.archerSkills[i]);
            }
            spawnButton.gameObject.SetActive(false);
        }
        spawnButton.gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            leftText.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            rightText.text = PhotonNetwork.LocalPlayer.NickName;
        }

        StartCoroutine(DeathCheck());
        StartCoroutine(PlayerCheck());
    }


    IEnumerator PlayerCheck()
    {
        while (players.Count < 2)
        {
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i < players.Count; i++)
            players[i].isStart = true;
        isStart = true;
        gameTimerCoroutine = TimerCoroutine();
        StartCoroutine(gameTimerCoroutine);
    }

    IEnumerator TimerCoroutine()
    {
        timer = 120f;

        while (timer > 0 && isStart)
        {
            timer -= Time.deltaTime;
            timerText.text = ((int)timer).ToString();
            yield return null;

            if (timer <= 0)
            {
                isDraw = true;
                StopCoroutine(gameTimerCoroutine);
            }
        }
    }

    IEnumerator DeathCheck()
    {
        while (true)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].currentHp <= 0)
                {
                    players[i].isDead = true;
                    players[i].isKill = false;
                    for (int j = 0; j < players.Count; j++)
                    {
                        if (players[i] != players[j])
                        {
                            players[j].isKill = true;
                            players[j].isDead = false;
                            StopCoroutine(gameTimerCoroutine);
                            yield return new WaitForSeconds(1f);
                            ScoreSet(players[j], players[i]);
                            yield break;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void ScoreSet(PVPCharacter _winner, PVPCharacter _loser)
    {
        if (_winner.isMaster && !isDraw)
        {
            leftWin++;
            leftWinImage[round].SetActive(true);
            leftDefeatImage[round].SetActive(false);
            rightDefeatImage[round].SetActive(true);
            rightWinImage[round].SetActive(false);
        }
        else if (!_winner.isMaster && !isDraw)
        {
            rightWin++;
            rightWinImage[round].SetActive(true);
            rightDefeatImage[round].SetActive(false);
            leftDefeatImage[round].SetActive(true);
            leftWinImage[round].SetActive(false);
        }
        else if (isDraw)
        {
            rightDefeatImage[round].SetActive(true);
            rightWinImage[round].SetActive(false);
            leftDefeatImage[round].SetActive(true);
            leftWinImage[round].SetActive(false);
        }
        isDraw = false;
        isStart = false;

        round++;
        if (round <= 2)
            StartCoroutine(SetPosition());
        else if (round > 2)
            StartCoroutine(GameSet());

    }

    IEnumerator SetPosition()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].isMaster)
            {
                players[i].transform.position = leftSpawn.position;
                players[i].currentHp = players[i].maxHp;
                players[i].SetFillAmount();
                players[i].isDead = false;
                players[i].ptView.RPC("RPCBoolAnimation", RpcTarget.AllBuffered, "isDead", false);
               players[i].ptView.RPC("ResetCollider", RpcTarget.All);
                for (int j = 0; j < players.Count; j++)
                {
                    if (players[i] != players[j] && !players[j].isMaster)
                    {
                        players[j].currentHp = players[j].maxHp;
                        players[j].transform.position = rightSpawn.position;
                        players[j].SetFillAmount();
                        players[j].isDead = false;
                        players[j].ptView.RPC("RPCBoolAnimation", RpcTarget.AllBuffered, "isDead", false);
                        players[j].ptView.RPC("ResetCollider", RpcTarget.All);
                        RoundStart();
                        yield break;
                    }
                }
            }
        }
    }

    IEnumerator GameSet()
    {

        scoreText.gameObject.SetActive(true);
        if (leftWin > rightWin)
            scoreText.text = $"<color=#00ff00>{leftWin}</color> : <color=#ff0000>{rightWin}</color> \nGame Set";
        else if (rightWin > leftWin)
            scoreText.text = $"<color=#ff0000>{leftWin}</color> : <color=#00ff00>{rightWin}</color> \nGame Set";
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll();
        yield return new WaitForSeconds(3f);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("PVPLobby");

    }
    void RoundStart()
    {
        roundText.text = $"Round {round + 1}";
        isStart = true;
        timer = 120f;
        StartCoroutine(gameTimerCoroutine);
        StartCoroutine(DeathCheck());
    }
}

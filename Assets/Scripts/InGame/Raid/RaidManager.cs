using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Firebase.Database;



public class RaidManager : MonoBehaviourPunCallbacks
{
    public class Reward
    {
        public int playerGold;
        public float currentExp;
    }

    public static RaidManager instance;

    private PVPSkillManager skillManager;
    private PVPQuickSlot[] quickSlots;
    private DatabaseReference reference;

    public List<PVPCharacter> players;
    public Transform[] spawnPos;
    public GameObject spawnButton;
    public Character player;

    float spawnTimer;
    //float itemSpawnTimer;

    private void Awake()
    {
        instance = this;
        skillManager = FindObjectOfType<PVPSkillManager>();
        quickSlots = FindObjectsOfType<PVPQuickSlot>();
        players = new List<PVPCharacter>();
        player = FindObjectOfType<Character>();
        spawnTimer = 10f;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void Spawn()
    {
        if (FirebaseManager.instance.playerClass == 1)
        {
            var character = PhotonNetwork.Instantiate("PVPWarrior", PhotonNetwork.IsMasterClient ? spawnPos[0].position : spawnPos[1].position, Quaternion.identity).GetComponent<PVPCharacter>();
            character.isPVP = false;
            character.playerDamage = player.playerDamage + player.equipDamage;
            character.playerDef = player.playerDef + player.equipDef;
            character.currentHp = player.maxHp + player.equipHp;
            character.maxHp = player.maxHp + player.equipHp;
            character.currentMp = player.maxMp + player.equipMp;
            character.maxMp = player.maxMp + player.equipMp;

            for (int i = 0; i < quickSlots.Length; i++)
            {
                quickSlots[i].SetSkill(skillManager.warriorSkills[i]);
            }
        }
        else if (FirebaseManager.instance.playerClass == 2)
        {
            var character = PhotonNetwork.Instantiate("PVPArcher", PhotonNetwork.IsMasterClient ? spawnPos[0].position : spawnPos[1].position, Quaternion.identity).GetComponent<PVPCharacter>();
            character.isPVP = false;
            character.playerDamage = player.playerDamage + player.equipDamage;
            character.playerDef = player.playerDef + player.equipDef;
            character.currentHp = player.maxHp + player.equipHp;
            character.maxHp = player.maxHp + player.equipHp;
            character.currentMp = player.maxMp + player.equipMp;
            character.maxMp = player.maxMp + player.equipMp;
            for (int i = 0; i < quickSlots.Length; i++)
            {
                quickSlots[i].SetSkill(skillManager.archerSkills[i]);
            }
        }
        spawnButton.SetActive(false);
    }

    public void SpawnBoss()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate("RaidBoss", Vector3.zero, Quaternion.identity);
    }

    public void RaidClear()
    {
        Reward reward = new Reward();

        reward.playerGold = player.playerGold + 3500;
        reward.currentExp = player.currentExp + 5000;

        reference.Child("users").Child(FirebaseManager.instance.user.UserId).Child("playerGold").SetValueAsync(reward.playerGold);
        reference.Child("users").Child(FirebaseManager.instance.user.UserId).Child("currentExp").SetValueAsync(reward.currentExp);

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("RaidLobby");
    }

    public void RaidFail()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("RaidLobby");
    }

    public IEnumerator SpawnPlayer(PVPCharacter _player)
    {
        spawnTimer = 10.0f;
        while(spawnTimer <= 0)
        {
            spawnTimer -= Time.deltaTime;
            Debug.Log(spawnTimer);
            yield return null;
        }
    }
}

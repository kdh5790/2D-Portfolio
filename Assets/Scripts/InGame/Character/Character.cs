using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using Firebase.Database;

public abstract class Character : MonoBehaviour
{
    public const int Warrior = 1, Archer = 2;
    public static Character instance;
    public AudioClip clip;
    public AudioSource source;

    public class Data
    {
        public int playerClass;

        public string playerName = FirebaseManager.instance.user.DisplayName;
        public int playerLevel;
        public int playerGold;
        public float playerPosX;
        public float playerPosY;
        public float currentExp;
        public float maxExp;


        public float playerHp;
        public float playerMp;
        public float playerDef;
        public int playerDamage;
        public int playerStatusPoint;
        public int playerSkillPoint;

        public List<int> playerEquipItem;
        public List<int> playerInventoryItem;
        public List<int> playerInventoryItemCount;
    }

    protected AttackButton actionBtn;
    protected Joystick joystick;
    protected GameObject scanNPC;
    protected CapsuleCollider2D col;
    protected Rigidbody2D rigid;
    public Animator anim;
    public QuestManager questData;
    public ItemData itemData;
    public DatabaseReference reference;
    public Inventory inven;

    protected RaycastHit2D hitNPC;
    protected RaycastHit2D hitItem;

    public int playerClass;

    #region Status
    public string playerName;
    public int playerLevel;
    public int playerGold;

    public float playerDef;
    public int playerDamage;

    public int playerStatusPoint;
    public int playerSkillPoint;

    public float currentExp;
    public float maxExp;

    public float currentHp;
    public float maxHp;

    public float currentMp;
    public float maxMp;
    #endregion

    #region EquipStatus
    public int equipDamage;
    public float equipHp;
    public float equipMp;
    public float equipDef;
    #endregion

    public List<SkillData> playerSkill;

    public List<int> completedQuest; // 완료 퀘
    public List<int> acceptableQuest; // 수락 가능 퀘
    public Dictionary<int, Target> progressQuest; // 진행중인 퀘

    public float walkSpeed = 5f;
    public float jumpPower;

    public bool isRoll = false;
    protected bool isAttack = false;
    public bool isGround = true;
    public bool isJump = false;
    protected bool isDoubleJump = false;
    protected bool isMove = false;
    public bool isGuard;

    public bool isDead = false;
    public bool isTalk = false;

    protected float inputX;
    protected float joystick_InputX;

    public int currentDirection = 1;
    public int currentMap;

    protected virtual void AwakeInit()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    protected virtual void StartInit()
    {
        col = GetComponent<CapsuleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        itemData = FindObjectOfType<ItemData>();
        questData = FindObjectOfType<QuestManager>();
        inven = FindObjectOfType<Inventory>();
        joystick = FindObjectOfType<Joystick>();
        actionBtn = FindObjectOfType<AttackButton>();

        progressQuest = new Dictionary<int, Target>();

        if (FirebaseManager.instance != null)
        {
            if (FirebaseManager.instance.user.DisplayName == null)
                playerName = FirebaseManager.instance.nickname;
            FirebaseManager.instance.playerClass = playerClass;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        StartCoroutine(LoadData());

        var cinemachine = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
        cinemachine.Follow = transform;
        cinemachine.LookAt = transform;
        StatusUIManager.instance.PlayerStatusUpdate();
    }


    protected virtual void UpdateInit()
    {
        if (!isDead)
        {

            anim.SetFloat("AirSpeedY", rigid.velocity.y);

            if (!UIManager.instance.isAction && !Inventory.isInven)
            {
                Attack();
                Move();
                Rolling();
            }

            if (Inventory.isInven) rigid.velocity = Vector2.zero;

            hitNPC = Physics2D.Raycast(rigid.position, new Vector2(currentDirection, 0.5f), 1f, LayerMask.GetMask("NPC"));


            if (hitNPC.collider != null)
            {
                scanNPC = hitNPC.collider.gameObject;
                actionBtn.attack.SetActive(false);
                actionBtn.check.SetActive(true);
            }
            else
            {
                scanNPC = null;
                actionBtn.attack.SetActive(true);
                actionBtn.check.SetActive(false);
            }

            if (currentExp >= maxExp)
            {
                LevelUp();
            }


            MoveCheck();
            FlipSprite();
            GroundCheck();
            StatusCheck();
        }
    }


    protected void Move()
    {
        if (!isRoll && !isTalk && !isGuard && !isAttack)
        {
            joystick_InputX = joystick.GetInputX();
            if (joystick_InputX < 0)
                joystick_InputX = -1;
            else if (joystick_InputX > 0)
                joystick_InputX = 1;
            rigid.velocity = new Vector2(joystick_InputX * walkSpeed, rigid.velocity.y);

        }
    }

    public void Jump()
    {
        if (!isRoll && !isTalk && !isJump && !isAttack)
        {
            if (!isJump)
                isJump = true;

            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }
    }

    public void Rolling(int num = 0)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || num != 0 && !isRoll && isGround && !isAttack)
        {
            rigid.velocity = new Vector2(currentDirection * 8, rigid.velocity.y);
            col.enabled = false;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
            isRoll = true;
            anim.SetTrigger("Roll");
        }
    }
    protected void FlipSprite()
    {
        if (!isRoll && !isAttack && !isDead)
        {
            if (inputX > 0 || joystick_InputX > 0)
            {
                rigid.transform.eulerAngles = new Vector3(0, 0, 0);
                currentDirection = 1;
            }

            else if (inputX < 0 || joystick_InputX < 0)
            {
                rigid.transform.eulerAngles = new Vector3(0, 180, 0);
                currentDirection = -1;
            }
        }
    }
    protected void MoveCheck()
    {
        if (Mathf.Abs(inputX) > Mathf.Epsilon || Mathf.Abs(joystick_InputX) > Mathf.Epsilon)
        {
            isMove = true;
            anim.SetBool("isRun", true);
            if (!source.isPlaying)
                source.Play();
        }
        else
        {
            isMove = false;
            anim.SetBool("isRun", false);
        }
    }

    protected abstract void GroundCheck();

    protected void StatusCheck()
    {
        if (currentHp >= maxHp + equipHp)
            currentHp = maxHp + equipHp;

        if (currentMp >= maxMp + equipHp)
            currentMp = maxMp + equipHp;
    }

    public abstract void Attack(int num = 0);

    public abstract void UsingSkill(PlayerSkillData data);
    void LevelUp()
    {
        float tempExp;
        if (currentExp - maxExp > 0)
            tempExp = currentExp - maxExp;
        else
            tempExp = 0;


        playerLevel++;
        playerStatusPoint += 3;
        maxHp += 30;
        currentHp = maxHp + equipHp;
        maxMp += 30;
        currentMp = maxMp + equipMp;
        playerSkillPoint++;
        StartCoroutine(GameManager.instance.ChangeInfoText($"레벨업! {playerLevel}"));
        FindObjectOfType<SkillUI>().PointUpdate();
        FindObjectOfType<EquipUI>().StatusUpdate();
        currentExp = 0 + tempExp;
        maxExp *= 1.1f;
    }

    public void Damaged(int _damage)
    {
        if (!isAttack || !isGuard)
        {
            anim.SetTrigger("Hurt");
        }
        currentHp -= _damage;
        FindObjectOfType<StatusUIManager>().PlayerStatusUpdate();

        if (currentHp <= 0)
        {
            walkSpeed = 0;
            anim.SetBool("isDead", true);
            isDead = true;
            UIManager.instance.revivePanel.SetActive(true);
        }
    }


    public void TalkToNPC()
    {
        if (scanNPC != null && !isTalk)
        {
            isTalk = true;
            anim.SetBool("isRun", false);
            inputX = 0;
            rigid.velocity = Vector2.zero;
            if (scanNPC.name == "Shop")
            {
                UIManager.instance.npcName = scanNPC.name;
                Shop.instance.OpenShop();
            }
            else if (scanNPC.name == "PVP")
            {
                UIManager.instance.MultiPlay();
            }
            else if (scanNPC.GetComponent<NPCData>() != null && !GameManager.instance.isTalk)
            {
                GameManager.instance.ShowText(scanNPC);
            }
            else if (scanNPC.CompareTag("Portal"))
                scanNPC.GetComponent<Portal>().MapChange();
        }
    }


    void StopRoll() { isRoll = false; col.enabled = true; ; rigid.constraints = RigidbodyConstraints2D.None; rigid.constraints = RigidbodyConstraints2D.FreezeRotation; }
    public void SaveData()
    {
        if (FirebaseManager.instance.user != null)
        {
            Data data = new Data();
            data.playerClass = playerClass;
            data.playerName = playerName;
            data.playerLevel = playerLevel;
            data.playerPosX = this.transform.position.x;
            data.playerPosY = this.transform.position.y;
            data.playerGold = playerGold;
            data.currentExp = currentExp;
            data.maxExp = maxExp;
            data.playerDamage = playerDamage;
            data.playerDef = playerDef;
            data.playerHp = maxHp;
            data.playerMp = maxMp;
            data.playerStatusPoint = playerStatusPoint;
            data.playerSkillPoint = playerSkillPoint;

            List<Item> item = inven.SaveItem();

            data.playerEquipItem = new List<int>();
            data.playerInventoryItem = new List<int>();
            data.playerInventoryItemCount = new List<int>();

            data.playerEquipItem.Clear();
            data.playerInventoryItem.Clear();
            data.playerInventoryItemCount.Clear();

            for (int i = 0; i < item.Count; i++)
            {
                data.playerInventoryItem.Add(item[i].itemID);
                data.playerInventoryItemCount.Add(item[i].itemCount);
            }

            var equipSlot = FindObjectOfType<EquipUI>().equipSlots;
            for (int i = 0; i < equipSlot.Length; i++)
            {
                if (equipSlot[i].GetComponent<EquipmentSlot>().isEquip)
                {
                    data.playerEquipItem.Add(equipSlot[i].GetComponent<EquipmentSlot>().equipItem.itemID);
                }
            }

            string json = JsonUtility.ToJson(data);

            reference.Child("users").Child(FirebaseManager.instance.user.UserId).SetRawJsonValueAsync(json);
            StartCoroutine(GameManager.instance.ChangeInfoText("저장 완료!"));
        }
    }

    public IEnumerator LoadData()
    {
        UIManager.instance.loadingPanel.SetActive(true);


        var savetask = FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync();

        yield return new WaitUntil(predicate: () => savetask.IsCompleted);

        var result = savetask.Result;

        float fx = 0;
        float fy = 0;


        List<int> equipItemID = new List<int>();
        List<int> itemID = new List<int>();
        List<int> itemCount = new List<int>();
        List<Item> itemList = new List<Item>();

        Debug.Log(FirebaseManager.instance.user.UserId);

        var count = result.Child(playerName);
        Debug.Log(count.ChildrenCount);
        if (count.ChildrenCount > 0)
        {
            playerName = FirebaseManager.instance.user.DisplayName;

            foreach (var data in result.Child(FirebaseManager.instance.user.UserId).Children)
            {
                switch (data.Key)
                {
                    case "playerClass":
                        playerClass = int.Parse(data.Value.ToString());
                        break;

                    case "playerName":
                        if (playerName != "")
                            playerName = data.Value.ToString();
                        else
                            playerName = FirebaseManager.instance.user.DisplayName;
                        break;

                    case "playerLevel":
                        playerLevel = int.Parse(data.Value.ToString());
                        break;

                    case "playerGold":
                        playerGold = int.Parse(data.Value.ToString());
                        break;

                    case "currentExp":
                        currentExp = float.Parse(data.Value.ToString());
                        break;

                    case "maxExp":
                        maxExp = float.Parse(data.Value.ToString());
                        break;

                    case "playerHp":
                        maxHp = float.Parse(data.Value.ToString());
                        currentHp = float.Parse(data.Value.ToString());
                        break;

                    case "playerMp":
                        maxMp = float.Parse(data.Value.ToString());
                        currentMp = float.Parse(data.Value.ToString());
                        break;

                    case "playerDamage":
                        playerDamage = int.Parse(data.Value.ToString());
                        break;

                    case "playerPosX":
                        fx = float.Parse(data.Value.ToString());
                        Debug.Log($"fx : {fx}");
                        break;

                    case "playerPosY":
                        fy = float.Parse(data.Value.ToString());
                        Debug.Log($"fx : {fy}");
                        break;

                    case "playerStatusPoint":
                        playerStatusPoint = int.Parse(data.Value.ToString());
                        break;

                    case "playerSkillPoint":
                        playerSkillPoint = int.Parse(data.Value.ToString());
                        break;

                    case "playerInventoryItem":
                        foreach (var _item in data.Children)
                            itemID.Add(int.Parse(_item.Value.ToString()));
                        break;

                    case "playerInventoryItemCount":
                        foreach (var _itemCount in data.Children)
                            itemCount.Add(int.Parse(_itemCount.Value.ToString()));
                        break;

                    case "playerEquipItem":
                        foreach (var _equipItem in data.Children)
                            equipItemID.Add(int.Parse(_equipItem.Value.ToString()));
                        break;


                    default:
                        Debug.Log($"{data.Key} : {data.Value}");
                        break;
                }
            }

            transform.position = new Vector2(fx, fy);

            if (fy > -5f)
                FindObjectOfType<CinemachineRange>().ChangeCollider(1);
            else
                FindObjectOfType<CinemachineRange>().ChangeCollider(2);

            maxHp = maxHp + equipHp;
            currentHp = maxHp;
            maxMp = maxMp + equipMp;
            currentMp = maxMp;

            for (int i = 0; i < itemID.Count; i++)
            {
                for (int x = 0; x < itemData.itemList.Count; x++)
                {
                    if (itemID[i] == itemData.itemList[x].itemID)
                    {
                        itemList.Add(itemData.itemList[x]);
                        break;
                    }
                }
            }

            for (int i = 0; i < itemCount.Count; i++)
                itemList[i].itemCount = itemCount[i];

            for (int i = 0; i < equipItemID.Count; i++)
            {
                var item = itemData.FindItem(equipItemID[i]);
                itemData.EquipItem(item.itemID, true);
                inven.slots[0].SetEquipStatus(item.itemID, item);
            }

            inven.LoadItem(itemList);
            questData.FindQuest();

            yield return new WaitForSeconds(1.0f);

            UIManager.instance.loadingPanel.SetActive(false);

            
        }

        else
        {
            if (FirebaseManager.instance.user.DisplayName == "")
                playerName = FirebaseManager.instance.nickname;
            else
                playerName = FirebaseManager.instance.user.DisplayName;
            questData.FindQuest();
            StatusUIManager.instance.PlayerStatusUpdate();
            UIManager.instance.loadingPanel.SetActive(false);
        }

    }

}

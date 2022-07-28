using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private ItemData itemData;
    public InventorySlot[] slots;
    private PotionQuickSlot[] potionQuickSlots;

    public List<Item> inventoryItemList;
    private List<Item> inventoryTapList;

    public GameObject quickSlotBtn;
    public GameObject equipPanel;
    public GameObject inventoryUI;
    public GameObject descriptionUI;
    public Transform tf;
    public UIManager uiManager;
    public EquipUI equipUI;
    public AudioClip clip;

    public int selectedItem;
    public int selectedTap;

    public bool isSell;
    public bool isQuickSlot;

    public static bool isInven;

    void Start()
    {
        instance = this;
        inventoryItemList = new List<Item>();
        inventoryTapList = new List<Item>();
        itemData = FindObjectOfType<ItemData>();
        potionQuickSlots = FindObjectsOfType<PotionQuickSlot>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
        isSell = false;
    }

    public List<Item> SaveItem()
    {
        return inventoryItemList;
    }

    public void LoadItem(List<Item> _itemList)
    {
        inventoryItemList = _itemList;
    }

    public void EquipToInventory(Item _item)
    {
        inventoryItemList.Add(_item);
        ShowItem();
    }

    public void GetItem(int _itemId, int _count = 1)
    {
        for (int i = 0; i < itemData.itemList.Count; i++) 
        {
            if (_itemId == itemData.itemList[i].itemID)
            {
                for (int j = 0; j < inventoryItemList.Count; j++)
                {
                    if (inventoryItemList[j].itemID == _itemId)
                    {
                        if (inventoryItemList[j].itemType != Item.ItemType.Equip)
                        {
                            inventoryItemList[j].itemCount += _count;
                        }
                        else
                        {
                            inventoryItemList.Add(itemData.itemList[i]);
                        }
                        return;
                    }
                }
                inventoryItemList.Add(itemData.itemList[i]);
                return;
            }
        }
    }

    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].RemoveItem();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isInven)
            {
                isSell = false;
                OpenInventory();
                ShowItem();
            }
            else
                CloseInventory();
        }
    }

    public void CloseInventory()
    {
        AudioManager.instance.PlayButtonClip();
        isInven = false;
        quickSlotBtn.SetActive(false);
        inventoryUI.SetActive(false);
        descriptionUI.SetActive(false);
        equipPanel.SetActive(false);
    }

    public void OpenInventory()
    {
        AudioManager.instance.PlayClip(clip);
        isInven = true;
        ShowItem();
        inventoryUI.SetActive(true);
        if (!isSell)
        {
            equipPanel.SetActive(true);
            equipUI.StatusUpdate();
        }
    }

    public void ShowItem()
    {
        inventoryTapList.Clear();

        RemoveSlot();

        switch (selectedTap)
        {
            case 0:
                isQuickSlot = false;
                quickSlotBtn.SetActive(false);
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                        inventoryTapList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                isQuickSlot = false;
                quickSlotBtn.SetActive(true);
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                        inventoryTapList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                isQuickSlot = false;
                quickSlotBtn.SetActive(false);
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTapList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                isQuickSlot = false;
                quickSlotBtn.SetActive(false);
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                        inventoryTapList.Add(inventoryItemList[i]);
                }
                break;
        }

        for (int i = 0; i < inventoryTapList.Count; i++)
        {
            slots[i].Additem(inventoryTapList[i]);
        }
    }

    public void EquipItem(Item item)
    {
        for(int i = 0; i < inventoryItemList.Count; i++)
        {
            if(item.itemID == inventoryItemList[i].itemID)
            {
                AudioManager.instance.PlayButtonClip();
                inventoryItemList.RemoveAt(i);
                equipUI.StatusUpdate();
            }
        }
    }

    public void QuickSlot()
    {
        if (!isQuickSlot)
        {
            isQuickSlot = true;
            StartCoroutine(GameManager.instance.ChangeInfoText("퀵슬롯에 등록할 아이템을 선택해주세요."));
        }
        else
            isQuickSlot = false;
    }

    public void EquipButton() { selectedTap = 0; ShowItem(); }
    public void UseButton() { selectedTap = 1; ShowItem(); }
    public void QuestButton() { selectedTap = 2; ShowItem(); }
    public void ETCButton() { selectedTap = 3; ShowItem(); }
}

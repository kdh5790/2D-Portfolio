using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Character player;

    float lastTimeClick;

    public Item item;
    private Inventory inven;

    public Image icon;
    public Image descriptionImage;

    public GameObject countImage;
    public GameObject descriptionPanel;
    public Text descriptionPriceTxt;

    private ItemData itemData;

    public Text itemNameTxt;
    public Text categoryTxt;
    public Text descriptionTxt;
    public Text itemCount_txt;

    public EquipmentSlot[] equipSlots;

    private PotionQuickSlot[] potionQuickSlots;

    private const int SWORD = 0, HELMET = 1, ARMOR = 2, BOOTS = 3, SHIELD = 4;

    public bool isItem = false; // 아이템이 있으면 true

    private void Start()
    {
        player = FindObjectOfType<Character>();
        inven = FindObjectOfType<Inventory>();
        itemData = FindObjectOfType<ItemData>();
        potionQuickSlots = FindObjectsOfType<PotionQuickSlot>();
        countImage.SetActive(false);
    }

    public void Additem(Item _item, int _count = 1)
    {
        isItem = true;
        item = _item;
        icon.sprite = _item.itemIcon;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1);

        if (Item.ItemType.Equip != _item.itemType)
        {
            if (_item.itemCount > 0)
            {
                countImage.SetActive(true);
                itemCount_txt.text = _item.itemCount.ToString();
            }
            else
            {
                itemCount_txt.text = "";
                RemoveItem();
            }
        }
        else
        {
            if (_item.itemCount > 0)
            {
                countImage.SetActive(false);
            }
            else if (_item.itemCount <= 0)
            {
                RemoveItem();
            }
        }
    }
    public void RemoveItem()
    {
        isItem = false;
        countImage.SetActive(false);
        itemCount_txt.text = "";
        icon.sprite = null;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (inven.isQuickSlot && item.itemType == Item.ItemType.Use)
            {
                switch (item.itemID)
                {
                    case 1001:
                        potionQuickSlots[1].SetSlot(item, this);
                        break;

                    case 1002:
                        potionQuickSlots[0].SetSlot(item, this);
                        break;
                }
            }
        }

        float currentTimeClick = eventData.clickTime;
        //Debug.Log(Mathf.Abs(currentTimeClick - lastTimeClick));
        Debug.Log(eventData.clickTime);
        if (Mathf.Abs(currentTimeClick - lastTimeClick) < 0.75f)
        {
            if (isItem)
            {
                if (!inven.isSell)
                {
                    if (item.itemType == Item.ItemType.Use)
                    {
                        item.itemCount--;
                        itemData.UseItem(item.itemID);
                        if (item.itemCount < 1)
                        {
                            RemoveItem();
                        }
                        inven.ShowItem();
                    }
                    if (item.itemType == Item.ItemType.Equip)
                    {
                        bool isEquip = SetEquipStatus(item.itemID, item);
                        if (!isEquip)
                        {
                            itemData.EquipItem(item.itemID, true);
                            inven.EquipItem(item);
                            RemoveItem();
                        }
                    }
                }
                else if (inven.isSell)
                {
                    item.itemCount--;
                    if (item.itemCount < 1)
                    {
                        RemoveItem();
                    }
                    inven.ShowItem();
                    Character.instance.playerGold += item.itemPrice / 2;
                }
            }
        }
            lastTimeClick = currentTimeClick;
    }

    public bool SetEquipStatus(int _itemID, Item _item)
    {
        bool isEquip = false;
        string temp = _itemID.ToString();
        temp = temp.Substring(0, 3);
        switch (temp)
        {
            case "500":
                if (!equipSlots[SWORD].isEquip)
                {
                    equipSlots[SWORD].InventoryToEquip(_item);
                    isEquip = false;
                    break;
                }
                else
                    isEquip = true;
                break;
            case "501":
                if (!equipSlots[HELMET].isEquip)
                {
                    equipSlots[HELMET].InventoryToEquip(_item);
                    isEquip = false;
                    break;
                }
                else
                    isEquip = true;
                break;
            case "502":
                if (!equipSlots[ARMOR].isEquip)
                {
                    equipSlots[ARMOR].InventoryToEquip(_item);
                    isEquip = false;
                    break;
                }
                else
                    isEquip = true;
                break;
            case "503":
                if (!equipSlots[BOOTS].isEquip)
                {
                    equipSlots[BOOTS].InventoryToEquip(_item);
                    isEquip = false;
                    break;
                }
                else
                    isEquip = true;
                break;
            case "504":
                if (!equipSlots[SHIELD].isEquip)
                {
                    equipSlots[SHIELD].InventoryToEquip(_item);
                    isEquip = false;
                    break;
                }
                else
                    isEquip = true;
                break;
        }

        return isEquip;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (isItem)
        {
            descriptionPanel.SetActive(true);

            descriptionImage.sprite = item.itemIcon;
            itemNameTxt.text = item.itemName;
            descriptionTxt.text = item.itemDescription;
            if (item.itemType == Item.ItemType.Use)
            {
                categoryTxt.text = "소 비";
            }
            else if (item.itemType == Item.ItemType.Equip)
            {
                categoryTxt.text = "장 비";
            }
            else if (item.itemType == Item.ItemType.Quest)
            {
                categoryTxt.text = "퀘스트";
            }
            else if (item.itemType == Item.ItemType.ETC)
            {
                categoryTxt.text = "기 타";
            }
            if (inven.isSell)
            {
                descriptionPriceTxt.gameObject.SetActive(true);
                descriptionPriceTxt.text = $"판매 가격 : {item.itemPrice / 2}G";
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }
}

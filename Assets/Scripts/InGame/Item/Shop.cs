using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public ShopSlot[] slots;

    public GameObject shopPanel;
    public GameObject descriptionPanel;
    public GameObject infoPanel;

    public Transform tf;

    public Image descriptionImage;

    public Text descriptionTxt;
    public Text categoryTxt;
    public Text itemNameTxt;
    public Text priceTxt;
    public Text goldTxt;
    public Text infoTxt;

    public int selectedTap;

    public int[] itemID; // 상점에서 판매할 아이템 아이디

    public List<Item> itemList;
    private ItemData itemData;

    private bool isOpen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        itemList = new List<Item>();
        itemData = FindObjectOfType<ItemData>();
        slots = tf.GetComponentsInChildren<ShopSlot>();
    }

    void Update()
    {
        if (isOpen)
            goldTxt.text = Character.instance.playerGold.ToString();
    }

    public void OpenShop()
    {
        isOpen = true;
        shopPanel.SetActive(true);
        infoPanel.SetActive(true);
        selectedTap = 1;
        Inventory.instance.isSell = true;

        infoTxt.text = "구입 버튼을 클릭하여 아이템을 구매 할 수 있습니다.";

        for (int i = 0; i < itemID.Length; i++)
        {
            itemList.Add(itemData.FindItem(itemID[i]));
        }
        LoadItem();
    }

    public void LoadItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = itemList[i];
            slots[i].ShowItem();
        }
    }

    public void CloseShop()
    {
        isOpen = false;
        infoPanel.SetActive(false);
        shopPanel.SetActive(false);
        descriptionPanel.SetActive(false);
        Character.instance.isTalk = false;
        Inventory.instance.isSell = false;
        Inventory.instance.CloseInventory();
    }

    public void ShowDescription(Item _item)
    {
        descriptionPanel.SetActive(true);

        descriptionImage.sprite = _item.itemIcon;
        itemNameTxt.text = _item.itemName;
        descriptionTxt.text = _item.itemDescription;
        priceTxt.text = $"구매 가격 : {_item.itemPrice}G";

        if (_item.itemType == Item.ItemType.Use)
        {
            categoryTxt.text = "소 비";
        }
        else if (_item.itemType == Item.ItemType.Equip)
        {
            categoryTxt.text = "장 비";
        }
        else if (_item.itemType == Item.ItemType.Quest)
        {
            categoryTxt.text = "퀘스트";
        }
        else if (_item.itemType == Item.ItemType.ETC)
        {
            categoryTxt.text = "기 타";
        }
    }

    public void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }

    public void ChangeTap()
    {
        if(selectedTap == 1)
        {
            selectedTap = 2;
            shopPanel.SetActive(false);
            Inventory.instance.OpenInventory();
            Inventory.instance.ShowItem();
            infoTxt.text = "슬롯을 우클릭하여 아이템을 판매 할 수 있습니다.";
        }
        else
        {
            selectedTap = 1;
            Inventory.instance.CloseInventory();
            shopPanel.SetActive(true);
            infoTxt.text = "구입 버튼을 클릭하여 아이템을 구매 할 수 있습니다.";
        }
    }
}

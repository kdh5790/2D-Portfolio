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

    public int[] itemID; // �������� �Ǹ��� ������ ���̵�

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

        infoTxt.text = "���� ��ư�� Ŭ���Ͽ� �������� ���� �� �� �ֽ��ϴ�.";

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
        priceTxt.text = $"���� ���� : {_item.itemPrice}G";

        if (_item.itemType == Item.ItemType.Use)
        {
            categoryTxt.text = "�� ��";
        }
        else if (_item.itemType == Item.ItemType.Equip)
        {
            categoryTxt.text = "�� ��";
        }
        else if (_item.itemType == Item.ItemType.Quest)
        {
            categoryTxt.text = "����Ʈ";
        }
        else if (_item.itemType == Item.ItemType.ETC)
        {
            categoryTxt.text = "�� Ÿ";
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
            infoTxt.text = "������ ��Ŭ���Ͽ� �������� �Ǹ� �� �� �ֽ��ϴ�.";
        }
        else
        {
            selectedTap = 1;
            Inventory.instance.CloseInventory();
            shopPanel.SetActive(true);
            infoTxt.text = "���� ��ư�� Ŭ���Ͽ� �������� ���� �� �� �ֽ��ϴ�.";
        }
    }
}

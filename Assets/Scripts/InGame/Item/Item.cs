using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public int itemID; // �������� ������, �ߺ� �Ұ���
    public string itemName;  // �������� �̸�, �ߺ� ����
    public string itemDescription; // ������ ����
    public int itemCount; // ���� ����
    public int itemPrice; // ������ ���� ����
    public Sprite itemIcon; // �������� ������
    public ItemType itemType;

    public enum ItemType
    {
        Use,
        Equip,
        Quest,
        ETC
    }

    public Item(int _itemID, string _itemName, string _itemDescription, ItemType _itemType, int _itemPrice,int _itemCount = 1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemType = _itemType;
        itemPrice = _itemPrice;
        itemCount = _itemCount;
        itemIcon = Resources.Load<Sprite>("Texture/" + _itemID.ToString());
    }
}

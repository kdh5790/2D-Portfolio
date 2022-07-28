using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public int itemID; // 아이템의 고유값, 중복 불가능
    public string itemName;  // 아이템의 이름, 중복 가능
    public string itemDescription; // 아이템 설명
    public int itemCount; // 소지 개수
    public int itemPrice; // 아이템 구매 가격
    public Sprite itemIcon; // 아이템의 아이콘
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();
    Character player;
    void Start()
    {
        player = FindObjectOfType<Character>();

        itemList.Add(new Item(1001, "빨간 포션", "체력을 30 회복시켜준다.", Item.ItemType.Use, 200));
        itemList.Add(new Item(1002, "파란 포션", "마나를 30 회복시켜준다.", Item.ItemType.Use, 200));
        itemList.Add(new Item(2001, "돌 조각", "낡은 열쇠를 제작하기 위해 필요한 퀘스트 아이템.", Item.ItemType.Quest, 50));
        itemList.Add(new Item(2002, "금 조각", "금 열쇠를 제작하기 위해 필요한 퀘스트 아이템.", Item.ItemType.Quest, 50));
        itemList.Add(new Item(5001, "소드", "낡은 검이다.", Item.ItemType.Equip, 600));
        itemList.Add(new Item(5002, "아이언 소드", "철로 만들어진 검이다.", Item.ItemType.Equip, 1200));
        itemList.Add(new Item(5003, "골드 소드", "금으로 만들어진 검이다.", Item.ItemType.Equip, 3500));
        itemList.Add(new Item(5011, "아이언 헬멧", "철로 만들어진 투구이다.", Item.ItemType.Equip, 700));
        itemList.Add(new Item(5021, "아이언 아머", "철로 만들어진 갑옷이다.", Item.ItemType.Equip, 900));
        itemList.Add(new Item(5031, "아이언 부츠", "철로 만들어진 신발이다.", Item.ItemType.Equip, 500));
        itemList.Add(new Item(5041, "아이언 쉴드", "철로 만들어진 방패이다.", Item.ItemType.Equip, 700));
    }

    public string GetItemName(int _itemID)
    {
        foreach(var item in itemList)
        {
            if (item.itemID == _itemID)
            {
                return item.itemName;
            }
        }

        return "알수없음";
    }

    public void UseItem(int _itemID)
    {
        switch(_itemID)
        {
            case 1001:
                player.currentHp += 30;
                break;

            case 1002:
                player.currentMp += 30;
                break;
        }
    }

    public void EquipItem(int _itemID, bool isEquip)
    {
        if (isEquip)
        {
            switch (_itemID)
            {
                case 5001:
                    player.equipDamage += 15;
                    break;
                case 5002:
                    player.equipDamage += 28;
                    break;
                case 5003:
                    player.equipDamage += 45;
                    break;
                case 5011:
                    player.equipDef += 2;
                    player.equipHp += 25;
                    break;
                case 5021:
                    player.equipDef += 5;
                    player.equipHp += 40;
                    break;
                case 5031:
                    player.equipDef += 2;
                    break;
                case 5041:
                    player.equipDef += 5;
                    break;
            }
        }
        else if(!isEquip)
        {
            switch (_itemID)
            {
                case 5001:
                    player.equipDamage -= 15;
                    break;
                case 5002:
                    player.equipDamage -= 28;
                    break;
                case 5003:
                    player.equipDamage -= 45;
                    break;
                case 5011:
                    player.equipDef -= 2;
                    player.equipHp -= 25;
                    break;
                case 5021:
                    player.equipDef -= 5;
                    player.equipHp -= 40;
                    break;
                case 5031:
                    player.equipDef -= 2;
                    break;
                case 5041:
                    player.equipDef -= 5;
                    break;
            }
        }
    }

    public Item FindItem(int _itemID)
    {
        Item item = null;
        for(int i = 0; i < itemList.Count; i++)
        {
            if(_itemID == itemList[i].itemID)
            {
                item = itemList[i];
                return item;
            }
        }
        return item;
    }
}

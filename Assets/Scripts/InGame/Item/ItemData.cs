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

        itemList.Add(new Item(1001, "���� ����", "ü���� 30 ȸ�������ش�.", Item.ItemType.Use, 200));
        itemList.Add(new Item(1002, "�Ķ� ����", "������ 30 ȸ�������ش�.", Item.ItemType.Use, 200));
        itemList.Add(new Item(2001, "�� ����", "���� ���踦 �����ϱ� ���� �ʿ��� ����Ʈ ������.", Item.ItemType.Quest, 50));
        itemList.Add(new Item(2002, "�� ����", "�� ���踦 �����ϱ� ���� �ʿ��� ����Ʈ ������.", Item.ItemType.Quest, 50));
        itemList.Add(new Item(5001, "�ҵ�", "���� ���̴�.", Item.ItemType.Equip, 600));
        itemList.Add(new Item(5002, "���̾� �ҵ�", "ö�� ������� ���̴�.", Item.ItemType.Equip, 1200));
        itemList.Add(new Item(5003, "��� �ҵ�", "������ ������� ���̴�.", Item.ItemType.Equip, 3500));
        itemList.Add(new Item(5011, "���̾� ���", "ö�� ������� �����̴�.", Item.ItemType.Equip, 700));
        itemList.Add(new Item(5021, "���̾� �Ƹ�", "ö�� ������� �����̴�.", Item.ItemType.Equip, 900));
        itemList.Add(new Item(5031, "���̾� ����", "ö�� ������� �Ź��̴�.", Item.ItemType.Equip, 500));
        itemList.Add(new Item(5041, "���̾� ����", "ö�� ������� �����̴�.", Item.ItemType.Equip, 700));
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

        return "�˼�����";
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

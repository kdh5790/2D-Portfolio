using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    private const int SWORD = 0, HELMET = 1, ARMOR = 2, BOOTS = 3, SHIELD = 4;

    Character player;
    float lastTimeClick;

    public Image itemImage;
    public Sprite defaultImage;
    public Inventory inven;
    public EquipUI equipUi;

    public Item equipItem;

    public bool isEquip = false;

    private void Start()
    {
        player = FindObjectOfType<Character>();
        inven = FindObjectOfType<Inventory>();   
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTimeClick = eventData.clickTime;
        if (Mathf.Abs(currentTimeClick - lastTimeClick) < 0.75f)
        { 
            if (isEquip)
            {
                inven.EquipToInventory(equipItem);
                FindObjectOfType<ItemData>().EquipItem(equipItem.itemID, false);
                RemoveItem();
                FindObjectOfType<EquipUI>().StatusUpdate();
            }
        }
        lastTimeClick = currentTimeClick;
    }

    public void RemoveItem()
    {
        isEquip = false;
        equipItem = new Item(0, "", "", Item.ItemType.Equip, 0);
        itemImage.sprite = defaultImage;
    }

    public void InventoryToEquip(Item _item)
    {
        equipItem = _item;
        isEquip = true;
        itemImage.sprite = equipItem.itemIcon;
    }
}

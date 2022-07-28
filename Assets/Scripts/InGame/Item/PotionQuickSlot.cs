using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionQuickSlot : MonoBehaviour
{
    InventorySlot slot;
    Item item;
    ItemData itemData;

    public Button useButton;
    public Image itemImage;
    public Image countImage;
    public Text countText;
    public Image cursorImage;
    public AudioClip clip;

    private bool isItem;

    private void Start()
    {
        itemData = FindObjectOfType<ItemData>();
    }

    public void SetSlot(Item _item, InventorySlot _slot)
    {
        item = _item;
        slot = _slot;
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = item.itemIcon;
        countImage.gameObject.SetActive(true);
        countText.text = slot.item.itemCount.ToString();
        useButton.interactable = true;
        isItem = true;
        FindObjectOfType<Inventory>().isQuickSlot = false;
    }

    public void UsePotion()
    {
        if(isItem)
        {
            AudioManager.instance.PlayClip(clip);
            itemData.UseItem(item.itemID);
            slot.item.itemCount--;
            slot.itemCount_txt.text = slot.item.itemCount.ToString();
            countText.text = slot.item.itemCount.ToString();
            if(slot.item.itemCount <= 0)
            {
                DeleteItem();
            }
        }
    }

    public void DeleteItem()
    {
        isItem = false;
        item = null;
        slot = null;
        itemImage.gameObject.SetActive(false);
        itemImage.sprite = null;
        countImage.gameObject.SetActive(false);
        useButton.interactable = false;
    }
}

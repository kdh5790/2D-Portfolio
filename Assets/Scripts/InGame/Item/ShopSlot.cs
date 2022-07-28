using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public Image itemImage;
    public Button buyBtn;
    public AudioClip clip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Shop.instance.ShowDescription(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Shop.instance.HideDescription();
    }

    public void ShowItem()
    {
        itemImage.sprite = item.itemIcon;
    }

    public void BuyItem()
    {
        if(Character.instance.playerGold >= item.itemPrice)
        {
            AudioManager.instance.PlayClip(clip);
            Character.instance.playerGold -= item.itemPrice;
            Inventory.instance.GetItem(item.itemID);
            Inventory.instance.ShowItem();
            Shop.instance.infoTxt.text = $"{item.itemName}을/를 {item.itemPrice}골드로 구매하였습니다.";
        }
        else
        {
            Shop.instance.infoTxt.text = "소지한 골드가 부족합니다.";
        }
    }
}

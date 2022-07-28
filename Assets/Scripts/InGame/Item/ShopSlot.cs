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
            Shop.instance.infoTxt.text = $"{item.itemName}��/�� {item.itemPrice}���� �����Ͽ����ϴ�.";
        }
        else
        {
            Shop.instance.infoTxt.text = "������ ��尡 �����մϴ�.";
        }
    }
}

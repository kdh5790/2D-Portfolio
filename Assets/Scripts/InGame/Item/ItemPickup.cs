using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    public int itemID;
    public int count;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory.instance.GetItem(itemID, count);
            AudioManager.instance.PlayClip(pickupSound);
            Destroy(this.gameObject);
        }
    }
}

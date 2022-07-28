using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAndUIButton : MonoBehaviour
{
    public void JumpButton() => FindObjectOfType<Character>().Jump(1);
    public void AttackButton() => FindObjectOfType<Character>().Attack(1);
    public void InventoryButton() => FindObjectOfType<Inventory>().OpenInventory();
}

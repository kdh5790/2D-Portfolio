using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public GameObject[] UIButton;

    private bool isOpen;

    public void JumpButton() => FindObjectOfType<Character>().Jump();
    public void AttackButton() => FindObjectOfType<Character>().Attack(1);

    public void OpenButton()
    {
        if(isOpen)
        {
            for (int i = 0; i < UIButton.Length; i++)
                UIButton[i].SetActive(false);
            isOpen = false;
        }
        else if(!isOpen)
        {
            for (int i = 0; i < UIButton.Length; i++)
                UIButton[i].SetActive(true);
            isOpen = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipUI : MonoBehaviour
{
    public GameObject equipPanel;

    public Text[] statusTxt;
    public Image[] equipIcon;
    public Sprite[] defaultEquipIcon;
    public GameObject[] equipSlots;

    Character player;

    private const int Damage = 0, Health = 1, Mana = 2, Armor = 3, StatPoint = 4;

    void Start()
    {
        player = FindObjectOfType<Character>();
        StatusUpdate();
    }

    public void StatusUpdate()
    {
        if (player == null)
            player = FindObjectOfType<Character>();
        statusTxt[Damage].text = $"{player.playerDamage} +({player.equipDamage})";
        statusTxt[Health].text = $"{player.maxHp} +({player.equipHp})";
        statusTxt[Mana].text = $"{player.maxMp} +({player.equipMp})";
        statusTxt[Armor].text = $"{player.playerDef} +({player.equipDef})";
        statusTxt[StatPoint].text = player.playerStatusPoint.ToString();
    }

    public void PlusBtn(int _int)
    {
        if (player.playerStatusPoint > 0)
        {
            switch (_int)
            {
                case Damage:
                    player.playerDamage += 3;
                    break;
                case Health:
                    player.maxHp += 15;
                    break;
                case Mana:
                    player.maxMp += 10;
                    break;
                case Armor:
                    player.playerDef += 0.3f;
                    break;
            }
            player.playerStatusPoint -= 1;
        }
        else
        {
            StartCoroutine(GameManager.instance.ChangeInfoText("포인트가 부족합니다."));
        }

        StatusUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class SaveNLoadManager : MonoBehaviour
{
    private Character player;

    private void SaveData()
    {
        player = FindObjectOfType<Character>();

        if (FirebaseManager.instance.user != null && player != null)
        {
            Character.Data data = new Character.Data();
            data.playerName = FirebaseManager.instance.user.DisplayName;
            data.playerLevel = player.playerLevel;
            data.playerPosX = player.transform.position.x;
            data.playerPosY = player.transform.position.y;
            data.playerGold = player.playerGold;
            data.currentExp = player.currentExp;
            data.maxExp = player.maxExp;
            data.playerDamage = player.playerDamage;
            data.playerDef = player.playerDef;
            data.playerHp = player.maxHp;
            data.playerMp = player.maxMp;

            List<Item> item = player.inven.SaveItem();

            data.playerEquipItem = new List<int>();
            data.playerInventoryItem = new List<int>();
            data.playerInventoryItemCount = new List<int>();


            data.playerInventoryItem.Clear();
            data.playerInventoryItemCount.Clear();

            for (int i = 0; i < item.Count; i++)
            {
                data.playerInventoryItem.Add(item[i].itemID);
                data.playerInventoryItemCount.Add(item[i].itemCount);
            }

            var equipSlot = FindObjectsOfType<EquipmentSlot>();

            for (int i = 0; i < equipSlot.Length; i++)
            {
                if (equipSlot[i].isEquip)
                {
                    data.playerEquipItem.Add(equipSlot[i].equipItem.itemID);
                }
            }

            string json = JsonUtility.ToJson(data);

            player.reference.Child("users").Child(player.playerName).SetRawJsonValueAsync(json);
        }
    }

    IEnumerator LoadData()
    {
        var savetask = FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync();

        yield return new WaitUntil(predicate: () => savetask.IsCompleted);

        var result = savetask.Result;

        float fx = 0;
        float fy = 0;


        List<int> itemID = new List<int>();
        List<int> itemCount = new List<int>();
        List<Item> itemList = new List<Item>();

        Debug.Log(FirebaseManager.instance.user.UserId);

        foreach (var data in result.Child(player.playerName).Children)
        {
            switch (data.Key)
            {
                case "playerLevel":
                    player.playerLevel = int.Parse(data.Value.ToString());
                    break;

                case "playerGold":
                    player.playerGold = int.Parse(data.Value.ToString());
                    break;

                case "currentExp":
                    player.currentExp = float.Parse(data.Value.ToString());
                    break;

                case "maxExp":
                    player.maxExp = float.Parse(data.Value.ToString());
                    break;

                case "playerHp":
                    player.maxHp = float.Parse(data.Value.ToString());
                    player.currentHp = float.Parse(data.Value.ToString());
                    break;

                case "playerMp":
                    player.maxMp = float.Parse(data.Value.ToString());
                    player.currentMp = float.Parse(data.Value.ToString());
                    break;

                case "playerDamage":
                    player.playerDamage = int.Parse(data.Value.ToString());
                    break;

                case "playerPosX":
                    fx = float.Parse(data.Value.ToString());
                    Debug.Log($"fx : {fx}");
                    break;

                case "playerPosY":
                    fy = float.Parse(data.Value.ToString());
                    Debug.Log($"fx : {fy}");
                    break;

                case "playerInventoryItem":
                    foreach (var _item in data.Children)
                        itemID.Add(int.Parse(_item.Value.ToString()));
                    break;

                case "playerInventoryItemCount":
                    foreach (var _itemCount in data.Children)
                        itemCount.Add(int.Parse(_itemCount.Value.ToString()));
                    break;

                default:
                    Debug.Log($"{data.Key} : {data.Value}");
                    break;
            }
        }

        player.transform.position = new Vector2(fx, fy);

        for (int i = 0; i < itemID.Count; i++)
        {
            for (int x = 0; x < player.itemData.itemList.Count; x++)
            {
                if (itemID[i] == player.itemData.itemList[x].itemID)
                {
                    itemList.Add(player.itemData.itemList[x]);
                    break;
                }
            }
        }

        for (int i = 0; i < itemCount.Count; i++)
            itemList[i].itemCount = itemCount[i];

        player.inven.LoadItem(itemList);
        player.questData.FindQuest();
    }
}

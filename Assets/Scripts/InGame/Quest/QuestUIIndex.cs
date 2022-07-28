using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestUIIndex : MonoBehaviour, IPointerClickHandler
{
    public QuestData questData;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (questData != null)
            GetComponentInParent<QuestUI>().Progress(questData);
    }
}

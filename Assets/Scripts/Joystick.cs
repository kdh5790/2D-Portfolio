using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image backImage;
    public Image stickImage;
    private Vector3 inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(backImage.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / backImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / backImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2, pos.y * 2, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            stickImage.rectTransform.anchoredPosition =
                new Vector3(inputVector.x * (backImage.rectTransform.sizeDelta.x / 3), inputVector.y * (backImage.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector3.zero;
        stickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float GetInputX()
    {
        return inputVector.x;
    }
}

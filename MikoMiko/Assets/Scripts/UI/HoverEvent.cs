using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public Color normalColor = new Color(1f, 172/255f, 196/255f);
    public Color hoverColor = new Color(210/255f, 83/255f, 102/255f);
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalColor;

    }
}

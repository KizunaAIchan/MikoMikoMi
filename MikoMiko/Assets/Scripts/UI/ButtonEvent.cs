using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Image image;
    public Vector3 normalScale =Vector3.one;
    public Vector3 pressScale = new Vector3(0.7f, 0.7f, 0.7f);


   // public Transform transform;
    public void Awake()
    {
   //     transform = this.transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = normalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = pressScale;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour, IPointerUpHandler, IPointerDownHandler,IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public Vector3 normalScale =Vector3.one;
    public Vector3 pressScale = new Vector3(0.7f, 0.7f, 0.7f);


    public bool OpenEnterEvent = false;


    public Text txt;
   // public Transform transform;
    public void Awake()
    {
   //     transform = this.transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!OpenEnterEvent)
            return;

        Color c = image.color;
        c.a = 1f;
        image.color = c;

        Color cc = txt.color;
        cc.a = 1f;
        txt.color = cc;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!OpenEnterEvent)
            return;
        Color c = image.color;
        c.a = 0.5f;
        image.color = c;

        Color cc = txt.color;
        cc.a = 0.5f;
        txt.color = cc;
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

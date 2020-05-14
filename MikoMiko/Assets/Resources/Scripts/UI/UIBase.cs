using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [HideInInspector]
    public string uiName = "";


    public int width = 0;
    public int height = 0;
    protected virtual void Init()
    {

    }
    








    protected virtual void Close()
    {
        UIManager.instance.CloseUIByName(uiName);
    }
}

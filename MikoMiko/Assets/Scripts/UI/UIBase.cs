using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [HideInInspector]
    public string uiName = "";

    public Vector3 InitPos = Vector3.zero;

    public int width = 0;
    public int height = 0;
    public  virtual void Init()
    {

    }
    








    protected virtual void Close()
    {
        UIManager.instance.CloseUIByName(uiName);
    }
}

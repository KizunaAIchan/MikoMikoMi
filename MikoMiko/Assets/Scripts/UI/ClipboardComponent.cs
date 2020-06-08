using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipboardComponent : UIComponentBase
{

    public InputField text;
    

    public void Init(string s)
    {
        text.text = s;
    }

    public void onBtnClickCopy()
    {
        GUIUtility.systemCopyBuffer = text.text;
    }
}

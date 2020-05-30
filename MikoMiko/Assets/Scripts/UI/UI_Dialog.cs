using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialog : UIBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public delegate void DialogCallBack();
    private DialogCallBack callback = null;
    public Text text;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitDialog(string content, DialogCallBack callback)
    {
        this.callback = callback;
        text.text = content;
    }

    public void onBtnClose()
    {
        Close();
    }

    public void OnBtnClickConfirm()
    {
        if (callback != null)
            callback();
        Close();
    }

    
}

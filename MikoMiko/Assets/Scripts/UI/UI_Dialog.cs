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

    public GameObject errorNode;
    public GameObject warningNode;
    public GameObject MikoNode;
    public GameObject NormalNode;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitDialog(string content,bool error , DialogCallBack callback)
    {
        this.callback = callback;
        text.text = content;
        MikoNode.SetActive(false);
        NormalNode.SetActive(true);
        errorNode.SetActive(error);
        warningNode.SetActive(!error);
    }

    public void onBtnClose()
    {
        Close();
    }

    public void ShowMiko()
    {
        MikoNode.SetActive(true);
        NormalNode.SetActive(false);
    }

    public void OnBtnClickConfirm()
    {
        if (callback != null)
            callback();
        Close();
    }

    
}

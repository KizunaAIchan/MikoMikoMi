using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RightClickMenu : UIBase
{

    public AVGDialog dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        OnShowDialog();
    }

    public void OnBtnClickClose()
    {
        Close();
    }

    public void OnShowDialog()
    {
        var dialoginfo = AVGDataManager.instance.GetRandomDialogue();
        dialog.ResetContent(dialoginfo.Id);
        dialog.StartFadeIn();
    }

    public void OnBtnClickExit()
    {
        Application.Quit();
    }

    public void OnBtnClickShowEditor()
    {
        var menu = UIManager.instance.ShowUI<AVGEditor>(UINames.AvgEditor);
        menu.Init();
        menu.transform.localPosition = Vector3.zero;
        Close();
    }

    public void OnBtnClickShowConfig()
    {
        var menu = UIManager.instance.ShowUI<UI_Config>(UINames.configPage);
        menu.InitComponent();
     //   menu.transform.localPosition = new Vector3(0, -35f, 0);
        menu.transform.localPosition = new Vector3(0, -60f, 0);
        Close();
    }
}

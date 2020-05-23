using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVGEditorOption : MonoBehaviour
{

    public InputField idField;
    public InputField ReplyField;
    public InputField ContentField;



    public OptionInfo info = new OptionInfo();
    public AVGEditor editorrr;

    public void InitData()
    {
        idField.interactable = true;
        info = new OptionInfo();
        idField.text = string.Empty;
        ReplyField.text = string.Empty;
        ContentField.text = string.Empty;
    }

    public void InitData(OptionInfo config)
    {
        info = new OptionInfo();
        info = config;
        idField.interactable = false;
        idField.text = info.Id.ToString();
        ReplyField.text = info.DialogueId.ToString();
        ContentField.text = info.content.ToString();
    }

    public void OnBtnClickSave()
    {
        info.Id = int.Parse(idField.text);
        info.content = ContentField.text;
        info.DialogueId = int.Parse(ReplyField.text);
        AVGDataManager.instance.SaveOption(info);
        editorrr.AddOrRefreshOpComponent(info);
        gameObject.SetActive(false);

    }

    public void OnBtnClickClose()
    {
        gameObject.SetActive(false);

    }

    public void OnBtnClickDel()
    {
        AVGDataManager.instance.DeleteOption(info);
        editorrr.DelOpComponent(info);
        gameObject.SetActive(false);
    }
}

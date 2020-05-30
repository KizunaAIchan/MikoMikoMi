using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVGEditorOption : MonoBehaviour
{

    public InputField idField;
    public InputField ReplyField;
    public InputField ContentField;
    public InputField addlove;

    public Dropdown replyIds;


    public OptionInfo info = new OptionInfo();
    public AVGEditor editorrr;
    public List<Dropdown.OptionData> _dropdownlist = new List<Dropdown.OptionData>();
    public ConfigType configType = ConfigType.AddNew;

    public void InitData()
    {
        configType = ConfigType.AddNew;
        idField.interactable = true;
        info = new OptionInfo();
        idField.text = string.Empty;
        ReplyField.text = string.Empty;
        ContentField.text = string.Empty;
        addlove.text = string.Empty;
        InitDropDown();

    }

    public void InitData(OptionInfo config)
    {
        configType = ConfigType.Modify;

        info = new OptionInfo();
        info = config;
        idField.interactable = false;
        idField.text = info.Id.ToString();
        ReplyField.text = info.DialogueId.ToString();
        ContentField.text = info.content.ToString();
        addlove.text = info.addLove.ToString();
        InitDropDown();
        SetDropDownData();
    }

    public void OnBtnClickSave()
    {
        info.Id = int.Parse(idField.text);
        info.content = ContentField.text;
        info.DialogueId = int.Parse(ReplyField.text);
        info.addLove = int.Parse(addlove.text);
        AVGDataManager.instance.SaveOption(info);
        editorrr.AddOrRefreshOpComponent(info);
        gameObject.SetActive(false);

    }

    public void InitDropDown()
    {
        _dropdownlist.Clear();
        Dropdown.OptionData noneData = new Dropdown.OptionData();
        noneData.text = "None";
        _dropdownlist.Add(noneData);

        foreach ( var item in AVGDataManager.instance.dialogueInfos)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = item.Key.ToString();
            _dropdownlist.Add(data);
        }

        replyIds.AddOptions(_dropdownlist);
    }

    public void SetDropDownData()
    {
        int idx = 0;

        for (int i = 0; i < _dropdownlist.Count; ++i)
        {
            if (_dropdownlist[i].text == info.DialogueId.ToString())
            {
                idx = i;
                break;
            }
        }

        replyIds.value = idx;
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

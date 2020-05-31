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
        idField.text = (AVGDataManager.instance.optionInfos.Count+1).ToString();
        addlove.text = "0";
        ReplyField.text = string.Empty;
        ContentField.text = string.Empty;
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
        if (idField.text.Length == 0)
        {
            var d = UIManager.instance.ShowUI<UI_Dialog>(UINames.Dialog);
            d.transform.localPosition = Vector3.zero;
            d.InitDialog(LanguageManager.instance.GetStringByLID("[LID:24]"), true, null);

            return;
        }

        if (configType == ConfigType.AddNew && AVGDataManager.instance.HasOptionID(int.Parse(idField.text)))
        {
            var d = UIManager.instance.ShowUI<UI_Dialog>(UINames.Dialog);
            d.transform.localPosition = Vector3.zero;
            d.InitDialog(LanguageManager.instance.GetStringByLID("[LID:23]"), true, null);

            return;
        }

        info.Id = int.Parse(idField.text);
        info.content = ContentField.text;
        info.DialogueId = int.Parse(GetDropDownText(replyIds));
        info.addLove = addlove.text.Length > 0 ? int.Parse(addlove.text) : 0;
        AVGDataManager.instance.SaveOption(info);
        editorrr.AddOrRefreshOpComponent(info);
        gameObject.SetActive(false);

    }

    public void InitDropDown()
    {
        _dropdownlist.Clear();
        replyIds.ClearOptions();

        //Dropdown.OptionData noneData = new Dropdown.OptionData();
        //noneData.text = "None";
        //_dropdownlist.Add(noneData);

        foreach ( var item in AVGDataManager.instance.dialogueInfos)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = item.Key.ToString() + ":" + item.Value.content;
            _dropdownlist.Add(data);
        }

        replyIds.AddOptions(_dropdownlist);
    }

    public void SetDropDownData()
    {
        int idx = 0;

        for (int i = 0; i < _dropdownlist.Count; ++i)
        {
            var str = _dropdownlist[i].text;
            var strs = str.Split(':');
            if (strs[0] == info.DialogueId.ToString())
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

    public string GetDropDownText(Dropdown drop)
    {
        var list = drop.options;
        var str = list[drop.value].text;
        var strs = str.Split(':');
        return strs[0];
    }

    public void CheckID()
    {
        if (idField.text.Length == 0)
            idField.text = "1";
        else if (idField.text.Contains("-"))
            idField.text = "1";
        else if (int.Parse(idField.text) < 1)
            idField.text = "1";

    }
}

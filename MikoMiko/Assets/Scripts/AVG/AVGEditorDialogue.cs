using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVGEditorDialogue : MonoBehaviour
{
    public InputField idField;
    public InputField ContentField;
    public InputField requireLove;

    public List<InputField> opField;

    public Dropdown voice;
    public Dropdown animation;
    public Toggle type;
    public Toggle random;
    public Toggle chatBubble;


    public DialogueInfo info = new DialogueInfo();
    private List<Dropdown.OptionData> _dropdownlist = new List<Dropdown.OptionData>();
    private List<Dropdown.OptionData> _Anidropdownlist = new List<Dropdown.OptionData>();

    public AVGEditor editorrr;
    public ConfigType configType = ConfigType.AddNew;







    public void InitData()
    {
        configType = ConfigType.AddNew;
        idField.interactable = true;

        info = new DialogueInfo();
        RefreshAnimatonDropDown();

        RefreshVoiceDropDown();
        InitComponentData();
    }


    public void InitData(DialogueInfo config)
    {
        configType = ConfigType.Modify;

        idField.interactable = false;
        info = new DialogueInfo();
        info = config;
        InitComponentData();
        RefreshAnimatonDropDown();
        RefreshVoiceDropDown();
        SetDropDownData();
        SetAniDropDownData();
    }

    public void InitComponentData()
    {
        idField.text = info.Id.ToString();
        ContentField.text = info.content;
        for (int i = 0, jCount = info.optionIds.Count; i < opField.Count; ++i)
        {
            var o = opField[i];
            if (i >= jCount)
                o.text = string.Empty;
            else
                o.text = info.optionIds[i].ToString();
        }

        random.isOn = info.canRandom == 0 ? true : false;
        type.isOn = info.type == 0 ? true : false;
    }

    public void OnBtnClickSave()
    {
        info.Id = int.Parse(idField.text);
        info.content = ContentField.text;

        info.optionIds.Clear();
        
        for (int i=0; i<opField.Count; ++i)
        {
            var o = opField[i];
            if (o.text.Length > 0)
                info.optionIds.Add(int.Parse(o.text));
        }
        info.animation = GetDropDownText(animation);
        info.voice = GetDropDownText(voice);
        info.type = type.isOn ? 0:1;
        
        info.canRandom = random.isOn ? 0 : 1;
        info.isChatBubble = chatBubble.isOn ? 1 : 0;
        info.love = requireLove.text.Length > 0? int.Parse(requireLove.text) : 0;
       
        AVGDataManager.instance.SaveDialogue(info);
        editorrr.AddOrRefreshDiaComponent(info);
        gameObject.SetActive(false);

    }
    public void SetAniDropDownData()
    {
        int oidx = 0;

        for (int i = 0; i < _Anidropdownlist.Count; ++i)
        {
            if (_Anidropdownlist[i].text == info.animation)
                oidx = i;
        }

        animation.value = oidx;
    }


    public void SetDropDownData()
    {
        int oidx = 0;

        for (int i = 0; i < _dropdownlist.Count; ++i)
        {
            if (_dropdownlist[i].text == info.voice)
                oidx = i;
        }

        voice.value = oidx;
    }

    public void RefreshVoiceDropDown()
    {
        voice.ClearOptions();
        _dropdownlist.Clear();

        var voices = ResourcesManager.instance.audioCllips;
        foreach (var v in voices)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = v.Key;
            _dropdownlist.Add(data);
        }

        voice.AddOptions(_dropdownlist);
    }



    public void OnAddVoice()
    {
        string path = OpenFileDialog.OpenFile("mp3|wav");
        path = path.Replace(@"\", "/");
    }

    public void OnBtnClickClose()
    {
        gameObject.SetActive(false);
    }

    public void OnBtnClickDel()
    {
        AVGDataManager.instance.DeleteDialogue(info);
        editorrr.DelDiaComponent(info);
        gameObject.SetActive(false);
    }



    public void RefreshAnimatonDropDown()
    {
        animation.ClearOptions();
        _Anidropdownlist.Clear();

        var ani = MikoChi.instance.GetAnimatorList();

        for (int i = 0; i < ani.Count; ++i)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = ani[i].animationName;
            _Anidropdownlist.Add(data);
        }

        animation.AddOptions(_Anidropdownlist);
    }




    public string GetDropDownText(Dropdown drop)
    {
        var list = drop.options;

        return list[drop.value].text;
    }
}

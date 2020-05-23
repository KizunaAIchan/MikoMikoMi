using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_DetailPage : UIComponentBase
{

    public UI_Config mainPage;

    public Dropdown notificationStart;
    public Dropdown notificationOver;

    public Dropdown animationStart;
    public Dropdown animationOver;

    public InputField name;
    public InputField ChannelId;

    private List<Dropdown.OptionData> _dropdownlist = new List<Dropdown.OptionData>();


    private ChannelConfig config;

    public AudioSource audio;
    public void Init()
    {
        config = new ChannelConfig();
        name.text = "";
        ChannelId.text = "";
        RefreshNotificationDropDown();
    }

    public override void Init(object args)
    {
        config = new ChannelConfig();
        config = (ChannelConfig)args;

        name.text = config.name;
        ChannelId.text = config.channelId;
        RefreshNotificationDropDown();
        SetDropDownData();
    }

    public void SetDropDownData()
    {
        int sidx = 0;
        int oidx = 0;

        for (int i =0;i< _dropdownlist.Count; ++i)
        {
            if (_dropdownlist[i].text == config.startNotification)
                sidx = i;
            if (_dropdownlist[i].text == config.closureNotice)
                oidx = i;
        }

        notificationOver.value = oidx;
        notificationStart.value = sidx;
    }

    public void RefreshNotificationDropDown()
    {
        notificationOver.ClearOptions();
        notificationStart.ClearOptions();
        _dropdownlist.Clear();

        var voices = ResourcesManager.instance.audioCllips;
        foreach(var v in voices)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = v.Key;
            _dropdownlist.Add(data);
        }

        notificationOver.AddOptions(_dropdownlist);
        notificationStart.AddOptions(_dropdownlist);

    }

    //0 start 1 over
    public void OnBtnClickPlay(int index)
    {
        string audio = "";
        var drop = index == 0 ? notificationStart : notificationOver;

        var list = drop.options;
        
        audio = list[drop.value].text;
   //     GameEngine.instance.miko.PlayAudio(audio, true);
        PlayAudio(audio);

    }

    public void OnBtnClickOpenFile(int index)
    {
        string path = OpenFileDialog.OpenFile("mp3|wav");
        path = path.Replace(@"\", "/");
        PlayAudio(path);
        RefreshNotificationDropDown();
    }

    public void OnBtnClickPlayAnimation(int index)
    {
       
    }

    public void OnClickDel()
    {
        audio.Stop();
        if (config.channelId == "UC-hM6YJuNYVAmUWxeIr9FeA")
        {
            PlayAudio("aasdsad");
            var d = UIManager.instance.ShowUI<UI_Dialog>(UINames.Dialog);
            d.transform.localPosition = Vector3.zero;
            return;
        }

        ResourcesManager.instance.DeleteChannelConfig(config);
        mainPage.DelComponent(config);

        mainPage.OnBtnClickBack();

    }
    public void OnClickSave()
    {
        config.channelId = ChannelId.text;
        config.name = name.text;
        config.startNotification = GetDropDownText(notificationStart);
        config.closureNotice = GetDropDownText(notificationOver);
        config.startAnima = GetDropDownText(animationStart);
        config.endAnima = GetDropDownText(animationOver);
        config.monitor = 0;

        ResourcesManager.instance.SaveChannelConfig(config);
        mainPage.AddOrRefreshComponent(config);

        audio.Stop();
        mainPage.OnBtnClickBack();
    }

    public string GetDropDownText(Dropdown drop)
    {
        var list = drop.options;

        return list[drop.value].text;
    }

    public void StopAudio()
    {
        audio.Stop();
    }

    public void PlayAudio(string str)
    {
        var audioclip = ResourcesManager.instance.GetAudioClipByName(str);
        if (audioclip == null)
        {
            GameEngine.instance.Error("Can't find audio：" + str);
            return;
        }

        audio.Stop();
        audio.clip = audioclip;
        audio.Play();
    }
}

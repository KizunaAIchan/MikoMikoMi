using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Config : UIBase
{

    public Sprite normalTab;
    public Sprite selectedTab;

    public List<Image> tabs;
    public List<Text> tabTexts;

    public List<Transform> nodelist;

    public Transform scrollView;
    public Transform tabButtonsNode;
    // Start is called before the first frame update


    public List<Toggle> toggleList;

    public UI_DetailPage detailpage;

    public Dictionary<string, UI_ConfigComponent> componentDic = new Dictionary<string, UI_ConfigComponent>();
    public enum ConfigPage
    {
        main = 0,
        normal,
        detail,
        //sub
    }
    public ConfigPage curpage = ConfigPage.main;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool chatbubble = false;
    private bool top = false;
    private bool mut = false;
    private bool cpu = false;



    public void InitComponent()
    {
        ClearComponent();
        OnBtnClickBack();
       
        var info = ResourcesManager.instance.GetChannelConfigs();
        for (int i =0; i< info.Count; ++i)
        {
            var item = UIManager.instance.CreateComponent<UI_ConfigComponent>(UINames.ConfigComponet, scrollView);
            item.Init(info[i]);
            item.callback = OnBtnClickDetail;
            componentDic[info[i].channelId] = item;
        }
    }

    public void AddOrRefreshComponent(ChannelConfig config)
    {
        UI_ConfigComponent component = null;
        if (componentDic.TryGetValue(config.channelId, out component))
        {
            component.Init(config);
            return;
        }

        var item = UIManager.instance.CreateComponent<UI_ConfigComponent>(UINames.ConfigComponet, scrollView);
        item.Init(config);
        item.callback = OnBtnClickDetail;
        componentDic[config.channelId] = item;
    }

    public void ChangeCurPage(ConfigPage page)
    {
        curpage = page;
    }

    public void OnBtnClickAdd()
    {
        ChangeCurPage(ConfigPage.detail);
        RefreshPage();
        detailpage.Init();
    }

    public void OnBtnClickBack()
    {
        ChangeCurPage(ConfigPage.main);
        RefreshPage();
    }

    public void OnBtnClickQuit()
    {
        Close();
    }


    public void RefreshPage()
    {
        tabButtonsNode.gameObject.SetActive(curpage != ConfigPage.detail);


        if (curpage == ConfigPage.normal)
            RefreshNormalPage();

        for (int i =0; i< nodelist.Count; ++i)
        {
            nodelist[i].gameObject.SetActive(i == (int)curpage);
        }

        for (int i = 0; i < tabs.Count; ++i)
        {
            tabs[i].sprite = i == (int)curpage ? selectedTab:normalTab;
            tabTexts[i].color = i == (int)curpage ? Color.white : Color.red;
        }
    }

    public void DelComponent(ChannelConfig config)
    {
        UI_ConfigComponent component = null;
        if (componentDic.TryGetValue(config.channelId, out component))
        {
            componentDic.Remove(config.channelId);
            component.Close();
            return;
        }
    }

    public void ClearComponent()
    {
        foreach(var v in componentDic)
        {
            v.Value.Close();

        }
        componentDic.Clear();
    }

    public void OnBtnClickDetail(ChannelConfig config)
    {
        ChangeCurPage(ConfigPage.detail);
        RefreshPage();
        detailpage.Init(config);
    }




    public void OnBtnClickTab(int idx)
    {
        curpage = (ConfigPage)idx;
        RefreshPage();
    }

    public void ChangeLanguage(int index)
    {
        for (int i = 0; i < toggleList.Count; ++i)
        {
            if (toggleList[i].isOn)
            {
                LanguageManager.instance.ChangeLanuage((LanguageManager.LanguageType)i);
                return;
            }

        }
    }


    public void SetWindowTop()
    {

    }

    public void RefreshNormalPage()
    {
        for (int i = 0; i < toggleList.Count; ++i)
        {
            toggleList[i].isOn = (int)LanguageManager.instance.curLagType == i;

        }

        chatbubble = ResourcesManager.instance.GetChatBubble();
        top = ResourcesManager.instance.GetTopOn();
        mut = ResourcesManager.instance.GetMute();
        cpu = GameEngine.instance.cpuNode.gameObject.activeSelf;
        RefreshBtn();
    }






    public enum BtnType
    {
        TOP = 0,
        MUTE = 1,
        STARUP = 2,
        CHATBUBBLE = 3,
        CPURAM = 4,
    }

    public Sprite onsp;
    public Sprite offsp;

    public Image[] btns;
    public Text[] btntxts;

    public void OnSettingBtnClick(int index)
    {
        //on top
        if (index == (int)BtnType.TOP)
        {
            top = !top;
            ResourcesManager.instance.SetTop(top);
        }
        //on mute
        if (index == (int)BtnType.MUTE)
        {
            mut = !mut;
            ResourcesManager.instance.SetMute(mut);
        }
        //onstarup
        if (index == (int)BtnType.STARUP)
        {

        }

        //on chatbubble
        if (index == (int)BtnType.CHATBUBBLE)
        {
            chatbubble = !chatbubble;
            ResourcesManager.instance.SetChatBubble(chatbubble);
        }

        if (index == (int)BtnType.CPURAM)
        {
            cpu = !cpu;
            GameEngine.instance.SetCPURAM(cpu);
        }

        RefreshBtn();
        ResourcesManager.instance.SaveToJsonConfig();

    }

    public void RefreshBtn()
    {
        btns[(int)BtnType.TOP].sprite = ResourcesManager.instance.GetTopOn() ? onsp : offsp;
        btns[(int)BtnType.MUTE].sprite = ResourcesManager.instance.GetMute() ? onsp : offsp;
        btns[(int)BtnType.CHATBUBBLE].sprite = ResourcesManager.instance.GetChatBubble() ? onsp : offsp;
        btns[(int)BtnType.STARUP].sprite = PlayerPrefs.HasKey("MikoMiko") ? onsp : offsp;
        btns[(int)BtnType.CPURAM].sprite = GameEngine.instance.cpuNode.gameObject.activeSelf ? onsp : offsp;

        btntxts[(int)BtnType.STARUP].text = PlayerPrefs.HasKey("MikoMiko") ? "ON" :"OFF";
        btntxts[(int)BtnType.TOP].text = ResourcesManager.instance.GetTopOn() ? "ON" :"OFF";
        btntxts[(int)BtnType.CHATBUBBLE].text = ResourcesManager.instance.GetChatBubble() ? "ON" :"OFF";
        btntxts[(int)BtnType.MUTE].text = ResourcesManager.instance.GetMute() ? "ON" : "OFF";
        btntxts[(int)BtnType.CPURAM].text = GameEngine.instance.cpuNode.gameObject.activeSelf? "ON" : "OFF";


        btntxts[(int)BtnType.STARUP].color = PlayerPrefs.HasKey("MikoMiko") ? Color.red : Color.white;
        btntxts[(int)BtnType.TOP].color = ResourcesManager.instance.GetTopOn() ? Color.red : Color.white;
        btntxts[(int)BtnType.CHATBUBBLE].color = ResourcesManager.instance.GetChatBubble() ? Color.red : Color.white;
        btntxts[(int)BtnType.MUTE].color = ResourcesManager.instance.GetMute() ? Color.red : Color.white;
        btntxts[(int)BtnType.CPURAM].color = GameEngine.instance.cpuNode.gameObject.activeSelf ? Color.red : Color.white;

    }
}

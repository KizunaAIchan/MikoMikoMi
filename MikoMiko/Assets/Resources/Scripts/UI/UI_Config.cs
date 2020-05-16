using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Config : UIBase
{

    public Transform configNode;
    public Transform detailNode;
    public Transform scrollView;
    public List<Transform> nodelist;

    // Start is called before the first frame update

    public UI_DetailPage detailpage;

    public Dictionary<string, UI_ConfigComponent> componentDic = new Dictionary<string, UI_ConfigComponent>();
    public enum ConfigPage
    {
        main = 0,
        detail,
    }
    public ConfigPage curpage = ConfigPage.main;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitComponent()
    {
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
        for (int i =0; i< nodelist.Count; ++i)
        {
            nodelist[i].gameObject.SetActive(i == (int)curpage);
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

    }

    public void OnBtnClickDetail(ChannelConfig config)
    {
        ChangeCurPage(ConfigPage.detail);
        RefreshPage();
        detailpage.Init(config);
    }
}

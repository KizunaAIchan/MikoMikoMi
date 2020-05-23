using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVGEditor : UIBase
{


    public Transform DialogueNode;
    public Transform OptionNode;


    public Transform dialogScroll;
    public Transform optionScroll;
    public Dictionary<int, AVGDataComponent> diaComponentDic =new Dictionary<int, AVGDataComponent>();
    public Dictionary<int, AVGDataComponent> opComponentDic = new Dictionary<int, AVGDataComponent>();

    public AVGEditorDialogue dialogue;
    public AVGEditorOption optionpage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init()
    {
        ClearDiaComponents();
        ClearOpComponents();

        InitDialogueComponent();
        InitOptionCompoent();

        DialogueNode.gameObject.SetActive(false);
        OptionNode.gameObject.SetActive(false);
    }

    public  void InitDialogueComponent()
    {
        var items = AVGDataManager.instance.dialogueInfos;
        foreach(var item in items)
        {
            var com = UIManager.instance.CreateComponent<AVGDataComponent>(UINames.AVGDataComponet, dialogScroll);
            com.Init(AVGDataComponent.AVGComponentType.Dialogue, item.Value);
            com.callback = OnBtnClickDialogueDetail;
            diaComponentDic[item.Key] = com;
        }
    }

    public void InitOptionCompoent()
    {
        var items = AVGDataManager.instance.optionInfos;
        foreach (var item in items)
        {
            var com = UIManager.instance.CreateComponent<AVGDataComponent>(UINames.AVGDataComponet, optionScroll);
            com.Init(AVGDataComponent.AVGComponentType.Option, item.Value);
            com.callback = OnBtnClickOptionDetail;
            opComponentDic[item.Key] = com;
        }
    }

    public  void OnBtnClickExit()
    {
        Close();


    }


    #region Add Del


    public void AddOrRefreshDiaComponent(DialogueInfo config)
    {
        AVGDataComponent component = null;
        if (diaComponentDic.TryGetValue(config.Id, out component))
        {
            component.Init(AVGDataComponent.AVGComponentType.Dialogue,  config);
            return;
        }

        var item = UIManager.instance.CreateComponent<AVGDataComponent>(UINames.AVGDataComponet, dialogScroll);
        item.Init(AVGDataComponent.AVGComponentType.Dialogue, config);
        item.callback = OnBtnClickDialogueDetail;
        diaComponentDic[config.Id] = item;
    }

    public void AddOrRefreshOpComponent(OptionInfo config)
    {
        AVGDataComponent component = null;
        if (opComponentDic.TryGetValue(config.Id, out component))
        {
            component.Init(AVGDataComponent.AVGComponentType.Option, config);
            return;
        }

        var item = UIManager.instance.CreateComponent<AVGDataComponent>(UINames.AVGDataComponet, optionScroll);
        item.Init(AVGDataComponent.AVGComponentType.Option, config);
        item.callback = OnBtnClickOptionDetail;
        opComponentDic[config.Id] = item;
    }








    public void DelDiaComponent(DialogueInfo config)
    {
        AVGDataComponent component = null;
        if (diaComponentDic.TryGetValue(config.Id, out component))
        {
            diaComponentDic.Remove(config.Id);
            component.Close();
            return;
        }
    }

    public void DelOpComponent(OptionInfo config)
    {
        AVGDataComponent component = null;
        if (opComponentDic.TryGetValue(config.Id, out component))
        {
            opComponentDic.Remove(config.Id);
            component.Close();
            return;
        }
    }


    #endregion

    public void ClearOpComponents()
    {
        foreach (var v in opComponentDic)
        {
            v.Value.Close();

        }
        opComponentDic.Clear();
    }

    public void ClearDiaComponents()
    {
        foreach (var v in diaComponentDic)
        {
            v.Value.Close();

        }
        diaComponentDic.Clear();
    }

    public void OnBtnClickClose()
    {
        Close();
    }
    public void OnBtnClickDialogueDetail(object config)
    {
        var c = (DialogueInfo)config;
        dialogue.InitData(c);
        DialogueNode.gameObject.SetActive(true);
    }

    public void OnBtnClickOptionDetail(object config)
    {
        var c = (OptionInfo)config;
        optionpage.InitData(c);
        OptionNode.gameObject.SetActive(true);
    }

    public void OnBtnClickAddDialogue()
    {
        DialogueNode.gameObject.SetActive(true);
        dialogue.InitData();

    }

    public void OnBtnClickAddOption()
    {
        OptionNode.gameObject.SetActive(true);
        optionpage.InitData();
    }
}

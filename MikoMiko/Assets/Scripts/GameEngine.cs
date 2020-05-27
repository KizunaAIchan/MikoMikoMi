﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{
    public static GameEngine instance = null;

    public string mikoPath = "MikoMikoModels/MikoChi";

    //test
    public MikoChi miko = null;
    private void Awake()
    {
        instance = this;
      
        
    }
    public void Start()
    {
        Init();
        CreateMikoChi();
    }

    public void Init()
    {

        TimerManager.InitSingletonInstance();
        EventManager.InitSingletonInstance();
        WinForm.InitSingletonInstance();



        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Faq, ForTest, 1);
     //   EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Oyasumi, ForTest, 1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Oyasumi, LiveStop, 1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Hajimaruyo, ShowChatBubble, 1);

        HttpRequest.instance.InitListener();
        LanguageManager.instance.InitLanguage();


    }

    public void CreateMikoChi()
    {
        if (this.miko == null)
        {
            var mikoO = Resources.Load(mikoPath) as GameObject;
            var miko = GameObject.Instantiate(mikoO, null);
            miko.transform.position = Vector3.zero;
            miko.transform.rotation = Quaternion.identity;
            this.miko = miko.GetComponent<MikoChi>();
        }

        this.miko.InitMikoChi();
     //   this.miko.PlayAnimator("drag");
    }



    // Update is called once per frame
    void Update()
    {
        TimerManager.instance.Update();
    }
    private void LateUpdate()
    {
       
    }


    public void ShowChatBubble(int id, object args)
    {
        var config = args as ChannelConfig;
        if (config == null) return;
        ChatBubble chatBubble = null;
        if (UIManager.instance.IsAlive(UINames.ChatBubble))
        {
            chatBubble = UIManager.instance.GetAliveUI<ChatBubble>(UINames.ChatBubble);
        }
        else
        {
            chatBubble = UIManager.instance.ShowUI<ChatBubble>(UINames.ChatBubble);
        }

        if (UIManager.instance.IsAlive(UINames.QuickJump))
        {
            var j = UIManager.instance.GetAliveUI<UI_QuickJump>(UINames.QuickJump);
            if (j != null)
                j.AddComponent(config.channelId);

        }
        else
        {
            var j = UIManager.instance.ShowUI<UI_QuickJump>(UINames.QuickJump);
            j.transform.localPosition = new Vector3(120, 140, 0);
            j.HideComponentNode();
            j.InitComponent();

        }
        chatBubble.transform.localPosition = new Vector3(-160, 130, 0);

        chatBubble.DoFadeIn();

        chatBubble.ShowNotification(config.name, config.channelId);
    }



    public void LiveStop(int id, object args)
    {
        
        var config = args as ChannelConfig;
        if (config == null) return;
        var close = HttpRequest.instance.GetChannelsByStatus(LiveStatus.Streaming).Count == 0;

            if (HttpRequest.instance.GetChannelsByStatus(LiveStatus.Streaming).Count == 0)
        {

        }
        if (UIManager.instance.IsAlive(UINames.QuickJump))
        {
            var j = UIManager.instance.GetAliveUI<UI_QuickJump>(UINames.QuickJump);
            if (j != null)
                j.RemoveComponent(config.channelId);

            if (close)
                j.onCloseUI();
        }
    }

    public void qweqweqweqweqwe()
    {
        ChatBubble chatBubble = null;
        if (UIManager.instance.IsAlive(UINames.ChatBubble))
        {

        }
        else
        {
            chatBubble = UIManager.instance.ShowUI<ChatBubble>(UINames.ChatBubble);
        }
        chatBubble.transform.localPosition = new Vector3(-160, 130, 0);

        chatBubble.DoFadeIn();
    //    chatBubble.ShowNotification("MikoChi");
    }

   

    public Text t;

    public void ForTest(int id, object args)
    {
        if ((int)EventManager.EventType.MikoChi_Hajimaruyo == id)
        {

        }
      //  string a = (string)args;
      ////  Debug.Log("111");
        t.text = (int)EventManager.EventType.MikoChi_Hajimaruyo == id ? "Online" :"offline";
    }

    public void ForTest1(int id, object args)
    {
       // string a = (string)args;
        //Debug.Log("222");
    }



    public void Error(string log)
    {
        Debug.Log(log);
    }
}

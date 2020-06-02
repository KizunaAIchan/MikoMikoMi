using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{

    [DllImport("DesktopApplication")]
    private static extern int GetCPUPercent();

    public static GameEngine instance = null;

    public string mikoPath = "MikoMikoModels/MikoChi";


    public Transform cpuNode;
    public Image cpu;
    public Image RAM;
    public Text cput;
    public Text Ramt;

    public bool showChatBubble = true;

    public float audioVolume = 1;
    //test
    public MikoChi miko = null;

    public GameObject Loading;

    private float timer = 1;
    private void Awake()
    {
        instance = this;
      
        
    }
    public void Start()
    {
        Init();
        InitTray();
        CreateMikoChi();
    }

    public void Init()
    {

        TimerManager.InitSingletonInstance();
        EventManager.InitSingletonInstance();



        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Faq, ForTest, 1);
     //   EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Oyasumi, ForTest, 1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Oyasumi, LiveStop, 1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Stop_Listen, LiveStop, 1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Hajimaruyo, ShowChatBubble, 1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Bug, ShowChatBubblev2, 1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Chat, ShowChatBubblev2, 1);

        //   DBManager.instance.InitDB();
        HttpRequest.instance.InitListener();
        LanguageManager.instance.InitLanguage();
        TimerManager.instance.Init();


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
        this.miko.PlayAnimator("WavingHand");
        this.miko.PlayAudio("nya");
        Loading.SetActive(false);
        EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Chat, 1, "にゃっはろ～");
        this.miko.AddRandomChat();

    }



    // Update is called once per frame
    void Update()
    {
        if (TimerManager.instance == null)
            return;
        TimerManager.instance.Update();

        if (!GameEngine.instance.cpuNode.gameObject.activeSelf) return;

        MemoryInformation memInfo = Rainity.GetMemoryInformation();
        Ramt.text = Mathf.Round(memInfo.ramUsed / memInfo.ramTotal * 100).ToString() + "%";
        RAM.fillAmount = memInfo.ramUsed / memInfo.ramTotal;
        DiskInformation diskInfo = Rainity.GetDiskInformation("C:\\");

     

        if (timer <= 0)
        {
            timer = 1f;
            var cpuUsage = GetCPUPercent() / 1000000f;
            cput.text = Mathf.Round(cpuUsage).ToString() + "%";
            cpu.fillAmount = cpuUsage / 100;
        }
        timer -= Time.deltaTime;
        //var d = Rainity.GetWeatherInformation();
        // Rainity.GetWallpaperImage();
        //int x = 0;
    }
    private void LateUpdate()
    {
       
    }


    public void OnDestroy()
    {
       // DBManager.instance.CloseDB();
        Debug.Log("exit");
    }

    public void ShowChatBubblev2(int id, object args)
    {
        if (!showChatBubble) return;
        string content = args as string;
        ChatBubble chatBubble = null;
        if (UIManager.instance.IsAlive(UINames.ChatBubble))
        {
            chatBubble = UIManager.instance.GetAliveUI<ChatBubble>(UINames.ChatBubble);
        }
        else
        {
            chatBubble = UIManager.instance.ShowUI<ChatBubble>(UINames.ChatBubble);
            chatBubble.transform.localPosition = new Vector3(-160, 130, 0);

        }
        chatBubble.DoFadeIn();

        chatBubble.ShowNormalMessage(content);
    }


    public void ShowChatBubble(int id, object args)
    {
        var config = args as ChannelConfig;
        if (config == null) return;
        if (showChatBubble)
        {
            ChatBubble chatBubble = null;
            if (UIManager.instance.IsAlive(UINames.ChatBubble))
            {
                chatBubble = UIManager.instance.GetAliveUI<ChatBubble>(UINames.ChatBubble);
            }
            else
            {
                chatBubble = UIManager.instance.ShowUI<ChatBubble>(UINames.ChatBubble);
            }

            chatBubble.transform.localPosition = new Vector3(-160, 130, 0);

            chatBubble.DoFadeIn();

            chatBubble.ShowNotification(config.name, config.channelId);
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
       
    }



    public void LiveStop(int id, object args)
    {
        
        var config = args as ChannelConfig;
        if (config == null) return;
        var close = HttpRequest.instance.GetChannelsByStatus(LiveStatus.Streaming).Count == 0;

       
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
        //t.text = (int)EventManager.EventType.MikoChi_Hajimaruyo == id ? "Online" :"offline";
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

    public void InitTray()
    {
        var tray = Rainity.CreateSystemTrayIcon();
        if (tray != null)
        {
            tray.AddItem("Exit", null);
            tray.SetTitle("Rainity Demo Application");
        }
    }


    public void SetCPURAM(bool show)
    {
        cpuNode.gameObject.SetActive(show);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{
    public static GameEngine instance = null;

    public string mikoPath = "MikoMikoModels/MikoChi";

    //test
    public MikoChi miko;
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
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Oyasumi, ForTest, 1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Hajimaruyo, ForTest, 1);

        HttpRequest.instance.InitListener();
        LanguageManager.instance.InitLanguage();


    }

    public void CreateMikoChi()
    {
        var mikoO = Resources.Load(mikoPath) as GameObject;
        var miko = GameObject.Instantiate(mikoO, null);
        miko.transform.position = Vector3.zero;
        miko.transform.rotation = Quaternion.identity;
        this.miko = miko.GetComponent<MikoChi>();
        this.miko.InitMikoChi();
    }



    // Update is called once per frame
    void Update()
    {
       
    }
    private void LateUpdate()
    {
       
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

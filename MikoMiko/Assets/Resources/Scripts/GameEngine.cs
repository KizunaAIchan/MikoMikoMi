using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public static GameEngine instance = null;

    public string mikoPath = "MikoMikoModels/MikoChi";

    //test
    public GameObject miko;
    private void Awake()
    {
        instance = this;
        var mikoO = Resources.Load(mikoPath) as GameObject;
        miko = GameObject.Instantiate(mikoO, null);
        miko.transform.position = Vector3.zero;
        miko.transform.rotation = Quaternion.identity;
        Init();
    }

    public void Init()
    {
        TimerManager.InitSingletonInstance();
        EventManager.InitSingletonInstance();


        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Hajimaruyo, ForTest,1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Faq, ForTest1, 1);

    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForTest(int id, object args)
    {
        string a = (string)args;
        Debug.Log(a);
    }

    public void ForTest1(int id, object args)
    {
        string a = (string)args;
        Debug.Log(a);
    }
}

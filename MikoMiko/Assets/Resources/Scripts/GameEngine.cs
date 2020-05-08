using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public static GameEngine instance = null;

    public string mikoPath = "MikoMikoModels/MikoChi";

    //test
    public MikoChi miko;
    private void Awake()
    {
        instance = this;
      
        Init();
        CreateMikoChi();
    }

    public void Init()
    {
        TimerManager.InitSingletonInstance();
        EventManager.InitSingletonInstance();


       
    //    EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Oyasumi, ForTest1, 1);

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

    public void ForTest(int id, object args)
    {
       // string a = (string)args;
        Debug.Log("111");
    }

    public void ForTest1(int id, object args)
    {
       // string a = (string)args;
        Debug.Log("222");
    }



    public void Error(string log)
    {
        Debug.Log(log);
    }
}

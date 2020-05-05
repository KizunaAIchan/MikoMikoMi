using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer{
        public int id = 0;
        public float duration = 0f;
        public float startTime = 0f;
        public float endTime = 0f;

        public float delay = 0f;
        public int faceId = 0;
        public TimerManager.TimerCallBack callBack;
    }
public class TimerManager
{
    private static TimerManager _instance;
    private static int timerId = 0;
    private MikoChi miko;
    public static TimerManager instance{
        get{

            if (_instance == null)
                _instance = new TimerManager();
            return _instance;
        }
    }
    public delegate void TimerCallBack(int id);
    private Dictionary<int,Timer> timers = new Dictionary<int,Timer>();
    private List<int> waitForDel = new List<int>();
   public void Init(MikoChi miko){
       this.miko = miko;
       timers.Clear();
       waitForDel.Clear();
   }

    
    public void Update()
    {
        waitForDel.Clear();
        float curTime = Time.realtimeSinceStartup;
        foreach(var v in timers){
            var timer = v.Value;
            if (curTime >= timer.endTime){
                timer.callBack(timer.faceId);
                waitForDel.Add(v.Key);
                continue;
            }
            if (curTime - timer.delay >= timer.startTime){
                
            }
        }

        for (int i =0; i< waitForDel.Count; ++i){
            timers.Remove(waitForDel[i]);
        }
    }

    public int AddTimer(float duration, int faceId, TimerCallBack callBack){
        float curTime = Time.realtimeSinceStartup;
        Timer timer = new Timer();
        timer.startTime = curTime;
        timer.duration = duration;
        timer.endTime = curTime + duration;
        timer.faceId = faceId;
        timer.callBack = callBack;
        timer.id = timerId++;
        timers[timer.id] = timer;
        return timer.id;
    }
}

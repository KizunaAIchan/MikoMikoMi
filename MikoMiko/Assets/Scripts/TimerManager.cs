using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timer
{
    public int id = 0;
    public float duration = 0f;
    public float startTime = 0f;
    public float endTime = 0f;

    public float delay = 0f;
    public int faceId = 0;
    public TimerManager.TimerCallBack callBack;
}

public class TimerManager : Singleton<TimerManager>
{
    private static int timerId = 0;
    private MikoChi miko;
    public delegate void TimerCallBack();
    private Dictionary<int, Timer> timers = new Dictionary<int, Timer>();
    private List<int> waitForDel = new List<int>();


    public bool sleepMode = true;

    public int AlarmId = 0;

    public struct MuteModeInfo
    {
        public int hour;
        public int minute;

        public bool Earlier(int h, int m)
        {
            if (h == hour)
                return minute > m;
            return hour > h;
        }

        public void SetData(TimeInfo t)
        {
            hour = t.hour;
            minute = t.minute;
        }
    }
    public MuteModeInfo startMuteTime;
    public MuteModeInfo endMuteTime;
    //public void Init(MikoChi miko)
    //{
    //    this.miko = miko;
    //    timers.Clear();
    //    waitForDel.Clear();
    //}
    public override void Init()
    {
        var s = ResourcesManager.instance.GetMuteTime(0);
        var e = ResourcesManager.instance.GetMuteTime(1);

        startMuteTime.SetData(s);
        endMuteTime.SetData(e);
        sleepMode = ResourcesManager.instance.GeSleepMode();
    }

    public void Update()
    {
        waitForDel.Clear();
        float curTime = Time.realtimeSinceStartup;
        foreach (var v in timers)
        {
            var timer = v.Value;
            if (curTime >= timer.endTime)
            {
                timer.callBack();
                waitForDel.Add(v.Key);
                continue;
            }
            if (curTime - timer.delay >= timer.startTime)
            {

            }
        }

        for (int i = 0; i < waitForDel.Count; ++i)
        {
            timers.Remove(waitForDel[i]);
        }



        if (InSleepMode())
        {
            GameEngine.instance.audioVolume = 0;
        }
        else
        {
            GameEngine.instance.audioVolume = ResourcesManager.instance.GetMute() ? 0 : 1;
        }
    }

    public int AddTimer(float duration, TimerCallBack callBack)
    {
        float curTime = Time.realtimeSinceStartup;
        Timer timer = new Timer();
        timer.startTime = curTime;
        timer.duration = duration;
        timer.endTime = curTime + duration;
        timer.callBack = callBack;
        timer.id = timerId++;
        timers[timer.id] = timer;
        return timer.id;
    }

    public void RemoveTimer(int id)
    {

        if (timers.ContainsKey(id))
            timers.Remove(id);
    }


    public void SetAlarm(DateTime alarm,string content = "Kimo",  string voice = "nya", string anima = "Idle")
    {
        StopAlarm();
        DateTime s = DateTime.Now;
        var w = alarm - s;
        AlarmId = TimerManager.instance.AddTimer((float)w.TotalSeconds, () =>
        {
            GameEngine.instance.miko.PlayAnimator(anima);
            GameEngine.instance.miko.PlayAudio(voice, true);
            EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Chat, 1, content);
        });
    }

    public void StopAlarm()
    {
        RemoveTimer(AlarmId);
    }


    public bool InSleepMode()
    {
        if (!sleepMode) return false;

        DateTime currentTime = DateTime.Now;
        var hour = currentTime.Hour;
        var Min = currentTime.Minute;

        if (startMuteTime.hour <= endMuteTime.hour)
        {
            if (startMuteTime.Earlier(hour, Min))
                return false;
            if (endMuteTime.Earlier(hour, Min))
                return true;
        }
        else
        {
            if (endMuteTime.Earlier(hour, Min))
                return true;

            if (startMuteTime.Earlier(hour, Min))
                return false;
        }
        return true;

    }

}

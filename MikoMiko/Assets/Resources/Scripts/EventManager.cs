using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventArgs
{
    public Dictionary<string, string> strArgs;
}

public class EventManager : Singleton<EventManager>
{
    public enum EventType
    {
        MikoChi_Hajimaruyo,
        MikoChi_Oyasumi,
        Faq,
        Bug,
    }


    public enum EventSender
    {
        MikoChi,
    }

    //public enum Default


    public delegate void EventCallBack(int eventId, object args);

    public Dictionary<int, Dictionary<int,
        Dictionary<int, List<EventCallBack>>>> _listeners = new Dictionary<int, Dictionary<int, Dictionary<int, List<EventCallBack>>>>();

    public override void Init()
    {
        _listeners.Clear();
    }


    public void AddListener(int sender, int evtType, EventCallBack cb, int id)
    {
        if (cb == null)
            return;
        Dictionary<int, Dictionary<int, List<EventCallBack>>> senderDic = null;
        if (!_listeners.TryGetValue(sender, out senderDic))
        {
            senderDic = new Dictionary<int, Dictionary<int, List<EventCallBack>>>();
            _listeners[sender] = senderDic;
        }

        Dictionary<int, List<EventCallBack>> eventDic = null;
        if (!senderDic.TryGetValue(evtType, out eventDic))
        {
            eventDic = new Dictionary<int, List<EventCallBack>>();
            senderDic[evtType] = eventDic;
        }

        List<EventCallBack> eventCBList = null;
        if (!eventDic.TryGetValue(id, out eventCBList))
        {
            eventCBList = new List<EventCallBack>();
            eventDic[id] = eventCBList;
        }

        eventCBList.Add(cb);
    }

    public void RemoveListener(int sender, int evtType, EventCallBack cb, int id)
    {
        Dictionary<int, Dictionary<int, List<EventCallBack>>> senderDic = null;
        if (_listeners.TryGetValue(sender, out senderDic))
        {
            Dictionary<int, List<EventCallBack>> eventDic = null;
            if (senderDic.TryGetValue(evtType, out eventDic))
            {
                List<EventCallBack> eventCBList = null;
                if (eventDic.TryGetValue(id, out eventCBList))
                {
                    eventCBList.Remove(cb);
                    if (eventCBList.Count == 0)
                        eventDic.Remove(id);

                }
            }
        }
    }

    public void SendEvent(int sender, int evtType, int id, object args = null)
    {
        Dictionary<int, Dictionary<int, List<EventCallBack>>> senderDic = null;
        if (_listeners.TryGetValue(sender, out senderDic))
        {
            Dictionary<int, List<EventCallBack>> eventDic = null;
            if (senderDic.TryGetValue(evtType, out eventDic))
            {
                List<EventCallBack> eventCBList = null;
                if (eventDic.TryGetValue(id, out eventCBList))
                {
                    for (int i = 0; i < eventCBList.Count; ++i)
                    {
                        var cb = eventCBList[i];
                        cb(evtType, args);
                    }

                }
            }
        }


    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;


//
[System.Serializable]
public class YoutubeSnippet
{
    public string publishedAt;
    public string title;
}
[System.Serializable]
public class YoutubeItemsId
{
    public string kind;
    public string videoId;
}
[System.Serializable]
public class YoutubeItemsInfo
{
    public string kind;
    public string etag;
    public YoutubeItemsId id;
    public YoutubeSnippet snippet;
}
[System.Serializable]
public class YoutubePageInfo
{
    public int totalResults;
    public int resultsPerPage;
}
[System.Serializable]
public class YoutubeJson
{
    public string kind;
    public YoutubePageInfo pageInfo;
    public YoutubeItemsInfo[] items;
}

public class HttpRequest : MonoBehaviour
{
    public static HttpRequest instance = null;
    public float updateInterval = 10f;

    #region 
    //---------------------------------------------------
    //only for mikochi now
    //  public static string youtubeUrl = "https://www.googleapis.com/youtube/v3/search?part=snippet&channelId=UC-//hM6YJuNYVAmUWxeIr9FeA&eventType=live&type=video&key=AIzaSyBqEEHXfDAZ-4v8PEClTy0H6xWUE7SIxo4";


    //private const string _contactString1 = "https://www.googleapis.com/youtube/v3/search?part=snippet&channelId=";
    //private const string _contactString2 = "&eventType=live&type=video&key=";
    //private const string _youtubeApiKey = "AIzaSyBqEEHXfDAZ-4v8PEClTy0H6xWUE7SIxo4";
    //---------------------------------------------------
    #endregion


    // this url leads to the latest live streaming
    private const string youtubeUrl = "https://www.youtube.com/embed/live_stream?channel=";
    //to do
    public string[] channelIds = new string[] { "UC-hM6YJuNYVAmUWxeIr9FeA" };


    // Save curLiveStream State key channelid   value  State
    private Dictionary<string,bool> _curLiveStreamState = new Dictionary<string, bool>();
    private StringBuilder _strBuilder = new StringBuilder(256);
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        for (int i=0; i<channelIds.Length; ++i)
        {
            _curLiveStreamState[channelIds[i]] = false;
            StartCoroutine(CheckLiveState(ContactGetUrl(youtubeUrl, channelIds[i]), channelIds[i]));
        }
    }

    void OnDestory()
    {
        _curLiveStreamState.Clear();
    }


    public string HttpPost()
    {
        return "";
    }

    public IEnumerator CheckLiveState(string _url, string _waifu = "")
    {
        
        while (true)
        {
            WWW res = new WWW(_url);
            yield return res;
            if (res.error != null)
            {
                EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Faq, 1, res.error);
            }
            else
            {
                GetLiveStateFromViewCount(res.text, _waifu);
            }

            yield return new WaitForSecondsRealtime(updateInterval);
        }
    }



    public bool GetLiveStateFromViewCount(string _body, string _waifu)
    {
        bool isNew = false;
        int idx = _body.LastIndexOf("\"view_count\":");
        if (idx + 13 >= _body.Length)
            return false;
        char count = _body[idx + 13];
        EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Oyasumi, 1, null);

        if (count == '0')
        {
            if (_curLiveStreamState[_waifu])
                EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Oyasumi, 1, null);
            _curLiveStreamState[_waifu] = false;
            return false;
        }
        else
        {
            if (_curLiveStreamState[_waifu])
                return true;
            EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Hajimaruyo, 1, null);
            _curLiveStreamState[_waifu] = true;
        }

        return false;
    }


    public string ContactGetUrl(string url, string _channelId)
    {
        _strBuilder.Length = 0;
        _strBuilder.Append(url);
        _strBuilder.Append(_channelId);
        return _strBuilder.ToString();
   
    }
}
using System.Collections;
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


    //only for mikochi now
    public static string youtubeUrl = "https://www.googleapis.com/youtube/v3/search?part=snippet&channelId=UC-hM6YJuNYVAmUWxeIr9FeA&eventType=live&type=video&key=AIzaSyBqEEHXfDAZ-4v8PEClTy0H6xWUE7SIxo4";


    private const string _contactString1 = "https://www.googleapis.com/youtube/v3/search?part=snippet&channelId=";
    private const string _contactString2 = "&eventType=live&type=video&key=";
    private const string _youtubeApiKey = "AIzaSyBqEEHXfDAZ-4v8PEClTy0H6xWUE7SIxo4";


    //to do
    public string[] channelIds = new string[] { "UC-hM6YJuNYVAmUWxeIr9FeA" };
    //backup


    // Save curLiveStream Title key channelid key video id   value  title
    private Dictionary<string, Dictionary<string,string>> _curLiveStream = new Dictionary<string, Dictionary<string, string>>();
    private StringBuilder _strBuilder = new StringBuilder(256);
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i=0; i<channelIds.Length; ++i)
        {
            StartCoroutine(CheckLiveState(ContactGetUrl(channelIds[i], _youtubeApiKey), channelIds[i]));
        }
    }

    void OnDestory()
    {
        _curLiveStream.Clear();
    }


    public string HttpPost()
    {
        return "";
    }

    public IEnumerator CheckLiveState(string _url, string _waifu = "")
    {
        
        while (true)
        {
            WWW res = new WWW(youtubeUrl);
            yield return res;
            if (res.error != null)
            {
                EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Faq, 1, res.error);
            }
            else
            {
                EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Hajimaruyo, 1, res.error);
                GetLiveStateFromJson(res.text, _waifu);
            }

            yield return new WaitForSecondsRealtime(updateInterval);
        }
    }



    public bool GetLiveStateFromJson(string _url, string _waifu)
    {
        bool isNew = false;
        var youtube = JsonUtility.FromJson<YoutubeJson>(_url);
        Dictionary<string, string> dic = null;
        if (!_curLiveStream.TryGetValue(_waifu, out dic))
        {
            dic = new Dictionary<string, string>();
            _curLiveStream[_waifu] = dic;
        }

        if (youtube.pageInfo.totalResults < 1 || youtube.items == null)
        {
            dic.Clear();
            return false;
        }

        for (int i=0; i< youtube.pageInfo.totalResults; ++i)
        {
            var item = youtube.items[i];
            if (item.id == null || item.snippet == null) continue;
            if (dic.ContainsKey(item.id.videoId)) continue;
            dic[item.id.videoId] = item.snippet.title;
            isNew = true;
        }

        return false;
    }


    public string ContactGetUrl(string _channelId, string _apiKey)
    {
        _strBuilder.Length = 0;
        _strBuilder.Append(_contactString1);
        _strBuilder.Append(channelIds);
        _strBuilder.Append(_contactString2);
        _strBuilder.Append(_apiKey);

        return _strBuilder.ToString();
   
    }
}

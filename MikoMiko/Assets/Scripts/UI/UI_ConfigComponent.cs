using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConfigComponent : UIComponentBase
{
    // Start is called before the first frame update
    public Text status;
    public Text Name;
    public Text Livestatus;


    public string youtubeLink = "https://www.youtube.com/channel/";

    public delegate void OnClickDetail(ChannelConfig config);

    public OnClickDetail callback;

    public ChannelConfig ChannelConfig;
    public Image mask;
    public Button btn;
    public ButtonEvent btnEvent;
    private bool _canClick = true;
    private bool monitor = false;
    private float _nextClickTime = 0;
    private LiveStatus liveStatus = LiveStatus.Error;

    public override void Init(object args)
    {
        ChannelConfig = (ChannelConfig)args;
        youtubeLink += ChannelConfig.channelId;
        Name.text = ChannelConfig.name;
        _canClick = true;
        monitor = ResourcesManager.instance.CanMonitorChannel(ChannelConfig.channelId);
        status.text = monitor ? "ON" : "OFF";
        mask.gameObject.SetActive(!_canClick);
        btnEvent.enabled = _canClick;
        liveStatus = HttpRequest.instance.GetLiveStatus(ChannelConfig.channelId);
        SetLiveStatusText();

    }

    public void Update()
    {
        float time = _nextClickTime - Time.realtimeSinceStartup;
        if (time> 0)
        {
            if (_canClick)
            {
                _canClick = false;
            }

            if (!_canClick)
            {
                mask.fillAmount =time/ (HttpRequest.instance.updateInterval * 1.2f);
            }
            return;
        }
        else
        {
            if (!_canClick)
            {
                _canClick = true;
                btn.interactable = _canClick;
                btnEvent.enabled = _canClick;


            }
        }
        var s = HttpRequest.instance.GetLiveStatus(ChannelConfig.channelId);

        if (s != liveStatus)
        {
            liveStatus = s;
            SetLiveStatusText();
         
        }

        //if (!monitor  && ResourcesManager.instance.CanMonitorChannel(ChannelConfig.channelId))
        //{
        //    monitor = true;
        //    status.text = monitor ? "ON" : "OFF";
        //}

        //if (monitor && !ResourcesManager.instance.CanMonitorChannel(ChannelConfig.channelId))
        //{
        //    monitor = false;
        //    status.text = monitor ? "ON" : "OFF";
        //}
    }

    public void SetLiveStatusText()
    {
        switch (liveStatus)
        {
            case LiveStatus.Error:
                Livestatus.text = "Error";
                break;
            case LiveStatus.Offline:
                Livestatus.text = "Offline";

                break;
            case LiveStatus.Streaming:
                Livestatus.text = "Streaming";

                break;
            case LiveStatus.Notlisten:
                Livestatus.text = "X";

                break;
            default:
                break;
        }
    }

    public void OnBtnClickYoutube()
    {
        Application.OpenURL(youtubeLink);
    }

    public void OnBtnOpenNotification()
    {
        if (!_canClick) return;
        _canClick = !_canClick;
        btn.interactable = _canClick;
        btnEvent.enabled = _canClick;
        mask.gameObject.SetActive(!_canClick);
        _nextClickTime = Time.realtimeSinceStartup + HttpRequest.instance.updateInterval*1.2f;
        monitor = !monitor;
        ChannelConfig.monitor = monitor ? 0 : 1;
        status.text = monitor ? "ON" : "OFF";
        Livestatus.text = monitor ? "Checking" : "X";

        ResourcesManager.instance. SaveChannelConfig(ChannelConfig);
    }

    public void OnBtnDetail()
    {
        if (callback != null)
            callback(ChannelConfig);
    }
}

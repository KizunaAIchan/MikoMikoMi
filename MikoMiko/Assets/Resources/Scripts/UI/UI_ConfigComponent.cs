using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConfigComponent : UIComponentBase
{
    // Start is called before the first frame update
    public Text status;
    public Text Name;

    public string youtubeLink = "https://www.youtube.com/channel/";

    public delegate void OnClickDetail(ChannelConfig config);

    public OnClickDetail callback;

    public ChannelConfig ChannelConfig;

    public override void Init(object args)
    {
        ChannelConfig = (ChannelConfig)args;
        youtubeLink += ChannelConfig.channelId;
        Name.text = ChannelConfig.name;

    }

    public void OnBtnClickYoutube()
    {

    }

    public void OnBtnOpenNotification()
    {

    }

    public void OnBtnDetail()
    {
        if (callback != null)
            callback(ChannelConfig);
    }
}

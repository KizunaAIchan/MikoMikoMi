using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickJumpComponent : UIComponentBase
{



    public Text state;
    public Text name;

    private string jumpLink = "https://www.youtube.com/channel/";
    private string jumpLienEnd = "/live";
    public string channelId = "";
    public override void Init(object args)
    {
        var config = (ChannelConfig)args;
        if (config == null) return;

        name.text = config.name;
        state.text = "On Air";
        channelId = config.channelId;
    }
    public void onBtnClickJump()
    {
        var link = jumpLink + channelId + jumpLienEnd;
        Application.OpenURL(link);
    }
}

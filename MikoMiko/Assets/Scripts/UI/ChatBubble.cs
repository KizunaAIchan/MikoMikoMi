using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubble : UIBase
{
    public Text content;
    public Image bg;



    public float chatBubbleDuration = 3f;
    public float chatBubbleLife = 0f;
    public bool isShow = false;
    public Animation animation;
    private int endTimerId = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;

        if (isShow)
        {
            chatBubbleLife -= dt;
            if (chatBubbleLife < 0)
            {
                isShow = false;
                DoFadeOut();
                endTimerId = TimerManager.instance.AddTimer(0.3f, () =>
                {
                    CloseChatBubble();
                });
                //  CloseChatBubble();
            }
        }
    }



    public void DoFadeIn()
    {
        animation.Stop();
        animation.Play("chatBubbleFadeIn");
    }

    public void DoFadeOut()
    {
        animation.Stop();
        animation.Play("chatBubbleFadeOut");
    }

    public void ShowChatBubble()
    {
        DoFadeIn();
    }


    public void RandChat()
    {

    }

    public void CloseChatBubble()
    {
        endTimerId = -1;

        isShow = false;
        Close();
    }


    public void ShowNotification(string name, string channleid,  float delay = 0.2f, float duration = -1f)
    {
        if (endTimerId > 0)
            TimerManager.instance.RemoveTimer(endTimerId);

        isShow = true;
        chatBubbleLife = duration > 0 ? duration : chatBubbleDuration;
        content.text = "";

        string str = MikoMikoMi.mikomikomi.GetChatBubbleString(name, channleid, "[LID:18]");
        TimerManager.instance.AddTimer(delay, () =>
        {
           // string str = LanguageManager.instance.GetStringByLID("[LID:18]");
            content.text =  str;
        });
        
    }
}

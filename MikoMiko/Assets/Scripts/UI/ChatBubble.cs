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
            }
        }
    }



    public void DoFadeIn()
    {

    }

    public void DoFadeOut()
    {

    }

    public void ShowChatBubble()
    {
        DoFadeIn();
    }


    public void RandChat()
    {

    }


    public void ShowNotification()
    {

    }
}

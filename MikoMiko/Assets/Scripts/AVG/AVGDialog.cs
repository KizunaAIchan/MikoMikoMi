using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AVGDialog : MonoBehaviour
{
    [System.Serializable]
    public struct OptionButton
    {
        public Text text;
        public Image image;
        public GameObject gameObject;
    }


    private List<OptionInfo> optionsInfo = new List<OptionInfo>();
    public OptionButton[] optionBtns;
    public TextFadeIn textFadeIn;
    public Text content;
    public GameObject mask;
    public GameObject opNode;

    private DialogueInfo config;

    public void ShowDialog(DialogueInfo info)
    {
        mask.SetActive(true);
        opNode.SetActive(false);

        content.text = info.content;
        textFadeIn.SetText(info.content);
        optionsInfo.Clear();
        for (int i =0; i<info.optionIds.Count; ++i)
        {
            optionsInfo.Add(AVGDataManager.instance.GetOptionById(info.optionIds[i]));
        }
   //     optionsInfo = info.optionsInfo;
        textFadeIn.callback = ShowTextImmediately;

        GameEngine.instance.miko.PlayAnimator(info.animation);
        GameEngine.instance.miko.PlayAudio(info.voice,true);
        //if (info.Id == 6)
        //    GameEngine.instance.miko.PlayAnimation("StayHome");
        //if (info.Id ==7 )
        //    GameEngine.instance.miko.PlayAnimation("nyahello");
    }

    public void StartFadeIn()
    {
        textFadeIn.ShowText();
    }

    public void SetOptions(int noOption)
    {
        opNode.SetActive(noOption == 0 ? false : true);
        if (noOption == 1)
        {
            for (int i = 0, jCount = optionsInfo.Count; i < optionBtns.Length; ++i)
            {
                var btn = optionBtns[i];
                if (i >= jCount)
                {
                    btn.gameObject.SetActive(false);
                    continue;
                }

                Color c = btn.image.color;
                c.a = 0.5f;
                btn.image.color = c;

                Color cc = btn.text.color;
                cc.a = 0.5f;
                btn.text.color = cc;
    
                btn.gameObject.SetActive(true);
                btn.text.text = optionsInfo[i].content;
            }
        }
       
    }


    public void OnSelected(int index)
    {
        var info = optionsInfo[index];
        ResetContent(info.DialogueId);
        MikoChi.instance.AddLove(info.addLove);
        StartFadeIn();
    }

    public void ResetContent(int dialogId)
    {
        var info = AVGDataManager.instance.GetDialogueInfoById(dialogId);
        config = info;
        ShowDialog(info);
       // SetOptions(info.type);
    }

    public void ShowTextImmediately()
    {
        textFadeIn.Stop();

        mask.SetActive(false);
        SetOptions(config.type);
    }



    public void OnFinishFadeIn()
    {
        ShowOptions();
    }


    public void ShowOptions()
    {

    }

}

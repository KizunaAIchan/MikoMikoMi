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
    public UI_RightClickMenu mmm;

    private DialogueInfo config;


    public enum DialogType
    {
        Config,
        Normal,
    }

    public DialogType currentTyep = DialogType.Config;

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
        textFadeIn.callback = ShowTextImmediately;

        GameEngine.instance.miko.PlayAnimator(info.animation);
        GameEngine.instance.miko.PlayAudio(info.voice,true);
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
        if (currentTyep == DialogType.Config)
        {

            if (index == 0)
            {
                currentTyep = DialogType.Normal;
                var dialoginfo = AVGDataManager.instance.GetRandomDialogue();

                ResetContent(dialoginfo.Id);
                StartFadeIn();
            }
            if (index == 1)
            {
                mmm.OnBtnClickShowConfig();
              //  Close();
            }
            if (index == 2)
            {

            }

            return;
        }

        var info = optionsInfo[index];
        ResetContent(info.DialogueId);
        MikoChi.instance.AddLove(info.addLove, AddLoveType.Dialogue);
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
        if (currentTyep == DialogType.Config)
        {
            opNode.SetActive(true);
            for (int i = 0; i < optionBtns.Length; ++i)
            {
                var btn = optionBtns[i];

                Color c = btn.image.color;
                c.a = 0.5f;
                btn.image.color = c;

                Color cc = btn.text.color;
                cc.a = 0.5f;
                btn.text.color = cc;

                btn.gameObject.SetActive(true);
                if (i == 0)
                    btn.text.text =LanguageManager.instance.GetStringByLID("[LID:28]");
                if (i == 1)
                    btn.text.text = LanguageManager.instance.GetStringByLID("[LID:36]");
                if (i == 2)
                    btn.text.text = LanguageManager.instance.GetStringByLID("[LID:37]");
            }

        }
        else
        {
            SetOptions(config.type);
        }
    }



    public void OnFinishFadeIn()
    {
        ShowOptions();
    }


    public void ShowOptions()
    {
        currentTyep = DialogType.Config;
        mask.SetActive(true);
        opNode.SetActive(false);

        var s = LanguageManager.instance.GetStringByLID("[LID:35]");

        content.text = s;
        textFadeIn.SetText(s);
        textFadeIn.callback = ShowTextImmediately;
        
    }

}

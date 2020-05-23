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
        public GameObject gameObject;
    }


    private List<OptionInfo> optionsInfo;
    public OptionButton[] optionBtns;
    public TextFadeIn textFadeIn;
    public Text content;
    public GameObject mask;

    public void ShowDialog(DialogueInfo info)
    {
        mask.SetActive(true);
        content.text = info.content;
       // optionsInfo = info.optionsInfo;
        textFadeIn.callback = ShowTextImmediately;
    }

    public void SetOptions()
    {
        for (int i=0, jCount = optionsInfo.Count; i< optionBtns.Length; ++i)
        {
            var btn = optionBtns[i];
            if (i >= jCount)
            {
                btn.gameObject.SetActive(false);
                continue;
            }
            btn.gameObject.SetActive(true);
            btn.text.text = optionsInfo[i].content;
        }
    }


    public void OnSelected(int index)
    {
        var info = optionsInfo[index];
        ResetContent(info.DialogueId);
    }

    public void ResetContent(int dialogId)
    {
        var info = AVGDataManager.instance.GetDialogueInfoById(dialogId);

        ShowDialog(info);
        SetOptions();
    }

    public void ShowTextImmediately()
    {
        mask.SetActive(false);
        textFadeIn.Stop();
    }



    public void OnFinishFadeIn()
    {
        ShowOptions();
    }


    public void ShowOptions()
    {

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVGDataComponent : UIComponentBase
{

    public enum AVGComponentType
    {
        Option = 0,
        Dialogue,
    }

    public Text id;
    public Text Content;

    private DialogueInfo dialogConfig;
    private OptionInfo optionConfig;

    public delegate void OnClickDetailCallBack(object arg);
    // Start is called before the first frame update

    public OnClickDetailCallBack callback;

    public AVGComponentType type = AVGComponentType.Option;

    public void Init(AVGComponentType type, object arg)
    {
        this.type = type;
        if (type == AVGComponentType.Dialogue)
        {
            var config = (DialogueInfo)arg;
            id.text = config.Id.ToString();
            Content.text = config.content;
            dialogConfig = config;
        }
        else
        {
            var config = (OptionInfo)arg;
            id.text = config.Id.ToString();
            Content.text = config.content;
            optionConfig = config;
        }
    }

    public void OnBtnClickDetail()
    {
        if (callback != null)
        {
            if (type == AVGComponentType.Option)
                callback( optionConfig);
            else
                callback(dialogConfig);

        }
    }
}

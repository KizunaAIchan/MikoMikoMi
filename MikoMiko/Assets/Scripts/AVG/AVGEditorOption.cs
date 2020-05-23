using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVGEditorOption : MonoBehaviour
{

    public InputField idField;
    public InputField ReplyField;
    public InputField ContentField;



    public OptionInfo info = new OptionInfo();



    public void OnBtnClickSave()
    {
        info.Id = int.Parse(idField.text);
        info.content = ContentField.text;
        info.DialogueId = int.Parse(ReplyField.text);
    }

    public void OnBtnClickClose()
    {

    }
}

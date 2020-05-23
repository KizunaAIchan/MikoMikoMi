using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVGEditorDialogue : MonoBehaviour
{
    public InputField idField;
    public InputField ContentField;

    public List<InputField> opField;

    public Dropdown voice;
    public Dropdown animation;
    public Toggle type;
    public Toggle random;


    public DialogueInfo info = new DialogueInfo();



    public void OnBtnClickSave()
    {
        info.Id = int.Parse(idField.text);
        info.content = ContentField.text;

        for (int i=0; i<opField.Count; ++i)
        {
            var o = opField[i];
            if (o.text.Length > 0)
                info.optionIds.Add(int.Parse(o.text));
        }
        info.animation = GetDropDownText(animation);
        info.voice = GetDropDownText(voice);
        info.type = type.isOn ? 0:1;
        info.canRandom = random.isOn ? 0 : 1;
    }

    public void OnBtnClickClose()
    {

    }


    public string GetDropDownText(Dropdown drop)
    {
        var list = drop.options;

        return list[drop.value].text;
    }
}

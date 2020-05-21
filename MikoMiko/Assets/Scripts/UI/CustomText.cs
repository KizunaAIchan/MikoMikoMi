using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CustomText : Text
{
    [SerializeField]
    public bool isLanguageText = false;
    // Start is called before the first frame update
    public string LanguageId = "";
    void Start()
    {
        base.Start();
        ChangeLanguageSetting();
        UIManager.instance.AddCustomText(this);
    }



    public void ChangeLanguageSetting()
    {
        if (!Application.isPlaying)
            return;
        if (isLanguageText)
            text = LanguageManager.instance.GetStringByLID(LanguageId);

    }
}

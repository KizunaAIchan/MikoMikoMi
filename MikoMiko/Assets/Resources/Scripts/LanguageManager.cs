using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public enum LanguageType
    {
        Japanese,
        English,
        Chinese,
    }

    public LanguageType curLagType = LanguageType.Japanese;

    public static LanguageManager instance;
    public Dictionary<string, string> lagToJp = new Dictionary<string, string>(
        )
    {   {"[LID:1]", "終了"},
        {"[LID:2]", "通知設定"},
    };

    public Dictionary<string, string> lagToEn = new Dictionary<string, string>(
        )
    {   {"[LID:1]", "Exit"},
        {"[LID:2]", "Notification Settings"},
    };

    public Dictionary<string, string> lagToCn = new Dictionary<string, string>(
    )
    {   {"[LID:1]", "退出"},
        {"[LID:2]", "通知设定"},
    };

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public string GetStringByLID(string str)
    {
        string tmp = "";

        if (curLagType == LanguageType.Japanese)
        {
            lagToJp.TryGetValue(str, out tmp);
        }

        if (curLagType == LanguageType.English)
        {
            lagToEn.TryGetValue(str, out tmp);
        }

        if (curLagType == LanguageType.Chinese)
        {
            lagToCn.TryGetValue(str, out tmp);
        }

        return tmp;
    }
}

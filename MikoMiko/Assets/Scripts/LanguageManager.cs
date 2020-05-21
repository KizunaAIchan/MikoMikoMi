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
    public static Dictionary<string, string> lagToJp = new Dictionary<string, string>(
        )
    {   {"[LID:1]", "終了"},
        {"[LID:2]", "通知設定"},
        {"[LID:3]", "名前"},
        {"[LID:4]", "チャンネルID"},
        {"[LID:5]", "開始"},
        {"[LID:6]", "音声"},
        {"[LID:7]", "詳細"},
        {"[LID:8]", "通知"},
        {"[LID:9]", "追加"},
        {"[LID:10]", "戻る"},
        {"[LID:11]", "削除"},
        {"[LID:12]", "確認"},
        {"[LID:13]", "チャンネルID"},
        {"[LID:14]", "名前"},
        {"[LID:15]", "終わり"},
        {"[LID:16]", "動画"},
        {"[LID:17]", "通用"},

    };

    public static Dictionary<string, string> lagToEn = new Dictionary<string, string>(
        )
    {   {"[LID:1]", "Exit"},
        {"[LID:2]", "Notification Settings"},
        {"[LID:3]", "Name"},
        {"[LID:4]", "ChannelID"},
        {"[LID:5]", "Start"},
        {"[LID:6]", "Voice"},
        {"[LID:7]", "Detail"},
        {"[LID:8]", "Notice"},
        {"[LID:9]", "Add"},
        {"[LID:10]", "Back"},
        {"[LID:11]", "Del"},
        {"[LID:12]", "Save"},
        {"[LID:13]", "ChannelID"},
        {"[LID:14]", "Name"},
        {"[LID:15]", "End"},
        {"[LID:16]", "Animation"},
        {"[LID:17]", "Normal"},


    };

    public static Dictionary<string, string> lagToCn = new Dictionary<string, string>(
    )
    {   {"[LID:1]", "退出"},
        {"[LID:2]", "通知设定"},
        {"[LID:3]", "名字"},
        {"[LID:4]", "频道ID"},
        {"[LID:5]", "开始"},
        {"[LID:6]", "通知音"},
        {"[LID:7]", "详细"},
        {"[LID:8]", "通知"},
        {"[LID:9]", "添加"},
        {"[LID:10]", "返回"},
        {"[LID:11]", "删除"},
        {"[LID:12]", "确定"},
        {"[LID:13]", "频道ID"},
        {"[LID:14]", "名字"},
        {"[LID:15]", "结束"},
        {"[LID:16]", "动作"},
        {"[LID:17]", "常规"},

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

    public void InitLanguage()
    {
        ChangeLanuage((LanguageType)ResourcesManager.instance.GetLanguageType());
    }

    public void ChangeLanuage(LanguageType type)
    {
        curLagType = type;
        UIManager.instance.OnChangeLanguageSetting();
        ResourcesManager.instance.SetLanguageType((int)curLagType);

        ResourcesManager.instance.SaveToJsonConfig();

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

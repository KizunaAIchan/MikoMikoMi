using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class OptionInfo
{
    public int Id;
    //selected -->Dialogue
    public string content;
    public int DialogueId;
    public int addLove;
}

[Serializable]
public class DialogueInfo
{
    public int Id;
    public string content = "";
    public int type; //0 no option   1 normal
    public int canRandom; // 0 can be random   1  only for reply
    public int isChatBubble; // 0 no 1 yes
    public int love;  // to show must  miko.love > this.love
    //選択肢 Options for this dialogue
    public List<int> optionIds = new List<int>();
    public string voice = "";
    public string animation = "";
}

[Serializable]
public class DialogueInfoList
{
    public List<DialogueInfo> list;
}

[Serializable]
public class OptionInfoList
{
    public List<OptionInfo> list;
}

public class AVGDataManager : MonoBehaviour
{
    public static AVGDataManager instance = null;

    public Dictionary<int, DialogueInfo> dialogueInfos = new Dictionary<int, DialogueInfo>();
    public Dictionary<int, OptionInfo> optionInfos = new Dictionary<int, OptionInfo>();
    private DialogueInfo nullInfo = new DialogueInfo();
    private OptionInfo nullOpInfo = new OptionInfo();



    public string dialogueConfigJson = "DialogueConfig.txt";
    public string optionConfigJson = "OptionConfig.txt";

    private List<DialogueInfo> dialogueList = new List<DialogueInfo>();
    private List<OptionInfo> optionList = new List<OptionInfo>();
    [HideInInspector]
    public DialogueInfoList dialoginfolist;
    [HideInInspector]
    public OptionInfoList optioninfoList;

    [Serializable]
    public struct LoveLevel
    {
        public int lv;
        public int lovePoint;

        public int totalPoint;
    }

    public LoveLevel[] LoveLevelConfig;


    private LoveLevel aaaaaaaa;

    private void Awake()
    {
        instance = this;
        InitDialogueConfigs();
        InitOptionConfigs();
        nullOpInfo.addLove = 0;
        nullOpInfo.content = "Error";
        aaaaaaaa.lv = -35;
        aaaaaaaa.lovePoint = -35;
        aaaaaaaa.totalPoint = -35;
    }

    private void Start()
    {
        
    }

    public void InitDialogueConfigs()
    {
        
        string path = Application.dataPath;
        path += "/../Saves";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path += "/" + dialogueConfigJson;

        if (!File.Exists(path))
            File.Create(path).Dispose();
        string json = File.ReadAllText(path, Encoding.UTF8);

        //if(!json.Contains(":\\"))
        //    json.Replace(":\", ":\\");

        if (json == null || json.Length == 0) json = "";
        dialoginfolist = JsonUtility.FromJson<DialogueInfoList>(json);
        if (dialoginfolist == null)
        {
            dialoginfolist = new DialogueInfoList();
            dialoginfolist.list = new List<DialogueInfo>();

        }
        dialogueList = dialoginfolist.list;
        dialogueInfos = new Dictionary<int, DialogueInfo>();
        for (int i = 0; i < dialogueList.Count; ++i)
        {
            dialogueInfos[dialogueList[i].Id] = dialogueList[i];
        }
    }



    public void InitOptionConfigs()
    {
        string path = Application.dataPath;
        path += "/../Saves";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path += "/" + optionConfigJson;

        if (!File.Exists(path))
            File.Create(path).Dispose();
        string json = File.ReadAllText(path, Encoding.UTF8);

        //if(!json.Contains(":\\"))
        //    json.Replace(":\", ":\\");

        if (json == null || json.Length == 0) json = "";
        optioninfoList = JsonUtility.FromJson<OptionInfoList>(json);
        if (optioninfoList == null)
        {
            optioninfoList = new OptionInfoList();
            optioninfoList.list = new List<OptionInfo>();
           optionList = new List<OptionInfo>();
          //  optionInfos = new Dictionary<int, OptionInfo>();
            //PaddingConfigDefaultConfig() ;
        }

        optionList = optioninfoList.list;
        optionInfos = new Dictionary<int, OptionInfo>();
        for (int i =0; i<optionList.Count; ++i)
        {
            optionInfos[optionList[i].Id] = optionList[i];
        } 
        //         if (mikoConfig.channelConfigs == null || mikoConfig.channelConfigs.Count == 0)
        //             PaddingConfigDefaultChannelId();

    }

    public void SaveOption(OptionInfo config)
    {
        for (int i = 0; i < optionList.Count; ++i)
        {
            if (config.Id == optionList[i].Id)
            {
                optionList[i] = config;
                optionInfos[config.Id] = config;

                SaveOptionToJson();
                return;
            }
        }

        optionList.Add(config);
        optionInfos[config.Id] = config;
        SaveOptionToJson();
    }

    public void SaveDialogue(DialogueInfo config)
    {
        for (int i = 0; i < dialogueList.Count; ++i)
        {
            if (config.Id == dialogueList[i].Id)
            {
                dialogueList[i] = config;
                dialogueInfos[config.Id] = config;

                SaveDialogueToJson();
                return;
            }
        }

        dialogueList.Add(config);
        dialogueInfos[config.Id] = config;
        SaveDialogueToJson();
    }

    public void SaveOptionToJson()
    {
        string path = Application.dataPath;
        path += "/../Saves";
        path += "/" + optionConfigJson;
        optioninfoList.list = optionList;
        string json = JsonUtility.ToJson(optioninfoList);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    public void SaveOptionToDB()
    {
       // DBManager.instance.in
    }

    public void SaveDialogueToJson()
    {
        string path = Application.dataPath;
        path += "/../Saves";
        path += "/" + dialogueConfigJson;
        dialoginfolist.list = dialogueList;
        string json = JsonUtility.ToJson(dialoginfolist);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    public DialogueInfo GetDialogueInfoById(int id)
    {
        DialogueInfo info = null;
        if (!dialogueInfos.TryGetValue(id, out info)) info = nullInfo;

        return info;

    }

    public bool HasDialogueID(int id)
    {
        return dialogueInfos.ContainsKey(id);
    }

    public bool HasOptionID(int id)
    {
        return optionInfos.ContainsKey(id);
    }

    List<DialogueInfo> tmp = new List<DialogueInfo>();

    //0  normal  1 chatbubble
    public DialogueInfo GetRandomDialogue(int type = 0)
    {
        tmp.Clear();

        for (int i=0; i< dialogueList.Count; ++i)
        {
            if (dialogueList[i].canRandom == 0)
            {
                if (type == 1 && dialogueList[i].isChatBubble == 0)
                    continue;
                if (dialogueList[i].love > MikoChi.instance.GetLove())
                    continue;
                tmp.Add(dialogueList[i]);

            }
        }
        if (tmp.Count == 0)
            return nullInfo;

        int idx = UnityEngine.Random.Range(0, tmp.Count);

        return tmp[idx];
    }


    public OptionInfo GetOptionById(int id)
    {
        OptionInfo info = null;
        if (!optionInfos.TryGetValue(id, out info)) info = nullOpInfo;

        return info;
    }


    public void DeleteDialogue(DialogueInfo config)
    {
        for (int i = 0; i < dialogueList.Count; ++i)
        {
            if (config.Id == dialogueList[i].Id)
            {
               
                dialogueInfos.Remove(dialogueList[i].Id);
                dialogueList.RemoveAt(i);

                break;
            }
        }

        SaveDialogueToJson();
    }

    public void DeleteOption(OptionInfo config)
    {
        for (int i = 0; i < optionList.Count; ++i)
        {
            if (config.Id == optionList[i].Id)
            {

                optionInfos.Remove(optionList[i].Id);
                optionList.RemoveAt(i);

                break;
            }
        }

        SaveOptionToJson();
    }


    public bool DialogueIdIsExists(int id)
    {
        return dialogueInfos.ContainsKey(id);
    }

    public bool OPidIsExists(int id)
    {
        return optionInfos.ContainsKey(id);
    }


    public int GetCurrentLoveLevel(int love)
    {
        if (love < 0)
            return -1;

        if (love < LoveLevelConfig[0].totalPoint)
            return LoveLevelConfig[0].lv;
        else if (love < LoveLevelConfig[1].totalPoint)
            return LoveLevelConfig[1].lv;
        else if (love < LoveLevelConfig[2].totalPoint)
            return LoveLevelConfig[2].lv;
        else if (love < LoveLevelConfig[3].totalPoint)
            return LoveLevelConfig[3].lv;
        else if (love < LoveLevelConfig[4].totalPoint)
            return LoveLevelConfig[4].lv;
        else if (love < LoveLevelConfig[5].totalPoint)
            return LoveLevelConfig[5].lv;
        return 999;
    }

    public LoveLevel GetLoveConfigByLv(int lv)
    {
        if (lv < 0 || lv >= LoveLevelConfig.Length)
            return aaaaaaaa;

        return LoveLevelConfig[lv];
    }
}

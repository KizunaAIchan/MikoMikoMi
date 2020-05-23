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
}

[Serializable]
public class DialogueInfo
{
    public int Id;
    public string content;
    public int type; //0 no option   1 normal
    public int canRandom; // 0 can be random   1  only for reply
    //選択肢 Options for this dialogue
    public List<int> optionIds;
    public string voice;
    public string animation;
}

public class AVGDataManager : MonoBehaviour
{
    public static AVGDataManager instance = null;

    public Dictionary<int, DialogueInfo> dialogueInfos = new Dictionary<int, DialogueInfo>();
    public Dictionary<int, OptionInfo> optionInfos = new Dictionary<int, OptionInfo>();
    private DialogueInfo nullInfo = new DialogueInfo();



    public string dialogueConfigJson = "DialogueConfig.txt";
    public string optionConfigJson = "OptionConfig.txt";

    private List<DialogueInfo> dialogueList = new List<DialogueInfo>();
    private List<OptionInfo> optionList = new List<OptionInfo>();

    private void Awake()
    {
        instance = this;
    }

    public void InitDialogueConfigs()
    {
        string path = Application.dataPath;
        path += "/" + dialogueInfos;

        if (!File.Exists(path))
            File.Create(path).Dispose();
        string json = File.ReadAllText(path, Encoding.UTF8);

        //if(!json.Contains(":\\"))
        //    json.Replace(":\", ":\\");

        if (json == null || json.Length == 0) json = "";
        dialogueList = JsonUtility.FromJson<List<DialogueInfo>>(json);
        if (dialogueList == null)
        {
            //PaddingConfigDefaultConfig();
        }

//         if (mikoConfig.channelConfigs == null || mikoConfig.channelConfigs.Count == 0)
//             PaddingConfigDefaultChannelId();

    }



    public void InitOptionConfigs()
    {
        string path = Application.dataPath;
        path += "/" + optionConfigJson;

        if (!File.Exists(path))
            File.Create(path).Dispose();
        string json = File.ReadAllText(path, Encoding.UTF8);

        //if(!json.Contains(":\\"))
        //    json.Replace(":\", ":\\");

        if (json == null || json.Length == 0) json = "";
        optionList = JsonUtility.FromJson<List<OptionInfo>>(json);
        if (optionList == null)
        {
            //PaddingConfigDefaultConfig();
        }

        //         if (mikoConfig.channelConfigs == null || mikoConfig.channelConfigs.Count == 0)
        //             PaddingConfigDefaultChannelId();

    }

    public DialogueInfo GetDialogueInfoById(int id)
    {
        DialogueInfo info = null;
        if (!dialogueInfos.TryGetValue(id, out info)) info = nullInfo;

        return info;

    }

}

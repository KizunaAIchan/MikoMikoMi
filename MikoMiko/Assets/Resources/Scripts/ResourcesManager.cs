using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class ChannelConfig
{
    public string channelId = "";
    public string name = "";
    public string startNotification = "";
    public string closureNotice = "";
    public string startAnima = "";
    public string endAnima = "";
}

[Serializable]
public class MikoWindowConfig
{
    public List<ChannelConfig> channelConfigs;
    public int autoStart = 0;// 0- open 1-close
    public string startVoice = "";
    public string startAnima = "";
}


public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance = null;


    public string configJson = "Config.txt";
    public string audioClipsPath = "Audios/";

    public Dictionary<string, AudioClip> audioCllips = new Dictionary<string, AudioClip>();
    private MikoWindowConfig mikoConfig;
    private void Awake()
    {
        instance = this;
        InitAudioClips();
        InitChannelConfigs();
    }
    // Start is called before the first frame update
    void Start()
    {
       // instance = this;
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitChannelConfigs()
    {
        string path = Application.dataPath;
        path += "/" + configJson;

        if (!File.Exists(path))
            File.Create(path).Dispose();
        string json = File.ReadAllText(path,Encoding.UTF8);

        if (json == null || json.Length == 0) json = "";
        mikoConfig = JsonUtility.FromJson<MikoWindowConfig>(json);
        if (mikoConfig == null)
        {
            PaddingConfigDefaultConfig();
        }

        if (mikoConfig.channelConfigs == null || mikoConfig.channelConfigs.Count == 0)
            PaddingConfigDefaultChannelId();
        SaveToJsonConfig();

    }

    public void PaddingConfigDefaultChannelId()
    {
        mikoConfig.channelConfigs = new List<ChannelConfig>();
        mikoConfig.channelConfigs.Add(new ChannelConfig());
        // mikoConfig.channelConfigs[0] = new ChannelConfig();
        mikoConfig.channelConfigs[0].channelId = "UC-hM6YJuNYVAmUWxeIr9FeA";
        mikoConfig.channelConfigs[0].name = "さくらみこ";
        mikoConfig.channelConfigs[0].startAnima = "さくらみこ";
        mikoConfig.channelConfigs[0].endAnima = "さくらみこ";
        mikoConfig.channelConfigs[0].startNotification = "nya";
        mikoConfig.channelConfigs[0].closureNotice = "FAQ";
    }
    public void PaddingConfigDefaultConfig()
    {
        mikoConfig = new MikoWindowConfig();
        mikoConfig.autoStart = 1;
        mikoConfig.startAnima = "";
        mikoConfig.startVoice = "";



      

    }

    public void SaveToJsonConfig()
    {
        string path = Application.dataPath;
        path += "/" + configJson;
        string json = JsonUtility.ToJson(mikoConfig);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    public void InitAudioClips()
    {
        var audios = Resources.LoadAll<AudioClip>(audioClipsPath);
        for (int i=0; i< audios.Length; ++i)
        {
            var audio = Instantiate<AudioClip>(audios[i]);
            audioCllips.Add(audios[i].name, audio);
      //      Debug.Log(audios[i].name);

        }
    }

    public AudioClip GetAudioClipByName(string name)
    {
        AudioClip audio = null;
        audioCllips.TryGetValue(name, out audio);
        return audio;
    }

    public List<ChannelConfig> GetChannelConfigs()
    {
        return mikoConfig.channelConfigs;
    }
}

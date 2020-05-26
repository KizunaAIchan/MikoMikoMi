using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using NAudio;
using NAudio.Wave;

[Serializable]
public class ChannelConfig
{
    public string channelId = "";
    public string name = "";
    public string startNotification = "";
    public string closureNotice = "";
    public string startAnima = "";
    public string endAnima = "";
    public int channelType = 0; // 0 youtube 1 bilibil
    public int monitor = 0; // 0 open 1 close
}

[Serializable]
public class MikoWindowConfig
{
    public List<ChannelConfig> channelConfigs;
    public int autoStart = 0;// 0- open 1-close
    public string startVoice = "";
    public string startAnima = "";
    public int language = 0;
    public int love　= 0; //好感度
}


public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance = null;


    public string configJson = "Config.txt";
    public string audioClipsPath = "Audios/";
    public string uiPath = "UI/Prefabs/";
    
    public Dictionary<string, AudioClip> audioCllips = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> audioCllipsByPath = new Dictionary<string, AudioClip>();
    public Dictionary<string, UIBase> uiPrefabs = new Dictionary<string, UIBase>();
    public Dictionary<string, UIComponentBase> uiComponentPrefabs = new Dictionary<string, UIComponentBase>();


    private Dictionary<string, ChannelConfig> channelConfigs = new Dictionary<string, ChannelConfig>();



    private MikoWindowConfig mikoConfig;
    private void Awake()
    {
        instance = this;
        InitAudioClips();
        InitUIPrefabs();
        InitChannelConfigs();
        InitChannelConfigDic();
        LoadCustomAudio();
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

        //if(!json.Contains(":\\"))
        //    json.Replace(":\", ":\\");

        if (json == null || json.Length == 0) json = "";
        mikoConfig = JsonUtility.FromJson<MikoWindowConfig>(json);
        if (mikoConfig == null)
        {
            PaddingConfigDefaultConfig();
        }

        if (mikoConfig.channelConfigs == null || mikoConfig.channelConfigs.Count == 0)
            PaddingConfigDefaultChannelId();

    }

    public void InitUIPrefabs()
    {
        var uis = Resources.LoadAll<UIBase>(uiPath);
        for (int i = 0; i < uis.Length; ++i)
        {
            uiPrefabs[uis[i].name] = uis[i];
        }

        var components = Resources.LoadAll<UIComponentBase>(uiPath);
        for (int i = 0; i < components.Length; ++i)
        {
            uiComponentPrefabs[components[i].name] = components[i];
        }
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
        //   mikoConfig.channelConfigs[0].closureNotice = "FAQ";
        mikoConfig.channelConfigs[0].closureNotice = "E:\\Unity";
        mikoConfig.channelConfigs[0].monitor = 0;
        mikoConfig.channelConfigs[0].channelType = 0;

    }

    public void InitChannelConfigDic()
    {
        for (int i = 0; i < mikoConfig.channelConfigs.Count; ++i)
        {
            channelConfigs[mikoConfig.channelConfigs[i].channelId] = mikoConfig.channelConfigs[i];

        }
    }

    public void DeleteChannelConfig(ChannelConfig config)
    {
        for (int i = 0; i < mikoConfig.channelConfigs.Count; ++i)
        {
            if (config.channelId == mikoConfig.channelConfigs[i].channelId)
            {;
                HttpRequest.instance.StopListener(mikoConfig.channelConfigs[i].channelId);
                channelConfigs.Remove(mikoConfig.channelConfigs[i].channelId);
                mikoConfig.channelConfigs.RemoveAt(i);

                break;
            }
        }

        SaveToJsonConfig();
    }

    public void SaveChannelConfig(ChannelConfig config)
    {
        for (int i = 0; i< mikoConfig.channelConfigs.Count; ++i){
            if (config.channelId == mikoConfig.channelConfigs[i].channelId)
            {
                mikoConfig.channelConfigs[i] = config;
                channelConfigs[mikoConfig.channelConfigs[i].channelId] = mikoConfig.channelConfigs[i];

                SaveToJsonConfig();
                return;
            }
        }

        mikoConfig.channelConfigs.Add(config);
        channelConfigs[config.channelId] = config;
        HttpRequest.instance.AddListener(config);


        SaveToJsonConfig();
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

    public void SetLanguageType(int id)
    {
        mikoConfig.language = id;
    }

    public void InitAudioClips()
    {
        var audios = Resources.LoadAll<AudioClip>(audioClipsPath);
        for (int i=0; i< audios.Length; ++i)
        {
            var audio = Instantiate<AudioClip>(audios[i]);
            audioCllips.Add(audios[i].name, audio);

        }
    }

    public AudioClip GetAudioClipByName(string name)
    {
        AudioClip audio = null;
        if(!audioCllips.TryGetValue(name, out audio))
        {
            return LoadAudio(name);
        }
        return audio;
    }

    public List<ChannelConfig> GetChannelConfigs()
    {
        return mikoConfig.channelConfigs;
    }

    public void LoadCustomAudio()
    {
        for (int i = 0; i < mikoConfig.channelConfigs.Count; ++i)
        {
            if (!audioCllips.ContainsKey( mikoConfig.channelConfigs[i].startNotification))
            {
                LoadAudio(mikoConfig.channelConfigs[i].startNotification);
            }

            if (!audioCllips.ContainsKey(mikoConfig.channelConfigs[i].closureNotice))
            {
                LoadAudio(mikoConfig.channelConfigs[i].closureNotice);
            }
        }
    }

    public AudioClip LoadAudio(string path)
    {
        Debug.Log(path);
        Debug.Log(Application.dataPath);
        var name = Path.GetFileNameWithoutExtension(path);
        try
        {
            var aud = new AudioFileReader(path);
            var AudioData = new float[aud.Length];
            aud.Read(AudioData, 0, (int)aud.Length);
            var clip = AudioClip.Create(name, (int)aud.Length, aud.WaveFormat.Channels, aud.WaveFormat.SampleRate, false);
            clip.SetData(AudioData, 0);

            if (clip.isReadyToPlay)
            {
                aud.Dispose();
                audioCllips.Add(path, clip);
                // audioCllips.Add(name, clip);
                return clip;
            }
        }
        catch
        {
            return null;
        }
       
        return null;
    }



    public UIBase CreateUIPrefab(string name)
    {
        UIBase ui = null;

        uiPrefabs.TryGetValue(name, out ui);
        var t = Instantiate<UIBase>(ui);
        return t;
    }

    public UIComponentBase CreateUIComponent(string name)
    {
        UIComponentBase ui = null;

        uiComponentPrefabs.TryGetValue(name, out ui);
        var t = Instantiate<UIComponentBase>(ui);
        return t;
    }


    public bool CanMonitorChannel(string id)
    {
        ChannelConfig config = null;
        if (channelConfigs.TryGetValue(id, out config))
        {
            return config.monitor == 0;
        }
        return false;
    }

    public int GetLanguageType()
    {
        return mikoConfig.language;
    }

   
}

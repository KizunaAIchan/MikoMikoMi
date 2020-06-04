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
public class TimeInfo
{
    public int hour = 0;
    public int minute = 0;
}

[Serializable]
public class LoveSystem
{
    public int lastUpdateDay;
    public int LastUpdateYear;
    public int touchCount;
    public int dialogueCount;
    public int idleCount;

}

[Serializable]
public class MikoWindowConfig
{
  //  public List<ChannelConfig> channelConfigs;
    public int autoStart = 0;// 0- open 1-close
    public string startVoice = "";
    public string startAnima = "";
    public int language = 0;
    public int love　= 0; //好感度
    public int onTop = 0;  //0  top 1 normal
    public int mute = 0; // 0 off 1 on
    public int chattbubble = 0; //0 on 1 off
    public int sleepMode = 0; //0 off 1 on

    public LoveSystem loveConfig;

    public TimeInfo muteStartTime;
    public TimeInfo muteEndTime;
}

[Serializable]
public class ChannelData
{
    public List<ChannelConfig> channelConfigs;
}

public enum ConfigType
{
    AddNew,
    Modify,
}

public enum AddLoveType{
    Touch,
    Dialogue,
    Idle,
    }

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance = null;


    public string configJson = "Config.txt";
    public string ChannelsJson = "ChannelConfig.txt";
    public string audioClipsPath = "Audios/";
    public string uiPath = "UI/Prefabs/";

    public string audioClipsLevelPath = "Audios/LV";
    

    public class AudioLoveConfig
    {
        public AudioClip clip;
        public int requireLv = 0;
    }


    public Dictionary<string, AudioLoveConfig> audioCllips = new Dictionary<string, AudioLoveConfig>();
    public Dictionary<string, AudioClip> audioCllipsByPath = new Dictionary<string, AudioClip>();
    public Dictionary<string, UIBase> uiPrefabs = new Dictionary<string, UIBase>();
    public Dictionary<string, UIComponentBase> uiComponentPrefabs = new Dictionary<string, UIComponentBase>();


    private Dictionary<string, ChannelConfig> channelConfigs = new Dictionary<string, ChannelConfig>();


    private ChannelData ChannelData;
    private MikoWindowConfig mikoConfig;
    private int LoveLevel = 0;
    private void Awake()
    {
        instance = this;
        InitAudioClips();
        InitUIPrefabs();
        InitChannelConfigs();
        InitChannelConfigDic();
        LoadCustomAudio();
        WindowSetting.instance.SetWindowTop(mikoConfig.onTop == 0);
        GameEngine.instance.showChatBubble = mikoConfig.chattbubble == 0;
        GameEngine.instance.audioVolume = mikoConfig.mute == 0 ? 1 : 0;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        LoveLevel = AVGDataManager.instance.GetCurrentLoveLevel(mikoConfig.love);


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitChannelConfigs()
    {
        {
            string path = Application.dataPath;
            path += "/../Saves";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            path += "/" + configJson;

            if (!File.Exists(path))
                File.Create(path).Dispose();
            string json = File.ReadAllText(path, Encoding.UTF8);


            if (json == null || json.Length == 0) json = "";
            mikoConfig = JsonUtility.FromJson<MikoWindowConfig>(json);
            if (mikoConfig == null)
            {
                PaddingConfigDefaultConfig();
            }
            if (DifferentDay(mikoConfig.loveConfig.LastUpdateYear, mikoConfig.loveConfig.lastUpdateDay, DateTime.Now))
                ResetLoveCount();

            if (mikoConfig.muteStartTime == null)
                mikoConfig.muteStartTime = new TimeInfo();
            if (mikoConfig.muteEndTime == null)
                mikoConfig.muteEndTime = new TimeInfo();
        }

        {
            string path = Application.dataPath;
            path += "/../Saves";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += "/" + ChannelsJson;

            if (!File.Exists(path))
                File.Create(path).Dispose();
            string json = File.ReadAllText(path, Encoding.UTF8);
            if (json == null || json.Length == 0) json = "";
            ChannelData = JsonUtility.FromJson<ChannelData>(json);
            if (ChannelData == null)
                ChannelData = new ChannelData();

            if (ChannelData.channelConfigs == null || ChannelData.channelConfigs.Count == 0)
                PaddingConfigDefaultChannelId();
        }
       

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
        ChannelData.channelConfigs = new List<ChannelConfig>();
        ChannelData.channelConfigs.Add(new ChannelConfig());
        // ChannelData.channelConfigs[0] = new ChannelConfig();
        ChannelData.channelConfigs[0].channelId = "UC-hM6YJuNYVAmUWxeIr9FeA";
        ChannelData.channelConfigs[0].name = "さくらみこ";
        ChannelData.channelConfigs[0].startAnima = "さくらみこ";
        ChannelData.channelConfigs[0].endAnima = "さくらみこ";
        ChannelData.channelConfigs[0].startNotification = "nya";
        //   ChannelData.channelConfigs[0].closureNotice = "FAQ";
        ChannelData.channelConfigs[0].closureNotice = "E:\\Unity";
        ChannelData.channelConfigs[0].monitor = 0;
        ChannelData.channelConfigs[0].channelType = 0;

    }

    public void InitChannelConfigDic()
    {
        for (int i = 0; i < ChannelData.channelConfigs.Count; ++i)
        {
            channelConfigs[ChannelData.channelConfigs[i].channelId] = ChannelData.channelConfigs[i];

        }
    }

    public void DeleteChannelConfig(ChannelConfig config)
    {
        for (int i = 0; i < ChannelData.channelConfigs.Count; ++i)
        {
            if (config.channelId == ChannelData.channelConfigs[i].channelId)
            {;
                HttpRequest.instance.StopListener(ChannelData.channelConfigs[i].channelId);
                channelConfigs.Remove(ChannelData.channelConfigs[i].channelId);
                ChannelData.channelConfigs.RemoveAt(i);

                break;
            }
        }

        SaveChannelToJson();
      //  SaveToJsonConfig();
    }

    public void SaveChannelConfig(ChannelConfig config)
    {
        for (int i = 0; i< ChannelData.channelConfigs.Count; ++i){
            if (config.channelId == ChannelData.channelConfigs[i].channelId)
            {
                ChannelData.channelConfigs[i] = config;
                channelConfigs[ChannelData.channelConfigs[i].channelId] = ChannelData.channelConfigs[i];
                SaveChannelToJson();

             //   SaveToJsonConfig();
                return;
            }
        }

        ChannelData.channelConfigs.Add(config);
        channelConfigs[config.channelId] = config;
        HttpRequest.instance.AddListener(config);

        SaveChannelToJson();

      //  SaveToJsonConfig();
    }

    public void PaddingConfigDefaultConfig()
    {
        mikoConfig = new MikoWindowConfig();
        mikoConfig.loveConfig = new LoveSystem();
        mikoConfig.loveConfig.lastUpdateDay = DateTime.Now.DayOfYear;
        mikoConfig.loveConfig.LastUpdateYear = DateTime.Now.Year;
        mikoConfig.autoStart = 1;
        mikoConfig.startAnima = "";
        mikoConfig.startVoice = "";
    }

    public void SaveConfigToJsonConfig()
    {
        string path = Application.dataPath;
        path += "/../Saves";
        path += "/" + configJson;
        string json = JsonUtility.ToJson(mikoConfig);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    public void SaveChannelToJson()
    {
        string path = Application.dataPath;
        path += "/../Saves";
        path += "/" + ChannelsJson;
        string json = JsonUtility.ToJson(ChannelData);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    public void SetLanguageType(int id)
    {
        mikoConfig.language = id;
    }

    public void InitAudioClips()
    {
        //var audios = Resources.LoadAll<AudioClip>(audioClipsPath);
        //for (int i=0; i< audios.Length; ++i)
        //{
        //    var audio = Instantiate<AudioClip>(audios[i]);
        //    audioCllips.Add(audios[i].name, audio);

        //}
        for(int i =0; i<AVGDataManager.instance.LoveLevelConfig.Length; ++i)
        {
            InitAudioByLoveLevel(AVGDataManager.instance.LoveLevelConfig[i].lv);
        }
    }

    public void InitAudioByLoveLevel(int lv)
    {
        string path = audioClipsLevelPath + lv.ToString() + "/";
        var audios = Resources.LoadAll<AudioClip>(path);
        for (int i = 0; i < audios.Length; ++i)
        {
            var audio = Instantiate<AudioClip>(audios[i]);
            AudioLoveConfig config = new AudioLoveConfig();
            config.requireLv = lv;
            config.clip = audio;
            if (audioCllips.ContainsKey(audios[i].name))
                Debug.Log(audios[i].name + " " + path + "  " + i + "   " + audios.Length);
            audioCllips.Add(audios[i].name, config);

        }
    }

    public AudioClip GetAudioClipByName(string name)
    {
        AudioLoveConfig audio = null;
        if(!audioCllips.TryGetValue(name, out audio))
        {
            return LoadAudio(name);
        }
        return audio.clip;
    }

    public List<ChannelConfig> GetChannelConfigs()
    {
        return ChannelData.channelConfigs;
    }

    public void LoadCustomAudio()
    {
        for (int i = 0; i < ChannelData.channelConfigs.Count; ++i)
        {
            if (!audioCllips.ContainsKey( ChannelData.channelConfigs[i].startNotification))
            {
                LoadAudio(ChannelData.channelConfigs[i].startNotification);
            }

            if (!audioCllips.ContainsKey(ChannelData.channelConfigs[i].closureNotice))
            {
                LoadAudio(ChannelData.channelConfigs[i].closureNotice);
            }
        }
    }

    public AudioClip LoadAudio(string path)
    {
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
                AudioLoveConfig config = new AudioLoveConfig();
                config.clip = clip;
                config.requireLv = 1;

                audioCllips.Add(path, config);
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

    public ChannelConfig GetChannelConfigById(string id)
    {
        ChannelConfig config = null;
        channelConfigs.TryGetValue(id, out config);
        return config ;

    }

    public void AddLove(int n, AddLoveType type)
    {
        mikoConfig.love += n;
        int Lv = AVGDataManager.instance.GetCurrentLoveLevel(mikoConfig.love);
        SetLoveCount(type);
        if (Lv != LoveLevel)
        {
            string str = "好感度Level" + Lv + "だにぇ";
            if (Lv >= 4)
                str += "じゅ~~";
            EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Chat, 1, str);
            LoveLevel = Lv;
        }
        SaveConfigToJsonConfig();
    }

    public int GetLove()
    {
        return mikoConfig.love;
    }

    public bool ChannelIsExists(string id)
    {
        return channelConfigs.ContainsKey(id);
    }

    public void OnDestroy()
    {
        SaveConfigToJsonConfig();
        SaveChannelToJson();
    }


    public void SetMute(bool mute)
    {
        mikoConfig.mute = mute ? 1 : 0;
        GameEngine.instance.audioVolume = mute ? 0 : 1;
    }

    public void SetTop(bool top)
    {
        mikoConfig.onTop = top ?0 : 1;
        WindowSetting.instance.SetWindowTop(mikoConfig.onTop == 0);

    }

    public void SetChatBubble(bool on)
    {
        mikoConfig.chattbubble = on ? 0 : 1;
        GameEngine.instance.showChatBubble = on;
    }



    public bool GetTopOn()
    {
        return mikoConfig.onTop == 0;
    }

    public bool GetChatBubble()
    {
        return mikoConfig.chattbubble == 0;
    }

    public bool GetMute()
    {
        return mikoConfig.mute == 1;
    }

    public bool GeSleepMode()
    {
        return mikoConfig.sleepMode == 1;
    }

    public TimeInfo GetMuteTime(int idx)
    {
        return idx == 0 ? mikoConfig.muteStartTime : mikoConfig.muteEndTime;
    }


    public int GetLoveTypeCount(AddLoveType type)
    {
        if (type == AddLoveType.Dialogue)
        {
            return mikoConfig.loveConfig.dialogueCount;
        }

        if (type == AddLoveType.Idle)
        {
            return mikoConfig.loveConfig.idleCount;
        }

        return mikoConfig.loveConfig.touchCount;

    }

    public void SetLoveCount(AddLoveType type)
    {
        if (type == AddLoveType.Dialogue)
        {
            mikoConfig.loveConfig.dialogueCount--;
        }

        if (type == AddLoveType.Idle)
        {
            mikoConfig.loveConfig.idleCount--;
        }

        if (type == AddLoveType.Touch)
        {
            mikoConfig.loveConfig.touchCount--;
        }
        mikoConfig.loveConfig.lastUpdateDay = DateTime.Now.DayOfYear;
        mikoConfig.loveConfig.LastUpdateYear = DateTime.Now.Year;

        //   SaveConfigToJsonConfig();
    }

    public void ResetLoveCount()
    {
        mikoConfig.loveConfig.dialogueCount = 12;
        mikoConfig.loveConfig.idleCount = 8;
        mikoConfig.loveConfig.touchCount = 4;
        mikoConfig.loveConfig.lastUpdateDay = DateTime.Now.DayOfYear;
        mikoConfig.loveConfig.LastUpdateYear = DateTime.Now.Year;
        Debug.Log("rest");
        SaveConfigToJsonConfig();


    }

    public bool DifferentDay(int year, int day, DateTime d2)
    {
        if (d2.Year != year)
            return true;
        if (d2.DayOfYear != day)
            return true;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance = null;



    public string audioClipsPath = "Audios/";

    public Dictionary<string, AudioClip> audioCllips = new Dictionary<string, AudioClip>();
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InitAudioClips();
    }

    // Update is called once per frame
    void Update()
    {

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
}

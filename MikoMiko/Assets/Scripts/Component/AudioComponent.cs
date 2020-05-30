using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioComponent : ComponentBase
{
    public AudioSource audioSource;
    public MikoChi miko;

    private Queue<AudioClip> _waitForPlay = new Queue<AudioClip>();
    private AudioClip _currentAudioClip;
    private float _audioLeftTime = 0f;
    public override void Destory()
    {
       // throw new NotImplementedException();
    }

    public override void Init(MikoChi mikochi)
    {
        miko = mikochi;
        audioSource = miko.GetComponent<AudioSource>();
        
        //throw new NotImplementedException();
    }



    public override void Update(float deltatime)
    {
      //  if (!audioSource.isPlaying) return;
        _audioLeftTime -= deltatime;
        if (_audioLeftTime <= 0)
        {
            if (_waitForPlay.Count > 0)
            {
                var audioClip = _waitForPlay.Dequeue();
                Play(audioClip, true);
            }
        }
    }




    public void Play(AudioClip audio, bool force = false)
    {
        if (force) audioSource.Stop();
        else
        {
            _waitForPlay.Enqueue(audio);
            return;
        }
        audioSource.clip = audio;
        _audioLeftTime = audio.length;
        audioSource.Play();
    }

    public void Play(string str, bool force = false)
    {
        var audio = ResourcesManager.instance.GetAudioClipByName(str);
        if (audio ==null)
        {
            GameEngine.instance.Error("Can't find audio：" + str);
            return;
        }

        if (force) audioSource.Stop();
        else
        {
            _waitForPlay.Enqueue(audio);
            return;
        }
        audioSource.clip = audio;
        _audioLeftTime = audio.length;
        audioSource.volume = GameEngine.instance.audioVolume;
        audioSource.Play();
    }

}

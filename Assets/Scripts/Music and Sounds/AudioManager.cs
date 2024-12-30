using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    //Use this for initialization
    void Awake () {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.loop = s.Loop;
        }
    }

    private void Start() {
        Play("OpeningScene");
    }

    //play sound
    public void Play (string name)
    {
        Sound s = Find(name);
        s.source.Play();        
    }
    public Sound Find (string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.Name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        return s;    
    }
    public void PlaySoundCoins ()
    {
        Sound s = Find("TakeCoin");
        for (int i = Array.IndexOf(sounds, s); i < sounds.Length; i++)
        {
            if(!sounds[i].source.isPlaying){
                sounds[i].source.Play();
                return;
            }
        }
    }
}

using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.loop = s.Loop;
        }
    }

    private void Start()
    {
        Play("OpeningScene");
    }

    public void Play(string name)
    {
        Sound s = Find(name);
        if (s == null) return;
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Find(name);
        if (s == null) return;
        s.source.Stop();
    }

    public Sound Find(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s == null)
            Debug.LogWarning($"AudioManager: sonido '{name}' no encontrado.");
        return s;
    }

    // Reproduce el sonido de moneda usando la primera fuente disponible
    // para permitir superposición de sonidos (polyphony)
    public void PlaySoundCoins()
    {
        Sound s = Find("TakeCoin");
        if (s == null) return;

        for (int i = Array.IndexOf(sounds, s); i < sounds.Length; i++)
        {
            if (!sounds[i].source.isPlaying)
            {
                sounds[i].source.Play();
                return;
            }
        }
    }
}
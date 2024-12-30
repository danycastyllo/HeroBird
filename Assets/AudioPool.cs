using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    public static AudioPool Instance;

    public AudioSource audioSourcePrefab;
    public int poolSize = 10;

    private Queue<AudioSource> audioSources = new Queue<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < poolSize; i++)
        {
            var source = Instantiate(audioSourcePrefab, transform);
            source.gameObject.SetActive(false);
            audioSources.Enqueue(source);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSources.Count > 0)
        {
            var source = audioSources.Dequeue();
            source.gameObject.SetActive(true);
            source.clip = clip;
            source.Play();

            StartCoroutine(ReleaseSource(source, clip.length));
        }
    }

    private IEnumerator ReleaseSource(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.gameObject.SetActive(false);
        audioSources.Enqueue(source);
    }
}


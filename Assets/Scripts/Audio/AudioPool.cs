// AudioPool.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    public static AudioPool Instance;

    public AudioSource audioSourcePrefab;
    public int poolSize = 10;

    Queue<AudioSource> pool = new Queue<AudioSource>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = Instantiate(audioSourcePrefab, transform);
            source.gameObject.SetActive(false);
            pool.Enqueue(source);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (pool.Count == 0) return;

        AudioSource source = pool.Dequeue();
        source.gameObject.SetActive(true);
        source.clip = clip;
        source.Play();
        StartCoroutine(ReleaseSource(source, clip.length));
    }

    IEnumerator ReleaseSource(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.gameObject.SetActive(false);
        pool.Enqueue(source);
    }
}
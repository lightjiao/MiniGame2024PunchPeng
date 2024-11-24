using UnityEngine;
using UnityEngine.Assertions;

public class AudioSourceHelper : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
    }

    public void Play(AudioClip clip, bool loop, float volume = 1)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
        audioSource.volume = volume;
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.Play();
    }

    private void OnDisable()
    {
        audioSource.clip = null;
        audioSource.loop = false;
        audioSource.volume = 1;
    }
}

using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.AudioSource = gameObject.AddComponent<AudioSource>();
            sound.AudioSource.clip = sound.clip;

            sound.AudioSource.volume = sound.volume;
            sound.AudioSource.pitch = sound.pitch;
            sound.AudioSource.loop = sound.loop;
        }
    }

    private delegate void SoundDelegate(Sound sound);

    public void Play(string name) => Execute(name, (sound) => sound.AudioSource.Play());

    public void Play(string name, float fadeInTime) => Execute(name, (sound) => StartCoroutine(FadeIn(sound, fadeInTime)));

    public void Pause(string name) => Execute(name, (sound) => sound.AudioSource.Pause());

    public void Stop(string name) => Execute(name, (sound) => sound.AudioSource.Stop());


    private void Execute(string name, SoundDelegate soundDelegate)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        soundDelegate.Invoke(sound);
    }

    private IEnumerator FadeIn(Sound sound, float duration)
    {
        sound.AudioSource.Play();

        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            sound.AudioSource.volume = Mathf.Lerp(0, sound.volume, currentTime / duration);

            yield return null;
        }

        sound.AudioSource.volume = sound.volume;
    }

}

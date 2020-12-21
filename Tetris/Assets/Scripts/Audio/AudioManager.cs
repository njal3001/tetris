using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

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

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        sound.AudioSource.Play();
    }

}

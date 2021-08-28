using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundObject : MonoBehaviour, IPooledObject
{
    AudioSource source;
    AudioManager audioManager;
    public void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Start()
    {
        audioManager = AudioManager.Instance;
    }

    public void OnObjectSpawn(ParticleInfo info)
    {
        Sound s = Array.Find(AudioManager.Instance.sounds, sound => sound.name == info.sound_name);
        s.loop = info.loop;
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " was not found!");
            return;
        }

        float pitch = UnityEngine.Random.Range(0.8f, 1.0f);
        
        source.clip = s.clip;
        source.volume = s.volume;
        source.pitch = pitch;
        source.loop = s.loop;
        source.Play();
    }
}

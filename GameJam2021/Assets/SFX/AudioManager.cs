using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;
    ObjectPooler objectPooler;
    private void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    public Sound Play2DSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        { 
            Debug.LogWarning("Sound: " + name + " was not found!"); 
            return null; 
        }
        s.source.Play();
        return s;
    }

    public void Play3DSound(string name, Vector2 pos)
    {
        ParticleInfo info = new ParticleInfo
        {
            loop = false,
            sound_name = name,
            tag = "Sound_Object",
            pos = pos,
            rot = Quaternion.identity
        };

        objectPooler.SpawnFromPool<SoundObject>(info);
    }
}

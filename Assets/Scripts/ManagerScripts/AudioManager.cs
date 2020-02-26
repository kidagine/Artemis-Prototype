using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] _sounds = default;
    [SerializeField] private SoundGroup[] _soundGroups = default;
    private Sound _previousRandomSound;
    public static AudioManager Instance { get; private set; }


    void Awake()
    {
        CheckInstance();
        SetSounds();
        SetSoundGroups();
    }

    private void CheckInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void SetSounds()
    {
        foreach (Sound sound in _sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
            if (sound.source.playOnAwake)
            {
                sound.source.Play();
            }
        }
    }

    private void SetSoundGroups()
    {
        foreach (SoundGroup soundGroup in _soundGroups)
        {
            foreach (Sound sound in soundGroup.sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;

                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.playOnAwake = sound.playOnAwake;
                if (sound.source.playOnAwake)
                {
                    sound.source.Play();
                }
            }
        }
    }

    public void Play(string name, float pitch = default)
    {
        Sound sound = Array.Find(_sounds, s => s.name == name);
        if (pitch != default)
        {
            sound.source.pitch = pitch;
        }
        sound.source.Play();
    }

    public bool IsPlaying(string name)
    {
        Sound sound = Array.Find(_sounds, s => s.name == name);
        if (sound.source.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(_sounds, s => s.name == name);
        sound.source.Stop();
    }

    public void PlayRandomFromSoundGroup(string name)
    {
        SoundGroup soundGroup = Array.Find(_soundGroups, sg => sg.name == name);
        Sound randomSound = soundGroup.sounds[UnityEngine.Random.Range(0, soundGroup.sounds.Length)];
        if (randomSound != _previousRandomSound)
        {
            _previousRandomSound = randomSound;
            randomSound.source.Play();
        }
        else
        {
            PlayRandomFromSoundGroup(name);
        }
    }

    public void FadeIn(string name)
    {
        Sound sound = Array.Find(_sounds, s => s.name == name);
        StartCoroutine(FadeInCoroutine(sound));
    }

    IEnumerator FadeInCoroutine(Sound sound)
    {
        sound.volume = 0;
        sound.source.Play();
        bool isVolumeMaxed = false;
        while (!isVolumeMaxed)
        {
            sound.source.volume += 0.1f;
            if (sound.source.volume >= 1.0f)
            {
                isVolumeMaxed = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void FadeOut(string name)
    {
        Sound sound = Array.Find(_sounds, s => s.name == name);
        StartCoroutine(FadeOutCoroutine(sound));
    }

    IEnumerator FadeOutCoroutine(Sound sound)
    {
        sound.volume = 1;
        sound.loop = false;

        bool isVolumeLowered = false;
        while (!isVolumeLowered)
        {
            sound.source.volume -= 0.05f;
            if (sound.source.volume <= 0.0f)
            {
                isVolumeLowered = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
        sound.source.Stop();
    }
}

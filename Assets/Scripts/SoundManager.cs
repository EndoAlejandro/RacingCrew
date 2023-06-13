using System.Collections.Generic;
using CustomUtils;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicAudioSource;

    [SerializeField] private AudioSource fxAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip uiAudioClip;

    private Dictionary<Sfx, AudioClip> _audioClips;

    protected override void Awake()
    {
        base.Awake();

        _audioClips = new Dictionary<Sfx, AudioClip>
        {
            { Sfx.UI, uiAudioClip }, // This is where we add all the Sfx enum cases.
        };
    }

    // This is the method that we can call from any class.
    public void PlayFx(Sfx fxType) => fxAudioSource.PlayOneShot(_audioClips[fxType]);
}

// Add as many as you need to ID different audio cases.
public enum Sfx
{
    UI,
}
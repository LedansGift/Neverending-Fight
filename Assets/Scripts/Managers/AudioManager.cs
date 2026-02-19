using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private float logMinThreshold = 0.0001f;
    private float logMult = 20f;

    private const float MIN_PITCH_VARIATION = 0.9f;
    private const float MAX_PITCH_VARIATION = 1.1f;

    private static int sfxPoolCounter = 0;

    [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField]
    private AudioMixer volumeMixer;

    [SerializeField]
    private AudioSource[] localSfxPool;
    private static AudioSource[] sfxPool;

    private void OnEnable()
    {
        OptionsUI.OnMasterVolumeUpdated += UpdateMasterVolume;
        OptionsUI.OnMusicVolumeUpdated += UpdateMusicVolume;
        OptionsUI.OnSFXVolumeUpdated += UpdateSFXVolume;
        OptionsUI.OnVoiceVolumeUpdated += UpdateVoiceVolume;

        sfxPool = localSfxPool;
        sfxPoolCounter = 0;
    }

    private void OnDisable()
    {
        OptionsUI.OnMasterVolumeUpdated -= UpdateMasterVolume;
        OptionsUI.OnMusicVolumeUpdated -= UpdateMusicVolume;
        OptionsUI.OnSFXVolumeUpdated -= UpdateSFXVolume;
        OptionsUI.OnVoiceVolumeUpdated -= UpdateVoiceVolume;
    }

    private void Start()
    {
        UpdateMasterVolume(this, PlayerOptions.GetMasterVolume());
        UpdateMusicVolume(this, PlayerOptions.GetMusicVolume());
        UpdateSFXVolume(this, PlayerOptions.GetSFXVolume());
        UpdateVoiceVolume(this, PlayerOptions.GetVoiceVolume());
    }

    private static AudioSource PlaySFXClip(
        AudioClip clip,
        Vector3 position,
        float volume,
        float pitch
    )
    {
        AudioSource source = sfxPool[sfxPoolCounter];
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;

        source.Play();

        sfxPoolCounter++;

        if (sfxPoolCounter >= sfxPool.Length)
        {
            sfxPoolCounter = 0;
        }

        return source;
    }

    public static AudioSource PlaySFX(
        AudioClip clip,
        float volume,
        float pitch,
        Vector3 originPosition,
        bool varyPitch = true
    )
    {
        if (!Application.isPlaying)
        {
            return null;
        }

        float pitchVariance = 1f;

        if (varyPitch)
        {
            pitchVariance = Random.Range(MIN_PITCH_VARIATION, MAX_PITCH_VARIATION);
        }

        return PlaySFXClip(clip, originPosition, volume, pitch * pitchVariance);
    }

    private void UpdateMasterVolume(object sender, float newVolume)
    {
        volumeMixer.SetFloat(
            "master",
            Mathf.Log10(Mathf.Clamp(newVolume, logMinThreshold, 1f)) * logMult
        );
    }

    private void UpdateMusicVolume(object sender, float newVolume)
    {
        volumeMixer.SetFloat(
            "music",
            Mathf.Log10(Mathf.Clamp(newVolume, logMinThreshold, 1f)) * logMult
        );
    }

    private void UpdateSFXVolume(object sender, float newVolume)
    {
        volumeMixer.SetFloat(
            "sfx",
            Mathf.Log10(Mathf.Clamp(newVolume, logMinThreshold, 1f)) * logMult
        );
    }

    private void UpdateVoiceVolume(object sender, float newVolume)
    {
        volumeMixer.SetFloat(
            "voice",
            Mathf.Log10(Mathf.Clamp(newVolume, logMinThreshold, 1f)) * logMult
        );
    }
}

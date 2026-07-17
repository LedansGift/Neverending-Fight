using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private float logMinThreshold = 0.0001f;
    private float logMult = 20f;

    private const float MIN_PITCH_VARIATION = 0.9f;
    private const float MAX_PITCH_VARIATION = 1.1f;

    private static int sfxPoolCounter = 0;

    private MusicPlayer musicPlayer;

    [SerializeField]
    private AudioMixer volumeMixer;

    [SerializeField]
    private AudioSource[] localSfxPool;
    private static AudioSource[] sfxPool;

    private static EventHandler<MusicTrack> OnNewMusicTrack;

    private void Awake()
    {
        musicPlayer = GetComponent<MusicPlayer>();
    }

    private void OnEnable()
    {
        OptionsUI.OnMasterVolumeUpdated += UpdateMasterVolume;
        OptionsUI.OnMusicVolumeUpdated += UpdateMusicVolume;
        OptionsUI.OnSFXVolumeUpdated += UpdateSFXVolume;
        OptionsUI.OnVoiceVolumeUpdated += UpdateVoiceVolume;

        OnNewMusicTrack += SetNewMusicTrack;

        sfxPool = localSfxPool;
        sfxPoolCounter = 0;
    }

    private void OnDisable()
    {
        OptionsUI.OnMasterVolumeUpdated -= UpdateMasterVolume;
        OptionsUI.OnMusicVolumeUpdated -= UpdateMusicVolume;
        OptionsUI.OnSFXVolumeUpdated -= UpdateSFXVolume;
        OptionsUI.OnVoiceVolumeUpdated -= UpdateVoiceVolume;

        OnNewMusicTrack -= SetNewMusicTrack;
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
        SFXObject sfxObj,
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

        return PlaySFXClip(
            sfxObj.sfxClip,
            originPosition,
            sfxObj.volume,
            sfxObj.pitch * pitchVariance
        );
    }

    public static void SetMusicTrack(MusicTrack musicTrack)
    {
        OnNewMusicTrack?.Invoke(null, musicTrack);
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

    private void SetNewMusicTrack(object sender, MusicTrack musicTrack)
    {
        musicPlayer.SetMusicTrack(musicTrack);
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    private bool vSyncState;
    private bool fullScreenState;

    private AudioSource sampleAudioSource;

    [SerializeField]
    private Slider masterSlider;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private AudioClip sampleSFX;

    [SerializeField]
    private Slider voiceSlider;

    [SerializeField]
    private AudioClip sampleVoice;

    [SerializeField]
    private Image vsyncButtonImage;

    [SerializeField]
    private Image fullScreenButtonImage;

    public static EventHandler<float> OnMasterVolumeUpdated;
    public static EventHandler<float> OnMusicVolumeUpdated;
    public static EventHandler<float> OnSFXVolumeUpdated;
    public static EventHandler<float> OnVoiceVolumeUpdated;

    public static EventHandler<bool> OnVSyncUpdated;
    public static EventHandler<bool> OnFullScreenUpdated;

    private void OnEnable()
    {
        masterSlider.value = PlayerOptions.GetMasterVolume();
        musicSlider.value = PlayerOptions.GetMusicVolume();
        sfxSlider.value = PlayerOptions.GetSFXVolume();
        voiceSlider.value = PlayerOptions.GetVoiceVolume();

        vSyncState = PlayerOptions.GetVSync();
        fullScreenState = PlayerOptions.GetFullScreen();

        if (vSyncState)
        {
            vsyncButtonImage.color = Color.white;
        }
        else
        {
            vsyncButtonImage.color = Color.gray;
        }

        if (fullScreenState)
        {
            fullScreenButtonImage.color = Color.white;
        }
        else
        {
            fullScreenButtonImage.color = Color.gray;
        }

        sampleAudioSource = GetComponent<AudioSource>();
    }

    public void SetMasterVolume(float newVolume)
    {
        PlayerOptions.SetMasterVolume(newVolume);
        OnMasterVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetMusicVolume(float newVolume)
    {
        PlayerOptions.SetMusicVolume(newVolume);
        OnMusicVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetSFXVolume(float newVolume)
    {
        PlayerOptions.SetSFXVolume(newVolume);

        OnSFXVolumeUpdated?.Invoke(this, newVolume);

        if (!sampleAudioSource)
        {
            return;
        }

        sampleAudioSource.clip = sampleSFX;
        sampleAudioSource.volume = PlayerOptions.GetSFXVolume();
        sampleAudioSource.Play();
    }

    public void SetVoiceVolume(float newVolume)
    {
        PlayerOptions.SetVoiceVolume(newVolume);

        OnVoiceVolumeUpdated?.Invoke(this, newVolume);

        if (!sampleAudioSource)
        {
            return;
        }

        sampleAudioSource.clip = sampleVoice;
        sampleAudioSource.volume = PlayerOptions.GetVoiceVolume();
        sampleAudioSource.Play();
    }

    public void ToggleVSync()
    {
        vSyncState = !vSyncState;

        if (vSyncState)
        {
            vsyncButtonImage.color = Color.white;
        }
        else
        {
            vsyncButtonImage.color = Color.gray;
        }

        PlayerOptions.SetVSync(vSyncState);

        OnVSyncUpdated?.Invoke(this, vSyncState);
    }

    public void ToggleFullScreen()
    {
        fullScreenState = !fullScreenState;

        if (fullScreenState)
        {
            fullScreenButtonImage.color = Color.white;
        }
        else
        {
            fullScreenButtonImage.color = Color.gray;
        }

        PlayerOptions.SetFullScreen(fullScreenState);

        OnFullScreenUpdated?.Invoke(this, fullScreenState);
    }
}

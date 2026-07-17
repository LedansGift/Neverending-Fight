using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private bool fadeMusic = false;

    private float targetMusicVolume;
    private float fadeSpeed = 0.5f;

    private MusicTrack activeMusicTrack;
    private MusicTrack newMusicTrack;

    [SerializeField]
    private AudioSource musicAudioSource;

    private void Awake()
    {
        musicAudioSource.volume = 0f;
    }

    private void Update()
    {
        if (activeMusicTrack && (musicAudioSource.time >= (activeMusicTrack.track.length - 0.001f)))
        {
            musicAudioSource.Play();
            musicAudioSource.time = activeMusicTrack.loopTime * activeMusicTrack.track.length;
        }

        if (fadeMusic)
        {
            FadeMusic();
        }
    }

    private void FadeMusic()
    {
        musicAudioSource.volume = Mathf.MoveTowards(
            musicAudioSource.volume,
            targetMusicVolume,
            activeMusicTrack.trackVolume * fadeSpeed * Time.unscaledDeltaTime
        );

        if (musicAudioSource.volume == targetMusicVolume)
        {
            fadeMusic = false;

            if (targetMusicVolume == 0f)
            {
                activeMusicTrack = null;
            }

            if (newMusicTrack != null)
            {
                activeMusicTrack = newMusicTrack;
                newMusicTrack = null;
                FadeInActiveTrack();
            }
        }
    }

    private void FadeOutActiveTrack()
    {
        targetMusicVolume = 0f;

        fadeMusic = true;
    }

    private void FadeInActiveTrack()
    {
        targetMusicVolume = activeMusicTrack.trackVolume;
        musicAudioSource.clip = activeMusicTrack.track;
        musicAudioSource.Play();

        fadeMusic = true;
    }

    public void SetMusicTrack(MusicTrack musicTrack)
    {
        newMusicTrack = musicTrack;

        if (activeMusicTrack)
        {
            FadeOutActiveTrack();
        }
        else if (newMusicTrack)
        {
            activeMusicTrack = newMusicTrack;
            newMusicTrack = null;
            FadeInActiveTrack();
        }
    }
}

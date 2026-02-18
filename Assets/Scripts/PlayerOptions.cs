using UnityEngine;

public class PlayerOptions
{
    private const string MASTER_VOLUME = "MasterVolume";
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string VOICE_VOLUME = "VoiceVolume";
    private const string VSYNC = "VSync";
    private const string FULLSCREEN = "Fullscreen";

    private static float MASTER_VOLUME_DEF = 0.5f;
    private static float MUSIC_VOLUME_DEF = 1f;
    private static float SFX_VOLUME_DEF = 1f;
    private static float VOICE_VOLUME_DEF = 0.8f;

    private static bool VSYNC_DEF = true;
    private static bool FULLSCREEN_DEF = true;

    public static void SetMasterVolume(float newVolume)
    {
        //MASTER_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(MASTER_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetMusicVolume(float newVolume)
    {
        //MUSIC_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetSFXVolume(float newVolume)
    {
        // SFX_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(SFX_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetVoiceVolume(float newVolume)
    {
        //VOICE_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(VOICE_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetVSync(bool toggle)
    {
        int vSync = 0;
        if (toggle)
        {
            vSync = 1;
        }

        PlayerPrefs.SetInt(VSYNC, vSync);
    }

    public static void SetFullScreen(bool toggle)
    {
        int fullScreen = 0;
        if (toggle)
        {
            fullScreen = 1;
        }

        PlayerPrefs.SetInt(FULLSCREEN, fullScreen);
    }

    public static float GetMasterVolume()
    {
        if (!PlayerPrefs.HasKey(MASTER_VOLUME))
        {
            return MASTER_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(MASTER_VOLUME);
        }
    }

    public static float GetMusicVolume()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            return MUSIC_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
    }

    public static float GetSFXVolume()
    {
        if (!PlayerPrefs.HasKey(SFX_VOLUME))
        {
            return SFX_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(SFX_VOLUME);
        }
    }

    public static float GetVoiceVolume()
    {
        if (!PlayerPrefs.HasKey(VOICE_VOLUME))
        {
            return VOICE_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(VOICE_VOLUME);
        }
    }

    public static bool GetVSync()
    {
        if (!PlayerPrefs.HasKey(VSYNC))
        {
            return VSYNC_DEF;
        }
        else
        {
            bool vSync = false;
            if (PlayerPrefs.GetInt(VSYNC) == 1)
            {
                vSync = true;
            }

            return vSync;
        }
    }

    public static bool GetFullScreen()
    {
        if (!PlayerPrefs.HasKey(FULLSCREEN))
        {
            return FULLSCREEN_DEF;
        }
        else
        {
            bool fullScreen = false;
            if (PlayerPrefs.GetInt(FULLSCREEN) == 1)
            {
                fullScreen = true;
            }

            return fullScreen;
        }
    }
}

using UnityEngine;

public class TimelineFunctions : MonoBehaviour
{
    public void DisplayBossTitle()
    {
        BossTitleUI.DisplayTitle();
    }

    public void StartNewMusic(MusicTrack musicTrack)
    {
        AudioManager.SetMusicTrack(musicTrack);
    }
}

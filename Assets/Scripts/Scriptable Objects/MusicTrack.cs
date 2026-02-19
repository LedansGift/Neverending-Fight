using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Music Track", menuName = "Music Track", order = 4)]
public class MusicTrack : ScriptableObject
{
    public AudioClip track;

    [Range(0, 1)]
    public float trackVolume;

    [Range(0, 1)]
    public float loopTime;
}

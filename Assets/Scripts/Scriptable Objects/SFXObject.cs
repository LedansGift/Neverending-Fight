using System;
using UnityEngine;

[Serializable]
public struct SFX
{
    public AudioClip sfxClip;

    [Range(0, 1)]
    public float volume;

    [Range(0, 2)]
    public float pitch;
}

[CreateAssetMenu(fileName = "SFX Object", menuName = "SFX/SFX Object", order = 3)]
public class SFXObject : ScriptableObject
{
    public SFX soundEffect;

    public virtual AudioSource PlaySFX(Vector3 sfxPosition, bool varyPitch = true)
    {
        return AudioManager.PlaySFX(soundEffect, sfxPosition, varyPitch);
    }
}

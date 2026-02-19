using UnityEngine;

[CreateAssetMenu(fileName = "SFX Object", menuName = "SFX Object", order = 3)]
public class SFXObject : ScriptableObject
{
    public AudioClip sfxClip;

    [Range(0, 1)]
    public float volume;

    [Range(0, 2)]
    public float pitch;
}

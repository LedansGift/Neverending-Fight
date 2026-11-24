using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SFXDelayHolder
{
    public SFXObject sfxObject;
    public float sfxDelay;
}

public class VFXPlayer : MonoBehaviour
{
    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    [SerializeField]
    private string vfxKey = "DEFAULT_KEY";

    [SerializeField]
    private ParticleSystem particleVFX;

    [SerializeField]
    private SFXDelayHolder[] soundEffects;

    private IEnumerator PlayDelayedSFX(SFXDelayHolder sfx)
    {
        yield return new WaitForSeconds(sfx.sfxDelay);

        AudioSource sfxSource = sfx.sfxObject.PlaySFX(transform.position);
        activeAudioSources.Add(sfxSource);
    }

    public string GetVFXKey()
    {
        return vfxKey;
    }

    public void PlayVFX(Vector3 vfxPosition, Quaternion vfxRotation)
    {
        transform.position = vfxPosition;
        transform.rotation = vfxRotation;

        particleVFX.Play();

        activeAudioSources = new List<AudioSource>();

        foreach (SFXDelayHolder sfx in soundEffects)
        {
            if (sfx.sfxDelay <= 0f)
            {
                AudioSource sfxSource = sfx.sfxObject.PlaySFX(transform.position);
                activeAudioSources.Add(sfxSource);
            }
            else
            {
                StartCoroutine(PlayDelayedSFX(sfx));
            }
        }
    }

    public void PlayVFX(Transform vfxParent)
    {
        PlayVFX(vfxParent.position, vfxParent.rotation);
    }

    public void StopVFX()
    {
        particleVFX.Stop();

        StopAllCoroutines();

        foreach (AudioSource activeSource in activeAudioSources)
        {
            activeSource.Stop();
        }
    }
}

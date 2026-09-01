using UnityEngine;

[CreateAssetMenu(fileName = "Layered SFX", menuName = "SFX/Layered SFX", order = 3)]
public class SFXObjectLayered : SFXObject
{
    public float initialSFXDelay;

    public SFX[] additionalSFX;
    public float[] additionalSFXDelays;

    public override AudioSource PlaySFX(Vector3 sfxPosition, bool varyPitch = true)
    {
        AudioManager.PlayDelayedSFX(soundEffect, sfxPosition, initialSFXDelay, varyPitch);

        for (int i = 0; i < additionalSFX.Length; i++)
        {
            float delay = 0f;

            if (i < additionalSFXDelays.Length)
            {
                delay = additionalSFXDelays[i];
            }

            AudioManager.PlayDelayedSFX(additionalSFX[i], sfxPosition, delay, varyPitch);
        }

        return null;
    }
}

using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField]
    private string vfxKey = "DEFAULT_KEY";

    [SerializeField]
    private ParticleSystem particleVFX;

    public string GetVFXKey()
    {
        return vfxKey;
    }

    public void PlayVFX(Transform vfxParent)
    {
        transform.position = vfxParent.position;
        transform.rotation = vfxParent.rotation;
        particleVFX.Play();
    }

    public void StopVFX()
    {
        particleVFX.Stop();
    }
}

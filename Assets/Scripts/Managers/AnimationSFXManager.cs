using UnityEngine;

public class AnimationSFXManager : MonoBehaviour
{
    [SerializeField]
    private float audioOutputHeight;

    public void PlaySFX(SFXObject sfx)
    {
        Vector3 outputPosition = transform.position + new Vector3(0f, audioOutputHeight, 0f);
        AudioManager.PlaySFX(sfx, outputPosition);
    }
}

using UnityEngine;

public class TimepieceCanister : MonoBehaviour
{
    private bool disable = false;
    private float defaultFlameBlur = 0.6f;

    private float fadeSpeed = 3f;

    private Material canisterMaterial;

    [SerializeField]
    private MeshRenderer canisterRenderer;

    private void Awake()
    {
        canisterMaterial = canisterRenderer.material;
    }

    private void Update()
    {
        if (disable)
        {
            float flameBlur = canisterMaterial.GetFloat("_Y_Segment_Blur");

            canisterMaterial.SetFloat(
                "_Y_Segment_Blur",
                flameBlur - (fadeSpeed * Time.unscaledDeltaTime)
            );

            if (flameBlur < -1f)
            {
                disable = false;
            }
        }
    }

    public void DisableCanister()
    {
        disable = true;
    }

    public void ReplenishCanister()
    {
        canisterMaterial.SetFloat("_Y_Segment_Blur", defaultFlameBlur);
    }
}

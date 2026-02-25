using UnityEngine;

public class GlaiveFlame : MonoBehaviour
{
    private bool flameChange = false;
    private bool flameActive = false;
    private float flameTimer = 0f;
    private float flameChangeDuration = 0.5f;
    private float flameNonTarget = 0f;
    private float flameTarget = 1f;
    private const float FLAME_ACTIVE_FADE = 0f;
    private const float FLAME_INACTIVE_FADE = 20f;

    [SerializeField]
    private MeshRenderer flameRenderer;

    [SerializeField]
    private MeshRenderer spikesRenderer;
    private Material flameMaterial;
    private Material spikesMaterial;

    private void Awake()
    {
        flameMaterial = flameRenderer.material;
        spikesMaterial = spikesRenderer.material;

        flameMaterial.SetFloat("_Edge_Fade", FLAME_INACTIVE_FADE);
        spikesMaterial.SetFloat("_Edge_Fade", FLAME_INACTIVE_FADE);
    }

    private void Update()
    {
        if (flameChange)
        {
            FlameChange();
        }
    }

    private void FlameChange()
    {
        flameTimer += Time.deltaTime;

        if (flameTimer >= flameChangeDuration)
        {
            flameTimer = flameChangeDuration;
            flameChange = false;
        }

        float flameLerp = flameTimer / flameChangeDuration;

        float newFlame = Mathf.Lerp(flameNonTarget, flameTarget, flameLerp);

        flameMaterial.SetFloat("_Edge_Fade", newFlame);
        spikesMaterial.SetFloat("_Edge_Fade", newFlame);
    }

    public void StartFlameChange(bool flameActive)
    {
        if (this.flameActive == flameActive)
        {
            return;
        }

        this.flameActive = flameActive;
        flameChange = true;
        flameTimer = 0f;

        if (flameActive)
        {
            flameTarget = FLAME_ACTIVE_FADE;
            flameNonTarget = FLAME_INACTIVE_FADE;
        }
        else
        {
            flameTarget = FLAME_INACTIVE_FADE;
            flameNonTarget = FLAME_ACTIVE_FADE;
        }
    }
}

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DamageZoneCircle : DamageZone
{
    [SerializeField]
    private Transform innerBorder1;

    [SerializeField]
    private Transform innerBorder2;

    [SerializeField]
    private Renderer borderRenderer;

    [SerializeField]
    private DecalProjector innerRenderer;

    protected override void ChangeSize()
    {
        zoneGrowTimer += Time.deltaTime;

        if (zoneGrowTimer >= zoneGrowDuration)
        {
            zoneGrowTimer = zoneGrowDuration;
            sizeChange = false;

            if (zoneTarget <= 0f)
            {
                zoneVisual.gameObject.SetActive(false);
                return;
            }
        }

        float growLerp = zoneGrowTimer / zoneGrowDuration;

        if (easeGrowth)
        {
            growLerp = AdditionalMath.EaseOutCubic(growLerp);
        }

        float zoneScale = Mathf.Lerp(1f - zoneTarget, zoneTarget, growLerp) * zoneSize.x;

        zoneVisual.localScale = new Vector3(zoneScale, zoneVisual.localScale.y, zoneScale);
        decalProjector.size = new Vector3(zoneScale * 2f, zoneScale * 2f, zoneVisual.localScale.y);
    }

    private void SetupArc(float arc)
    {
        borderRenderer.material.SetFloat("_Border_Arc", arc);
        innerRenderer.material.SetFloat("_Border_Arc", arc);

        if (arc >= 1f)
        {
            innerBorder1.gameObject.SetActive(false);
            innerBorder2.gameObject.SetActive(false);

            return;
        }

        float innerBorderRotation = Mathf.Lerp(0f, 180f, arc);

        innerBorder1.localEulerAngles = new Vector3(0f, innerBorderRotation, 0f);
        innerBorder2.localEulerAngles = new Vector3(0f, -innerBorderRotation, 0f);

        innerBorder1.gameObject.SetActive(true);
        innerBorder2.gameObject.SetActive(true);
    }

    public override void ActivateZone(
        Vector2 targetRadius,
        float lifeTime = 0,
        float growDuration = 0.35F
    )
    {
        base.ActivateZone(targetRadius, lifeTime, growDuration);

        SetupArc(targetRadius.y);
    }
}

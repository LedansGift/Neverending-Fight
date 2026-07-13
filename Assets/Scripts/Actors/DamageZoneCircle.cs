using UnityEngine;

public class DamageZoneCircle : DamageZone
{
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

        if (fadeOut)
        {
            decalProjector.fadeFactor = 1f - growLerp;

            return;
        }
        else
        {
            decalProjector.fadeFactor = growLerp;
        }

        float zoneScale = Mathf.Lerp(1f - zoneTarget, zoneTarget, growLerp) * zoneSize.x;

        zoneVisual.localScale = new Vector3(zoneScale, zoneVisual.localScale.y, zoneScale);
        decalProjector.size = new Vector3(zoneScale * 2f, zoneScale * 2f, zoneVisual.localScale.y);
    }

    private void SetupArc(float arc)
    {
        decalProjector.material.SetFloat("_Border_Arc", arc);
        decalProjector.material.SetFloat("_Donut_Size", 0f);
    }

    public override void ActivateZone(
        Vector2 targetRadius,
        float lifeTime = 0,
        float growDuration = 0.35f
    )
    {
        base.ActivateZone(targetRadius, lifeTime, growDuration);

        SetupArc(targetRadius.y);
    }
}

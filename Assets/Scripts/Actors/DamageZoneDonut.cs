using UnityEngine;

public class DamageZoneDonut : DamageZoneCircle
{
    private void SetupDonut(float donut)
    {
        decalProjector.material.SetFloat("_Border_Arc", 1f);
        decalProjector.material.SetFloat("_Donut_Size", donut);
    }

    public override void ActivateZone(
        Vector2 targetRadius,
        float lifeTime = 0,
        float growDuration = 0.35f
    )
    {
        base.ActivateZone(targetRadius, lifeTime, growDuration);

        SetupDonut(targetRadius.y);
    }
}

using UnityEngine;

public class TomeAttackVisual : DamageZone
{
    private float maxRangeVisualGrowTime = 0.1f;

    [SerializeField]
    private DamageZone maxAttackRangeVisual;

    private void Start()
    {
        transform.SetParent(null);
    }

    public override void ActivateZone(
        Vector2 targetRadius,
        float lifeTime = 0,
        float growDuration = 0.35F
    )
    {
        base.ActivateZone(targetRadius, lifeTime, growDuration);
        maxAttackRangeVisual.ActivateZone(targetRadius, lifeTime, maxRangeVisualGrowTime);
    }

    public override void DeactivateZone(float growDuration = 0.35F)
    {
        base.DeactivateZone(growDuration);
        maxAttackRangeVisual.DeactivateZone(maxRangeVisualGrowTime);
    }
}

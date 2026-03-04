using UnityEngine;

public class DamageZone : MonoBehaviour
{
    protected bool sizeChange = false;

    protected Vector2 zoneSize = Vector2.one;
    protected float zoneGrowTimer = 0f;
    protected float zoneGrowDuration = 0.35f;
    protected float lifeTime = 0f;
    protected float zoneTarget = 0f;

    [SerializeField]
    protected bool easeGrowth = true;

    [SerializeField]
    protected Transform zoneVisual;

    private void Awake()
    {
        zoneVisual.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (sizeChange)
        {
            ChangeSize();
        }

        if (lifeTime > 0f)
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0f)
            {
                DeactivateZone();
            }
        }
    }

    protected virtual void ChangeSize()
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

        float zoneScaleX = Mathf.Lerp(1f - zoneTarget, zoneTarget, growLerp) * zoneSize.x;
        float zoneScaleZ = Mathf.Lerp(1f - zoneTarget, zoneTarget, growLerp) * zoneSize.y;

        zoneVisual.localScale = new Vector3(zoneScaleX, zoneVisual.localScale.y, zoneScaleZ);
    }

    public virtual void ActivateZone(
        Vector2 targetRadius,
        float lifeTime = 0f,
        float growDuration = 0.35f
    )
    {
        this.lifeTime = lifeTime;
        zoneGrowDuration = growDuration;
        zoneSize = targetRadius;

        zoneTarget = 1f;
        zoneGrowTimer = 0f;
        zoneVisual.localScale = new Vector3(0f, zoneVisual.localScale.y, 0f);
        zoneVisual.gameObject.SetActive(true);

        sizeChange = true;
    }

    public void DeactivateZone(float growDuration = 0.35f)
    {
        zoneTarget = 0f;
        zoneGrowDuration = growDuration;
        zoneGrowTimer = 0f;
        zoneSize = new Vector2(zoneVisual.localScale.x, zoneVisual.localScale.z);
        sizeChange = true;
    }
}

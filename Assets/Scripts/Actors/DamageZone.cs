using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private bool sizeChange = false;
    private float zoneRadius = 1f;
    private float zoneGrowTimer = 0f;
    private float zoneGrowDuration = 0.35f;
    private float lifeTime = 0f;
    private float zoneTarget = 0f;

    [SerializeField]
    private bool easeGrowth = true;

    [SerializeField]
    private Transform zoneVisual;

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

    private void ChangeSize()
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

        float zoneScale = Mathf.Lerp(1f - zoneTarget, zoneTarget, growLerp) * zoneRadius;

        zoneVisual.localScale = new Vector3(zoneScale, zoneVisual.localScale.y, zoneScale);
    }

    public void ActivateZone(float targetRadius, float lifeTime = 0f, float growDuration = 0.35f)
    {
        this.lifeTime = lifeTime;
        zoneGrowDuration = growDuration;
        zoneRadius = targetRadius;
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
        zoneRadius = zoneVisual.localScale.x;
        sizeChange = true;
    }
}

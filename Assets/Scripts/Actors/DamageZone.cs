using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DamageZone : MonoBehaviour
{
    protected bool sizeChange = false;
    protected bool fadeOut = false;

    protected Vector2 zoneSize = Vector2.one;
    protected float zoneGrowTimer = 0f;
    protected float zoneGrowDuration = 0.6f;
    protected float lifeTime = 0f;
    protected float zoneTarget = 0f;

    [SerializeField]
    protected bool easeGrowth = true;

    [SerializeField]
    protected Transform zoneVisual;

    [SerializeField]
    protected DecalProjector decalProjector;

    //Include the default material that it uses when being activated
    //Allow input of a material that it changes to when an attack happens

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

        if (fadeOut)
        {
            decalProjector.fadeFactor = 1f - growLerp;

            return;
        }
        else
        {
            decalProjector.fadeFactor = growLerp;
        }

        float zoneScaleX = Mathf.Lerp(1f - zoneTarget, zoneTarget, growLerp) * zoneSize.x;
        float zoneScaleZ = Mathf.Lerp(1f - zoneTarget, zoneTarget, growLerp) * zoneSize.y;

        zoneVisual.localScale = new Vector3(zoneScaleX, zoneVisual.localScale.y, zoneScaleZ);
        decalProjector.size = new Vector3(
            zoneScaleX * 2f,
            zoneScaleZ * 2f,
            zoneVisual.localScale.y
        );
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

        fadeOut = false;
        sizeChange = true;
    }

    public void DeactivateZone(float growDuration = 0.35f)
    {
        zoneTarget = 0f;
        zoneGrowDuration = growDuration;
        zoneGrowTimer = 0f;
        zoneSize = new Vector2(zoneVisual.localScale.x, zoneVisual.localScale.z);
        fadeOut = true;
        sizeChange = true;
    }
}

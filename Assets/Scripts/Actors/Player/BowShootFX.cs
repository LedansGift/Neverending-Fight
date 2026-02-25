using UnityEngine;
using UnityEngine.Rendering;

public class BowShootFX : MonoBehaviour
{
    private bool chargeShot = false;
    private bool specialShot = false;
    private float chargeTimer = 0f;
    private float chargeDuration = 1f;
    private float specialTimer = 0f;
    private float specialDuration = 1.25f;

    private float chargeMaterialPowerStart = 20f;
    private float chargeMaterialPowerEnd = 1f;

    [SerializeField]
    private ParticleSystem shootParticle;

    [SerializeField]
    private ParticleSystem specialParticle;

    [SerializeField]
    private GameObject chargeObject;

    [SerializeField]
    private MeshRenderer chargeObjectRenderer;
    private Material chargeObjectMaterial;

    private float chargeScaleStart = 20f;
    private float chargeScaleEnd = 0.5f;

    [SerializeField]
    private GameObject chargeCircle;
    private Material chargeCircleMaterial;

    [SerializeField]
    private AnimationCurve chargeCurve;

    [SerializeField]
    private GameObject specialShotVisual;

    [SerializeField]
    private AnimationCurve specialShotCurve;

    [SerializeField]
    private AnimationCurve specialShotYScaleCurve;

    [SerializeField]
    private Volume specialShotVolume;

    private void Start()
    {
        chargeCircleMaterial = chargeCircle.GetComponent<MeshRenderer>().material;
        chargeObjectMaterial = chargeObjectRenderer.material;

        specialShotVisual.SetActive(false);
        specialShotVolume.weight = 0f;

        ResetFX();
    }

    private void Update()
    {
        if (chargeShot)
        {
            ChargeShot();
        }

        if (specialShot)
        {
            SpecialShot();
        }
    }

    private void ChargeShot()
    {
        chargeTimer += Time.deltaTime;

        if (chargeTimer >= chargeDuration)
        {
            chargeTimer = chargeDuration;
        }

        float chargeLerp = chargeTimer / chargeDuration;
        float curveLerp = chargeCurve.Evaluate(chargeLerp);

        float chargeScale = Mathf.Lerp(chargeScaleStart, chargeScaleEnd, curveLerp);
        float chargePower = Mathf.Lerp(chargeMaterialPowerStart, chargeMaterialPowerEnd, curveLerp);

        chargeObject.transform.localScale = new Vector3(curveLerp, curveLerp, curveLerp);

        chargeObjectMaterial.SetFloat("_Power", chargePower);

        chargeCircle.transform.localScale = new Vector3(chargeScale, chargeScale, chargeScale);
        chargeCircleMaterial.color = new Color(
            chargeCircleMaterial.color.r,
            chargeCircleMaterial.color.g,
            chargeCircleMaterial.color.b,
            Mathf.Pow(chargeLerp, 2f) * 0.5f
        );
    }

    private void SpecialShot()
    {
        specialTimer += Time.deltaTime;

        if (specialTimer >= specialDuration)
        {
            specialTimer = specialDuration;
            specialShotVisual.SetActive(false);
            specialShot = false;
            specialShotVolume.weight = 0f;

            return;
        }

        float specialLerp = specialTimer / specialDuration;
        float curveLerp = specialShotCurve.Evaluate(specialLerp);
        float curveYLerp = specialShotYScaleCurve.Evaluate(specialLerp);

        specialShotVisual.transform.localScale = new Vector3(
            curveLerp * 1.5f,
            curveYLerp * 100f,
            curveLerp * 1.5f
        );
        specialShotVolume.weight = curveLerp;
    }

    public void StartSpecialVisual()
    {
        specialShotVisual.transform.localScale = Vector3.zero;
        specialShotVisual.SetActive(true);
        specialParticle.Play();

        specialTimer = 0f;

        specialShot = true;
    }

    public void StartCharge(float chargeDuration)
    {
        chargeCircle.SetActive(true);
        chargeObject.SetActive(true);

        chargeObject.transform.localScale = Vector3.zero;

        chargeCircleMaterial.color = new Color(
            chargeCircleMaterial.color.r,
            chargeCircleMaterial.color.g,
            chargeCircleMaterial.color.b,
            0f
        );

        chargeShot = true;
        chargeTimer = 0f;
        this.chargeDuration = chargeDuration;
    }

    public void ResetFX()
    {
        chargeCircle.SetActive(false);
        chargeObject.SetActive(false);

        chargeShot = false;
    }

    public void FinishCharge()
    {
        float particlesScale = chargeTimer / chargeDuration;
        shootParticle.transform.localScale = new Vector3(
            particlesScale,
            particlesScale,
            particlesScale
        );
        shootParticle.Play();

        ResetFX();
    }
}

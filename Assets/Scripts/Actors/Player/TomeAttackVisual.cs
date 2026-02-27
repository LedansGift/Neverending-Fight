public class TomeAttackVisual : DamageZone
{
    // private bool visualCharging = false;
    // private float chargeProgress = 0f;
    // private float chargeTime = 1f;
    // private float targetRadius = 1f;

    // [SerializeField]
    // private GameObject attackVisual;

    // private void Awake()
    // {
    //     transform.SetParent(null);
    //     DeactivateVisual();
    // }

    private void Start()
    {
        transform.SetParent(null);
    }

    // private void Update()
    // {
    //     if (visualCharging)
    //     {
    //         chargeProgress = Mathf.Min(chargeProgress + Time.deltaTime, chargeTime);
    //         float lerp = chargeProgress / chargeTime;

    //         float chargeScale = Mathf.Lerp(0f, targetRadius, lerp);

    //         transform.localScale = new Vector3(chargeScale, chargeScale, chargeScale);
    //     }
    // }

    // public void ActivateVisual(float targetRadius, float chargeTime)
    // {
    //     attackVisual.SetActive(true);
    //     visualCharging = true;
    //     transform.localScale = Vector3.zero;
    //     this.targetRadius = targetRadius;
    //     this.chargeTime = chargeTime;
    //     chargeProgress = 0f;
    // }

    // public void DeactivateVisual()
    // {
    //     attackVisual.SetActive(false);
    //     visualCharging = false;
    // }
}

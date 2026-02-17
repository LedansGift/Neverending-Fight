using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    private bool uiChange = false;
    private float uiChangeTimer = 0f;
    private float uiScaleNonTarget = 0f;
    private float uiScaleTarget = 1f;

    private const float INACTIVE_UI_SCALE = 0.6f;
    private const float UI_CHANGE_SPEED = 1f;
    private Color uiColourNonTarget = Color.black;
    private Color uiColourTarget = Color.white;

    private Color inactiveUIColour;

    private AnimationCurve scaleChangeCurve;

    [SerializeField]
    private Transform weaponUIHolder;

    [SerializeField]
    private Image[] weaponUIImages;

    [SerializeField]
    private Transform weaponAbilityUI;

    private void Update()
    {
        if (uiChange)
        {
            UpdateWeaponUI();
        }
    }

    private void UpdateWeaponUI()
    {
        uiChangeTimer += Time.deltaTime;
        float changeLerp = uiChangeTimer / UI_CHANGE_SPEED;

        if (changeLerp >= 1f)
        {
            changeLerp = 1f;
            uiChange = false;
        }

        float curveVal = scaleChangeCurve.Evaluate(changeLerp);

        float newScale = Mathf.Lerp(uiScaleNonTarget, uiScaleTarget, curveVal);
        float newColour = Mathf.Lerp(uiColourNonTarget.a, uiColourTarget.a, curveVal);

        weaponUIHolder.localScale = new Vector3(newScale, newScale, newScale);

        foreach (Image uiImage in weaponUIImages)
        {
            uiImage.color = new Color(newColour, newColour, newColour, newColour);
        }
    }

    public void SetInactiveUIVars(Color uiColour, AnimationCurve scaleCurve)
    {
        inactiveUIColour = uiColour;
        scaleChangeCurve = scaleCurve;
    }

    public virtual void SetAbilityCharge(float newCharge)
    {
        weaponAbilityUI.localScale = new Vector3(newCharge, newCharge, newCharge);
    }

    public void SetUIActive(bool toggle)
    {
        if (toggle)
        {
            uiScaleTarget = 1f;
            uiScaleNonTarget = INACTIVE_UI_SCALE;

            uiColourTarget = Color.white;
            uiColourNonTarget = inactiveUIColour;
        }
        else
        {
            uiScaleTarget = INACTIVE_UI_SCALE;
            uiScaleNonTarget = 1f;

            uiColourTarget = inactiveUIColour;
            uiColourNonTarget = Color.white;
        }

        uiChangeTimer = 0f;
        uiChange = true;
    }
}

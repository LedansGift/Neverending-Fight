using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct CastInfo
{
    public CastInfo(string castName, float castTime)
    {
        this.castName = castName;
        this.castTime = castTime;
    }

    public string castName;
    public float castTime;
}

public class BossCastBarUI : MonoBehaviour
{
    private bool castActive = false;
    private float castTime = 0f;
    private float castTimeTarget = 0f;

    [SerializeField]
    private Slider castSlider;

    [SerializeField]
    private TextMeshProUGUI castText;

    [SerializeField]
    private CanvasGroupFader fader;

    private static Action OnCancelCast;
    private static EventHandler<CastInfo> OnBossCast;

    private void Awake()
    {
        fader.SetCanvasGroupAlpha(0f);
    }

    private void OnEnable()
    {
        OnBossCast += StartCast;
        OnCancelCast += FinishCast;
    }

    private void OnDisable()
    {
        OnBossCast -= StartCast;
        OnCancelCast = FinishCast;
    }

    private void Update()
    {
        if (!castActive)
        {
            return;
        }

        castTime += Time.deltaTime;

        if (castTime >= castTimeTarget)
        {
            castTime = castTimeTarget;
            FinishCast();
        }

        castSlider.value = castTime / castTimeTarget;
    }

    private void InitialiseCastBar(string castName, float castTime)
    {
        castText.text = castName;
        castTimeTarget = castTime;

        castTime = 0f;
        castSlider.value = 0f;

        fader.ToggleFade(true);

        castActive = true;
    }

    private void FinishCast()
    {
        fader.ToggleFade(false);

        castActive = false;
    }

    private void StartCast(object sender, CastInfo castInfo)
    {
        InitialiseCastBar(castInfo.castName, castInfo.castTime);
    }

    public static void InitiateCastEvent(CastInfo castInfo)
    {
        OnBossCast?.Invoke(null, castInfo);
    }

    public static void CancelCast()
    {
        OnCancelCast?.Invoke();
    }
}

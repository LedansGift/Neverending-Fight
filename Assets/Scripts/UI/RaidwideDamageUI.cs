using System;
using UnityEngine;
using UnityEngine.UI;

public class RaidwideDamageUI : MonoBehaviour
{
    private bool uiActive = false;
    private int raidwideDamageValue;
    private int playerCurrentHealth;
    private float playerMaxHealth = 100f;
    private float displayDuration = 0f;
    private float displayTime = 0f;

    [SerializeField]
    private GameObject lethalDamageVisual;

    [SerializeField]
    private Slider damageSlider;

    [SerializeField]
    private CanvasGroupFader fader;

    private static EventHandler<float> OnStartUI;
    private static EventHandler<int> OnUpdateDamageValue;

    private void Awake()
    {
        lethalDamageVisual.SetActive(false);
        fader.SetCanvasGroupAlpha(0f);

        PlayerHealth.OnInitialisePlayerHealth += SetInitialPlayerHealth;
    }

    private void Start()
    {
        OnStartUI += StartUI;
        OnUpdateDamageValue += UpdateDamageValue;
        PlayerHealth.OnChangePlayerHealth += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        OnStartUI -= StartUI;
        OnUpdateDamageValue -= UpdateDamageValue;
        PlayerHealth.OnChangePlayerHealth -= UpdatePlayerHealth;
        PlayerHealth.OnInitialisePlayerHealth -= SetInitialPlayerHealth;
    }

    private void Update()
    {
        if (!uiActive)
        {
            return;
        }

        IncrementTimer();
    }

    private void IncrementTimer()
    {
        displayTime += Time.deltaTime;

        if (displayTime >= displayDuration)
        {
            displayTime = 0f;
            EndUI();
        }
    }

    private void EndUI()
    {
        fader.ToggleFade(false);
        uiActive = false;
    }

    private void UpdateDamageSlider()
    {
        damageSlider.value = Mathf.Clamp(raidwideDamageValue / playerMaxHealth, 0f, 1.5f);
    }

    private void UpdateLethalDamageVisual()
    {
        lethalDamageVisual.SetActive(raidwideDamageValue >= playerCurrentHealth);
        // Debug.Log(
        //     "Raidwide Damage: " + raidwideDamageValue + " , Player Health: " + playerCurrentHealth
        // );
    }

    private void UpdatePlayerHealth(object sender, int newHealth)
    {
        playerCurrentHealth = newHealth;
        UpdateLethalDamageVisual();
    }

    private void SetInitialPlayerHealth(object sender, int initialHealth)
    {
        playerMaxHealth = initialHealth;
        playerCurrentHealth = initialHealth;
    }

    private void UpdateDamageValue(object sender, int newDamage)
    {
        raidwideDamageValue = newDamage;
        UpdateLethalDamageVisual();
        UpdateDamageSlider();
    }

    private void StartUI(object sender, float warningTime)
    {
        fader.ToggleFade(true);
        displayDuration = warningTime;
        displayTime = 0f;
        uiActive = true;
    }

    public static void SetDamageValue(int damageValue)
    {
        OnUpdateDamageValue?.Invoke(null, damageValue);
    }

    public static void StartDamageUI(float warningTime)
    {
        OnStartUI?.Invoke(null, warningTime);
    }
}

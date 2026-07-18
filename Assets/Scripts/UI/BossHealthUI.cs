using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    private float bossMaxHealth = 100f;

    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private CanvasGroupFader healthFader;

    private void Start()
    {
        healthFader.SetCanvasGroupAlpha(0f);
    }

    private void OnEnable()
    {
        BossHealth.OnChangeBossHealth += ChangeHealth;
        BossHealth.OnInitialiseBossHealth += InitialiseHealth;
        BossHealth.OnBossDie += FadeOutHealth;
    }

    private void OnDisable()
    {
        BossHealth.OnChangeBossHealth -= ChangeHealth;
        BossHealth.OnInitialiseBossHealth -= InitialiseHealth;
        BossHealth.OnBossDie -= FadeOutHealth;
    }

    private void FadeOutHealth()
    {
        healthFader.ToggleFade(false);
    }

    private void InitialiseHealth(object sender, int maxHealth)
    {
        bossMaxHealth = maxHealth;
        ChangeHealth(this, maxHealth);

        healthFader.ToggleFade(true);
    }

    private void ChangeHealth(object sender, int newHealth)
    {
        float health = (float)newHealth / bossMaxHealth;
        healthSlider.value = health;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    private float bossMaxHealth = 100f;

    [SerializeField]
    private Slider healthSlider;

    private void OnEnable()
    {
        BossHealth.OnChangeBossHealth += ChangeHealth;

        BossHealth.OnInitialiseBossHealth += InitialiseHealth;
    }

    private void OnDisable()
    {
        BossHealth.OnChangeBossHealth -= ChangeHealth;

        BossHealth.OnInitialiseBossHealth -= InitialiseHealth;
    }

    private void InitialiseHealth(object sender, int maxHealth)
    {
        bossMaxHealth = maxHealth;
        ChangeHealth(this, maxHealth);
    }

    private void ChangeHealth(object sender, int newHealth)
    {
        float health = (float)newHealth / bossMaxHealth;
        healthSlider.value = health;
    }
}

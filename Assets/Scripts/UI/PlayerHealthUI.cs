using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private float playerMaxHealth = 100f;

    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private Image[] retryImages;

    private void OnEnable()
    {
        PlayerHealth.OnChangePlayerHealth += ChangeHealth;
        PlayerHealth.OnInitialisePlayerHealth += InitialiseHealth;
        PlayerManager.OnNewPlayerRetries += UpdateRetries;
    }

    private void OnDisable()
    {
        PlayerHealth.OnChangePlayerHealth -= ChangeHealth;
        PlayerHealth.OnInitialisePlayerHealth -= InitialiseHealth;
        PlayerManager.OnNewPlayerRetries -= UpdateRetries;
    }

    private void InitialiseHealth(object sender, int maxHealth)
    {
        playerMaxHealth = maxHealth;
        ChangeHealth(this, maxHealth);
    }

    private void ChangeHealth(object sender, int newHealth)
    {
        float health = (float)newHealth / playerMaxHealth;
        healthSlider.value = health;
    }

    private void UpdateRetries(object sender, int newRetries)
    {
        if ((newRetries < 0) || (newRetries >= 2))
        {
            return;
        }

        retryImages[newRetries].enabled = false;
    }
}

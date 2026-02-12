using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private float playerMaxHealth = 100f;

    [SerializeField]
    private Slider healthSlider;

    private void OnEnable()
    {
        PlayerHealth.OnChangePlayerHealth += ChangeHealth;
    }

    private void OnDisable()
    {
        PlayerHealth.OnChangePlayerHealth -= ChangeHealth;
    }

    private void ChangeHealth(object sender, int newHealth)
    {
        float health = (float)newHealth / playerMaxHealth;
        healthSlider.value = health;
    }
}

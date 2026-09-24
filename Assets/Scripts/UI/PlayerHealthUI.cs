using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private float playerMaxHealth = 100f;

    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private GameObject[] retryImages;

    [SerializeField]
    private GameObject rewindsUI;

    [SerializeField]
    private CanvasGroupFader healthFader;

    private void OnEnable()
    {
        PlayerHealth.OnChangePlayerHealth += ChangeHealth;
        PlayerHealth.OnInitialisePlayerHealth += InitialiseHealth;
        PlayerTimepiece.OnNewPlayerRetries += UpdateRetries;
        PlayerTimepiece.OnFirstTimeTimepiece += DisableRetryUI;

        TutorialFightManager.OnToggleHealthUI += ToggleHealthUI;
    }

    private void OnDisable()
    {
        PlayerHealth.OnChangePlayerHealth -= ChangeHealth;
        PlayerHealth.OnInitialisePlayerHealth -= InitialiseHealth;
        PlayerTimepiece.OnNewPlayerRetries -= UpdateRetries;
        PlayerTimepiece.OnFirstTimeTimepiece -= DisableRetryUI;

        TutorialFightManager.OnToggleHealthUI -= ToggleHealthUI;
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
        for (int i = 0; i < retryImages.Length; i++)
        {
            retryImages[i].SetActive(i < newRetries);
        }
    }

    private void DisableRetryUI()
    {
        rewindsUI.SetActive(false);
    }

    private void ToggleHealthUI(object sender, bool toggle)
    {
        healthFader.ToggleFade(toggle);
    }
}

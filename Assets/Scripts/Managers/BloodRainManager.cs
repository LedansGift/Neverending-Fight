using UnityEngine;
using UnityEngine.Rendering;

public class BloodRainManager : MonoBehaviour
{
    private bool rainActive = false;

    private int rainHealAmount = 5;
    private float rainHealTimer = 0f;
    private float rainHealDuration = 1f;
    private PlayerHealth playerHealth;

    [SerializeField]
    private ParticleSystem bloodRainEffect;

    [SerializeField]
    private Volume bloodRainVolume;

    private void Start()
    {
        Transform playerTransform = PlayerIdentifier.PlayerTransform;
        playerHealth = playerTransform.GetComponent<PlayerHealth>();

        //ActivateRain();
        DeactivateRain();
    }

    private void Update()
    {
        if (!rainActive)
        {
            return;
        }

        RainHeal();
    }

    private void RainHeal()
    {
        if (!playerHealth)
        {
            return;
        }

        rainHealTimer += Time.deltaTime;

        bloodRainEffect.transform.position = playerHealth.transform.position;

        if (rainHealTimer >= rainHealDuration)
        {
            rainHealTimer = 0f;

            playerHealth.Heal(rainHealAmount);
        }
    }

    private void ActivateRain()
    {
        bloodRainVolume.weight = 1f;
        bloodRainEffect.Play();

        rainActive = true;
    }

    private void DeactivateRain()
    {
        bloodRainVolume.weight = 0f;
        bloodRainEffect.Stop();

        rainActive = false;
    }
}

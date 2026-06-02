using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private bool pickupActive = false;
    private int healAmount = 10;
    private float pickupActiveDuration = 10f;
    private float pickupActiveTimer = 0f;
    private Collider pickupCollider;

    [SerializeField]
    private GameObject pickupVisual;

    private void Awake()
    {
        pickupCollider = GetComponent<Collider>();

        DespawnPickup();
        //SpawnPickup();
    }

    private void Update()
    {
        if (!pickupActive)
        {
            return;
        }

        pickupActiveTimer += Time.deltaTime;

        if (pickupActiveTimer >= pickupActiveDuration)
        {
            DespawnPickup();
        }
    }

    public void SpawnPickup()
    {
        pickupCollider.enabled = true;
        pickupVisual.SetActive(true);

        pickupActiveTimer = 0f;
        pickupActive = true;
    }

    private void DespawnPickup()
    {
        pickupCollider.enabled = false;
        pickupVisual.SetActive(false);

        pickupActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pickupActive)
        {
            return;
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            if (health.GetIsPlayer())
            {
                PlayerHealth playerHealth = health as PlayerHealth;
                playerHealth.Heal(healAmount);

                DespawnPickup();
            }
        }
    }
}

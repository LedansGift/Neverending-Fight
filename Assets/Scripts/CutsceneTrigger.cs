using UnityEngine;
using UnityEngine.Events;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent methodOnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Health>(out Health health))
        {
            if (health.GetIsPlayer() && health.GetComponent<PlayerManager>())
            {
                methodOnTrigger?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}

using Unity.Cinemachine;
using UnityEngine;

public class UIBossFollower : MonoBehaviour
{
    private Transform bossTransform;

    [SerializeField]
    private Vector2 positionOffset;

    private void Start()
    {
        //bossTransform = PlayerIdentifier.PlayerTransform;
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
        BossManager.OnNewBossForm += UpdateBossTarget;
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
        BossManager.OnNewBossForm -= UpdateBossTarget;
    }

    private void SetUIPosition()
    {
        if (!bossTransform)
        {
            return;
        }

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(bossTransform.position);

        transform.position = screenPosition + positionOffset;
    }

    private void UpdateBossTarget(object sender, BossFormManager newForm)
    {
        bossTransform = newForm.transform;
    }

    private void OnCameraUpdated(CinemachineBrain arg0)
    {
        SetUIPosition();
    }
}

using Unity.Cinemachine;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private float moveSpeed = 0.4f;

    protected Transform followTransform;

    protected virtual void Start()
    {
        SetTarget();
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    protected virtual void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    protected virtual void SetTarget()
    {
        Transform playerTransform = PlayerIdentifier.PlayerTransform;

        if (!playerTransform)
        {
            Debug.LogError("Player Not Found");
            return;
        }

        followTransform = playerTransform;
    }

    private void MoveTowardsFollowTransform()
    {
        if (!followTransform)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            followTransform.position,
            moveSpeed * Time.timeScale
        );
    }

    private void OnCameraUpdated(CinemachineBrain arg0)
    {
        MoveTowardsFollowTransform();
    }
}

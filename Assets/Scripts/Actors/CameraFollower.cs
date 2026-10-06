using Unity.Cinemachine;
using UnityEngine;

public class CameraFollower : ObjectFollower
{
    protected virtual void Start()
    {
        SetPlayerTarget();
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    protected virtual void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    protected override void Update() { }

    protected virtual void SetPlayerTarget()
    {
        Transform playerTransform = PlayerIdentifier.PlayerTransform;

        if (!playerTransform)
        {
            Debug.LogError("Player Not Found");
            return;
        }

        followTransform = playerTransform;
    }

    private void OnCameraUpdated(CinemachineBrain arg0)
    {
        MoveTowardsFollowTransform();
    }
}

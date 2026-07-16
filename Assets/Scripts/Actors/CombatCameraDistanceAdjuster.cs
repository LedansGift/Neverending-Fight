using Unity.Cinemachine;
using UnityEngine;

public class CombatCameraDistanceAdjuster : MonoBehaviour
{
    private float cameraDistanceTarget = 0f;
    private float adjustSpeed = 5f;

    private const float CAMERA_DISTANCE_MIN = 10f;
    private const float CAMERA_DISTANCE_MAX = 20f;

    //private const float BOSS_DISTANCE_MIN = 5f;
    private const float BOSS_DISTANCE_MAX = 50f;

    [SerializeField]
    private Transform playerFollower;

    [SerializeField]
    private Transform bossFollower;

    [SerializeField]
    private CinemachinePositionComposer positionComposer;

    private void Update()
    {
        AdjustComposerDistance();

        positionComposer.CameraDistance = Mathf.MoveTowards(
            positionComposer.CameraDistance,
            cameraDistanceTarget,
            adjustSpeed * Time.deltaTime
        );
    }

    private void AdjustComposerDistance()
    {
        float playerBossDistance = Vector3.Distance(playerFollower.position, bossFollower.position);

        float newComposerDistance = Mathf.Lerp(
            CAMERA_DISTANCE_MIN,
            CAMERA_DISTANCE_MAX,
            Mathf.Clamp01(playerBossDistance / BOSS_DISTANCE_MAX)
        );

        cameraDistanceTarget = newComposerDistance;
    }
}

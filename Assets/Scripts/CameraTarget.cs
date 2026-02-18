using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private InputManager input;

    private void Start()
    {
        input = InputManager.Instance;

        SetPlayerTarget();
    }

    private void Update()
    {
        SetTargetPosition();
    }

    private void SetPlayerTarget()
    {
        Transform playerTransform = PlayerIdentifier.PlayerTransform;

        if (!playerTransform)
        {
            Debug.LogError("Player Not Found");
            return;
        }

        PlayerManager playerManager = playerTransform.GetComponent<PlayerManager>();

        playerManager.SetMouseTarget(transform);
    }

    //Calculation for finding point of intersection of mouse and XZ plane by
    //Skilled Cookie on https://discussions.unity.com/t/making-the-player-face-the-direction-of-the-cursor/801532/3
    private void SetTargetPosition()
    {
        if (input.MousePosition == Vector2.zero)
        {
            return;
        }

        Vector3 point = Camera.main.ScreenToWorldPoint(
            new Vector3(input.MousePosition.x, input.MousePosition.y, 1)
        );

        float t = Camera.main.transform.position.y / (Camera.main.transform.position.y - point.y);

        Vector3 cameraTargetPosition = new Vector3(
            t * (point.x - Camera.main.transform.position.x) + Camera.main.transform.position.x,
            0f,
            t * (point.z - Camera.main.transform.position.z) + Camera.main.transform.position.z
        );

        transform.position = cameraTargetPosition;
    }
}

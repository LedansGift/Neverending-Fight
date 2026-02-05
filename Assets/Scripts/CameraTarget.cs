using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Transform playerTransform;
    private InputManager input;

    [SerializeField]
    private float cameraLookaheadRatio;

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;
        input = InputManager.Instance;

        if (!playerTransform)
        {
            GameObject standIn = new GameObject("PLAYER TRANSFORM STAND-IN");
            playerTransform = standIn.transform;
        }
    }

    private void Update()
    {
        //Debug.Log(input.MousePosition);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(input.MousePosition.x, input.MousePosition.y, cameraLookaheadRatio)
        );

        // Vector2 cameraTargetPosition =
        //     (mousePosition + (cameraLookaheadRatio - 1) * (Vector2)playerTransform.position)
        //     / cameraLookaheadRatio;

        Vector3 cameraTargetPosition =
            playerTransform.position + new Vector3(mousePosition.x, 0f, mousePosition.y);

        //Debug.Log(cameraTargetPosition);

        transform.position = cameraTargetPosition;
    }
}

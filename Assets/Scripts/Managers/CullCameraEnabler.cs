using UnityEngine;

public class CullCameraEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject cullCamera;

    private void Start()
    {
        CutsceneManager.Instance.OnCutsceneStart += EnableCullCamera;
        CutsceneManager.Instance.OnCutsceneEnd += DisableCullCamera;
    }

    private void OnDisable()
    {
        CutsceneManager.Instance.OnCutsceneStart -= EnableCullCamera;
        CutsceneManager.Instance.OnCutsceneEnd -= DisableCullCamera;
    }

    private void EnableCullCamera()
    {
        cullCamera?.SetActive(true);
    }

    private void DisableCullCamera()
    {
        cullCamera?.SetActive(false);
    }
}

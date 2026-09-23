using UnityEngine;

[ExecuteAlways]
public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private bool localRotation = false;

    [SerializeField]
    private float xRotation;

    [SerializeField]
    private float yRotation;

    [SerializeField]
    private float zRotation;

    private void Update()
    {
        if (localRotation)
        {
            transform.localEulerAngles +=
                new Vector3(xRotation, yRotation, zRotation) * Time.unscaledDeltaTime;
        }
        else
        {
            transform.eulerAngles +=
                new Vector3(xRotation, yRotation, zRotation) * Time.unscaledDeltaTime;
        }
    }
}

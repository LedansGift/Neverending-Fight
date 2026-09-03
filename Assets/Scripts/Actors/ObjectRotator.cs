using UnityEngine;

[ExecuteAlways]
public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private float xRotation;

    [SerializeField]
    private float yRotation;

    [SerializeField]
    private float zRotation;

    private void Update()
    {
        transform.eulerAngles +=
            new Vector3(xRotation, yRotation, zRotation) * Time.unscaledDeltaTime;
    }
}

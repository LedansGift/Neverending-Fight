using UnityEngine;

public class ChangebringerFellWingPositioner : MonoBehaviour
{
    [SerializeField]
    private Transform leftAttackTransform;

    [SerializeField]
    private Transform rightAttackTransform;

    [SerializeField]
    private Transform leftWingTransform;

    [SerializeField]
    private Transform rightWingTransform;

    public Transform GetLeftAttackTransform()
    {
        return leftAttackTransform;
    }

    public Transform GetRightAttackTransform()
    {
        return rightAttackTransform;
    }

    public Transform GetLeftWingTransform()
    {
        return leftWingTransform;
    }

    public Transform GetRightWingTransform()
    {
        return rightWingTransform;
    }
}

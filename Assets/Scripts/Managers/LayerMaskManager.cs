using UnityEngine;

public class LayerMaskManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask localAttackLayermask;

    private static LayerMask attackLayermask;

    private void Awake()
    {
        attackLayermask = localAttackLayermask;
    }

    public static LayerMask GetAttackLayerMask()
    {
        return attackLayermask;
    }
}

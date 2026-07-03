using UnityEngine;

[CreateAssetMenu(
    fileName = "Test Attack Pattern",
    menuName = "Boss Attack/Test Pattern",
    order = 99
)]
public class TestAttackPattern : ScriptableObject
{
    public TestAttackNode[] attackNodes;

    public TestAttackPattern empoweredPattern;
}

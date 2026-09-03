using UnityEngine;

public class BossTopicInitialiser : MonoBehaviour
{
    [SerializeField]
    private GameObject[] initialTopics;

    public void InitialiseTopics()
    {
        //spawn all initial topics via topic manager
        //Do not initialise again on phase restart
    }
}

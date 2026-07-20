using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    private BossForm activeArena = BossForm.CROSSROADS;

    //Used enum int of boss form to correspong to correct arena
    [SerializeField]
    private GameObject[] arenas;

    [SerializeField]
    private GameObject introColliderSet1;

    [SerializeField]
    private GameObject introColliderSet2;

    [SerializeField]
    private Animator introPlatformSet1;

    [SerializeField]
    private Animator introPlatformSet2;

    private void Awake()
    {
        if (!introColliderSet1 || !introColliderSet2)
        {
            return;
        }

        introColliderSet1.SetActive(true);
        introColliderSet2.SetActive(false);
    }

    public void SwitchArena(BossForm newBossBorm)
    {
        arenas[(int)activeArena]?.SetActive(false);
        activeArena = newBossBorm;
        arenas[(int)activeArena]?.SetActive(true);
    }

    public void RemoveFirstPlatformSet()
    {
        introColliderSet1.SetActive(false);
        introColliderSet2.SetActive(true);
        introPlatformSet1.SetTrigger("fall");
    }

    public void RemoveSecondPlatformSet()
    {
        introColliderSet2.SetActive(false);
        introPlatformSet2.SetTrigger("fall");
    }
}

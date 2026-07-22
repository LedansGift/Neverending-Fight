using System.Collections;
using UnityEngine;

public class FormChangeManager : MonoBehaviour
{
    public static FormChangeManager Instance { get; private set; }

    [SerializeField]
    private ArenaManager arenaManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ChangeBossForm(BossForm newForm)
    {
        StartCoroutine(FormChange(newForm));
    }

    private IEnumerator FormChange(BossForm newForm)
    {
        LoadingScreenUI.ToggleLoadingScreen(true);

        yield return new WaitForSeconds(2.5f);

        arenaManager.SwitchArena(newForm);
        //Set player position to be in set arena position

        LoadingScreenUI.ToggleLoadingScreen(false);
        yield return new WaitForSeconds(2.5f);

        BossManager.Instance.ActivateBossForm(newForm);
        BattleManager.Instance.TogglePlayer(true);
    }
}

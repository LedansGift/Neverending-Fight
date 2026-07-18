using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField]
    private PlayableDirector bossIntroCutscene;

    [SerializeField]
    private PlayableDirector firstTimeTimepieceCutscene;

    public static EventHandler<bool> OnPlayerToggle;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //if not first time doing gameplay level

        StartCoroutine(SceneStartPlayerEnable());
    }

    private void StartBossFight()
    {
        TogglePlayer(true);
        BossManager.Instance.ActivateBossForm(BossForm.CROSSROADS);
    }

    private IEnumerator SceneStartPlayerEnable()
    {
        yield return new WaitForSeconds(0.5f);
        TogglePlayer(true);
    }

    private void TogglePlayer(bool toggle)
    {
        OnPlayerToggle?.Invoke(this, toggle);
    }

    private void EnableWeakenedPlayer()
    {
        TogglePlayer(true);
    }

    public void PlayTimepieceCutscene()
    {
        TogglePlayer(false);

        CutsceneManager.Instance.StartCutscene(firstTimeTimepieceCutscene, EnableWeakenedPlayer);
    }

    public void PlayBossIntroCutscene()
    {
        TogglePlayer(false);

        CutsceneManager.Instance.StartCutscene(bossIntroCutscene, StartBossFight);
    }
}

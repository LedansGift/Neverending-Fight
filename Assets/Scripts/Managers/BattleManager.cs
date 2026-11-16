using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField]
    private bool firstTimeRunOverride = false;

    [SerializeField]
    private bool tutorialCompleteOverride = false;

    [SerializeField]
    private PlayableDirector bossIntroCutscene;

    [SerializeField]
    private PlayableDirector firstTimeTimepieceCutscene;

    [SerializeField]
    private GameObject timepieceCutsceneTrigger;

    public static Action OnWeakenPlayer;

    public static EventHandler<bool> OnPlayerToggle;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (firstTimeRunOverride)
        {
            PlayerOptions.SetTutorialCompleted(tutorialCompleteOverride);
        }
    }

    private void Start()
    {
        timepieceCutsceneTrigger.SetActive(!PlayerOptions.GetTutorialCompleted());

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

        if (PlayerOptions.GetTutorialCompleted())
        {
            TogglePlayer(true);
        }
        else
        {
            TextCutsceneUI.StartTextCutsceneEvent(0, TextCutsceneEnd);
        }
    }

    private void TextCutsceneEnd()
    {
        TogglePlayer(true);
    }

    private void EnableWeakenedPlayer()
    {
        //Set player health to be critically low
        OnWeakenPlayer?.Invoke();
        //Enable timepiece visual
        TogglePlayer(true);
    }

    public void TogglePlayer(bool toggle)
    {
        OnPlayerToggle?.Invoke(this, toggle);
    }

    public void PlayTimepieceCutscene()
    {
        TogglePlayer(false);

        //CutsceneManager.Instance.StartCutscene(firstTimeTimepieceCutscene, EnableWeakenedPlayer);
        TextCutsceneUI.StartTextCutsceneEvent(1, EnableWeakenedPlayer);
    }

    public void PlayBossIntroCutscene()
    {
        TogglePlayer(false);

        CutsceneManager.Instance.StartCutscene(bossIntroCutscene, StartBossFight);
    }
}

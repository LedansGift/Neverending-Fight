using System;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    private bool cutsceneActive = false;
    private bool pauseActive = false;
    private int activeFader;

    private PauseManager pauseManager;

    [SerializeField]
    private CanvasGroupFader rootMenuFader;

    [SerializeField]
    private CanvasGroupFader[] subMenuFaders;

    private void Awake()
    {
        rootMenuFader.SetCanvasGroupAlpha(0f);
        rootMenuFader.ToggleBlockRaycasts(false);

        foreach (CanvasGroupFader menuFader in subMenuFaders)
        {
            menuFader.SetCanvasGroupAlpha(0f);
            menuFader.ToggleBlockRaycasts(false);
        }
    }

    private void Start()
    {
        CutsceneManager.Instance.OnCutsceneStart += SetCutsceneStarted;
        CutsceneManager.Instance.OnCutsceneEnd += SetCutsceneEnded;
    }

    private void OnEnable()
    {
        PauseManager.OnPauseGame += ToggleUI;
    }

    private void OnDisable()
    {
        PauseManager.OnPauseGame -= ToggleUI;
        CutsceneManager.Instance.OnCutsceneStart -= SetCutsceneStarted;
        CutsceneManager.Instance.OnCutsceneEnd -= SetCutsceneEnded;
    }

    public void SwitchSubmenu(int subMenu)
    {
        if (activeFader == subMenu)
        {
            return;
        }

        ToggleSubMenu(activeFader, false);

        activeFader = subMenu;

        ToggleSubMenu(activeFader, true);
    }

    public void ContinueGame()
    {
        if (!pauseManager || !pauseActive)
        {
            return;
        }

        ToggleUI(pauseManager, false);
        pauseManager.TryPauseGame();
    }

    private void ToggleSubMenu(int index, bool toggle)
    {
        subMenuFaders[index].ToggleFade(toggle);
        subMenuFaders[index].ToggleBlockRaycasts(toggle);
    }

    private void ToggleUI(object sender, bool toggle)
    {
        if (!pauseManager)
        {
            pauseManager = sender as PauseManager;
        }

        if (cutsceneActive)
        {
            return;
        }

        if (toggle == pauseActive)
        {
            return;
        }

        pauseActive = toggle;

        if (toggle)
        {
            activeFader = 0;

            ToggleSubMenu(activeFader, true);

            rootMenuFader.ToggleFade(true);
            rootMenuFader.ToggleBlockRaycasts(true);
        }
        else
        {
            rootMenuFader.ToggleFade(false);
            rootMenuFader.ToggleBlockRaycasts(false);

            ToggleSubMenu(activeFader, false);
        }
    }

    private void SetCutsceneStarted()
    {
        cutsceneActive = true;
    }

    private void SetCutsceneEnded()
    {
        cutsceneActive = false;
    }
}

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader mainMenuFader;

    [SerializeField]
    private CanvasGroupFader optionsFader;

    [SerializeField]
    private PlayableDirector introCutsceneDirector;

    public void StartGame()
    {
        // Test if tutorial has been played or not. If yes, go straight to gameplay level


        mainMenuFader.ToggleFade(false);
        mainMenuFader.ToggleBlockRaycasts(false);

        //Play Cutscene
        introCutsceneDirector.Play();
        introCutsceneDirector.stopped += OnCutsceneFinished;
    }

    private void OnDisable()
    {
        introCutsceneDirector.stopped -= OnCutsceneFinished;
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        LoadGameplayLevel();
    }

    public void ToggleOptionsMenu(bool toggle)
    {
        optionsFader.ToggleFade(toggle);
        optionsFader.ToggleBlockRaycasts(toggle);
    }

    private void LoadGameplayLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}

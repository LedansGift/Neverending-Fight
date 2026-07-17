using System;
using System.Collections;
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

    [SerializeField]
    private MusicTrack mainMenuMusic;

    private void Start()
    {
        InputManager.Instance.TogglePlayerControlActive(false);
        AudioManager.SetMusicTrack(mainMenuMusic);
    }

    public void StartGame()
    {
        // Test if tutorial has been played or not. If yes, go straight to gameplay level

        mainMenuFader.ToggleFade(false);
        mainMenuFader.ToggleBlockRaycasts(false);

        InputManager.Instance.TogglePlayerControlActive(true);

        CutsceneManager.Instance.StartCutscene(introCutsceneDirector, OnCutsceneFinished);
    }

    private void OnCutsceneFinished()
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

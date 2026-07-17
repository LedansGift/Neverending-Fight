using System;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    private Action onCutsceneEnd;
    private PlayableDirector activeDirector;
    public static CutsceneManager Instance { get; private set; }

    public Action OnCutsceneStart;
    public Action OnCutsceneEnd;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void EndCutscene(PlayableDirector director)
    {
        director.stopped -= EndCutscene;

        activeDirector = null;

        if (onCutsceneEnd != null)
        {
            onCutsceneEnd();
        }

        OnCutsceneEnd?.Invoke();
    }

    public void StartCutscene(PlayableDirector playableDirector, Action onCutsceneEnd)
    {
        playableDirector.Play();
        playableDirector.stopped += EndCutscene;

        this.onCutsceneEnd = onCutsceneEnd;

        activeDirector = playableDirector;

        OnCutsceneStart?.Invoke();
    }

    public void SkipActiveCutscene()
    {
        if (!activeDirector)
        {
            return;
        }

        activeDirector.time = activeDirector.duration - 0.001f;
        activeDirector.Play();
    }
}

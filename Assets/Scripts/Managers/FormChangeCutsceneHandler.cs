using UnityEngine;
using UnityEngine.Playables;

public class FormChangeCutsceneHandler : MonoBehaviour
{
    private BossForm bossFormChange;
    private ArenaManager arenaManager;

    [SerializeField]
    private PlayableDirector cutsceneDirector;

    public PlayableDirector GetCutsceneDirector()
    {
        return cutsceneDirector;
    }

    public void InitialiseCutsceneHandler(BossForm bossFormChange, ArenaManager arenaManager)
    {
        this.bossFormChange = bossFormChange;
        this.arenaManager = arenaManager;
    }

    public void SwitchArena()
    {
        arenaManager.SwitchArena(bossFormChange);
    }

    public void ToggleActorCullOverlay(bool toggle)
    {
        CameraRenderOverlay.ToggleOverlay(toggle);
    }

    public void HideAllBosses()
    {
        BossManager.Instance.HideAllBosses();
    }

    public void ShowBoss()
    {
        BossManager.Instance.ShowBoss(bossFormChange);
    }
}

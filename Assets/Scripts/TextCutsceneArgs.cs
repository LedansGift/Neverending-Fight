using System;

public class TextCutsceneArgs
{
    public TextCutsceneArgs(int cutsceneIndex, Action onCutsceneFinished)
    {
        this.cutsceneIndex = cutsceneIndex;
        this.onCutsceneFinished = onCutsceneFinished;
    }

    public int cutsceneIndex;
    public Action onCutsceneFinished;
}

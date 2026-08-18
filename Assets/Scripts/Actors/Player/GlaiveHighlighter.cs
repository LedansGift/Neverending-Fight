using UnityEngine;

public class GlaiveHighlighter : MonoBehaviour
{
    private bool alterHighlight = false;
    private float highlightValue;
    private float targetHighlightValue;

    private const float HIGHLIGHT_ALTER_SPEED = 30f;

    private const float ACTIVE_HIGHLIGHT_VALUE = 1f;
    private const float INACTIVE_HIGHLIGHT_VALUE = -1f;

    [SerializeField]
    private MeshRenderer[] highlightMeshes;

    private void Start()
    {
        highlightValue = INACTIVE_HIGHLIGHT_VALUE;
        SetMeshHighlight(highlightValue);
    }

    private void Update()
    {
        if (!alterHighlight)
        {
            return;
        }

        AlterHighlight();
    }

    private void AlterHighlight()
    {
        highlightValue = Mathf.MoveTowards(
            highlightValue,
            targetHighlightValue,
            HIGHLIGHT_ALTER_SPEED * Time.deltaTime
        );

        if (highlightValue == targetHighlightValue)
        {
            alterHighlight = false;
        }

        SetMeshHighlight(highlightValue);
    }

    private void SetMeshHighlight(float newValue)
    {
        foreach (MeshRenderer renderer in highlightMeshes)
        {
            renderer.material.SetFloat("_Y_Segment_Blur", newValue);
        }
    }

    public void ActivateHighlight()
    {
        targetHighlightValue = ACTIVE_HIGHLIGHT_VALUE;

        if (highlightValue == targetHighlightValue)
        {
            return;
        }

        alterHighlight = true;
    }

    public void DeactiveHighlight()
    {
        targetHighlightValue = INACTIVE_HIGHLIGHT_VALUE;

        if (highlightValue == targetHighlightValue)
        {
            return;
        }

        alterHighlight = true;
    }
}

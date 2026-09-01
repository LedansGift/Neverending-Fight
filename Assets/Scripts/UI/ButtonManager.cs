using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private SFXObject hoverSFX;

    [SerializeField]
    private SFXObject clickSFX;

    public void OnHoverOver()
    {
        if (!hoverSFX)
        {
            return;
        }

        hoverSFX.PlaySFX(Camera.main.transform.position, false);
    }

    public void OnClick()
    {
        if (!clickSFX)
        {
            return;
        }

        clickSFX.PlaySFX(Camera.main.transform.position, false);
    }
}

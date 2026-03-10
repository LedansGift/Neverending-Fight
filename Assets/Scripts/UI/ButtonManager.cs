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

        AudioManager.PlaySFX(hoverSFX, Camera.main.transform.position, false);
    }

    public void OnClick()
    {
        if (!clickSFX)
        {
            return;
        }

        AudioManager.PlaySFX(clickSFX, Camera.main.transform.position, false);
    }
}

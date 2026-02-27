using UnityEngine;

public class AbsorbFlameVisual : MonoBehaviour
{
    private bool flameFade = false;
    private float fadeDuration = 1f;
    private float fadeTimer = 0f;
    private float maxFlameSize = 4f;

    [SerializeField]
    private Transform flameVisual;
    private Material flameMaterial;

    private void Start()
    {
        flameVisual.localScale = Vector3.zero;
        flameMaterial = flameVisual.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);

        if (flameFade)
        {
            FadeFlame();
        }
    }

    private void FadeFlame()
    {
        fadeTimer += Time.deltaTime;
        float fadeLerp = fadeTimer / fadeDuration;

        if (fadeLerp >= 1f)
        {
            fadeLerp = 1f;
            flameFade = false;
            flameVisual.localScale = Vector3.zero;
            flameMaterial.SetFloat("_Dissolve", 0f);
            return;
        }

        flameMaterial.SetFloat("_Dissolve", fadeLerp);
    }

    public void SetVisualSize(float size)
    {
        size *= maxFlameSize;
        flameVisual.localScale = new Vector3(size, size, size);
    }

    public void StartBuffEffect(float buffDuration)
    {
        fadeDuration = buffDuration;
        fadeTimer = 0f;
        flameFade = true;
    }
}

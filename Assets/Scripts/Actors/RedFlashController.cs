using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedFlashController : MonoBehaviour
{
    private float flashTime = 0f;
    private float flashDuration = 0.5f;

    private float flashVisibleValue = 0f;

    private float flashInvisibleValue = 20f;

    private List<Material> flashMaterials = new List<Material>();

    [SerializeField]
    private int[] flashRendererMaterials;

    [SerializeField]
    private AnimationCurve flashCurve;

    [SerializeField]
    private Renderer flashRenderer;

    [SerializeField]
    private Health health;

    private void Start()
    {
        for (int i = 0; i < flashRenderer.materials.Length; i++)
        {
            if (flashRendererMaterials.Contains(i))
            {
                flashMaterials.Add(flashRenderer.materials[i]);
            }
        }

        flashTime = flashDuration;

        SetMaterialFloats(0f);
    }

    private void Update()
    {
        if (flashTime < flashDuration)
        {
            SetFlashVisibility();
        }
    }

    private void SetFlashVisibility()
    {
        flashTime += Time.deltaTime;

        float flashLerp = flashCurve.Evaluate(flashTime / flashDuration);

        SetMaterialFloats(flashLerp);
    }

    private void SetMaterialFloats(float flashLerp)
    {
        foreach (Material flashMat in flashMaterials)
        {
            flashMat.SetFloat(
                "_Edge_Fade",
                Mathf.Lerp(flashInvisibleValue, flashVisibleValue, flashLerp)
            );
        }
    }

    private void OnEnable()
    {
        health.OnTakeDamage += PerformFlash;
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= PerformFlash;
    }

    private void PerformFlash()
    {
        flashTime = 0f;
    }
}

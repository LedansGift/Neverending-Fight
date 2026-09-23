using UnityEngine;

public class TransformShrinkScaler : MonoBehaviour
{
    private bool alterScale = false;

    private float scaleValue;
    private float targetScaleValue;
    private float initialScaleValue;

    [SerializeField]
    private float yShrinkModifier = 0f;

    [SerializeField]
    private float shrinkSpeed = 2f;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private AnimationCurve lerpCurve;

    private void Start()
    {
        initialScaleValue = targetTransform.localScale.x;
    }

    private void Update()
    {
        if (!alterScale)
        {
            return;
        }

        AdjustScale();
    }

    private void AdjustScale()
    {
        scaleValue += Time.deltaTime * shrinkSpeed;

        if (scaleValue >= 1f)
        {
            scaleValue = 1f;
            alterScale = false;
        }

        float lerpValue = lerpCurve.Evaluate(scaleValue);

        float newScale = Mathf.Lerp(1f - targetScaleValue, targetScaleValue, lerpValue);

        targetTransform.localScale = new Vector3(
            newScale,
            1f - ((1f - newScale) * yShrinkModifier),
            newScale
        );
    }

    public void ShrinkTransform()
    {
        scaleValue = 0f;
        targetScaleValue = 0f;
        alterScale = true;
    }

    public void UnshrinkTransform()
    {
        scaleValue = 0f;
        targetScaleValue = initialScaleValue;
        alterScale = true;
    }
}

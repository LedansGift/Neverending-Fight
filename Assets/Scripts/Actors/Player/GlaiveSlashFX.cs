using UnityEngine;

public class GlaiveSlashFX : MonoBehaviour
{
    private bool slashing = false;
    private bool followSlashing = false;
    private int particleIndex = 0;
    private float slashTimer = 0f;
    private float followSlashTimer = 0f;
    private float slashSpeed = 1.5f;

    [SerializeField]
    private GameObject slashFX;

    [SerializeField]
    private ParticleSystem slashParticleSpray;

    [SerializeField]
    private GameObject slashFollowFX;

    [SerializeField]
    private ParticleSystem slashFollowParticles;

    [SerializeField]
    private ParticleSystem[] slashParticles;

    [SerializeField]
    private AnimationCurve slashDissolve;

    private Material slashShader;
    private Material slashFollowShader;

    private void Start()
    {
        slashShader = slashFX.GetComponent<MeshRenderer>().material;
        slashFollowShader = slashFollowFX.GetComponent<MeshRenderer>().material;
        slashFX.SetActive(false);
        slashFollowFX.SetActive(false);

        particleIndex = 0;
        foreach (ParticleSystem particle in slashParticles)
        {
            particle.transform.SetParent(null);
        }
    }

    private void Update()
    {
        PerformSlash();
        PerformFollowSlash();
    }

    private void PerformSlash()
    {
        if (slashing)
        {
            slashTimer += slashSpeed * Time.deltaTime;

            if (slashTimer >= 1f)
            {
                slashTimer = 1f;
                slashing = false;
            }

            slashShader.SetFloat("_Dissolve", slashDissolve.Evaluate(slashTimer));
        }
    }

    private void PerformFollowSlash()
    {
        if (followSlashing)
        {
            followSlashTimer += slashSpeed * Time.deltaTime;

            if (followSlashTimer >= 1f)
            {
                followSlashTimer = 1f;
                followSlashing = false;
            }

            slashFollowShader.SetFloat("_Dissolve", slashDissolve.Evaluate(followSlashTimer));
        }
    }

    public void GlaiveSlash()
    {
        slashFX.SetActive(false);
        slashFX.SetActive(true);
        slashing = true;
        slashTimer = 0f;
        slashParticleSpray.Play();
    }

    public void GlaiveFollowSlash()
    {
        slashFollowFX.SetActive(false);
        slashFollowFX.SetActive(true);
        followSlashing = true;
        followSlashTimer = 0f;
        slashFollowParticles.Play();
    }

    public void SpawnHitParticles(Vector3 hitPosition)
    {
        slashParticles[particleIndex].transform.position = hitPosition;
        slashParticles[particleIndex].gameObject.SetActive(false);
        slashParticles[particleIndex].gameObject.SetActive(true);
        particleIndex++;

        if (particleIndex >= slashParticles.Length)
        {
            particleIndex = 0;
        }
    }
}

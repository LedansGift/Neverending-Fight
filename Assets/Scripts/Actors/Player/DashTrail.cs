using UnityEngine;

public class DashTrail : MonoBehaviour
{
    private bool trailFade = false;
    private bool meshFade = false;

    [SerializeField]
    private bool worldSpaceMeshes = true;

    private float fadeTimer = 0f;

    [SerializeField]
    private float fadeSpeed = 25f;

    [SerializeField]
    private float fadeTrailGoal = 10f;

    [SerializeField]
    private float fadeGoal = 25f;

    [SerializeField]
    private GameObject dashParticles;

    [SerializeField]
    private GameObject meshHolder;

    [SerializeField]
    private MeshRenderer[] dashMeshes;

    [SerializeField]
    private SkinnedMeshRenderer[] championMeshes;

    [SerializeField]
    private TrailRenderer[] championTrails;

    private void Start()
    {
        if (worldSpaceMeshes)
        {
            meshHolder.transform.SetParent(null);
        }
    }

    private void Update()
    {
        if (meshFade)
        {
            TrailFade();
        }
    }

    private void TrailFade()
    {
        fadeTimer += Time.deltaTime * fadeSpeed;

        float fadeSlowdown = Mathf.Pow(fadeTimer / fadeGoal, 3f);

        foreach (MeshRenderer renderer in dashMeshes)
        {
            renderer.material.SetFloat("_Edge_Fade", fadeTimer * fadeSlowdown);
        }

        if (trailFade && (fadeTimer > fadeTrailGoal))
        {
            trailFade = false;
            foreach (TrailRenderer trail in championTrails)
            {
                trail.emitting = false;
            }
        }

        if (fadeTimer > fadeGoal)
        {
            meshFade = false;
        }
    }

    private void SpawnTrail()
    {
        for (int i = 0; i < championMeshes.Length; i++)
        {
            MeshRenderer dashMesh = dashMeshes[i];
            dashMesh.transform.SetPositionAndRotation(
                championMeshes[i].transform.position,
                transform.rotation
            );

            dashMesh.material.SetFloat("_Edge_Fade", 0f);

            MeshFilter mf = dashMesh.GetComponent<MeshFilter>();

            Mesh mesh = new Mesh();
            championMeshes[i].BakeMesh(mesh);

            mf.mesh = mesh;
        }

        fadeTimer = 0f;
        trailFade = true;
        meshFade = true;
    }

    public void ActivateDashTrail(Vector2 movementDirection)
    {
        dashParticles.SetActive(false);

        dashParticles.transform.rotation = Quaternion.LookRotation(
            new Vector3(movementDirection.x, 0f, movementDirection.y)
        );

        dashParticles.SetActive(true);

        foreach (TrailRenderer trail in championTrails)
        {
            trail.Clear();
            trail.emitting = true;
        }

        SpawnTrail();
    }
}

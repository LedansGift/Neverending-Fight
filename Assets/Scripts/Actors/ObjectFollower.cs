using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField]
    private bool followActive = true;

    [SerializeField]
    protected float moveSpeed = 0.4f;

    [SerializeField]
    protected Transform followTransform;

    protected virtual void Update()
    {
        MoveTowardsFollowTransform();
    }

    protected void MoveTowardsFollowTransform()
    {
        if (!followActive || !followTransform)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            followTransform.position,
            moveSpeed * Time.timeScale
        );
    }

    public void SetFollowTarget(Transform newTarget)
    {
        followTransform = newTarget;
    }

    public void ToggleFollow(bool toggle)
    {
        followActive = toggle;
    }
}

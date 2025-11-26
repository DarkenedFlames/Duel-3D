using UnityEngine;

[RequireComponent(typeof(SpawnContext))]
public class MHoming : MonoBehaviour
{
    [Header("Homing Settings")]
    [Tooltip("The global character runtime set for targeting.")]
    public CharacterSet allCharacters;

    [Tooltip("Whether or not the source Character is targeted for homing.")]
    public bool targetsSource = false;

    [Tooltip("The rate (degrees/second) at which the region turns towards its target.")]
    public float TurnRate = 360f;

    [Tooltip("The speed (meters/second) at which the region moves.")]
    public float Speed = 10f;

    [Tooltip("The distance (meters) at which the region may reacquire homing targets.")]
    public float HomingDistance = 20f;

    Character currentTarget;

    void Update()
    {
        if (currentTarget == null)
        {
            Character best;
            float trueDistance;
            GameObject ownerObject = GetComponent<SpawnContext>().Owner;
            
            if (ownerObject != null && !targetsSource && ownerObject.TryGetComponent(out Character character))
            {
                best = allCharacters.GetClosestExcluding(transform.position, character, out float distance);
                trueDistance = distance;
            }
            else
            {
                best = allCharacters.GetClosest(transform.position, out float distance);
                trueDistance = distance;
            }
            
            if (best != null && trueDistance <= HomingDistance) currentTarget = best;
            else currentTarget = null;
        }
        else
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.forward = Vector3.RotateTowards(
                transform.forward,
                direction,
                TurnRate * Mathf.Deg2Rad * Time.deltaTime,
                0f
            );

            if (Vector3.Distance(transform.position, currentTarget.transform.position) > HomingDistance)
                currentTarget = null;
        }

        transform.position += Speed * Time.deltaTime * transform.forward;
    }
}

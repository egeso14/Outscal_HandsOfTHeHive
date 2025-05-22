using UnityEngine;
/// <summary>
/// current implementation doesn't slow down while reaching the target
/// </summary>
public class AlignBehavior : ISteeringBehavior
{
    protected Transform rootTransform;
    protected Vector3 target;
    protected float angularSpeed;
    protected float rotationConstraintX;
    protected float rotationConstraintZ;
    protected bool isCompleted;

    public AlignBehavior(Transform rootTransform, float angularSpeed, float rotationConstraintX, float rotationConstraintZ)
    {
        this.rootTransform = rootTransform;
        this.angularSpeed = angularSpeed;
        this.rotationConstraintX = rotationConstraintX;
        this.rotationConstraintZ = rotationConstraintZ;
        target = Vector3.zero;
    }

    public virtual SteeringOutput GetSteering()
    {
        SteeringOutput output = new SteeringOutput();
        output.angular = DetermineRotation();
        return output;
    }

    public void DebugBehavior()
    { }
    public bool IsCompleted()
    {
        return isCompleted;
    }

    private Quaternion DetermineRotation()
    {
        // clamp the target rotation
        Quaternion fullRotation = Quaternion.FromToRotation(Vector3.forward, target);
        Vector3 eulerAngles = fullRotation.eulerAngles;
        eulerAngles.x = NormalizeAngle(eulerAngles.x);
        eulerAngles.z = NormalizeAngle(eulerAngles.z);
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, -1 * rotationConstraintX, rotationConstraintX);
        eulerAngles.z = Mathf.Clamp(eulerAngles.z, -1 * rotationConstraintZ, rotationConstraintZ);

        Quaternion clampedFullRotation = Quaternion.Euler(eulerAngles);
        float angle = Quaternion.Angle(rootTransform.rotation, clampedFullRotation);

        // determine how much of this rotation angle we can achieve this cycle
        float k = Mathf.Min(angularSpeed * Time.fixedDeltaTime / angle, 1);
        Quaternion actualRotation = Quaternion.Slerp(rootTransform.rotation, clampedFullRotation, k);
        return Quaternion.Inverse(rootTransform.rotation) * actualRotation;
    }
    private float NormalizeAngle(float angle)
    {
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;
        return angle;
    }
}

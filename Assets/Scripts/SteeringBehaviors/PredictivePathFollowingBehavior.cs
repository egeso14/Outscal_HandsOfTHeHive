using Unity.VisualScripting;
using UnityEngine;
public interface IPath
{
    public float GetParameter(Vector3 position, float lastParam);
    public Vector3 GetPosition(float param);
    public bool IsEndOfPath(float param);
    public void DebugPath();
}
public class PredictivePathFollowingBehavior : ISteeringBehavior
{
    private float pathOffset;
    private float currentParam; // the current index with the list of node positions
    private IPath path;
    private float predictionTime;
    private Rigidbody rigidbody;
    private Transform rootTransform;
    private SeekBehavior seekBehavior;
    private SteeringOutput lastOutput;
 


    public PredictivePathFollowingBehavior(IPath path, float pathOffset, float predictionTime, Rigidbody rigidbody, Transform rootTransform, float maxAcceleration)
    {
        this.path = path;
        this.pathOffset = pathOffset;
        this.predictionTime = predictionTime;
        this.rigidbody = rigidbody;
        this.rootTransform = rootTransform;
        seekBehavior = new SeekBehavior(rootTransform, Vector3.zero, maxAcceleration);
        currentParam = 0; // Start at the beginning of the path
        lastOutput = new SteeringOutput();
    }

    public SteeringOutput GetSteering()
    {
        Vector3 predictedPosition = rootTransform.position + rigidbody.linearVelocity * predictionTime;
        currentParam = path.GetParameter(predictedPosition, currentParam);
        Vector3 nextTarget = path.GetPosition(currentParam + pathOffset);
        seekBehavior.SetTarget(nextTarget);
        lastOutput = seekBehavior.GetSteering();
        return lastOutput;
    }

    public void DebugBehavior()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(rootTransform.position, lastOutput.linear.normalized + rootTransform.position);
        path.DebugPath();
    }

    public bool IsCompleted()
    {
        return path.IsEndOfPath(currentParam);
    }

}

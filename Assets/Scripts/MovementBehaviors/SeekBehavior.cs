using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SeekBehavior: ISteeringBehavior
{
    private float maxAcceleration;
    private Vector3 target;
    private Transform transform;
    private bool isCompleted;
    public SeekBehavior(Transform transform, Vector3 target, float maxAcceleration)
    {
        this.transform = transform;
        this.target = target;
        this.maxAcceleration = maxAcceleration;
    }

    public SteeringOutput GetSteering()
    {
        var output = new SteeringOutput();
        output.linear = (target - transform.position).normalized * maxAcceleration;
        return output;
    }

    public void DebugBehavior()
    {

    }

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }
    
}

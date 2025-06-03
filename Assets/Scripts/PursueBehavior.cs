using UnityEngine;
using UnityEngine.Rendering;

public class PursueBehavior : ISteeringBehavior
{
    private Rigidbody targetBody;
    private Transform targetTransform;
    private Rigidbody myBody;
    private Transform rootTransform;
    private ArriveBehavior arriveBehavior;
    private float maxPredictionTime;
    private float maxAcceleration;
    private SteeringOutput lastOutput;
    private float sqrApproachDistance;


    public PursueBehavior(Rigidbody targetBody, Transform targetTransform, Rigidbody myBody, Transform rootTransform,
                          float maxPredictionTime, float maxAcceleration, float approachDistance, float maxSpeed,
                          float stopRadius, float slowRadius, float timeToReach)
    {
        this.targetBody = targetBody;
        this.rootTransform = rootTransform;
        this.targetTransform = targetTransform;
        this.myBody = myBody;
        this.maxPredictionTime = maxPredictionTime;
        this.maxAcceleration = maxAcceleration;
        this.sqrApproachDistance = MathU.Square(approachDistance);
        this.arriveBehavior = new ArriveBehavior(myBody, rootTransform, rootTransform.position, maxAcceleration, maxSpeed, 
                                                    stopRadius, slowRadius, timeToReach);
        this.lastOutput = new SteeringOutput();
    }

    public SteeringOutput GetSteering()
    {
        Vector3 direction = targetTransform.position - rootTransform.position;
        float distance = direction.magnitude;

        float speed = myBody.linearVelocity.magnitude;
        float prediction;
        if (speed <= distance / maxPredictionTime)
        {
            prediction = maxPredictionTime;
        }
        else
        {
            prediction = distance / speed;
        }

        Vector3 target = targetTransform.position + targetBody.linearVelocity * prediction;
        arriveBehavior.SetTarget(target);
        lastOutput = arriveBehavior.GetSteering();
        return lastOutput;
    }

    public bool IsCompleted()
    {
        if ((targetTransform.position - rootTransform.position).sqrMagnitude < sqrApproachDistance)
        {
            return true;
        }
        return false;
        
    }
    public void DebugBehavior()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rootTransform.position, rootTransform.position + lastOutput.linear.normalized);
        
    }



}

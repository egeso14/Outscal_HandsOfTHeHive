using System.Collections.Generic;
using UnityEngine;

public class WanderAroundPointBehavior : BlendedSteering
{
    private float maxDistance;
    private float centerSeekMaxWeight;
    private Vector3 centerPoint;
    private WanderBehavior wanderBehavior;
    private SeekBehavior centerSeekBehavior;
    private Transform rootTransform;
    private SteeringOutput lastOutput;
    public WanderAroundPointBehavior(float maxDistance, Vector3 centerPoint, Transform rootTransform, float maxAcceleration,
                                    float wanderBehaviorRadius, float wanderBehaviorCircleOffset, float wanderBehaviorRate,
                                    float centerSeekMaxWeight, Rigidbody body) : base(maxAcceleration)
    {
        this.centerPoint = centerPoint;
        this.maxDistance = maxDistance;
        this.rootTransform = rootTransform;
        this.centerSeekMaxWeight = centerSeekMaxWeight;

        wanderBehavior = new WanderBehavior(wanderBehaviorRadius, wanderBehaviorCircleOffset, wanderBehaviorRate, maxAcceleration, rootTransform, body);
        centerSeekBehavior = new SeekBehavior(rootTransform, centerPoint, maxAcceleration);
        // now we determine the weights of these behaviors
        var seekWeight = CalculateSeekWeight();
        behaviors = new List<BehaviorAndWeight>{ new BehaviorAndWeight(wanderBehavior, 1),
                                                new BehaviorAndWeight(centerSeekBehavior, 0)};
        lastOutput = new SteeringOutput();
    }

    public void SetMaxDistance(float newMaxDistance)
    {
        maxDistance = newMaxDistance;
    }

    private float CalculateSeekWeight()
    {
        float sqrDistance = (rootTransform.position - centerPoint).sqrMagnitude;
        float k = Mathf.InverseLerp(MathU.Square(maxDistance/2), MathU.Square(maxDistance), sqrDistance);
        k = Mathf.Clamp(k, 0, centerSeekMaxWeight);
        return k;
    }

    public override SteeringOutput GetSteering()
    {
        var seekWeight = CalculateSeekWeight();
        SetWeight(seekWeight, centerSeekBehavior);
        SetWeight(1 - seekWeight, wanderBehavior);
        lastOutput = base.GetSteering();
        return lastOutput;
    }

    public override void DebugBehavior()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rootTransform.position, lastOutput.linear.normalized + rootTransform.position);
    }


}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class does 3 things:
///     - defines and initializes the steeringBehaviors that compose the underlying prioritySteering class
///     - dynamically changes the wander radius of the WanderAroundPoint behavior to account for the density of bees around it.
///     - runs a rotation steering behavior which takes as input the linear acceleration from the steeringOutput;
/// </summary>
public class BuzzingBehavior : PrioritySteering
{
    SwarmParameters swarmParams;
    // composed of 3 behaviors for priority steering
    private ObstacleAndWallAvoidanceBehavior obstacleAvoidance;
    private WanderAroundPointBehavior wanderAroundPoint;
    private CollisionAvoidanceBehavior collisionAvoidance;
    private WanderBehavior testWanderBehavior;
    // and one behavior for the rotation
    private LookWhereYouAreGoingBehavior lookWhereYouAreGoing;
    private Vector3 centerPoint;
    private float beeDensityPerUnitRadius;
    private float currentBuzzingRadius;
    public BuzzingBehavior(Vector3 centerPoint, Transform rootTransform, Rigidbody rigidBody) 
    {

        swarmParams = SwarmParameters.Instance;
        Debug.Assert(swarmParams != null);
        this.centerPoint = centerPoint;
        LayerMask obstacleAvoidanceLayerMask = LayerMask.GetMask("FlyZoneBoundaries", "Obstacle");
        LayerMask collisionAvoidanceLayerMask = LayerMask.GetMask("Bee");

        beeDensityPerUnitRadius = swarmParams.buzz_beeDensityPerUnitRadius;
        currentBuzzingRadius = swarmParams.buzz_baseMaxDistanceForWanderAP;

        obstacleAvoidance = new ObstacleAndWallAvoidanceBehavior(rootTransform, rigidBody, swarmParams.obs_maxAcceleration,
                                                                swarmParams.obs_avoidDistance, swarmParams.obs_rayLength,
                                                                 obstacleAvoidanceLayerMask);
        wanderAroundPoint = new WanderAroundPointBehavior(currentBuzzingRadius, centerPoint, rootTransform,
                                                            swarmParams.buzz_maxAcceleration, swarmParams.wander_radius,
                                                            swarmParams.wander_circleOffset, swarmParams.wander_rate, swarmParams.buzz_wanderAP_centerSeekMaxWeight, rigidBody);
        collisionAvoidance = new CollisionAvoidanceBehavior(rootTransform, rigidBody, swarmParams.buzz_maxAcceleration * 5,
                                                            swarmParams.colAvoidance_detectionRadius, swarmParams.colAvoidance_beeColliderRadius,
                                                            collisionAvoidanceLayerMask);
        testWanderBehavior = new WanderBehavior(swarmParams.wander_radius, swarmParams.wander_circleOffset, swarmParams.wander_rate, swarmParams.buzz_maxAcceleration, rootTransform, rigidBody);
        
        groups = new List<ISteeringBehavior> { obstacleAvoidance, collisionAvoidance, wanderAroundPoint };

        lookWhereYouAreGoing = new LookWhereYouAreGoingBehavior(rootTransform, rigidBody, swarmParams.buzz_angularSpeed);
                                                               

    }

    public override SteeringOutput GetSteering()
    {
        RecalculateCurrentBuzzingRadius();
        wanderAroundPoint.SetMaxDistance(currentBuzzingRadius);
        SteeringOutput linearOutput = base.GetSteering();
        SteeringOutput angularOutput = lookWhereYouAreGoing.GetSteering();
        // compose them
        linearOutput.angular = angularOutput.angular;
        Debug.Log(linearOutput.linear);
        return linearOutput;

        
    }

    private void RecalculateCurrentBuzzingRadius()
    {
        // to dynamically change the max wander distance
        // first need to determine how many bees are around this point
        Collider[] colliders = Physics.OverlapSphere(centerPoint, currentBuzzingRadius);

        if (colliders.Length > currentBuzzingRadius * beeDensityPerUnitRadius)
        {
            currentBuzzingRadius = Mathf.Min(currentBuzzingRadius + swarmParams.buzz_maxDistanceIncrementAmount
                                            , swarmParams.buzz_maxMaxDistanceForWanderAP);
        }
        else if (colliders.Length < currentBuzzingRadius * beeDensityPerUnitRadius)
        {
            currentBuzzingRadius = Mathf.Max(currentBuzzingRadius - swarmParams.buzz_maxDistanceIncrementAmount,
                                             swarmParams.buzz_minMaxDistanceForWanderAP);
        }
    }



}

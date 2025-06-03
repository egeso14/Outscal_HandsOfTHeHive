using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FlockingBehavior : PrioritySteering
{
    private LookWhereYouAreGoingBehavior lookWhereYouAreGoing;
    public FlockingBehavior(Transform rootTransform, float maxAcceleration, Rigidbody myRigidBody,
                  Rigidbody leaderRigidbody, Transform leaderTransform,
                  float maxPredictionTime, float approachDistance,
                  float cohesionTreshold, float cohesionDecayCoefficient,
                  float seperationTreshold, float seperationDecayCoefficient,
                  float boidsDetectionRadius, float maxSpeed, float stopRadius,
                  float slowRadius, float timeToReach, float pursuitBaseWeight,
                  float pursuitBaseDistance, float avoidDistance,
                  float rayLength, LayerMask raycastMask, float angularSpeed)
    {
        // will arbitrate between 2 behaviors for velocity
        BoidsBehavior boidsBehavior = new BoidsBehavior(rootTransform, maxAcceleration, myRigidBody, leaderRigidbody,
                                                        leaderTransform, maxPredictionTime, approachDistance, cohesionTreshold,
                                                        cohesionDecayCoefficient, seperationTreshold, seperationDecayCoefficient, boidsDetectionRadius,
                                                        maxSpeed, stopRadius, slowRadius, timeToReach, pursuitBaseWeight, pursuitBaseDistance);
        
        ObstacleAndWallAvoidanceBehavior wallAndObstacleAvoidanceBehavior = new ObstacleAndWallAvoidanceBehavior(rootTransform, myRigidBody, 
                                                                                    40f, avoidDistance, rayLength, raycastMask);

        groups = new List<ISteeringBehavior>(){ wallAndObstacleAvoidanceBehavior, boidsBehavior };
        // and will use a single behavior for the angular output
        lookWhereYouAreGoing = new LookWhereYouAreGoingBehavior(rootTransform, myRigidBody, angularSpeed);
    }

    public override SteeringOutput GetSteering()
    {
        SteeringOutput linearOutput = base.GetSteering();
        SteeringOutput angularOutput = lookWhereYouAreGoing.GetSteering();
        // compose them
        linearOutput.angular = angularOutput.angular;
        return linearOutput;
    }


}

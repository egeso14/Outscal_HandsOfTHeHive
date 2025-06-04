using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BoidsBehavior : BlendedSteering
{
    private SteeringOutput lastOutput;
    private Transform rootTransform;
    private Transform leaderTransform;

    private CohesionBehavior cohesionBehavior;
    private SeparationBehavior separationBehavior;
    private PursueBehavior pursueBehavior;

    private float boidsDetectionRadius;
    private float pursuitBaseWeight;
    private float pursuitBaseDistance;

    public BoidsBehavior(Transform rootTransform, float maxAcceleration, Rigidbody myRigidBody,
                  Rigidbody leaderRigidbody, Transform leaderTransform,
                  float maxPredictionTime, float approachDistance,
                  float cohesionTreshold, float cohesionDecayCoefficient,
                  float seperationTreshold, float seperationDecayCoefficient,
                  float boidsDetectionRadius, float maxSpeed, float stopRadius,
                  float slowRadius, float timeToReach,
                  float pursuitBaseWeight, float pursuitBaseDistance)
        : base(maxAcceleration)
    {
        this.leaderTransform = leaderTransform;
        this.rootTransform = rootTransform;
        this.pursuitBaseWeight = pursuitBaseWeight;
        this.pursuitBaseDistance = pursuitBaseDistance;
        cohesionBehavior = new CohesionBehavior(rootTransform, maxAcceleration, cohesionTreshold, cohesionDecayCoefficient);
        separationBehavior = new SeparationBehavior(rootTransform, seperationTreshold, seperationDecayCoefficient, float.MaxValue);
        pursueBehavior = new PursueBehavior(leaderRigidbody, leaderTransform, myRigidBody, rootTransform, float.MaxValue, maxPredictionTime, 
                                            approachDistance, maxSpeed, stopRadius, slowRadius, timeToReach);
        this.boidsDetectionRadius = boidsDetectionRadius;

        behaviors = new List<BehaviorAndWeight>
        {
            new BehaviorAndWeight(cohesionBehavior, 0.2f),
            new BehaviorAndWeight(separationBehavior, 1f),
            new BehaviorAndWeight(pursueBehavior, 0.6f)
        };
        
    }

    public override SteeringOutput GetSteering()
    {
        LayerMask beeMask = LayerMask.GetMask("Bee"); // Assuming "Bee" is the layer for boids
        Collider[] nearbyEntities = Physics.OverlapSphere(rootTransform.position, boidsDetectionRadius, beeMask); // Example radius, adjust as needed

        cohesionBehavior.UpdateTargets(nearbyEntities);
        separationBehavior.UpdateTargets(nearbyEntities);
        SetWeight(GetScaledPursueWeight(), pursueBehavior);

        SteeringOutput output = base.GetSteering();
        lastOutput = output;
        // Ensure the output is normalized to prevent excessive acceleration

        return output;
    }

    public override void DebugBehavior()
    {
        foreach(var behavior in behaviors)
        {
            behavior.behavior.DebugBehavior();
        }
    }

    private float GetScaledPursueWeight()
    {
        // Calculate the weight for the pursue behavior based on distance to the leader
        float distanceToLeader = Vector3.Distance(rootTransform.position, leaderTransform.position);
        float weight = Mathf.Max(pursuitBaseWeight, distanceToLeader / pursuitBaseDistance * pursuitBaseWeight);
        return weight;
    }
}

using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
/*
public enum GoStates
{
    Flocking,
    Leading
}

public enum Behavior
{
    Avoidance,
    Cohesion,
    Alignment
}
public class BoidsBehavior : ISteeringBehavior
{
    private Vector3 target;
    private Transform rootTransform;
    private LayerMask detectionMask;
    private Collider myCollider;
    private GoParams myParams;
    private bool targetReached;
    private PathFollower pathFollower;
    private Rigidbody myRigidBody;

    private float visionDistance;


    private GoStates _myState;
    private GoStates myState
    {
        get { return _myState; }
        set
        {
            if (_myState == GoStates.Flocking && value == GoStates.Leading)
            {
                Navmesh navMesh = Navmesh.instance;
                Debug.Assert(navMesh != null);
                List<Vector3> path = navMesh.GetPathFromTo(rootTransform.position, target);
                Debug.Log(path);
                if (path.Count == 0)
                {
                    targetReached = true;
                    _myState = GoStates.Flocking;
                    //Debug.Log("path is empty");
                }
                else
                {
                    
                    pathFollower = new PathFollower(path, rootTransform);
                    Debug.Assert(pathFollower != null);
                    _myState = GoStates.Leading;
                }


            }
        }
    }

    public void Update(SteeringOutput outputToFill)
    {

    }
    public BoidsBehavior(Vector3 target, Transform rootTransform, Rigidbody myRigidBody)
    {
        myParams = SwarmParameters.Instance.GetGoParams();
        this.target = target;
        Debug.Assert(myRigidBody != null);
        // we are only detecting for bees
        detectionMask = LayerMask.GetMask("Bee");
        this.rootTransform = rootTransform;
        this.myRigidBody = myRigidBody;
        
        visionDistance = myParams.visionDistance;
        myCollider = rootTransform.GetComponent<Collider>();
        Debug.Assert(myCollider != null);
       
    }
    public Vector3 CalculateVelocityVector()
    {
        // do not use the same algorithm on boids and obstacles
        Collider[] nearbyColliders = Physics.OverlapSphere(rootTransform.position, visionDistance, detectionMask);
        Vector3 avoidancePositionSum = Vector3.zero,
            cohesionPositionSum = Vector3.zero,
            alignmentDirectionSum = Vector3.zero;

        int boidsNearby = 0;

        float weightedBoidsAvoidance = 0,
            weightedBoidsCohesion = 0,
            weightedBoidsAlignment = 0;
        int boidsInFoV = 0;
        foreach (Collider collider in nearbyColliders)
        {
            if (collider == myCollider) continue;

            Vector3 otherPosition = collider.transform.position;
            float sqrDistance = (otherPosition - rootTransform.position).sqrMagnitude;

            boidsNearby++; // we are counting so that we can determine if a mobing bee is the steerer

            if (! IsInMyFOV(otherPosition) )
            {
                // won't affect my calculations because I can't see them
                continue;
            }
            boidsInFoV++;
            Vector3 boidDirection = collider.transform.forward;

            float influenceWeight = GetInfluence(sqrDistance);

            float cohesionPortion = GetCohesionWeight(sqrDistance);
            float avoidancePortion = GetAvoidanceWeight(sqrDistance);
            float alignmentPortion = 1 - cohesionPortion - avoidancePortion;

            float avoidanceInfluence = influenceWeight * avoidancePortion,
                alignmentInfluence = influenceWeight * alignmentPortion,
                cohesionInfluence = influenceWeight * cohesionPortion;

            weightedBoidsAvoidance += avoidanceInfluence;
            weightedBoidsCohesion += cohesionInfluence;
            weightedBoidsAlignment += alignmentInfluence;

            avoidancePositionSum += avoidanceInfluence * otherPosition;
            cohesionPositionSum += cohesionInfluence * otherPosition;
            alignmentDirectionSum += alignmentInfluence * boidDirection;
        }

        // this next portion is very important

        // we want to adapt the vision distance so that at any time, the bees won't be affected by too many forces
        AdaptVisionDistance((int)(weightedBoidsAlignment +
                                  weightedBoidsAvoidance +
                                  weightedBoidsCohesion));

        AdaptState(boidsInFoV);

        if (myState == GoStates.Leading)
        {
            //Debug.Log("I am leading");
            var direction = CalculateLeaderDirection();
            Debug.Log(direction);
            return ScaleMovementDirection(direction);
        }

        float weightSum = weightedBoidsAvoidance +
            weightedBoidsCohesion +
            weightedBoidsAlignment;

        if (weightSum < Mathf.Epsilon)
        {
            return ScaleMovementDirection(rootTransform.forward);
        }
        

        Vector3 separationDirection = GetIdealDirectionForBehavior(
                Behavior.Avoidance, avoidancePositionSum, weightedBoidsAvoidance),
            alignmentDirection = GetIdealDirectionForBehavior(
                Behavior.Alignment, alignmentDirectionSum, weightedBoidsAlignment),
            cohesionDirection = GetIdealDirectionForBehavior(
                Behavior.Cohesion, cohesionPositionSum, weightedBoidsCohesion);

        float separationWeight = GetRealWeight(
                weightedBoidsAvoidance, myParams.avoidanceBaseWeight),
            alignmentWeight = GetRealWeight(
                weightedBoidsAlignment, myParams.alignmentBaseWeight),
            cohesionWeight = GetRealWeight(
                weightedBoidsCohesion, myParams.cohesionBaseWeight);



        return ScaleMovementDirection( (
            myParams.momentumWeight * rootTransform.forward +
            separationWeight * separationDirection +
            alignmentWeight * alignmentDirection +
            cohesionWeight * cohesionDirection
        ).normalized);

    }

    public Quaternion CalculateRotationQuaternion()
    {
        return Quaternion.LookRotation(rootTransform.forward);
    }

    public bool IsCompleted()
    {
        return targetReached;
    }

    public void DebugBehavior()
    {
        Gizmos.color = myState == GoStates.Leading ? Color.red : Color.green;
        Gizmos.DrawCube(rootTransform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }

    private void AdaptVisionDistance(int nbWithinVisionRange)
    {
        if (nbWithinVisionRange > SwarmParameters.Instance.idealNeighborCount)
        {
            // reduce vision distance by 1, but don't let it go below 1
            visionDistance = Mathf.Max(1, visionDistance - 1);
        }
        else if (nbWithinVisionRange < SwarmParameters.Instance.idealNeighborCount)
        {
            visionDistance = Mathf.Min(SwarmParameters.Instance.visionDistance, visionDistance + 1);
        }
    }

    private bool IsInMyFOV(Vector3 position)
    {
        //Debug.Log("might be worth double checking this");
        float cosineOfAngle = Vector3.Dot(rootTransform.forward, 
                            position - rootTransform.position);
        return cosineOfAngle >= myParams.cosineFOVRange;
    }

    private float GetInfluence(float otherSqrDistance)
    {
        float falloff = 1f;
        return Mathf.InverseLerp(MathU.Square(visionDistance),
                                 MathU.Square(visionDistance - falloff),
                                 otherSqrDistance);
    }

    private void AdaptState(int nearbyBoids)
    {
        if (nearbyBoids < SwarmParameters.Instance.boidsNearbyToBeAlone)
        {
            myState = GoStates.Leading;
        }
        else
        {
            myState = GoStates.Flocking;
        }
    }

    private Vector3 CalculateLeaderDirection()
    {

        Vector3 targetPos = pathFollower.GetNextTarget();
        Vector3 targetDirection = (targetPos - rootTransform.position);
        targetDirection.z = 0f;
        targetDirection.Normalize();

        targetReached = pathFollower.DestinationReached();

        return targetDirection;
    }

    private float GetCohesionWeight(float squaredDistance)
    {
        return Mathf.InverseLerp(
            SwarmParameters.Instance.squaredCohesionRadius,
            SwarmParameters.Instance.squaredFullCohesionRadius,
            squaredDistance
            );
    }
    private float GetAvoidanceWeight(float squaredDistance)
    {
        return Mathf.InverseLerp(
            SwarmParameters.Instance.squaredAvoidanceRadius,
            SwarmParameters.Instance.squaredFullAvoidanceRadius,
            squaredDistance
            );
    }

    protected Vector3 GetIdealDirectionForBehavior(
    Behavior behavior, Vector3 relevantSum, float weightedNbInvolvedEntities
)
    {
        if (weightedNbInvolvedEntities < Mathf.Epsilon)
            return Vector3.zero;

        if (behavior == Behavior.Alignment)
        {
            Vector3 averageDirection = relevantSum.normalized;
            return averageDirection;
        }

        Vector3 averagePosition = relevantSum / weightedNbInvolvedEntities;
        Vector3 directionToAveragePosition =
            (averagePosition - rootTransform.position).normalized;

        if (behavior == Behavior.Avoidance)
            return -directionToAveragePosition;

        return directionToAveragePosition;
    }

    protected float GetRealWeight(float weightedNbInvolvedEntities, float baseWeight)
    {
        return baseWeight * weightedNbInvolvedEntities;
    }

    private Vector3 ScaleMovementDirection(Vector3 movementDirection)
    {
        return movementDirection * myParams.movementSpeed;
    }
}
*/
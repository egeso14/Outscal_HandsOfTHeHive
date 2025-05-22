using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/*
public class CustomFaceBehavior : ISteeringBehavior
{
    private Vector3 targetPosition;
    private Transform rootTransform;
    private LayerMask detectionMask;
    private GoParams myParams;
    private Vector3 currentMovementDirection;
    
    public CustomFaceBehavior(Transform rootTransform, Vector3 targetPosition)
    {
        
        this.targetPosition = new Vector3(targetPosition.x, targetPosition.y, rootTransform.position.z);
        this.rootTransform = rootTransform;
        myParams = SwarmParameters.Instance.GetGoParams();
        detectionMask = LayerMask.GetMask("Bee", "FlyZoneBoundary");
    }

    public void Update(SteeringOutput outputToFill)
    {

    }

    public Vector3 CalculateVelocityVector()
    {
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, rootTransform.position.z);
        Vector3 targetDirection = (targetPosition - rootTransform.position).normalized;
        Collider[] entitiesAroundMe = Physics.OverlapSphere(rootTransform.position, myParams.visionDistance, detectionMask);

        int nbAffectedBy = 0;
        Vector3 avoidanceSum = new Vector3();

        foreach (Collider entity in entitiesAroundMe)
        {
            if (!IsInFoV(entity))
            {
                continue;
            }

            Vector3 distance = entity.transform.position - rootTransform.position;

            nbAffectedBy++;
            avoidanceSum -= distance.normalized * GetAvoidanceWeight(distance.sqrMagnitude);

        }
        Debug.Assert(nbAffectedBy > 0);

        var result = (targetDirection + avoidanceSum / nbAffectedBy).normalized;
        // result is our target velocity, we need to get there slowly
        Quaternion fromRotation = rootTransform.rotation;
        Quaternion toRotation = Quaternion.LookRotation(result);

        float angle = Quaternion.Angle(fromRotation, toRotation);
        // to prevent jitter
        if (angle > 0.01f)
        {
            float distanceSqr = (targetPosition - rootTransform.position).sqrMagnitude;
            // this line is the angular speed slow down
            float realAngularSpeed = angle < myParams.slowTresholdAngle ?
                myParams.angularSpeed * angle / myParams.slowTresholdAngle 
                : myParams.angularSpeed;
            //float realAngularSpeed = myParams.angularSpeed;
            float realLinearSpeed = distanceSqr < myParams.slowTresholdDistanceSqr ?
                myParams.movementSpeed * distanceSqr / myParams.slowTresholdDistanceSqr :
                myParams.movementSpeed;
            
            realLinearSpeed = Mathf.Max(realLinearSpeed, 0.1f); // TODO: implement a permanent fix to the close target spinning bug
            realAngularSpeed = Mathf.Max(realAngularSpeed, 5);

            float t = Mathf.Min(1, Time.fixedDeltaTime * realAngularSpeed / angle);
            var newDirectionRotation = Quaternion.Slerp(fromRotation, toRotation, t);
            currentMovementDirection = newDirectionRotation * Vector3.forward;
            return currentMovementDirection * realLinearSpeed;
        }
        
        currentMovementDirection = rootTransform.forward;
        return currentMovementDirection * myParams.movementSpeed;
    }

    public Quaternion CalculateRotationQuaternion()
    {
        // easier to make it rotate to face the direction it is moving in
        return Quaternion.LookRotation(currentMovementDirection, rootTransform.up);
    }

    private bool IsInFoV(Collider collider)
    {
        Vector3 otherPos = collider.transform.position;
        Vector3 otherDirection = (otherPos - rootTransform.position).normalized;
        Vector3 myDirection = rootTransform.forward;

        float cosineOfAngle = Vector3.Dot(myDirection, otherDirection);
        return myParams.cosineFOVRange <= cosineOfAngle;
    }

    private float GetAvoidanceWeight(float squaredDistance)
    {
        return Mathf.InverseLerp(
            myParams.squaredAvoidanceRadius,
            myParams.squaredFullAvoidanceRadius,
            squaredDistance
            );
    }

    public bool IsCompleted()
    {
        var targetDirection = targetPosition - rootTransform.position;
        var targetRotation = Quaternion.LookRotation(targetDirection.normalized, rootTransform.up);
        var currentRotation = rootTransform.rotation;

        if (Quaternion.Angle(currentRotation, targetRotation) < 10) // TODO: FIX VALUE
        {
            return true;
        }
        return false;
    }

    public void DebugBehavior()
    {
        return;
    }


}
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    private Rigidbody body;
    private StrategyData strategyBeingExecuted;
    public Transform rootTransform;
    private SwarmParameters swarmParams;

    private Queue<ISteeringBehavior> toDoList;
    private ISteeringBehavior currentBehavior;
    public event Action OnStrategyComplete;
    private Vector3 centerPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Assert(rootTransform != null);
        body = GetComponent<Rigidbody>();
        Debug.Assert(body != null);
        swarmParams = SwarmParameters.Instance;
       
        centerPoint = rootTransform.position;
        currentBehavior = new BuzzingBehavior(rootTransform.position, rootTransform, body);
        SetRandomStartingVelocity();
    }


    private void SetRandomStartingVelocity()
    {
        body.linearVelocity = UnityEngine.Random.onUnitSphere;            
    }


    void FixedUpdate()
    {
        if (currentBehavior.IsCompleted())
        {
            SwitchToNextBehavior();
        }
        else
        {
            UpdateWithAlgorithmResults();
        }
    }

    

    private void UpdateWithAlgorithmResults()
    {
        Debug.Assert(currentBehavior != null);

        var steeringOutput = currentBehavior.GetSteering();
        //currentBehavior.Update(steeringOutput);
        body.linearVelocity += steeringOutput.linear;
        rootTransform.localRotation = steeringOutput.angular * rootTransform.localRotation;
        ClampToAcceptableRanges(rootTransform, body);
  

    }

    private void ClampToAcceptableRanges(Transform rootTransform, Rigidbody body)
    {
        if (body.linearVelocity.sqrMagnitude > MathU.Square(swarmParams.beeSpeedCap))
        {
            Debug.Log("Had to manually cap velocity");
            body.linearVelocity = body.linearVelocity.normalized * swarmParams.beeSpeedCap;
        }

        Vector3 euler = rootTransform.rotation.eulerAngles;
        euler.x = euler.x > 180 ? euler.x - 360: euler.x;
        euler.z = euler.z > 180 ? euler.z -360 : euler.z;
        euler.x = Mathf.Clamp(euler.x, -swarmParams.beeRotationConstraintX, swarmParams.beeRotationConstraintX);
        euler.z = Mathf.Clamp(euler.z, -swarmParams.beeRotationConstraintZ, swarmParams.beeRotationConstraintZ);
        rootTransform.localRotation = Quaternion.Euler(euler);

    }

    void OnDrawGizmos()
    {

        currentBehavior.DebugBehavior();
        Gizmos.color = Color.black ;
        Gizmos.DrawLine(rootTransform.position, body.linearVelocity.normalized + rootTransform.position);
    }
    private void OnDrawGizmosSelected()
    {
        //strategy.OnSelectDebugStrategy();
    }

    public void InformOfStrategy(StrategyData currentBrainStrategy)
    {
        strategyBeingExecuted = currentBrainStrategy;
        toDoList = TemporaryFactory(currentBrainStrategy);
        currentBehavior = toDoList.Dequeue();
    }

    public void SwitchToNextBehavior()
    {
        if (toDoList.Count == 0)
        {
            OnStrategyComplete.Invoke();
        }
        else
        {
            currentBehavior = toDoList.Dequeue();
        }
    }

    // we need a factory that translates strategies from the brain to a list of behaviors
    private Queue<ISteeringBehavior> TemporaryFactory(StrategyData strategyData)
    {
        var steps = new Queue<ISteeringBehavior>();
        switch (strategyData.strategy)
        {
            case Strategy.Buzz:
                steps.Enqueue(new BuzzingBehavior(rootTransform.position, rootTransform, body));

                return steps;
            case Strategy.Go:
                
                steps.Enqueue(new GoThereBehavior(strategyData.targetPos, body, rootTransform));
                
                return steps;
            case Strategy.Flock:
                steps.Enqueue(new FlockingBehavior(rootTransform, swarmParams.f_b_maxAcceleration, body, strategyData.rigidbody, strategyData.transform,
                                                swarmParams.g_predictivePath_predictionTime, swarmParams.f_b_pursuitApproachDistance, swarmParams.f_b_cohesionTreshold,
                                                swarmParams.f_b_cohesionDecayCoefficient, swarmParams.f_b_seperationTreshold, swarmParams.f_b_seperationDecayCoefficient, swarmParams.f_boidsDetectionRadius,
                                                swarmParams.beeSpeedCap, swarmParams.f_b_p_arriveStopRadius, swarmParams.f_b_p_arriveSlowRadius, swarmParams.f_b_p_arriveTimeToReach,
                                                swarmParams.f_b_pursuitBaseWeight, swarmParams.f_b_pursuitBaseDistance,
                                                swarmParams.f_WAO_avoidDistance, swarmParams.f_WAO_rayLength, swarmParams.f_WAO_raycastMask, swarmParams.angularSpeed));

                return steps;

            default:
                
                return steps;
        }

    }


}

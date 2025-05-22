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

        if (body.linearVelocity.sqrMagnitude > MathU.Square(swarmParams.beeSpeedCap))
        {
            body.linearVelocity = body.linearVelocity.normalized * swarmParams.beeSpeedCap;
        }


        rootTransform.localRotation = steeringOutput.angular * rootTransform.localRotation;

    }

    void OnDrawGizmos()
    {
        currentBehavior.DebugBehavior();
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
                //steps.Enqueue(new FaceBehavior(rootTransform, strategyData.targetPos));
                
                return steps;
            default:
                
                return steps;
        }

    }


}

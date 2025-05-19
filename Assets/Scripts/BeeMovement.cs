using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    private Rigidbody body;
    private StrategyData strategyBeingExecuted;
    public Transform rootTransform;

    private Queue<ISteeringBehavior> toDoList;
    private ISteeringBehavior currentBehavior;
    public event Action OnStrategyComplete;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Assert(rootTransform != null);
        body = GetComponent<Rigidbody>();
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
        SteeringOutput steeringOutput = new SteeringOutput();
        currentBehavior.Update(steeringOutput);
        body.linearVelocity = steeringOutput.linear;
        transform.rotation = steeringOutput.angular;

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
                steps.Enqueue(new StillStrategy(rootTransform));
                return steps;
            case Strategy.Go:                
                //steps.Enqueue(new FaceBehavior(rootTransform, strategyData.targetPos));
                steps.Enqueue(new BoidsBehavior(strategyData.targetPos, rootTransform, body));
                return steps;
            default:
                steps.Enqueue(new StillStrategy(rootTransform));
                return steps;
        }

    }


}

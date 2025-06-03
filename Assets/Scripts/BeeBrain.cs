using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum Strategy
{
    Buzz,
    Go,
    Flock,
}

public struct StrategyData
{
    public Strategy strategy;
    public Vector3 targetPos;
    public Rigidbody rigidbody;
    public Transform transform;
}


public class BeeBrain : MonoBehaviour
{
    public static int numberOfActiveBees;
    public Strategy currentStrategy;
    private StrategyData _strategyData;
    private BeeMovement movement;
    private SwarmParameters swarmParameters;
    private Rigidbody rigidbody;
    public Transform rootTransform;
    private Coroutine stopFollowingTimer;

    private static event Action<Rigidbody, Transform> FollowMeBroadcast;
    private static event Action<Rigidbody> StopFollowingMeEvent;


    public StrategyData strategyData
    {
        get { return _strategyData; }
        set
        {
            _strategyData = value;
            TriggerStateEndEvents(currentStrategy);
            ChangeCallbacks(value.strategy);
            currentStrategy = value.strategy;
            TriggerNewStateEvents(currentStrategy);
            movement.InformOfStrategy(value);
        }
    }
    private void Start()
    {
        movement = gameObject.GetComponent<BeeMovement>();
        Debug.Assert(movement != null);
        swarmParameters = SwarmParameters.Instance;
        strategyData = new StrategyData { strategy = Strategy.Buzz };
        movement.OnStrategyComplete += ResetStrategyToDefault;

        rigidbody = GetComponent<Rigidbody>();
        Debug.Assert(rigidbody != null, "Rigidbody is null on BeeBrain");
        Debug.Assert(rootTransform != null, "Root transform is null on BeeBrain");

    }

    void FixedUpdate()
    {
        switch (currentStrategy)
        {
            case Strategy.Buzz:
                break;
            case Strategy.Go:
                BroadcastSelfAsLeader();
                break;
        }
    }


    private void OnDestroy()
    {
        numberOfActiveBees--;
    }

    private void Awake()
    {
        numberOfActiveBees++;
    }

    private void ChangeCallbacks(Strategy newStrategy)
    {
        UnsubscribeFromCallbacks(currentStrategy);
        SubscribeToCallbacks(newStrategy);
    }

    private void UnsubscribeFromCallbacks(Strategy callbacks)
    {
        if (callbacks == Strategy.Buzz)
        {
            FollowMeBroadcast -= FlockToIfClose;
        }
        else if (callbacks == Strategy.Go)
        {
            // Unsubscribe from Go specific callbacks
        }
        else if (callbacks == Strategy.Flock)
        {
            StopFollowingMeEvent -= StopFlocking;
        }
    }

    private void SubscribeToCallbacks(Strategy callbacks)
    {
        if (callbacks == Strategy.Buzz)
        {
            FollowMeBroadcast += FlockToIfClose;
        }
        else if (callbacks == Strategy.Go)
        {
            // Unsubscribe from Go specific callbacks
        }
        else if (callbacks == Strategy.Flock)
        {
            StopFollowingMeEvent += StopFlocking;
        }
    }

    private void FlockToIfClose(Rigidbody otherRigidBody, Transform otherTransform)
    {

        var xDistance = otherTransform.position.x - rootTransform.position.x;
        var yDistance = otherTransform.position.y - rootTransform.position.y;


        if (MathU.Square(xDistance) + MathU.Square(yDistance) < MathU.Square(swarmParameters.flockToRadius))
        {

            strategyData = new StrategyData
            {
                strategy = Strategy.Flock,
                transform = otherTransform,
                rigidbody = otherRigidBody,
            };
        }
    }

    private void StopFlocking(Rigidbody otherBody)
    {
        if (strategyData.rigidbody == otherBody)
        {
            ResetStrategyToDefault();
        }
    }

    private IEnumerator TimerToStopFollowing()
    {

        yield return new WaitForSeconds(1);
        StopFollowingMeEvent?.Invoke(rigidbody);

    }


    private void ResetStrategyToDefault()
    {
        strategyData = new StrategyData() { strategy = Strategy.Buzz };
    }

    public void InformOfCommand(CommandData commandData)
    {
        strategyData = new StrategyData
        {
            strategy = Strategy.Go,
            targetPos = commandData.goCommandTarget
        };

    }

    private void BroadcastSelfAsLeader()
    {
        FollowMeBroadcast?.Invoke(rigidbody, rootTransform);
    }

    private void StartTimerToStopFollowingMeBroadcast()
    {
        stopFollowingTimer = StartCoroutine(TimerToStopFollowing());
    }

    private void TriggerStateEndEvents(Strategy strategy)
    {
        if (strategy == Strategy.Go)
        {
            StartTimerToStopFollowingMeBroadcast();
        }
    }

    private void TriggerNewStateEvents(Strategy newStrategy)
    {
        if (newStrategy == Strategy.Flock)
        {
            BeeHighlightManager.HighlightMe(gameObject, HighlightColor.Green);
        }
        if (newStrategy == Strategy.Buzz)
        {
            BeeHighlightManager.HighlightMe(gameObject, HighlightColor.Blue);
        }
        if (newStrategy == Strategy.Go)
        {
            BeeHighlightManager.HighlightMe(gameObject, HighlightColor.Red);
            if (stopFollowingTimer != null)
            {
                StopCoroutine(stopFollowingTimer);
                stopFollowingTimer = null;
            }
        }
    }


}

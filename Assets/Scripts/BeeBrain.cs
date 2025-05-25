using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum Strategy
{
    Buzz,
    Go
}

public struct StrategyData
{
    public Strategy strategy;
    public Vector3 targetPos;
    
}


public class BeeBrain : MonoBehaviour
{
    public static int numberOfActiveBees;
    public Strategy currentStrategy;
    private StrategyData _strategyData;
    private BeeMovement movement;
    private SwarmParameters swarmParameters;
    public InputReader inputReader;
    private Camera nonBlurCamera;

    public StrategyData strategyData
    {
        get { return _strategyData; }
        set
        {
            _strategyData = value;
            currentStrategy = value.strategy;
            movement.InformOfStrategy(value);
        }
    }
    private void Start()
    {
        movement = gameObject.GetComponent<BeeMovement>();
        Debug.Assert(movement != null);

        Debug.Assert(inputReader != null);

        strategyData = new StrategyData { strategy = Strategy.Buzz };
        movement.OnStrategyComplete += ResetStrategyToDefault;

        var cameraObject = GameObject.FindWithTag("NonBlurCamera");
        Debug.Assert(cameraObject != null);
        nonBlurCamera = cameraObject.GetComponent<Camera>();
        Debug.Assert(nonBlurCamera != null);

    }


    private void OnDestroy()
    {
        numberOfActiveBees--;
    }

    private void Awake()
    {
        numberOfActiveBees++;
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

        movement.InformOfStrategy(strategyData);



    }
}

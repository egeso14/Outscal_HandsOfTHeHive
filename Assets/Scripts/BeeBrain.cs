using Unity.VisualScripting;
using UnityEngine;

public enum Commands
{
    None,
}



public class BeeBrain : MonoBehaviour
{
    public static int numberOfActiveBees;
    private Commands _command;
    private BeeMovement movement;
    private SwarmParameters swarmParameters;
    public Commands command
    {
        get { return _command; }
        set
        {
            _command = value;
            CommunicateStrategy(value);
        }
    }
    private void Start()
    {
        movement = gameObject.AddComponent<BeeMovement>();
        CommunicateStrategy(Commands.None);
    }

    private void OnDestroy()
    {
        numberOfActiveBees--;
    }

    private void Awake()
    {
        numberOfActiveBees++;
    }

    private void CommunicateStrategy(Commands command)
    {
        var strategy = TemporaryFactory(command);
        movement.SetStrategy(strategy);
    }

    private MovementStrategy TemporaryFactory(Commands command)
    {
        switch (command)
        {
            case Commands.None:
                return new StillStrategy(this);
            default:
                return new StillStrategy(this);
        }
        
    }
}

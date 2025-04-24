using Unity.VisualScripting;
using UnityEngine;

public enum Commands
{
    None,
}



public class BeeBrain : MonoBehaviour
{
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

    private void CommunicateStrategy(Commands command)
    {
        var strategy = TemporaryFactory(command);
        movement.SetStrategy(command);
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

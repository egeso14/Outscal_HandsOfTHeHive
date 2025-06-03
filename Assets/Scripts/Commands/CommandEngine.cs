using System;
using UnityEngine;

public enum CommandType
{
    Go
}

public class CommandData
{
    public CommandType commandType;
    public Vector3 goCommandTarget;
}

/// <summary>
/// Responsible for interpreting data passed down by the UI and player input
/// to communicate commands to Selectables.
/// </summary>
public class CommandEngine : MonoBehaviour
{
    public LayerMask raycastLayerMask;
    public static CommandEngine instance;
    public static event Action<CommandData> commandBroadcast;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        AssignCallbacks();
    }

    private void AssignCallbacks()
    {
        ClickRouter.mapClickEvent += MapClickCallback;
    }

    private void MapClickCallback(Vector3 positionOnMap)
    {
        commandBroadcast?.Invoke(new CommandData
        {
            commandType = CommandType.Go,
            goCommandTarget = positionOnMap
        });

    }


}

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

    
}

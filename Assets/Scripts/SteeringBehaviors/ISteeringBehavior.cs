using UnityEngine;

public interface ISteeringBehavior
{
    public SteeringOutput GetSteering(); 
    public bool IsCompleted();
    public void DebugBehavior();
}

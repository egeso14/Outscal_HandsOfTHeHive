using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PrioritySteering : ISteeringBehavior
{
    private bool isCompleted;
    private float epsilon;
    private List<ISteeringBehavior> groups;

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public void DebugBehavior()
    {

    }
    
    public SteeringOutput GetSteering()
    {
        SteeringOutput output = new SteeringOutput();
        foreach (ISteeringBehavior behavior in groups)
        {
            output = behavior.GetSteering();
            if (output.linear.sqrMagnitude > MathU.Square(epsilon)
                | Quaternion.Angle(output.angular, Quaternion.identity) > epsilon)
            {
                return output;
            }
        }
        return output;
    }
}

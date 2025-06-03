using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PrioritySteering : ISteeringBehavior
{
    protected bool isCompleted;
    protected float epsilon = 0.005f;
    protected List<ISteeringBehavior> groups;


    public virtual bool IsCompleted()
    {
        return isCompleted;
    }

    public virtual void DebugBehavior()
    {
        for (int i = 0; i < groups.Count; i++)
        {
            var behavior = groups[i];
            behavior.DebugBehavior();
        }
    }

    public virtual SteeringOutput GetSteering()
    {
        SteeringOutput output = new SteeringOutput();
        foreach (ISteeringBehavior behavior in groups)
        {
            var behaviorOutput = behavior.GetSteering();
            Debug.Log(behaviorOutput.linear);
            if (behaviorOutput.linear.sqrMagnitude > MathU.Square(epsilon)
                | Quaternion.Angle(behaviorOutput.angular, Quaternion.identity) > epsilon)
            {
                output = behaviorOutput;
                return output;
            }
        }
        return output;
    }
}

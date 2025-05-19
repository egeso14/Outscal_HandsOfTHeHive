using NUnit.Framework;
using System.Collections.Generic;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BlendedSteering : ISteeringBehavior
{
    public class BehaviorAndWeight
    {
        public BehaviorAndWeight(ISteeringBehavior behavior, float weight) 
        {
            this.behavior = behavior;
            this.weight = weight;
        }
        public ISteeringBehavior behavior;
        public float weight;
    }

    protected List<BehaviorAndWeight> behaviors;
    protected float maxAcceleration;
    protected float maxRotationAngle; // should we even include rotations in blended behaviors?
    protected bool isCompleted;

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public void DebugBehavior()
    {

    }

    public virtual SteeringOutput GetSteering()
    {
        var output = new SteeringOutput();
        foreach (var behavior in behaviors)
        {            
            output += behavior.weight * behavior.behavior.GetSteering(); ;
        }
        
        if (MathU.Square(maxAcceleration) < output.linear.sqrMagnitude)
        {
            output.linear = output.linear.normalized * maxAcceleration;
        }
        return output;

    }

    protected void SetWeight(float weight, ISteeringBehavior behavior)
    {
        for (int i = 0; i < behaviors.Count; i++)
        {
            if (behaviors[i].behavior == behavior)
            {
                behaviors[i].weight = weight;
            }
        }
    }
    

}

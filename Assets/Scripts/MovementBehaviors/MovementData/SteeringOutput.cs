using UnityEngine;

public class SteeringOutput
{
    public Vector3 linear;
    public Quaternion angular;
    public SteeringOutput()
    {
        Reset();
    }
    public void Reset()
    {
        linear = Vector3.zero;
        angular = Quaternion.identity;
    }

    public static SteeringOutput operator *(float scalar, SteeringOutput steeringOutput)
    {
        steeringOutput.linear = steeringOutput.linear * scalar;
        return steeringOutput;
    }
    public static SteeringOutput operator +(SteeringOutput a, SteeringOutput b)
    {
        var result = new SteeringOutput();
        result.linear = a.linear + b.linear;
        return result;
    }
}

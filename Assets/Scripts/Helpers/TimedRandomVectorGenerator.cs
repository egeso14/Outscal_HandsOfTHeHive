using UnityEngine;

public class TimedRandomVectorGenerator
{
    private float remembersFor;
    private float lastGeneratedAt;
    private Vector3 lastGenerated;

    public TimedRandomVectorGenerator(float remembersFor)
    {
        this.remembersFor = remembersFor;
        lastGeneratedAt = 0;
    }

    public Vector3 GenerateOfMagnitude(float magnitude)
    {
        if (Time.time < lastGeneratedAt + remembersFor)
        {
            return lastGenerated;
        }
        lastGeneratedAt = Time.time;
        Vector3 direction = Random.insideUnitSphere.normalized * magnitude;
        lastGenerated = direction;
        return direction;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "SwarmParameters", menuName = "Scriptable Objects/SwarmParameters")]
public class SwarmParameters : ScriptableObject
{
    private static SwarmParameters _Instance;
    public static SwarmParameters Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = Resources.Load<SwarmParameters>(typeof(SwarmParameters).Name);

                if (_Instance == null)
                {
                    Debug.LogError($"Singleton ScriptableObject of type {typeof(SwarmParameters).Name} not found in Resources!");
                }
            }
            return _Instance;
        }
    }

    [Header("Universal Params")]
    public float detectionRadius;
    public float beeSpeedCap; // has to be less than the overlap sphere radius
    public float cohesionMaxRange; // can be smaller if we have more bees
    public float cohesionMinRange;

    public float avoidanceMaxRange;
    public float avoidanceMinRange;

    public float buzzinessMaxRange;
    public float buzzinessMinRange;


    
    

}

using UnityEngine;
using UnityEngine.Rendering;

public struct GoParams
{
    public int idealNeighborCount;
    public float visionDistance;
    public float cosineFOVRange;
    public int boidsNearbyToBeAlone;

    public float cohesionRadius;
    public float squaredCohesionRadius;
    public float squaredFullCohesionRadius;

    public float avoidanceRadius;
    public float squaredAvoidanceRadius;
    public float squaredFullAvoidanceRadius;

    public float avoidanceBaseWeight;
    public float cohesionBaseWeight;
    public float alignmentBaseWeight;

    public float momentumWeight;
    public float angularSpeed;
    public float sqrFacingTreshold;
    public float movementSpeed;
    public float slowTresholdAngle;
    public float slowTresholdDistanceSqr;

}

public struct StillParams
{
    public float moveTime;
    public float waitTime;
}
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

    public GoParams GetGoParams()
    {
        return new GoParams
        {
            idealNeighborCount = this.idealNeighborCount,
            visionDistance = this.visionDistance,
            cosineFOVRange = Mathf.Cos(visionSemiAngle * Mathf.Deg2Rad),
            boidsNearbyToBeAlone = this.boidsNearbyToBeAlone,
            cohesionRadius = this.cohesionRadius,
            squaredCohesionRadius = MathU.Square(this.cohesionRadius),
            squaredFullCohesionRadius = MathU.Square(this.cohesionRadius + this.smoothnessRadiusOffset),
            avoidanceRadius = this.avoidanceRadius,
            squaredAvoidanceRadius = MathU.Square(this.avoidanceRadius),
            squaredFullAvoidanceRadius = MathU.Square(this.avoidanceRadius - this.smoothnessRadiusOffset),
            avoidanceBaseWeight = this.avoidanceBaseWeight,
            cohesionBaseWeight = this.cohesionBaseWeight,
            alignmentBaseWeight = this.avoidanceBaseWeight,
            momentumWeight = this.momentumWeight,
            angularSpeed = this.angularSpeed,
            sqrFacingTreshold = MathU.Square(this.facingThreshold),
            movementSpeed = this.movementSpeed,
            slowTresholdAngle = this.slowTresholdAngle,
            slowTresholdDistanceSqr = MathU.Square(this.slowTresholdDistance),
        };
    }


    [Header("Universal Params")]
    public float detectionRadius;
    public float beeSpeedCap; // has to be less than the overlap sphere radius
    public float cohesionMaxRange; // can be smaller if we have more bees
    public float cohesionMinRange;
  
    public float avoidanceMaxRange;
    public float avoidanceMinRange;
    public int detectNumForCohesion;
    public float targetDistance;


    [Header("Buzz Params")]
    public float moveTime;
    public float waitTime;

    [Header("Go Params")]
    public int idealNeighborCount;
    public float visionDistance;
    public float visionSemiAngle;
    public int boidsNearbyToBeAlone;

    public float cohesionRadius;
    [HideInInspector] public float squaredCohesionRadius;
    [HideInInspector] public float squaredFullCohesionRadius;

    public float avoidanceRadius;
    [HideInInspector] public float squaredAvoidanceRadius;
    [HideInInspector] public float squaredFullAvoidanceRadius;

    public float avoidanceBaseWeight;
    public float cohesionBaseWeight;
    public float alignmentBaseWeight;

    public float momentumWeight;
    public float angularSpeed;
    public float movementSpeed;
    [HideInInspector] public float sqrFacingThreshold;
    public float smoothnessRadiusOffset;
    public float facingThreshold;
    public float slowTresholdAngle;
    public float slowTresholdDistance;
}

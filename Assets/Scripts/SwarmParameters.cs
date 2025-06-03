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
    public float beeSpeedCap; // has to be less than the overlap sphere radius
    public float beeRotationConstraintX;
    public float beeRotationConstraintY;
    public float beeRotationConstraintZ;
    public float flockToRadius;


    [Header("BuzzingBehavior Params")]
    public float buzz_maxAcceleration;
    public float buzz_beeDensityPerUnitRadius;
    public float buzz_baseMaxDistanceForWanderAP; // for wanderAroundPoint
    public float buzz_maxMaxDistanceForWanderAP;
    public float buzz_minMaxDistanceForWanderAP;
    public float buzz_maxDistanceIncrementAmount;
    public float buzz_angularSpeed;
    public float obs_avoidDistance;
    public float obs_rayLength;
    public float obs_maxAcceleration;

    public float wander_radius;
    public float wander_circleOffset;
    public float wander_rate;
    public float colAvoidance_detectionRadius;
    public float colAvoidance_beeColliderRadius;

    [Header("GoThereBehavior Params")]
    public float g_predictivePath_pathOffset;
    public float g_predictivePath_predictionTime;
    public float g_predictivePath_maxAcceleration;
    public int g_p_linePath_pathLookAhead;
    public float g_colAvoidance_maxAcceleration;
    public float g_colAvoidance_detectionRadius;
    public float g_colAvoidance_collisionRadius;
    public LayerMask g_colAvoidance_detectionLayerMask;
    public float g_lookWhereYouAreGoing_angularSpeed;

    [Header("FlockingBehavior Params")]
    public float f_b_cohesionTreshold;
    public float f_b_seperationTreshold;
    public float f_b_cohesionDecayCoefficient;
    public float f_b_seperationDecayCoefficient;
    public float f_b_maxAcceleration;
    public float f_b_pursuitApproachDistance;
    public float f_b_pursuitMaxPredictionTime;
    public float f_boidsDetectionRadius;
    public float f_b_p_arriveStopRadius;
    public float f_b_p_arriveSlowRadius;
    public float f_b_p_arriveTimeToReach;
    public float f_WAO_avoidDistance;
    public float f_WAO_rayLength;
    public LayerMask f_WAO_raycastMask;
    public float f_WAO_angularSpeed;
    public float f_b_pursuitBaseWeight;
    public float f_b_pursuitBaseDistance;





    public int idealNeighborCount;
    public float visionDistance;
    public float visionSemiAngle;
    public int boidsNearbyToBeAlone;




    [Header("Go Params")]
    

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

using System.Collections.Generic;
using UnityEngine;

public class GoThereBehavior : PrioritySteering
{
    private LookWhereYouAreGoingBehavior rotationControllingBehavior;
    private SwarmParameters swarmParams;
    private PredictivePathFollowingBehavior pathFollowingBehavior;

    public GoThereBehavior(Vector3 target, Rigidbody rigidbody, Transform rootTransform)
    {
        swarmParams = SwarmParameters.Instance;
        Debug.Assert(swarmParams != null, "SwarmParameters instance is null");


        List<Vector3> travelNodes = Navmesh.instance.GetPathFromTo(rootTransform.position, target);
        IPath path = new LinePath(travelNodes, swarmParams.g_p_linePath_pathLookAhead);
        pathFollowingBehavior = new PredictivePathFollowingBehavior(path, swarmParams.g_predictivePath_pathOffset,
                                                                                                              swarmParams.g_predictivePath_predictionTime,
                                                                                                              rigidbody, rootTransform, swarmParams.g_predictivePath_maxAcceleration);
        
        CollisionAvoidanceBehavior collisionAvoidanceBehavior = new CollisionAvoidanceBehavior(rootTransform, rigidbody, swarmParams.g_colAvoidance_maxAcceleration,
                                                                                               swarmParams.g_colAvoidance_detectionRadius, swarmParams.g_colAvoidance_collisionRadius,
                                                                                               swarmParams.g_colAvoidance_detectionLayerMask);
        
        groups = new List<ISteeringBehavior>
        {
            pathFollowingBehavior,
            collisionAvoidanceBehavior
        };

        rotationControllingBehavior = new LookWhereYouAreGoingBehavior(rootTransform, rigidbody, swarmParams.g_lookWhereYouAreGoing_angularSpeed);
    }

    public override SteeringOutput GetSteering()
    {
        SteeringOutput linearOutput = base.GetSteering();
        SteeringOutput angularOutput = rotationControllingBehavior.GetSteering();

        // Combine linear and angular outputs
        linearOutput.angular = angularOutput.angular;
        return linearOutput;
    }

    public override bool IsCompleted()
    {
        return pathFollowingBehavior.IsCompleted();
    }


}

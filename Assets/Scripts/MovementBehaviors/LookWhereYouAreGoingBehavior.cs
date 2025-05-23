using UnityEngine;

public class LookWhereYouAreGoingBehavior : AlignBehavior
{
    private Rigidbody rigidBody;
    public LookWhereYouAreGoingBehavior(Transform rootTransform, Rigidbody rigidBody,
                                        float angularSpeed)
                                        : base(rootTransform, angularSpeed)
    {
        this.rigidBody = rigidBody;
    }

    public override SteeringOutput GetSteering()
    {
        var alignTarget = rigidBody.linearVelocity;
        if (alignTarget == Vector3.zero)
        {
            return new SteeringOutput();
        }

        target = alignTarget; // we do this to pass the base class info
        SteeringOutput output = base.GetSteering();
        return output;
    }
}

using UnityEngine;
public interface IPath
{
    public float GetParameter(Vector3 position, float lastParam);
    public Vector3 GetPosition(float param);
}
public class PredictivePathFollowingBehavior : ISteeringBehavior
{
    private float pathOffset;
    private float currentParam; // the current index with the list of node positions

}

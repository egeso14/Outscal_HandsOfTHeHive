using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    private Rigidbody body;
    private MovementStrategy strategy;

    private 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //body.linearVelocity = strategy.CalculateVelocityVector();
        body.linearVelocity = strategy.CalculateVelocityVector();
    }
    public void SetStrategy(MovementStrategy strategy)
    {
        this.strategy = strategy;
    }

}

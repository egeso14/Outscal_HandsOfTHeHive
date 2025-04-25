using Unity.VisualScripting;
using UnityEngine;

public class Timer
{
    private float prevTime;
    private float interval;
    private float counter;
    public Timer(float interval)
    {
        prevTime = Time.time;
        counter = 0;
        this.interval = interval;
    }
    public void TickTimer()
    {
        counter += Time.time - prevTime;
    }
    public bool Check()
    {
        Debug.Log("Counter");
        Debug.Log(counter);
        Debug.Log("Interval");
        Debug.Log(interval);
        if (counter >= interval)
        {
            counter = 0;
            return true;
        }
        return false;
    }
}

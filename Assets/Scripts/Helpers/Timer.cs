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
        prevTime = Time.time;
    }
    public bool Check()
    {

        if (counter >= interval)
        {
            counter = 0;
            return true;
           
        }
        return false;
    }
    public void Reset() 
    {
        counter = 0;
        prevTime = Time.time;
    }
}

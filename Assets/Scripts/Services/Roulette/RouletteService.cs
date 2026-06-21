using UnityEngine;

public class RouletteService : IRouletteService
{
    private bool _status = true;
    private int speed = 300;
    private int stop = 30;

    private int baseSpeed = 300;
    private int baseStop = 30;
    public bool GetRouletteStatus()
    {
        return _status;
    }

    public void StopRoulette()
    {
        _status = false;
    }

    public void StartRoulette()
    {
        _status = true;
    }

    public void ChangeSpeed(int newSpeed)
    {
        speed = newSpeed;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public void ChangeStop(int newStop)
    {
        stop = newStop;
    }

    public int GetStop()
    {
        return stop;
    }

    public void ResetSpeed()
    {
        speed = baseSpeed;
    }

    public void ResetStop()
    {
        stop = baseStop;
    }
}

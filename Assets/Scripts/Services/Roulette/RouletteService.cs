using UnityEngine;

public class RouletteService : IRouletteService
{
    private bool _status = true;
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
}

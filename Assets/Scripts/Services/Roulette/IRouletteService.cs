using UnityEngine;

public interface IRouletteService
{
    public bool GetRouletteStatus();
    public void StopRoulette();

    public void StartRoulette();
}

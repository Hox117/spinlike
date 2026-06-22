using UnityEngine;

public interface IRouletteService
{

    public void ChangeSpeed(int newSpeed);
    public int GetSpeed();
    public void ChangeStop(int newStop);
    public int GetStop();
    public void ResetStop();
    public void ResetSpeed();
}

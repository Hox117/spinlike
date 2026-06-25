using UnityEngine;

public interface IRouletteService
{
    public void ToogleStatus(bool newStatus);
    public bool GetStatus();
    public void ChangeSpeed(int newSpeed);
    public int GetSpeed();
    public void ChangeStop(int newStop);
    public int GetStop();
    public void ResetStop();
    public void ResetSpeed();
}

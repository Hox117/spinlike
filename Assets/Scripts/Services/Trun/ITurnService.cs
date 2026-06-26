using UnityEngine;

public interface ITurnService
{
    public bool IsPlayerTurn();
    public void ChangeTurn();
    
    public void resetTurn();
}

using UnityEngine;

public class TurnService : ITurnService
{
    private bool _isPlayerTurn = true;
    private IEventService eventService;
    private IEnemyService enemyService;
    public TurnService()
    {
        eventService = AppContainer.Get<IEventService>();
        enemyService = AppContainer.Get<IEnemyService>();
    }

    public void ChangeTurn()
    {
        
        _isPlayerTurn = !_isPlayerTurn;

        if (_isPlayerTurn)
        {
            enemyService.resetTurns();
            Debug.Log("Turno del jugador");
        }
        else
        {
            
            Debug.Log("Turno de los enemigos");
        }

        TurnChangeEvent turnChangeEvent = new TurnChangeEvent();
        eventService.Publish(turnChangeEvent);
    }

    public bool IsPlayerTurn()
    {
        return _isPlayerTurn;
    }
}

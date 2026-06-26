using UnityEngine;

public class TurnService : ITurnService
{
    private bool _isPlayerTurn = true;
    private IEventService eventService;
    private IEnemyService enemyService;
    private IRouletteService rouletteService;
    private IBuffService buffService;
    public TurnService()
    {
        eventService = AppContainer.Get<IEventService>();
        enemyService = AppContainer.Get<IEnemyService>();
        rouletteService = AppContainer.Get<IRouletteService>();
        buffService = AppContainer.Get<IBuffService>();
    }

    public void ChangeTurn()
    {
        
        _isPlayerTurn = !_isPlayerTurn;

        if (_isPlayerTurn)
        {
            if (enemyService == null) Debug.LogError("enemyService no instanciado");
            enemyService.resetTurns();
            rouletteService.ResetSpeed();
            buffService.ReduceDuration();
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

    public void resetTurn()
    {
       _isPlayerTurn = true;
    }
}

using UnityEngine;

public class StopWheelEvent : GameEventBase
{
    FichaData ficha;
    public StopWheelEvent(FichaData ficha)
    {
        this.ficha = ficha;
    }
}

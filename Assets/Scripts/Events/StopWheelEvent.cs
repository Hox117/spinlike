using UnityEngine;

public class StopWheelEvent : GameEventBase
{
    Ficha ficha;
    public StopWheelEvent(Ficha ficha)
    {
        this.ficha = ficha;
    }
}

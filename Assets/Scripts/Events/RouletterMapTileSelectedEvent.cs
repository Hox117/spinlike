using UnityEngine;

public class RouletterMapTileSelectedEvent : GameEventBase
{
    public int ordenHIjo;
   public RouletterMapTileSelectedEvent(int ordenHijo)
    {
        ordenHIjo = ordenHijo;
    }
}

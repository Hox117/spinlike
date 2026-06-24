using UnityEngine;

public class Buff
{
    public BuffType buffType;
    public int value;
    public int duration;
    public string Owner;
    public Buff(BuffType buffType, int value, int duration, string Owner)
    {
        this.buffType = buffType;
        this.value = value;
        this.duration = duration;
        this.Owner = Owner;
    }
}

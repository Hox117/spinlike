using System;
using UnityEngine;

public class Buff
{
    public BuffType buffType;
    public int value;
    public int duration;
    public Guid Owner;
    public Buff(BuffType buffType, int value, int duration, Guid Owner)
    {
        this.buffType = buffType;
        this.value = value;
        this.duration = duration;
        this.Owner = Owner;
    }
}

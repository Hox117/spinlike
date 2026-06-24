using System;
using UnityEngine;

public interface ICharacterService
{
    void resetPlayer();
    void heal(int value);
    void takeDamage(int value);
    void addShield(int value);
    public int getLife();
    public int getShield();
    public Guid getGuid();
}

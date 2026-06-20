using UnityEngine;

public interface ICharacterService
{
    void resetPlayer();
    void heal(int value);
    void takeDamage(int value);
    void addShield(int value);
}

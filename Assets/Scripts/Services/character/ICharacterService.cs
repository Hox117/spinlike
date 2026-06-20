using UnityEngine;

public interface ICharacterService
{
    void resetPlayer();
    void heal(int value);
    void takeDamege(int value);
}

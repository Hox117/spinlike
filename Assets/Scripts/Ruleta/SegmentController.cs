using System;
using Unity.VisualScripting;
using UnityEngine;

public class SegmentController : MonoBehaviour ,ISelectionable
{
    Ficha ficha;
    private bool _isSelected = true;
    private IEnemyService enemyService;
    private ICharacterService characterService;
    void Awake()
    {
        enemyService = AppContainer.Get<IEnemyService>();
        characterService = AppContainer.Get<ICharacterService>();       
    }

    public void OnSelected()
    {
        //Esto se regenera al volver a lanzar por lo que vuelve a ser true al regenerarse
        if (_isSelected)
        {
            Debug.Log("Gambleame esta " + ficha.id);
            _isSelected = false;
            switch (ficha.tipoDeFicha)
            {
                case FichaTypes.attack:
                    if (ficha.valor >= 0)
                        enemyService.getFirstEnemy().OnHit(ficha.valor);
                    else
                        characterService.takeDamage(Math.Abs(ficha.valor) );
                    break;
                case FichaTypes.defense:
                    break;
                case FichaTypes.buff:
                    break;
                case FichaTypes.debuff:
                    break;
                case FichaTypes.menu:
                    break;
            }

            
        }
        
    }

    public void addFicha(Ficha ficha)
    {
        this.ficha = ficha;
    }
}

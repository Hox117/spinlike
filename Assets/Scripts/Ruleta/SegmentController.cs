using System;
using Unity.VisualScripting;
using UnityEngine;

public class SegmentController : MonoBehaviour ,ISelectionable
{
    Ficha ficha;
    private bool _isSelected = true;
    private IEnemyService enemyService;
    private ICharacterService characterService;
    private ISceneService sceneService;
    private IAudioService audioService;
    private ITurnService turnService;
    void Awake()
    {

        enemyService = AppContainer.Get<IEnemyService>();
        characterService = AppContainer.Get<ICharacterService>();
        sceneService = AppContainer.Get<ISceneService>();
        audioService = AppContainer.Get<IAudioService>();

        turnService = AppContainer.Get<ITurnService>();

    }

    public void OnSelected()
    {
        //Esto se regenera al volver a lanzar por lo que vuelve a ser true al regenerarse
        if (_isSelected)
        {
            audioService.PlaySound(ficha.audioClip);
            _isSelected = false;


            foreach (Action action in ficha.actions)
            {
                switch (action.type)
                {
                    case FichaTypes.attack:
                        if (action.value >= 0)
                            enemyService.getFirstEnemy().OnHit(action.value );
                        else
                            characterService.takeDamage(Math.Abs(action.value ) );
                        break;
                    case FichaTypes.defense:
                        characterService.addShield(action.value );
                        break;
                    case FichaTypes.buff:
                        break;
                    case FichaTypes.debuff:
                        characterService.takeDamage(-action.value );
                        break;
                    case FichaTypes.menu:
                
                        sceneService.LoadScene((SceneNames) action.value );
                        break;
                }
            }


            

            
            turnService.ChangeTurn();
        }
        
    }

    public void addFicha(Ficha ficha)
    {
        this.ficha = ficha;
    }
}

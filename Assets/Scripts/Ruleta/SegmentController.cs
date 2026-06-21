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
            Debug.Log("Gambleame esta " + ficha.id);
            audioService.PlaySound(ficha.audioClip);
            _isSelected = false;
            switch (ficha.tipoDeFicha)
            {
                case FichaTypes.attack:
                    enemyService.getFirstEnemy().OnHit(ficha.valor);
                    break;
                case FichaTypes.defense:
                    characterService.addShield(ficha.valor);
                    break;
                case FichaTypes.buff:
                    break;
                case FichaTypes.debuff:
                    characterService.takeDamage(-ficha.valor);
                    break;
                case FichaTypes.menu:

                    sceneService.LoadScene((SceneNames) ficha.valor);
                    break;
            }

            
            turnService.ChangeTurn();
        }
        
    }

    public void addFicha(Ficha ficha)
    {
        this.ficha = ficha;
    }
}

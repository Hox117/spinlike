using UnityEngine;
/// <summary>
/// Clase estática encargada de inicializar y registrar todos los servicios de la aplicación antes de cargar cualquier escena utilizando <see cref="AppContainer"/>.
/// </summary>
public static class Program
{
    /// <summary>
    /// Método estático ejecutado automáticamente antes de cargar la primera escena gracias al atributo RuntimeInitializeOnLoadMethod. 
    ///Registra en <see cref="AppContainer"/> los distintos servicios necesarios para el funcionamiento del juego, incluyendo <see cref="AudioService"/>, 
    ///<see cref="EventService"/> <see cref="SceneService"/>
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Main()
    {
        // Registramos los servicios necesarios

        AppContainer.Register<IAudioService>(() => new AudioService());
        AppContainer.Register<IEventService>(() => new EventService());
        AppContainer.Register<ICharacterService>(() => new CharacterService());
        AppContainer.Register<ISceneService>(() => new SceneService(Resources.Load<PanelConfigurationScriptable>("Configuration/LoadingConfiguration")));
        AppContainer.Register<IInventoryService>(() => new InventoryService());
        AppContainer.Register<IRouletteService>(() => new RouletteService());
        AppContainer.Register<IEnemyService>(() => new EnemyService());        
    }
}
  
using UnityEngine;

public class ExiButton : MonoBehaviour
{
    ISceneService sceneService;
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");

        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void Back()
    {
        sceneService=AppContainer.Get<ISceneService>();
        sceneService.GoBack();

    }

}

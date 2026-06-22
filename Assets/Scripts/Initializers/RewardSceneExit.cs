using System.Collections;
using UnityEngine;

public class RewardSceneExit : MonoBehaviour
{
    [SerializeField] private SceneNames escenaACargar;
    ISceneService sceneService;
    void Awake()
    {
        sceneService = AppContainer.Get<ISceneService>();

    }
    public void ExitRewardScene()
    {
        StartCoroutine(Exit());
    }
    private IEnumerator Exit()
    {
        yield return new WaitForSeconds(6f);
        sceneService.LoadScene(escenaACargar);
    }
}

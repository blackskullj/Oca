using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoresUI : MonoBehaviour
{
    public void GoToGamesList()
    {
        SceneManager.LoadScene(Scenes.GAMES_LIST_SCENE);
    }
}

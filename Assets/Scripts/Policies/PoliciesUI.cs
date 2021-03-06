using UnityEngine;
using UnityEngine.SceneManagement;
public class PoliciesUI : MonoBehaviour
{
    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(Scenes.MAIN_MENU_SCENE);
    }
}

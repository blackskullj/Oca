using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(Scenes.MAIN_MENU_SCENE);
    }
}

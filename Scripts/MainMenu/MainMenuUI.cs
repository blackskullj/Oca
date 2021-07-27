﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(Scenes.LOGIN_SCENE);
    }

    //OnOptionsButtonClicked handled in the Unity Editor.

    public void OnQuitButtonClicked()
    {
    #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }

    public void OnCreditsButtonClicked()
    {
        SceneManager.LoadScene(Scenes.CREDITS_SCENE);
    }
}

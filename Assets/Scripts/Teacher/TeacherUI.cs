using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections;

public class TeacherUI : MonoBehaviour
{
    #region Private Serializable Fields
    /* *
     * Stores windows settings for every option in Teachers Menu
     * For the purpuse of Toggle windows while choosing options
     */
    [SerializeField]
    private GameObject[] __settings;

    [SerializeField]
    private TMP_InputField __input;

    #endregion

    public static List<string> questionPools = new List<string>();
    public static string __selected;

    public DropdownHandler dropdownH;

    #region Public Methods
    public void OnBackButtonClicked()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        SceneManager.LoadScene(Scenes.LOGIN_SCENE);
        Debug.LogFormat("TEACHER correctly disconnected from the server.");
    }

    // Toggles windows settings
    public void SetSettingsActive(int option)
    {
        foreach (GameObject element in __settings)
        {
            element.SetActive(false);
        }
        __settings[option].SetActive(true);
    }

    // Add to the List new QuestionsPool 
    public void AddOption()
    {
        dropdownH.Start();

        if (__input.text != "")
        {
            questionPools.Add(__input.text);
            __input.text = "";
            dropdownH.GetOptions();
        }
    }

    // Remove to the List QuestionsPool selected
    public void RemoveOption()
    {
        if (__selected != "")
        {
            questionPools.Remove(__selected);
            dropdownH.GetOptions();
        }
    }

    #endregion
}

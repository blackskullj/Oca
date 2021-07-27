using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LoginUI : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields

    [Tooltip("The UI Panel to let the user enter username and password, connect and play, or register.")]
    [SerializeField]
    private GameObject loginPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress.")]
    [SerializeField]
    private GameObject progressLabel;

    // Toggle temporal mientras terminamos la conexión a BD para revisar si el usuario es estudiante o docente.
    [SerializeField]
    private Toggle teacher_toggle;

    [SerializeField]
    private TextMeshProUGUI error_txt;

    [SerializeField]
    private TMP_InputField usernameInput;

    [SerializeField]
    private TMP_InputField passwordInput;

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// [Major build number].[Minor build number].[Revision].[Package]
    /// </summary>
    string gameVersion = "0.0.0.0";

    #endregion

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        progressLabel.SetActive(false);
        loginPanel.SetActive(true);
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region Public Methods

    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room.
    /// - if not yet connected, Connect this application instance to Photon Cloud Network.
    /// </summary>
    public void OnLoginButtonClicked()
    {
        // progressLabel activated and loginPanel deactivated from the Unity Editor.

        // Photon Network connection.
        if (!PhotonNetwork.IsConnected)
        {
            // #Critical, we must first and foremost connect to Photon Online Server.

            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        // With this we are able to see the games list.
        Debug.LogFormat("CONNECTED TO MASTER");

        if (teacher_toggle.isOn)
        {
            SceneManager.LoadScene(Scenes.TEACHER_SCENE);
        }
        else
        {
            SceneManager.LoadScene(Scenes.GAMES_LIST_SCENE);
        }
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(Scenes.MAIN_MENU_SCENE);
    }

    public void OnSignUpButtonClicked()
    {
        SceneManager.LoadScene(Scenes.SIGNUP_SCENE);
    }

    #endregion
}

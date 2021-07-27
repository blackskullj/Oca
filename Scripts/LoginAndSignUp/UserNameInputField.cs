using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


///<summary>
///Player's nickname that will be displayed in the lobby and in-game.
///</summary>
    
[RequireComponent(typeof(TMP_InputField))]
public class UserNameInputField : MonoBehaviour
{
    #region Private Constants

    // Store the PlayerPref Key to avoid typos.
    const string playerNamePrefKey = "PlayerName";

    #endregion


    #region Private Methods

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    private void Start()
    {
        string defaultName = string.Empty;
        TMP_InputField _inputField = this.GetComponent<TMP_InputField>();

        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Sets the name of the player, and saves it in the PlayerPrefs for future sessions.
    /// </summary>
    /// <param name="value">The name of the Player</param>

    public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                // TODO: Change this Debug to a message on screen for the user.
                Debug.LogError("Please enter a username.");
                return;
            }
            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
        #endregion
    }

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField __roomName;

    // Six players + the teacher.
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so a new room will be created.")]
    [SerializeField]
    private byte maxPlayersPerRoom = 7;

    public void OnCreateMatchButtonClicked()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogFormat("User not conected to the Photon Server.");
            return;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayersPerRoom;
        PhotonNetwork.JoinOrCreateRoom(__roomName.text, options, TypedLobby.Default);
    }

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnCreatedRoom()
    {
        Debug.LogFormat("Room: " + __roomName.text + " created successfully");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat("Room creation failed: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.LogFormat("Successfully joined room: " + __roomName.text);

        //// #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        //{
        //    // #Critical
        //    // Load the Room Level.
        //    PhotonNetwork.LoadLevel(Scenes.ROOM_SCENE);
        //}

        SceneManager.LoadScene(Scenes.ROOM_SCENE);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat("Unable to join room: " + message);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("User got disconnected: " + cause.ToString());
    }

    #endregion
}

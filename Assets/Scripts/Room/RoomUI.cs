using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class RoomUI : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI __btnText;
    [SerializeField]
    private TextMeshProUGUI _roomName;
    [SerializeField]
    private GameObject __button;

    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerInfo _playerInfo;

    private List<PlayerInfo> _playersList = new List<PlayerInfo>();


    #region Private Methods

    void Awake()
    {
        GetCurrentRoomPlayers();
    }

    void Start()
    {
        // If we're not the teacher, we are not going to be allowed to start the game.
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            __button.SetActive(false);
        }

        _roomName.SetText(PhotonNetwork.CurrentRoom.Name);
    }

    private void GetCurrentRoomPlayers()
    {
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player)
    {
        PlayerInfo newPlayerObject = Instantiate(_playerInfo, _content);

        if (newPlayerObject != null)
        {
            newPlayerObject.SetPlayerInfo(player);
            _playersList.Add(newPlayerObject);
        }
    }

    #endregion


    #region Public Methods

    public static GameScript Instance;

    public void OnBackButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnStartButtonClicked()
    {
        PhotonNetwork.LoadLevel(Scenes.GAME_SCENE);
    }

    #endregion

    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room.
    /// </summary>
    public override void OnLeftRoom()
    {
        Debug.LogFormat("PLAYER correctly left room.");
        SceneManager.LoadScene(Scenes.GAMES_LIST_SCENE);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
        Debug.LogFormat(newPlayer.NickName + " just entered the room.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _playersList.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_playersList[index].gameObject);
            _playersList.RemoveAt(index);
        }
    }

    #endregion
}
